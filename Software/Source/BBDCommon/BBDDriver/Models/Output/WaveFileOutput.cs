using BBDDriver.Models.Input;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BBDDriver.Models.Output
{
    public class WaveFileOutput : IDisposable
    {
        private List<byte> buffer = new List<byte>();
        private int latencyMs = 40;           // this means that the file will be written every 40ms (effectively 25fps)
        private Timer dataWriterTimer;        // this thread calles the DataWriter event handler

        private FileStream waveFile;
        private BinaryWriter waveWriter;
        private long bytesWritten;

        private HashSet<int> changedChannels;
        private List<DataChangedEventArgs> channelDataChanges;

        public event DataWrittenEventHandler DataWritten;

        public WaveFileOutput(MultiChannelInput<IDataChannel> mci, string path)
        {

            //this.waveFile = File.Create(path);
            this.waveFile = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            this.waveWriter = new BinaryWriter(waveFile);
            WriteHeader(mci.SamplesPerSecond, 16, mci.ChannelCount);

            this.changedChannels = new HashSet<int>();
            this.channelDataChanges = new List<Models.DataChangedEventArgs>();
            mci.AllChannelsDataChanged += WriteDataToBuffer;

            dataWriterTimer = new Timer(WriteDataFromBuffer, this, 0, latencyMs);
        }

        public void Dispose()
        {
            UpdateHeader(bytesWritten);
            waveWriter.Close();
            waveFile.Close();
        }

        private void WriteDataToBuffer(object sender, AllChannelsDataChangedEventArgs e)
        {
            foreach (var cdc in e.DataChanges)
            {
                if (cdc == null) continue;

                if (changedChannels.Add(cdc.Channel.ChannelId))
                {
                    channelDataChanges.Add(new DataChangedEventArgs(cdc.Channel, cdc.Position, cdc.DataCount));
                }
                else
                {
                    var channelDataChange = channelDataChanges.Find(dc => dc.Channel.ChannelId == cdc.Channel.ChannelId);
                    channelDataChange.Position = cdc.Position;
                    channelDataChange.DataCount += cdc.DataCount;
                }
            }

            int channelDataChangeCount = channelDataChanges.Min(dc => dc.DataCount);

            float[][] dataToWrite = new float[channelDataChanges.Count][];

            for (int i = 0; i < channelDataChanges.Count; i++)
            {
                var cdc = channelDataChanges[i];
                int channelPosition = cdc.Position - (cdc.DataCount - channelDataChangeCount);

                dataToWrite[i] = cdc.Channel.GetData(channelDataChangeCount, channelPosition);

                cdc.DataCount -= channelDataChangeCount;
            }

            lock (buffer)
            {
                for (int j = 0; j < channelDataChangeCount; j++)
                {
                    for (int i = 0; i < channelDataChanges.Count; i++)
                    {
                        buffer.AddRange(BitConverter.GetBytes((short)(dataToWrite[i][j] * 32767)));
                    }
                }
            }
        }

        private void WriteDataFromBuffer(object stateInfo)
        {
            byte[] dataToWrite = null;
            lock (buffer)
            {
                if (buffer.Count == 0) return;
                dataToWrite = buffer.ToArray();
                buffer.Clear();
            }

            waveWriter.Seek(0, SeekOrigin.End);

            waveWriter.Write(dataToWrite, 0, dataToWrite.Length);
            bytesWritten += dataToWrite.Length;

            DataWritten?.Invoke(this, new DataWrittenEventArgs(waveFile.Name, dataToWrite.Length, bytesWritten));

            UpdateHeader(this.bytesWritten);
        }

        private void WriteHeader(int samplesPerSecond, int bitsPerSample, int channelCount)
        {
            int bytesPerSample = ((bitsPerSample - 1) / 8) + 1;

            waveWriter.Seek(0, SeekOrigin.Begin);

            waveWriter.Write(Encoding.ASCII.GetBytes("RIFF"));
            waveWriter.Write(0);                                        // ChunkSize = 4 + (8 + SubChunk1Size) + (8 + SubChunk2Size)
            waveWriter.Write(Encoding.ASCII.GetBytes("WAVE"));
            waveWriter.Write(Encoding.ASCII.GetBytes("fmt "));
            waveWriter.Write(16);                                       // SubChunk1Size = 16
            waveWriter.Write((short)1);                                 // PCM = 1 (i.e. Linear quantization) Values other than 1 indicate some form of compression.
            waveWriter.Write((short)channelCount);                      // Channels
            waveWriter.Write((int)(samplesPerSecond));                  // Sample rate
            waveWriter.Write((int)(samplesPerSecond * bytesPerSample * channelCount));  // Average bytes per second
            waveWriter.Write((short)(bytesPerSample * channelCount));   // The number of bytes for one sample including all channels.
            waveWriter.Write((short)(bitsPerSample));                   // Bits per sample
            waveWriter.Write(Encoding.ASCII.GetBytes("data"));
            waveWriter.Write((int)(0));                                 // SubChunk2Size

            waveWriter.Flush();
        }

        private void UpdateHeader(long bytesWritten)
        {
            // update ChunkSize
            waveWriter.Seek(4, SeekOrigin.Begin);
            waveWriter.Write((int)(4 + 8 + 16 + 8 + bytesWritten));

            // update SubChunk2Size
            waveWriter.Seek(40, SeekOrigin.Begin);
            waveWriter.Write((int)bytesWritten);

            waveWriter.Flush();
        }
    }
}
