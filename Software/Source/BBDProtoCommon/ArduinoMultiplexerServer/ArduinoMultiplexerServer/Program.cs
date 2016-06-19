using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows;
using System.Web.Http.SelfHost;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NAudio.Wave;
using System.IO;

namespace ArduinoMultiplexerServer
{
    class Program
    {
        private static string selfHostedServerUrl = "http://localhost:8080";
        private static string arduinoPort = "COM3";

        private static SerialPort arduinoCOMPort;
        private static string dataBuffer = "";
        private static List<byte> dataBufferBytes = new List<byte>();
        private static ushort[] valueBuffer = new ushort[655360];
        private static int valueBufferIndex = 0;
        private static DateTime? valueBufferStartTime = null;
        private static double waveformResolutionInSeconds = 1.0 / 30.0; // 25 frames per second
        private static string sessionId;
        private static bool readyToRead = false;
        private static ADCChannelValue[] recentValues;
        private static double recentChangesSensitivity = 1.00;

        private static DateTime firstActivity;
        private static long arduinoCOMPortBytesReceived;
        private static long arduinoLLCommandReceived;

        private static HttpClient selfHostedClient;

        private static WaveFileWriter waveFile = null;
        private static long waveFileBytesWritten = 0;

        private static HighResolutionTimer hrt = new HighResolutionTimer();

        static void Main(string[] args)
        {

            // Set up the serial port communication with the Arduino on COM3 at 115200 baud
            arduinoCOMPort = new SerialPort(arduinoPort) { BaudRate = 115200*16 };
            //  hook up the event for receiving the data
            arduinoCOMPort.DataReceived += ArduinoCOMPort_DataReceived;


            // let's start our self-hosted server
            HttpSelfHostServer server = ConfigureServer(selfHostedServerUrl);
            var serverSession = server.OpenAsync();


            // initialize the client side
            selfHostedClient = InitializeClient(selfHostedServerUrl);


            try
            {
                serverSession.Wait();
            }
            catch
            {
                Console.WriteLine("You must have administrator privileges to run the web server.");
                Console.ReadLine();
                return;
            }





            IWaveIn waveSource = null;
            string waveFilename = GetWaveFilename();

            try
            {
                arduinoCOMPort.Open();
                //waveSource = WriteWaveFile(waveFilename, new BBDInput(64));
            }
            catch (System.IO.IOException)
            {
                Console.WriteLine(String.Format("Arduino is not connected on port '{0}', creating random signal generator as data-source.", arduinoPort));

                // Arduino is not connected, let create a 64-channel 16bit, 8kHz pseudo-source of data                
                waveSource = WriteWaveFile(waveFilename, new RandomInput(64));

                firstActivity = DateTime.UtcNow;
            }

            readyToRead = true;




            System.Timers.Timer printMatrixTimer = new System.Timers.Timer(100);
            printMatrixTimer.Elapsed += PrintMatrixTimer_Elapsed;
            printMatrixTimer.Start();




            if (waveSource != null)
            {
                waveSource.StartRecording();
            }

            Console.WriteLine("Press any key to quit");
            Console.ReadLine();

            if (waveSource != null)
            {
                waveSource.StopRecording();
                waveSource.Dispose();
            }

            if (arduinoCOMPort.IsOpen)
            {
                arduinoCOMPort.Close();
            }
        }

        private static void PrintMatrixTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            PrintValueMatrix();
        }

        private static void ArduinoCOMPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (!readyToRead)
            {
                return;
            }

            if (firstActivity == null)
            {
                firstActivity = DateTime.UtcNow;
            }

            int bytesToRead = arduinoCOMPort.BytesToRead;

            byte[] buffer = new byte[bytesToRead];
            arduinoCOMPortBytesReceived += arduinoCOMPort.Read(buffer, 0, bytesToRead);

            dataBufferBytes.AddRange(buffer);
            dataBuffer += System.Text.Encoding.ASCII.GetString(buffer);

            int dataBlockStartIndex = dataBuffer.IndexOf("<|");
            int dataBlockEndIndex = dataBuffer.IndexOf("|>");

            if ((dataBlockStartIndex < 0) || (dataBlockEndIndex < 0)) return;

            List<SerialCommand> commandsReceived = new List<SerialCommand>();
            while ((dataBlockStartIndex >= 0) && (dataBlockEndIndex >= 0))
            {
                if (dataBlockStartIndex + 1 < dataBlockEndIndex)
                {
                    SerialCommand sc = new SerialCommand();

                    string dataBlock = dataBuffer.Substring(dataBlockStartIndex + 2, dataBlockEndIndex - dataBlockStartIndex - 2);

                    if (dataBlock.Length > 3)
                    {
                        sc.Command = dataBlock.Substring(0, 2);
                        sc.ParametersRaw = new byte[dataBlock.Length - 3];
                        dataBufferBytes.CopyTo(dataBlockStartIndex + 2 + 3, sc.ParametersRaw, 0, dataBlock.Length - 3);
                        sc.Parameters = dataBlock.Substring(3).Split(new char[] { ',' });

                        commandsReceived.Add(sc);
                    }
                }

                dataBuffer = dataBuffer.Substring(dataBlockEndIndex + 2);
                dataBufferBytes = dataBufferBytes.Skip(dataBlockEndIndex + 2).ToList();
                dataBlockStartIndex = dataBuffer.IndexOf("<|");
                dataBlockEndIndex = dataBuffer.IndexOf("|>");
            }

            foreach (SerialCommand sc in commandsReceived)
            {
                if (sc.Command == "ch")
                {
                    byte channel = 0;
                    int parsedValue = -1;

                    if ((sc.Parameters.Length < 2) || (!Byte.TryParse(sc.Parameters[0], out channel)) || (!Int32.TryParse(sc.Parameters[1], out parsedValue)))
                    {
                        continue;
                    }

                    ushort value = (ushort)parsedValue;

                    if ((valueBufferStartTime != null) && (waveformResolutionInSeconds <= (DateTime.UtcNow - valueBufferStartTime.Value).TotalSeconds))
                    {
                        double sum = 0;
                        for (int i = 0; i < valueBufferIndex; i++)
                        {
                            sum += valueBuffer[i];
                        }
                        sum /= valueBufferIndex;

                        selfHostedClient.GetAsync("api/channels/appendvalue/" + sessionId + "/" + channel + "/" + valueBufferStartTime.Value.ToString("o") + "/" + sum);
                        //Console.WriteLine("api/channels/appendvalue/" + sessionId + "/" + channel + "/" + valueBufferStartTime.Value.ToString("o") + "/" + sum);

                        //HttpResponseMessage response = client.GetAsync("api/channels/appendvalue/" + sessionId + "/" + channel + "/" + valueBufferStartTime.Value.ToString("o") + "/" + sum).Result;
                        //response.EnsureSuccessStatusCode();

                        valueBufferIndex = 0;
                    }

                    if (valueBufferIndex == 0)
                    {
                        valueBufferStartTime = DateTime.UtcNow;
                    }

                    valueBuffer[valueBufferIndex] = value;
                    valueBufferIndex++;

                    if (channel == 0)
                    {
                        PrintValueMatrix();
                    }
                }
                else if (sc.Command == "ll")
                {
                    for (int i=0; i<sc.ParametersRaw.Length/2; i++)
                    {
                        ushort value = (ushort)(sc.ParametersRaw[i*2] * 256 + sc.ParametersRaw[i*2 + 1]);

                        selfHostedClient.GetAsync("api/channels/appendvalue/" + sessionId + "/" + i + "/" + DateTime.UtcNow.ToString("o") + "/" + value);
                    }

                    arduinoLLCommandReceived++;
                }
                else if (sc.Command == "bm")
                {
                    hrt.Stop();
                    Console.WriteLine($"Time elapsed until benchmark point '{sc.Parameters[0]}' - remote: {sc.Parameters[1]} ticks, local: {hrt.Duration.ToString("0.0000")} seconds");
                    hrt.Start();
                }
                else
                {
                    Console.WriteLine(sc);
                }
            }
        }

        static HttpSelfHostServer ConfigureServer(string serverUrl)
        {
            // set up the server side
            var config = new HttpSelfHostConfiguration(serverUrl);
            // Attribute routing
            config.MapHttpAttributeRoutes();

            // Convention-based routing
            config.Routes.MapHttpRoute(
                "API Default", "api/{controller}/{id}",
                new { id = RouteParameter.Optional });

            // set the default WebAPI format to JSON
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            // enable Cross Origin Resource Sharing
            config.EnableCors();

            return new HttpSelfHostServer(config);
        }

        static HttpClient InitializeClient(string serverUrl)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(selfHostedServerUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // test the server if it's working properly
            try
            {
                var response = client.GetAsync("api/test").Result;
            }
            catch
            {
                Console.WriteLine(String.Format("Could not communicate with '{0}', web server is not running.", selfHostedServerUrl));
            }


            if (sessionId == null)
            {
                var response = client.GetAsync("api/channels/createsession").Result;
                response.EnsureSuccessStatusCode();

                sessionId = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);
            }

            return client;
        }

        static IWaveIn WriteWaveFile(string waveFilename, IWaveIn waveSource)
        {
            waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
            waveSource.RecordingStopped += new EventHandler<StoppedEventArgs>(waveSource_RecordingStopped);

            waveFile = new WaveFileWriter(waveFilename, waveSource.WaveFormat);

            return waveSource;
        }

        static void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (waveFile != null)
            {
                lock (waveFile)
                {
                    waveFile.Write(e.Buffer, 0, e.BytesRecorded);
                    waveFile.Flush();

                    waveFileBytesWritten += e.BytesRecorded;
                }
            }
        }

        static void waveSource_RecordingStopped(object sender, StoppedEventArgs e)
        {
            if (waveFile != null)
            {
                lock (waveFile)
                {
                    waveFile.Dispose();
                    waveFile = null;
                }
            }
        }

        static async void PrintValueMatrix()
        {
            try
            {
                var response = await selfHostedClient.GetAsync("api/channels/getvalues/" + sessionId + "/" + new TimeSpan(0, 0, 3).ToString());

                var jsonString = response.Content.ReadAsStringAsync();
                jsonString.Wait();

                ADCChannelValue[] values = JsonConvert.DeserializeObject<ADCChannelValue[]>(jsonString.Result);

                lock (sessionId)
                {
                    if (recentValues != null)
                    {
                        bool isChanged = false;

                        for (int i = 0; i < values.Length; i++)
                        {
                            if (values[i]?.Value != recentValues[i]?.Value)
                            {
                                isChanged = true;
                                break;
                            }
                        }

                        if (!isChanged) return;
                    }


                    string[] valueStrings = new string[8 * 8];

                    double sum = 0;
                    int equalsCount = 0;

                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            ADCChannelValue value = values[i * 8 + j];

                            // A, show values
                            valueStrings[i * 8 + j] = value == null ? "-.---" : (value.Value / 65536).ToString("0.000");


                            // B, show changes
                            if (recentValues != null)
                            {
                                if ((values[i * 8 + j] == null) || (recentValues[i * 8 + j] == null))
                                {
                                    valueStrings[i * 8 + j] = "o";
                                }
                                else
                                {
                                    double diff = values[i * 8 + j].Value - recentValues[i * 8 + j].Value;

                                    if (diff > recentChangesSensitivity * recentChangesSensitivity * recentChangesSensitivity)
                                    {
                                        valueStrings[i * 8 + j] = "+++";
                                    }
                                    else if (diff > recentChangesSensitivity * recentChangesSensitivity)
                                    {
                                        valueStrings[i * 8 + j] = "++";
                                    }
                                    else if (diff > recentChangesSensitivity)
                                    {
                                        valueStrings[i * 8 + j] = "+";
                                    }

                                    if (diff < -recentChangesSensitivity)
                                    {
                                        valueStrings[i * 8 + j] = "-";
                                    }
                                    else if (diff < -recentChangesSensitivity * recentChangesSensitivity)
                                    {
                                        valueStrings[i * 8 + j] = "--";
                                    }
                                    else if (diff < -recentChangesSensitivity * recentChangesSensitivity * recentChangesSensitivity)
                                    {
                                        valueStrings[i * 8 + j] = "---";
                                    }

                                    if ((diff > -recentChangesSensitivity) && (diff < recentChangesSensitivity))
                                    {
                                        valueStrings[i * 8 + j] = "";
                                        equalsCount++;
                                    }
                                }
                            }

                            sum += (value == null ? 0 : (value.Value / 65536));
                        }
                    }

                    sum /= 64;

                    Console.WriteLine();
                    for (int i = 0; i < 8; i++)
                    {
                        Console.WriteLine(
                            String.Format("|{0,5}|{1,5}|{2,5}|{3,5}|{4,5}|{5,5}|{6,5}|{7,5}|",
                                valueStrings[i * 8 + 0], valueStrings[i * 8 + 1], valueStrings[i * 8 + 2], valueStrings[i * 8 + 3], valueStrings[i * 8 + 4], valueStrings[i * 8 + 5], valueStrings[i * 8 + 6], valueStrings[i * 8 + 7])
                            );
                    }

                    double timeElapsed = (DateTime.UtcNow - firstActivity).TotalSeconds;
                    Console.Title = $"{sessionId} - {sum.ToString("0.000")} - {(arduinoCOMPortBytesReceived / timeElapsed / 1024).ToString("0.00")} kbytes/sec - {((double)arduinoLLCommandReceived / timeElapsed).ToString("0.00")} fps - sensibility {recentChangesSensitivity.ToString("0.0000")} - wav file: {(waveFileBytesWritten / 1024).ToString("#,0")} kbytes";


                    // comment this line to disable the feature
                    //recentValues = values;
                    //if (equalsCount > 60)
                    //{
                    //    recentChangesSensitivity /= 2.0;
                    //}

                    //if (equalsCount < 4)
                    //{
                    //    recentChangesSensitivity *= 2.0;
                    //}

                }
            }
            catch (System.Net.Http.HttpRequestException)
            {
                Console.WriteLine("Communication error.");
            }
            catch (System.Threading.Tasks.TaskCanceledException)
            {
                Console.WriteLine("Thread was cancelled.");
            }
        }

        static string GetWaveFilename()
        {
            string waveFilePath = @"c:\Work\BioBalanceDetector\Recordings\BBDProto04\";
            string waveFilename = "";
            if (!String.IsNullOrEmpty(sessionId))
            {
                waveFilename = waveFilePath + sessionId + ".wav";
            }
            else
            {
                int i = 1;
                while (File.Exists(waveFilePath + "BBD_" + i.ToString("0000") + ".wav"))
                {
                    i++;
                }

                waveFilename = waveFilePath + "BBD_" + i.ToString("0000") + ".wav";
            }

            return waveFilename;
        }
    }
}
