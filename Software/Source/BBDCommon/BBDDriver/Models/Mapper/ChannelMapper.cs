using BBDDriver.Models.Source;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBDDriver.Models.Mapper
{
    public static class ChannelMapper
    {
        private static Dictionary<int, PhysicalPosition> coordinates = new Dictionary<int, PhysicalPosition>();

        private static float scaleFactor = 1000.0f;
        private static int[,] channelMapperMatrix = new int[8, 2] { { 0, 0 }, { 1, 0 }, { 2, 0 }, { 2, 1 }, { 2, 2 }, { 1, 2 }, { 0, 2 }, { 0, 1 } };

        public static PhysicalPosition GetChannelPosition(IDataChannel channel)
        {
            if (!coordinates.ContainsKey(channel.ChannelId))
            {
                coordinates.Add(channel.ChannelId, GetDefaultPosition(coordinates.Count));
            }

            return coordinates[channel.ChannelId];
        }

        public static PhysicalPosition GetDefaultPosition(int index)
        {
            int channelIndex = index % 8;
            int cellIndex = index / 8;

            int channelX = channelMapperMatrix[channelIndex, 0];
            int channelY = channelMapperMatrix[channelIndex, 1];

            return new PhysicalPosition {X = (cellIndex * 3 + channelX) * scaleFactor, Y = (channelY) * scaleFactor };
        }

        public static PhysicalBoundaries GetChannelInputBoundaries(MultiChannelInput<IDataChannel> mci)
        {
            PhysicalBoundaries result = new PhysicalBoundaries() { MinX = Int32.MaxValue, MinY = Int32.MaxValue, MinZ = Int32.MaxValue, MaxX = Int32.MinValue, MaxY = Int32.MinValue, MaxZ = Int32.MinValue };

            for (int i = 0; i < mci.ChannelCount; i++)
            {
                IDataChannel ch = mci.GetChannel(i);
                PhysicalPosition pp = ChannelMapper.GetChannelPosition(ch);

                if (pp.X < result.MinX) result.MinX = pp.X;
                if (pp.X > result.MaxX) result.MaxX = pp.X;
                if (pp.Y < result.MinY) result.MinY = pp.Y;
                if (pp.Y > result.MaxY) result.MaxY = pp.Y;
                if (pp.Z < result.MinZ) result.MinZ = pp.Z;
                if (pp.Z > result.MaxZ) result.MaxZ = pp.Z;
            }

            return result;
        }
    }
}
