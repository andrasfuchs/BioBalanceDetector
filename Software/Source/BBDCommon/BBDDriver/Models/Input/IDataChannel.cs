using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBDDriver.Models.Input
{
    public interface IDataChannel
    {
        int ChannelId { get; }

        int SamplesPerSecond { get; }

        event DataChangedEventHandler DataChanged;

        float[] GetData(int sampleCount, int? position = null);
    }
}
