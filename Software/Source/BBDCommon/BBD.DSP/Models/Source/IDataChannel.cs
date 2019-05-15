using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBD.DSP.Models.Source
{
    public interface IDataChannel
    {
        int ChannelId { get; }

        int SamplesPerSecond { get; }

        int BufferSize { get; }

        /// <summary>
        /// The event fires when new data is available in its buffer
        /// </summary>
        event DataChangedEventHandler DataChanged;

        /// <summary>
        /// The event fires after some data was read from the datachannel's buffer
        /// </summary>
        event DataChangedEventHandler DataRead;

        float[] GetData(int sampleCount, int? position = null);

        void AppendData(float[] data);
    }
}
