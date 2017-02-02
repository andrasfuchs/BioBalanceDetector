using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBDDriver
{
    public class DataRateBenchmarkEntry
    {
        public DateTime TimeStamp { get; set; }
        public bool IsRead { get; set; }
        public int BytesTransferred { get; set; }
        public int SamplesTransferred { get; set; }

        public ushort MaxJumpBetweenSampleValues { get; set; }
        public int EightBitChangeOverflowCount { get; set; }
        public int SameValueWarningCount { get; set; }
    }
}
