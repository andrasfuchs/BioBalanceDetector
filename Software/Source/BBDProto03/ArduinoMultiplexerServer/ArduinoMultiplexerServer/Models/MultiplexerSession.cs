using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoMultiplexerServer
{
    public class MultiplexerSession
    {
        private string sessionId;

        public string SessionId
        {
            get
            {
                if (sessionId == null)
                {
                    sessionId = "BBD_" + DateTime.UtcNow.ToString("yyyyMMdd_HHmmss_") + RandomString(4);
                }

                return sessionId;
            }
        }

        private List<ADCChannel> channelList;

        public List<ADCChannel> ChannelList
        {
            get
            {
                return channelList;
            }
        }

        public MultiplexerSession()
        {
            channelList = new List<ADCChannel>{
                new ADCChannel { ADCChannelId=1, BitRate=16, SamplesPerSecond=1000 },
                new ADCChannel { ADCChannelId=2, BitRate=16, SamplesPerSecond=1000 },
                new ADCChannel { ADCChannelId=3, BitRate=16, SamplesPerSecond=1000 },
                new ADCChannel { ADCChannelId=4, BitRate=16, SamplesPerSecond=1000 }
            };
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
