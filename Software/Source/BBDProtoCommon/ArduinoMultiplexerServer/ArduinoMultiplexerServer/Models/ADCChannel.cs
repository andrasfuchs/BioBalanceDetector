using System;
using System.Collections.Generic;

namespace ArduinoMultiplexerServer
{
    public class ADCChannel : IDataChannel
    {
        private List<ADCChannelValue> values;

        public int ADCChannelId { get; set; }
        public int BitRate { get; set; }
        public int SamplesPerSecond { get; set; }

        public ADCChannel()
        {
            values = new List<ADCChannelValue>();
        }

        internal void AppendValue(DateTime timeStamp, double value)
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
        }

        internal ADCChannelValue[] GetValues(DateTime dateTime)
        {
            List<ADCChannelValue> result = new List<ADCChannelValue>();

            lock (values)
            {
                foreach (var value in values)
                {
                    if (value.TimeStamp > dateTime)
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

        public short? Get16BitSignedIntData(long startTimeInTicks, long durationInTicks)
        {
            throw new NotImplementedException();
        }
    }
}
