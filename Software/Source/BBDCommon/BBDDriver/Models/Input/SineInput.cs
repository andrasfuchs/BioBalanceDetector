using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBDDriver.Models.Input
{
    internal class SineInput : MultiChannelInput<IDataChannel>
    {
        public SineInput(int samplesPerSecond, int channelCount) : base(samplesPerSecond, channelCount)
        {
            for (int i = 0; i < channelCount; i++)
            {
                this.SetChannel(i, new SineChannel(samplesPerSecond, i*10, (i+5)*10, 5.0f));
            }
        }
    }
}
