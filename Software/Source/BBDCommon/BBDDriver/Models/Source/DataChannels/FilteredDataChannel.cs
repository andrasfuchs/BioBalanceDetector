using BBDDriver.Models.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBDDriver.Models.Source
{
    public class FilteredDataChannel : SinglePrecisionDataChannel
    {
        public IDataChannel Input { get; private set; }
        public IChannelFilter Filter { get; private set; }

        public FilteredDataChannel(IDataChannel input, int samplesPerSecond, int bufferSize, IChannelFilter filter) : base(samplesPerSecond, bufferSize)
        {
            this.Input = input;
            this.Filter = filter;
        }
    }
}
