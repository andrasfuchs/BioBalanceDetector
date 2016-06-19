using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoMultiplexerServer
{
    internal class RandomInput : MultiChannelInput
    {
        public RandomInput(int channelCount) : base(channelCount)
        {
            for (int i=0; i<channelCount; i++)
            {
                this.channels.Add(new RandomChannel());
            }
        }

    }
}
