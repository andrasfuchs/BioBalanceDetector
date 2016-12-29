using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BBDDriver.Models.Input
{
    public class MultiChannelInput<T> where T : IDataChannel
    {
        public int SamplesPerSecond { get; private set; }

        public int ChannelCount
        {
            get
            {
                return channels.Length;
            }
        }

        public event DataChangedEventHandler DataChanged;
        public event AllChannelsDataChangedEventHandler AllChannelsDataChanged;

        protected T[] channels;

        private HashSet<int> changedChannels;
        private DataChangedEventArgs[] channelDataChanges;

        public MultiChannelInput(int samplesPerSecond, int channelCount)
        {
            if (channelCount <= 0) throw new ArgumentNullException("channels", "The channelCount parameter cannot be 0.");

            this.changedChannels = new HashSet<int>();
            this.SamplesPerSecond = samplesPerSecond;
            this.channels = new T[channelCount];
            this.channelDataChanges = new DataChangedEventArgs[channelCount];
        }

        public T GetChannel(int index)
        {
            lock (channels)
            {
                if ((index < 0) || (index >= channels.Length)) throw new ArgumentOutOfRangeException("index", $"Since the input has {channels.Length} channels, the index must be between 0 and {channels.Length - 1}");

                return channels[index];
            }
        }

        public void SetChannel(int index, T channel)
        {
            lock (channels)
            {
                if ((channel == null) || (channel.SamplesPerSecond != this.SamplesPerSecond)) throw new ArgumentNullException("SamplesPerSecond", "The channel you add to a multichannel input must have the same SamplePerSecond value as the multichannel input.");

                if (channels[index] != null) throw new ArgumentNullException("Index", "The channel with a particular index can be set only once.");

                channels[index] = channel;
                channel.DataChanged += Channel_DataChanged;
            }
        }

        private void Channel_DataChanged(object sender, DataChangedEventArgs e)
        {
            DataChanged?.Invoke(this, e);

            int index = Array.IndexOf(channels, e.Channel);
            if (index < 0) throw new ArgumentOutOfRangeException("channel", "The channel passed to the multichannel input's datachanged event is not part of that multichannel input.");

            lock (changedChannels)
            {
                if (changedChannels.Add(e.Channel.ChannelId))
                {
                    channelDataChanges[index] = new DataChangedEventArgs(e.Channel, e.Position, e.DataCount);
                }
                else
                {
                    channelDataChanges[index].Position = e.Position;
                    channelDataChanges[index].DataCount += e.DataCount;
                }

                if (changedChannels.Count == channels.Length)
                {
                    AllChannelsDataChanged?.Invoke(this, new AllChannelsDataChangedEventArgs(channelDataChanges));

                    changedChannels.Clear();
                    channelDataChanges = new DataChangedEventArgs[this.ChannelCount];
                }
            }
        }

        public virtual float[] GetValues()
        {
            return channels.Select(ch => ch.GetData(1)[0]).ToArray();
        }
    }
}
