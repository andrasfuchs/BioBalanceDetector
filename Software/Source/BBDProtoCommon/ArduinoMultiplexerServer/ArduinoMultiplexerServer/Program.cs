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

namespace ArduinoMultiplexerServer
{
    class Program
    {
        private static SerialPort arduinoCOMPort;
        private static ushort[] valueBuffer = new ushort[655360];
        private static int valueBufferIndex = 0;
        private static DateTime? valueBufferStartTime = null;
        private static double waveformResolutionInSeconds = 1.0 / 30.0; // 25 frames per second
        private static HttpClient client = new HttpClient();
        private static string sessionId;
        private static HttpSelfHostServer server;
        private static bool readyToRead = false;

        static void Main(string[] args)
        {
            // Set up the serial port communication with the Arduino on COM3 at 115200 baud
            arduinoCOMPort = new SerialPort("COM3") { BaudRate = 115200 };
            //  hook up the event for receiving the data
            arduinoCOMPort.DataReceived += ArduinoCOMPort_DataReceived;
            arduinoCOMPort.Open();


            // set up the client side
            client.BaseAddress = new Uri("http://localhost:8080");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            // set up the server side
            var config = new HttpSelfHostConfiguration("http://localhost:8080");
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

            using (server = new HttpSelfHostServer(config))
            {
                var serverSession = server.OpenAsync();

                if (sessionId == null)
                {
                    var response = client.GetAsync("api/channels/createsession").Result;
                    response.EnsureSuccessStatusCode();

                    sessionId = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);
                }

                try {
                    serverSession.Wait();
                }
                catch
                {
                    Console.WriteLine("You must have administrator privileges to run the web server.");
                    Console.ReadLine();
                    return;
                }

                readyToRead = true;

                Console.WriteLine("Press Enter to quit.");
                Console.ReadLine();
            }

            arduinoCOMPort.Close();
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

                        client.GetAsync("api/channels/appendvalue/" + sessionId + "/" + channel + "/" + valueBufferStartTime.Value.ToString("o") + "/" + sum);

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
    }
}
