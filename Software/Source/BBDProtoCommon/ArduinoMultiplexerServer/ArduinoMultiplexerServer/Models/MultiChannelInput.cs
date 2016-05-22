using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArduinoMultiplexerServer
{
    public class MultiChannelInput : IWaveIn
    {
        private int latencyMs = 5;      // this means 200 fps
        private Thread inputGeneratorThread;

        private HighResolutionTimer timer;
        private bool recordingInProgress = false;
        private Int64 counterAtStart;
        private Int64 previousCounterAtStart;

        private IDataChannel[] channels;

        private WaveFormat waveFormat;
        public WaveFormat WaveFormat
        {
            get { return waveFormat; }
            set { waveFormat = value; }
        }

        public event EventHandler<WaveInEventArgs> DataAvailable;
        public event EventHandler<StoppedEventArgs> RecordingStopped;

        public MultiChannelInput(params IDataChannel[] channels)
        {
            this.channels = channels;
            this.WaveFormat = new WaveFormat(8000, 16, channels.Length);

            timer = new HighResolutionTimer();

            inputGeneratorThread = new Thread(new ThreadStart(InputGeneratorLoop));
            inputGeneratorThread.Start();
        }

        public void Dispose()
        {
            if (recordingInProgress)
            {
                StopRecording();
            }

            if (inputGeneratorThread != null)
            {
                inputGeneratorThread.Abort();
            }
        }

        public void StartRecording()
        {
            // Get counter value before the operation starts.
            counterAtStart = timer.Value;

            recordingInProgress = true;
        }

        public void StopRecording()
        {
            RecordingStopped?.Invoke(this, new StoppedEventArgs());
        }

        private void InputGeneratorLoop()
        {
            List<byte> buffer = new List<byte>();

            while (true)
            {
                if (!recordingInProgress)
                {
                    Thread.Sleep(latencyMs);
                }
                else
                {
                    // Get counter value when the operation ends.
                    Int64 counterAtEnd = timer.Value;

                    // Get time elapsed in tenths of a millisecond.
                    Int64 timeElapsedInTicks = counterAtEnd - counterAtStart;
                    Int64 timeElapseInTenthsOfMilliseconds = (timeElapsedInTicks * 10000) / timer.Frequency;

                    if (timeElapseInTenthsOfMilliseconds >= latencyMs)
                    {
                        previousCounterAtStart = counterAtStart;
                        counterAtStart = timer.Value;

                        buffer.Clear();
                        foreach (IDataChannel channel in channels)
                        {
                            short channelValue = channel.Get16BitSignedIntData(previousCounterAtStart, latencyMs);
                            buffer.AddRange(BitConverter.GetBytes(channelValue));
                        }
                        DataAvailable?.Invoke(this, new WaveInEventArgs(buffer.ToArray(), buffer.Count));
                    }
                }
            }

        }
    }
}
