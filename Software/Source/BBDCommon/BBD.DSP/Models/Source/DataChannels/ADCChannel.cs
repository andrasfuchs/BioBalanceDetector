using System;
using System.Collections.Generic;
using System.Linq;

namespace BBD.DSP.Models.Source
{
    public class ADCChannel : SinglePrecisionDataChannel
    {
        private List<ADCChannelValue> values;

        public int ADCChannelId { get; set; }
        public int BitRate { get; set; }

        public ADCChannel(int samplePerSecond, int bufferSize) : base(samplePerSecond, bufferSize)
        {
            values = new List<ADCChannelValue>();
        }

        internal void AppendValue(DateTime timeStamp, float value)
        {
            lock (values)
            {
                values.Add(new ADCChannelValue() { TimeStamp = timeStamp, Value = value });

                int valuesCount = values.Count;
                if (valuesCount > 1)
                {
                    if (values[valuesCount - 2].TimeStamp.Second != values[valuesCount - 1].TimeStamp.Second)
                    {
                        values[valuesCount - 2].Label = values[valuesCount - 2].TimeStamp.Second + "s";
                    } else
                    {
                        values[valuesCount - 2].Label = ".";
                    }
                }
            }

            lock (this.Data)
            {
                // TODO: fill the end of the buffer with the previous data
                // this.AppendData();
            }
        }

        internal ADCChannelValue[] GetValues(DateTime after)
        {
            List<ADCChannelValue> result = new List<ADCChannelValue>();

            lock (values)
            {
                foreach (var value in values)
                {
                    if (value.TimeStamp > after)
                    {
                        result.Add(value);
                    }
                }
            }

            return result.ToArray();
        }

        public override string ToString()
        {
            return $"ch-{ADCChannelId} ({values.Count} samples)";
        }
    }
}
