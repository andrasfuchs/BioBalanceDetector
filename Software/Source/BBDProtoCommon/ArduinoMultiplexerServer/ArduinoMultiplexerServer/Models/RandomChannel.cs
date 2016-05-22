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
        private short lastValue = 0;
        private RNGCryptoServiceProvider rnd = new RNGCryptoServiceProvider();

        public short Get16BitSignedIntData(Int64 startTime, int duration)
        {
            byte[] rndValue = new byte[1];
            rnd.GetBytes(rndValue);

            int result = (lastValue + rndValue[0] - 127);
            result = (result >= 32768 ? 32767 : (result <= -32768 ? -32767 : result));
            lastValue = (short)result;

            return (short)result;
        }
    }
}
