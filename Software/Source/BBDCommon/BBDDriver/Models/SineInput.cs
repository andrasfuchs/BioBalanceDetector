using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoMultiplexerServer
{
    internal class SineInput : MultiChannelInput
    {
        public SineInput(int channelCount) : base(channelCount)
        {
            List<SineChannel> channels = new List<SineChannel>();
            for (int i=0; i<channelCount; i++)
            {
                channels.Add(new SineChannel());
            }

            this.channels = channels.ToArray();
        }

    }
}
