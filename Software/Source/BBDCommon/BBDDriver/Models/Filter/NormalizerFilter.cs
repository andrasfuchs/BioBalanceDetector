using BBDDriver.Models.Source;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBDDriver.Models.Filter
{
    public class NormalizeFilterSettings : ChannelFilterSettings
    {
        public int SampleCount { set; get; } = 1000;
        public float Gain { set; get; } = 1.0f;
        public float GainToNormalize { set; get; } = 1.0f;
        public float Offset { set; get; }
    }

    public class NormalizeFilter : BaseChannelFilter<NormalizeFilterSettings>
    {
        private float[] valuesToMeasure;
        private int position = 0;

        protected override void InputDataChanged(object sender, DataChangedEventArgs e)
        {
            float[] newData = e.Channel.GetData(e.DataCount);

            float[] result = new float[newData.Length];

            for (int i = 0; i < newData.Length; i++)
            {
                valuesToMeasure[position] = newData[i];
                position = (position + 1) % settings.SampleCount;
            }

            settings.Offset = -valuesToMeasure.Average();
            float maxAbsValue = Math.Max(valuesToMeasure.Max() + settings.Offset, -(valuesToMeasure.Min() + settings.Offset));            
            settings.GainToNormalize = 1.0f / maxAbsValue;

            for (int i = 0; i < newData.Length; i++)
            {
                result[i] = (newData[i] + settings.Offset) * settings.Gain;

                if (result[i] > +1.0) result[i] = +1.0f;
                if (result[i] < -1.0) result[i] = -1.0f;
            }


            this.Output.AppendData(result);
        }

        protected override void OnSettingsChanged(SettingsChangedEventArgs e)
        {
            valuesToMeasure = new float[settings.SampleCount];
        }
    }
}
