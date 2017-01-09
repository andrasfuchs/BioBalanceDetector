using BBDDriver.Models.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBDDriver.Models.Source
{
    internal class SineInput : MultiChannelInput<IDataChannel>
    {
        public SineInput(int samplesPerSecond, int channelCount) : base(samplesPerSecond, channelCount)
        {
            for (int i = 0; i < channelCount; i++)
            {
                //this.SetChannel(i, new SineChannel(samplesPerSecond, 555, 555, 5.0f));
                //this.SetChannel(i, new SineChannel(samplesPerSecond, (i+1) * 100, (i+1) * 100, 5.0f));
                this.SetChannel(i, new SineChannel(samplesPerSecond, i * 100, (i + 5) * 100, 5.0f));
            }

            //this.channels[0].DataChanged += SineInput_DataChanged;
            //this.channels[0].DataRead += SineInput_DataRead;
        }

        /*
        private void SineInput_DataRead(object sender, DataChangedEventArgs e)
        {
            Console.WriteLine($"R: {e.Channel} {e.Position} {e.DataCount}");
        }

        private void SineInput_DataChanged(object sender, DataChangedEventArgs e)
        {
            Console.WriteLine($"W: {e.Channel} {e.Position} {e.DataCount} {DateTime.UtcNow.Second}.{DateTime.UtcNow.Millisecond.ToString("000")}");
        }
        */
    }
}
