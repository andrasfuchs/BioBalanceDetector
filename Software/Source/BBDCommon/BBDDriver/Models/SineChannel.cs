using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoMultiplexerServer
{
    public class SineChannel : IDataChannel
    {
        private const double samplingFrequency = 8000.0;

        private const double minimumFrequency = 30.0;
        private const double maximumFrequency = 50.0;
        private double currentFrequency = minimumFrequency + ((maximumFrequency - minimumFrequency) / 2);
        private double frequencyChangeRate = 0.0;

        private double phase = 0.0;
        private double phaseReset = 2 * Math.PI;
        
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
            phase += phaseReset / samplingFrequency;
            while (phase > phaseReset)
            {
                phase -= phaseReset;

                byte[] rndValue = new byte[1];
                rnd.GetBytes(rndValue);

                frequencyChangeRate = ((rndValue[0] - 127) / 128.0) / samplingFrequency;
            }

            currentFrequency += frequencyChangeRate;
            if (currentFrequency < minimumFrequency) currentFrequency = minimumFrequency;
            if (currentFrequency > maximumFrequency) currentFrequency = maximumFrequency;


            double result = Math.Sin(phase * currentFrequency);
            result = (result >= 1.0 ? 1.0 : (result <= -1.0 ? -1.0 : result));

            return result;
        }
    }
}
