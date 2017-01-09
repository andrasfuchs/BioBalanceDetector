using BBDDriver.Models.Source;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBDDriver.Models.Filter
{
    public class MovingAverageFilterSettings : ChannelFilterSettings
    {
        public int MovingAverageLength { set; get; }
        public int InputDataDimensions { set; get; }
    }

    public class MovingAverageFilter : BaseChannelFilter<MovingAverageFilterSettings>
    {
        private float[][] valuesToAverage;
        private int position = 0;

        protected override void InputDataChanged(object sender, DataChangedEventArgs e)
        {
            float[] newData = e.Channel.GetData(e.DataCount);

            if (newData.Length % settings.InputDataDimensions != 0) throw new ArgumentException("The received data count must be a multiple of the dimension.", "DataCount");
            float[] result = new float[newData.Length];

            for (int i = 0; i < newData.Length; i++)
            {
                if (i % settings.InputDataDimensions != 0) continue;

                for (int j = 0; j < settings.InputDataDimensions; j++)
                {
                    valuesToAverage[j][position] = newData[i + j];
                    result[i + j] = valuesToAverage[j].Average();
                }

                position = (position + 1) % settings.MovingAverageLength;
            }

            this.Output.AppendData(result);
        }

        protected override void OnSettingsChanged(SettingsChangedEventArgs e)
        {
            if (settings.InputDataDimensions <= 0) throw new ArgumentException("The dimensions of the moving average must be 1 or more.", "InputDataDimensions");

            valuesToAverage = new float[settings.InputDataDimensions][];
            for (int j = 0; j < settings.InputDataDimensions; j++)
            {
                valuesToAverage[j] = new float[settings.MovingAverageLength];
            }
        }
    }
}
