using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBD.DSP.Models
{
    public class ADCChannelValue
    {
        public DateTime TimeStamp { get; set; }
        public string Label { get; set; }
        public double Value { get; set; }

        public override string ToString()
        {
            return Value.ToString("0.0000");
        }
    }
}
