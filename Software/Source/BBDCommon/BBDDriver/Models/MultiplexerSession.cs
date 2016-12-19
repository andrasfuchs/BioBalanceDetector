using BBDDriver.Models.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBDDriver.Models
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

        private List<IDataChannel> channelList;

        public List<IDataChannel> ChannelList
        {
            get
            {
                return channelList;
            }
        }

        public MultiplexerSession()
        {
            channelList = new List<IDataChannel>();

            for (int i=0; i<64; i++)
            {
                channelList.Add(new ADCChannel(1000, 1024*1024) { ADCChannelId = i, BitRate = 16 });
            }
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
