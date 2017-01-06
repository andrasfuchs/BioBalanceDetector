using BBDDriver.Models.Source;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BBDDriver.Models.Output
{
    public class FileOutput : IDisposable
    {
        private int latencyMs = 40;           // this means that the file will be written every 40ms (effectively 25fps)
        private Timer dataWriterTimer;        // this thread calles the DataWriter event handler

        protected string directory;
        protected string filename;
        protected string extension;

        protected long dataProcessed;
        protected long bytesWritten;
        private int smallestBufferSize;

        public int DataOverflowWarningCount { get; private set; }
        public int BufferJitter { get; private set; }

        private const float dataOverflowWarning = 0.8f;     // if the data waiting to be read is more then the 80% of the size of the smallest buffer amount the channels, we get a warning
        private const float fileWriteThreshold = 0.25f;      // if the amount of data to be written is less then the 50% of smallest buffer then we keep going, we don't need to write it just yet

        private HashSet<int> changedChannels;
        protected List<DataChangedEventArgs> channelDataChanges;

        public event DataWrittenEventHandler DataWritten;

        public FileOutput(MultiChannelInput<IDataChannel> mci, string path)
        {
            this.directory = Path.GetDirectoryName(path);
            this.filename = Path.GetFileNameWithoutExtension(path);
            this.extension = Path.GetExtension(path);

            this.changedChannels = new HashSet<int>();
            this.channelDataChanges = new List<Models.DataChangedEventArgs>();

            mci.AllChannelsDataChanged += AllDataChanged;

            dataWriterTimer = new Timer(WriteDataFromBuffer, this, 0, latencyMs);
        }

        public virtual void Dispose()
        {
        }

        private void AllDataChanged(object sender, AllChannelsDataChangedEventArgs e)
        {
            foreach (var cdc in e.DataChanges)
            {
                if (cdc == null) continue;

                if (changedChannels.Add(cdc.Channel.ChannelId))
                {
                    channelDataChanges.Add(new DataChangedEventArgs(cdc.Channel, cdc.Position, cdc.DataCount));
                    smallestBufferSize = channelDataChanges.Min(dc => dc.Channel.BufferSize);
                }
                else
                {
                    var channelDataChange = channelDataChanges.Find(dc => dc.Channel.ChannelId == cdc.Channel.ChannelId);
                    channelDataChange.Position = cdc.Position;
                    channelDataChange.DataCount += cdc.DataCount;
                }
            }

            int channelDataCanBeWrittenCount = channelDataChanges.Min(dc => dc.DataCount);
            int channelDataToBeWrittenCount = channelDataChanges.Max(dc => dc.DataCount);

            this.BufferJitter = channelDataToBeWrittenCount - channelDataCanBeWrittenCount;

            if (channelDataToBeWrittenCount > smallestBufferSize * dataOverflowWarning) DataOverflowWarningCount++;
            if (channelDataCanBeWrittenCount < smallestBufferSize * fileWriteThreshold) return;

            float[][] dataToWrite = new float[channelDataChanges.Count][];

            for (int i = 0; i < channelDataChanges.Count; i++)
            {
                var cdc = channelDataChanges[i];
                int channelPosition = cdc.Position - (cdc.DataCount - channelDataCanBeWrittenCount);

                dataToWrite[i] = cdc.Channel.GetData(channelDataCanBeWrittenCount, channelPosition);

                cdc.DataCount -= channelDataCanBeWrittenCount;
            }

            WriteDataToBuffer(dataToWrite, channelDataCanBeWrittenCount);
        }

        protected virtual void WriteDataToBuffer(float[][] dataToWrite, int dataCount)
        {
            throw new NotImplementedException();
        }

        protected virtual void WriteDataFromBuffer(object stateInfo)
        {
            throw new NotImplementedException();
        }

        protected virtual byte[] ConvertData(float data)
        {
            return BitConverter.GetBytes(data);
        }

        protected virtual void OnDataWritten(object sender, DataWrittenEventArgs e)
        {
            DataWritten?.Invoke(sender, e);
        }

    }
}
