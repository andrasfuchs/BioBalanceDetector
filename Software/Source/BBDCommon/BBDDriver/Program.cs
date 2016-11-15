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

        //private static ushort[] valueBuffer = new ushort[655360];
        private static string sessionId;

        private static double?[] recentValues;
        private static double recentChangesSensitivity = 1.00;

        private static DateTime firstActivity;

        private static HttpClient selfHostedClient;

        private static MultiChannelInput waveSource = null;
        private static WaveFileWriter waveFile = null;
        private static long waveFileBytesWritten = 0;


        static void Main(string[] args)
        {
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





            string waveFilename = GetWaveFilename();

            try
            {
                waveSource = (BBDInput)WriteWaveFile(waveFilename, new BBDInput(arduinoPort, 64));
            }
            catch (System.IO.IOException)
            {
                Console.WriteLine(String.Format("Arduino is not connected on port '{0}', creating random signal generator as data-source.", arduinoPort));

                // Arduino is not connected, let's create a 64-channel 16bit, 8kHz pseudo-source of data                
                //waveSource = (RandomInput)WriteWaveFile(waveFilename, new RandomInput(64));
                waveSource = (SineInput)WriteWaveFile(waveFilename, new SineInput(64));
            }



            System.Timers.Timer printMatrixTimer = new System.Timers.Timer(100);
            printMatrixTimer.Elapsed += PrintMatrixTimer_Elapsed;
            printMatrixTimer.Start();




            if (waveSource != null)
            {
                waveSource.StartRecording();
            }

            firstActivity = DateTime.UtcNow;
            Console.WriteLine("Press any key to quit");
            Console.ReadLine();

            if (waveSource != null)
            {
                waveSource.StopRecording();
                waveSource.Dispose();
            }
        }

        private static void PrintMatrixTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            PrintValueMatrix();
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
                //var response = await selfHostedClient.GetAsync("api/channels/getvalues/" + sessionId + "/" + new TimeSpan(0, 0, 3).ToString());

                //var jsonString = response.Content.ReadAsStringAsync();
                //jsonString.Wait();

                //ADCChannelValue[] values = JsonConvert.DeserializeObject<ADCChannelValue[]>(jsonString.Result);
                double?[] values = waveSource.GetValues();

                lock (sessionId)
                {
                    if (recentValues != null)
                    {
                        bool isChanged = false;

                        for (int i = 0; i < values.Length; i++)
                        {
                            if (values[i] != recentValues[i])
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
                            double? value = values[i * 8 + j];

                            // A, show values
                            valueStrings[i * 8 + j] = value == null ? "-.---" : value.Value.ToString("0.00");


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

                            sum += (value == null ? 0 : value.Value);
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
                    Console.Title = $"{sessionId} - sum: {sum.ToString("0.000")}";
                    if (waveSource is BBDInput)
                    {
                        BBDInput bbdInput = (BBDInput)waveSource;

                        Console.Title += $" - {(bbdInput.COMPortBytesReceived / timeElapsed / 1024).ToString("0.00")} kbytes/sec - {((double)bbdInput.LLCommandReceived / timeElapsed).ToString("0.00")} fps";
                    }
                    if (recentValues != null)
                    {
                        Console.Title += $" - sensibility { recentChangesSensitivity.ToString("0.0000")}";
                    }
                    Console.Title += $" - wav file: {(waveFileBytesWritten / 1024).ToString("#,0")} kbytes";


                    // comment this line to disable the feature
                    //recentValues = values;
                    if (equalsCount > 60)
                    {
                        recentChangesSensitivity /= 2.0;
                    }

                    if (equalsCount < 4)
                    {
                        recentChangesSensitivity *= 2.0;
                    }
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
            string waveFilePath = @"c:\Work\BioBalanceDetector\Recordings\";
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
