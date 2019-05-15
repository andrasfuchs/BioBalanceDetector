using BBD.DSP.Models.Filter;
using BBD.DSP.Models.Source;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBD.DSP.Helpers
{
    public static class FilterManager
    {
        public static MultiChannelInput<IDataChannel> ApplyFilters(MultiChannelInput<IDataChannel> mci, params IChannelFilter[] filters)
        {
            MultiChannelInput<IDataChannel> result = new MultiChannelInput<IDataChannel>(mci.SamplesPerSecond, mci.ChannelCount);

            for (int i = 0; i < mci.ChannelCount; i++)
            {
                IDataChannel dataChannel = mci.GetChannel(i);

                foreach (IChannelFilter filter in filters)
                {
                    dataChannel = filter.Copy().Apply(dataChannel);
                }

                result.SetChannel(i, dataChannel);
            }

            return result;
        }

        public static T FindFilter<T>(IDataChannel channel) where T : class, IChannelFilter
        {
            var filteredChannel = channel as FilteredDataChannel;

            if (filteredChannel == null) return null;

            if (filteredChannel.Filter is T) return filteredChannel.Filter as T;            

            return FindFilter<T>(filteredChannel.Input);
        }
    }
}
