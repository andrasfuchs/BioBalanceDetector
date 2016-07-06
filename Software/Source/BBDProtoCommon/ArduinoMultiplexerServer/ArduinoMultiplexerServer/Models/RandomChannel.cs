using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoMultiplexerServer
{
    public class RandomChannel : IDataChannel
    {
        private double lastValue = 0;
        private static RNGCryptoServiceProvider rnd = new RNGCryptoServiceProvider();

        public double? GetValue(long startTimeInTicks, long durationInTicks)
        {
            return GetValue();
        }

        public double? GetValue(long startTimeInTicks)
        {
            return GetValue();
        }

        public double? GetValue()
        {
            byte[] rndValue = new byte[1];
            rnd.GetBytes(rndValue);

            double result = (lastValue + (rndValue[0] - 127) / 1024.0);
            result = (result >= 1.0 ? 1.0 : (result <= -1.0 ? -1.0 : result));
            lastValue = result;

            return result;
        }
    }
}
