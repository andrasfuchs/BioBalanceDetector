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
        public IChannelFilter Filter { get; private set; }

        public FilteredDataChannel(int samplesPerSecond, int bufferSize, IChannelFilter filter) : base(samplesPerSecond, bufferSize)
        {
            this.Filter = filter;
        }
    }
}
