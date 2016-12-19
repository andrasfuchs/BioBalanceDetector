using BBDDriver.Models.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBDDriver.Models.Mapper
{
    public static class ChannelMapper
    {
        private static Dictionary<int, int[]> coordinates = new Dictionary<int, int[]>();

        public static int[] MapChannel(IDataChannel channel)
        {
            if (!coordinates.ContainsKey(channel.ChannelId))
            {
                coordinates.Add(channel.ChannelId, new int[] { coordinates.Count, 0, 0 });
            }

            return coordinates[channel.ChannelId];
        }
    }
}
