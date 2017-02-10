using BBDDriver.Models.Source;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBDDriver.Models.Filter
{
    public class ThresholdFilterSettings : ChannelFilterSettings
    {
        public float MinValue { get; set; }
        public float MaxValue { get; set; }
    }

    public class ThresholdFilter : BaseChannelFilter<ThresholdFilterSettings>
    {
        protected override void InputDataChanged(object sender, DataChangedEventArgs e)
        {
            float[] newData = e.Channel.GetData(e.DataCount);

            for (int i=0; i < e.DataCount; i++)
            {
                if (newData[i] < settings.MinValue) newData[i] = 0;

                if (newData[i] > settings.MaxValue) newData[i] = 0;
            }

            this.Output.AppendData(newData);
        }
    }
}
