using System;
using System.Collections.Generic;
using System.IO;

namespace BBDDriver.Models.Input
{
    /// <summary>
    /// General float-based data channel
    /// </summary>
    public class SinglePrecisionDataChannel : IDataChannel
    {
        public int ChannelId { get; protected set; }

        private int bufferPosition;
        public int BufferPosition
        {
            get
            {
                return bufferPosition;
            }

            protected set
            {
                while (value < 0)
                {
                    value += this.BufferSize;
                }

                while (value >= this.BufferSize)
                {
                    value -= this.BufferSize;
                }

                bufferPosition = value;
            }
        }
        public int BufferSize { get; private set; }

        public int SamplesPerSecond { get; protected set; }
        public float[] Data { get; protected set; }
        public string[] Labels { get; protected set; }
        public DateTime[] TimeStamps { get; protected set; }
        public float[] Quality { get; protected set; }

        public event DataChangedEventHandler DataChanged;
        public event DataChangedEventHandler DataRead;

        public SinglePrecisionDataChannel(int samplesPerSecond, int bufferSize)
        {
            HighResolutionTimer hrt = new HighResolutionTimer();
            this.ChannelId = (int)(hrt.Value % Int32.MaxValue);
            System.Threading.Thread.Sleep(1);

            this.SamplesPerSecond = samplesPerSecond;
            this.BufferSize = bufferSize;

            this.Data = new float[bufferSize];
            this.Labels = new string[bufferSize];
            this.TimeStamps = new DateTime[bufferSize];
            this.Quality = new float[bufferSize];
        }

        public void AppendData(float[] data)
        {
            AppendData(data, null, null, null);
        }

        public virtual void AppendData(float[] data, string[] labels = null, DateTime[] timeStamps = null, float[] quality = null)
        {
            int oldBufferPosition = this.BufferPosition;

            lock (this.Data)
            {
                int dataCount = data.Length;

                while (dataCount > 0)
                {
                    int copyCount = (BufferSize - BufferPosition < dataCount ? BufferSize - BufferPosition : dataCount);

                    Array.Copy(data, 0, this.Data, BufferPosition, copyCount);
                    BufferPosition = (BufferPosition + copyCount) % BufferSize;

                    dataCount -= copyCount;
                }
            }

            int newDataCount = (oldBufferPosition < this.BufferPosition ? this.BufferPosition - oldBufferPosition : this.BufferSize - oldBufferPosition + this.BufferPosition);
            OnDataChanged(new DataChangedEventArgs(this, this.BufferPosition, newDataCount));
        }

        protected virtual void OnDataChanged(DataChangedEventArgs e)
        {
            DataChanged?.Invoke(this, e);
        }

        public virtual float[] GetData(int sampleCount, int? position = null)
        {
            float[] result = new float[sampleCount];

            if (position.HasValue)
            {
                while (position < 0)
                {
                    position += this.BufferSize;
                }

                while (position >= this.BufferSize)
                {
                    position -= this.BufferSize;
                }
            }

            lock (this.Data)
            {
                int dataCount = (sampleCount > BufferSize ? BufferSize : sampleCount);
                int readRosition = (position.HasValue ? position.Value : BufferPosition);

                int secondSegmentDataCount = (readRosition < dataCount ? readRosition : dataCount);
                if (secondSegmentDataCount > 0)
                {
                    Array.Copy(this.Data, readRosition - secondSegmentDataCount, result, result.Length - secondSegmentDataCount, secondSegmentDataCount);
                }

                int firstSegmentDataCount = dataCount - secondSegmentDataCount;
                if (firstSegmentDataCount > 0)
                {
                    Array.Copy(this.Data, BufferSize - firstSegmentDataCount, result, result.Length - secondSegmentDataCount - firstSegmentDataCount, firstSegmentDataCount);
                }

                DataRead?.Invoke(this, new DataChangedEventArgs(this, readRosition, dataCount));
            }

            return result;
        }

        public float[] GetData(TimeSpan time)
        {
            checked
            {
                return GetData((int)(time.TotalSeconds / this.SamplesPerSecond));
            }
        }

        public override string ToString()
        {
            return $"#{this.ChannelId.ToString("X")} bf:{this.BufferSize}";
        }
    }
}
