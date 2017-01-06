using BBDDriver.Models.Source;
using System;

namespace BBDDriver.Models.Filter
{
    public interface IChannelFilter
    {
        IDataChannel Input { get; }
        IDataChannel Output { get; }

        ChannelFilterSettings Settings { get; set; }

        IDataChannel Apply(IDataChannel input);

        IChannelFilter Copy();
    }
}
