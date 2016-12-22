using BBDDriver.Models.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BBDDriver.Models.Output
{
    public class WaveFileOutput : FileOutput
    {
        private FileStream waveFileStream;
        private BinaryWriter waveWriter;

        public WaveFileOutput(MultiChannelInput<IDataChannel> mci, string path) : base(mci, path)
        {
            this.waveFileStream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            this.waveWriter = new BinaryWriter(waveFileStream);
            WriteHeader(mci.SamplesPerSecond, 16, mci.ChannelCount);            
        }

        public override void Dispose()
        {
            UpdateHeader(bytesWritten);
            this.waveWriter.Close();
            this.waveFileStream.Close();
        }


        protected override void AppendData(byte[] dataToWrite)
        {
            waveWriter.Seek(0, SeekOrigin.End);

            waveWriter.Write(dataToWrite, 0, dataToWrite.Length);
            bytesWritten += dataToWrite.Length;

            UpdateHeader(this.bytesWritten);
        }

        private void WriteHeader(int samplesPerSecond, int bitsPerSample, int channelCount)
        {
            if (samplesPerSecond <= 0) throw new ArgumentOutOfRangeException("samplesPerSecond");
            if (bitsPerSample != 16) throw new ArgumentOutOfRangeException("bitsPerSample");
            if (channelCount <= 0) throw new ArgumentOutOfRangeException("channelCount");

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

        protected override byte[] ConvertData(float data)
        {
            return BitConverter.GetBytes((short)(data * 32767));
        }
    }
}
