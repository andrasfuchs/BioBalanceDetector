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

namespace ArduinoMultiplexerServer
{
    class Program
    {
        private static string selfHostedServerUrl = "http://localhost:8080";
        private static string arduinoPort = "COM3";

        private static SerialPort arduinoCOMPort;
        private static ushort[] valueBuffer = new ushort[655360];
        private static int valueBufferIndex = 0;
        private static DateTime? valueBufferStartTime = null;
        private static double waveformResolutionInSeconds = 1.0 / 30.0; // 25 frames per second
        private static string sessionId;
        private static bool readyToRead = false;

        private static HttpClient selfHostedClient;
        private static WaveFileWriter waveFile = null;

        static void Main(string[] args)
        {
            // Set up the serial port communication with the Arduino on COM3 at 115200 baud
            arduinoCOMPort = new SerialPort(arduinoPort) { BaudRate = 115200 };
            //  hook up the event for receiving the data
            arduinoCOMPort.DataReceived += ArduinoCOMPort_DataReceived;

            try
            {
                arduinoCOMPort.Open();
            }
            catch (System.IO.IOException)
            {
                Console.WriteLine(String.Format("Arduino is not connected on port '{0}', creating random signal generator as data-source.", arduinoPort));
                // TODO: Arduino is not connected, let create a 64-channel 16bit, 8kHz pseudo-source of data
            }


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

            readyToRead = true;


            IWaveIn waveSource = WriteWaveFile(@"C:\Test0001.wav");
            waveSource.StartRecording();

            Console.WriteLine("Press any key to quit");
            Console.ReadLine();

            waveSource.StopRecording();
            waveSource.Dispose();

            if (arduinoCOMPort.IsOpen)
            {
                arduinoCOMPort.Close();
            }
        }

        private static void ArduinoCOMPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (!readyToRead)
            {
                return;
            }

            string[] dataReceived = arduinoCOMPort.ReadExisting().Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string data in dataReceived)
            {
                if (data.StartsWith("ch"))
                {
                    string[] values = data.Split(new string[] { "|", System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    byte channel = 0;
                    int parsedValue = -1;

                    if ((values.Length < 2) || (!Byte.TryParse(values[0].Substring(2), out channel)) || (!Int32.TryParse(values[1], out parsedValue)))
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
                }
            }
            //Console.Write(data);
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

        static IWaveIn WriteWaveFile(string waveFilename)
        {
            IWaveIn waveSource = null;

            waveSource = new MultiChannelInput(
                new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(),
                new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(),
                new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(),
                new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(),
                new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(),
                new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(),
                new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(),
                new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel(), new RandomChannel());

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
    }
}
