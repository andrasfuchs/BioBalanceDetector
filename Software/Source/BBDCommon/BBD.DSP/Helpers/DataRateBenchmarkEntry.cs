using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBD.DSP
{
    public class DataRateBenchmarkEntry
    {
        public DateTime TimeStamp { get; set; }
        public bool IsRead { get; set; }
        public int BytesTransferred { get; set; }
        public int SamplesTransferred { get; set; }
    }
}
