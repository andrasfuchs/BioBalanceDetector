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
        List<byte> buffer = new List<byte>();
        private Thread inputGeneratorThread;    // this generates the bitstream at the given frequency (e.g. 8000 Hz)
        private int latencyMs = 40;           // this means that the DataAvailable event will be fired at 20 fps tops
        private Timer dataAvailableTimer;     // this thread calles the DataAvailable event handler

        protected HighResolutionTimer timer;
        protected bool recordingInProgress = false;
        private Int64 counterAtStart;
        private Int64 previousCounterAtStart;

        protected IDataChannel[] channels;
        public IDataChannel[] Channels
        {
            get
            {
                return channels;
            }
        }

        private WaveFormat waveFormat;
        public WaveFormat WaveFormat
        {
            get { return waveFormat; }
            set { throw new NotSupportedException("WaveFormat can be set only once as a parameter of the MultiChannelInput's constructor!"); }
        }

        public event EventHandler<WaveInEventArgs> DataAvailable;
        public event EventHandler<StoppedEventArgs> RecordingStopped;

        public MultiChannelInput(int channelCount)
        {
            this.waveFormat = new WaveFormat(8000, 16, channelCount);

            timer = new HighResolutionTimer();

            inputGeneratorThread = new Thread(new ThreadStart(InputGeneratorLoop));
            inputGeneratorThread.Start();

            dataAvailableTimer = new Timer(DataAvailableEventFire, this, 0, latencyMs);
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
            long timeBetweenSamplesInTicks = (long)((1.0 / waveFormat.SampleRate) * timer.Frequency);

            while (true)
            {
                if (!recordingInProgress)
                {
                    Thread.Sleep(latencyMs);
                }
                else
                {
                    // Get counter value when the operation ends.
                    long counterAtEnd = timer.Value;

                    // Get time elapsed in tenths of a millisecond.
                    long timeElapsedInTicks = counterAtEnd - counterAtStart;

                    while (timeElapsedInTicks >= timeBetweenSamplesInTicks)
                    {
                        previousCounterAtStart = counterAtStart;
                        counterAtStart = timer.Value;

                        lock (buffer)
                        {
                            foreach (IDataChannel channel in channels)
                            {
                                double value = channel.GetValue(previousCounterAtStart, timeBetweenSamplesInTicks) ?? channel.GetValue(previousCounterAtStart) ?? 0.0;
                                buffer.AddRange(BitConverter.GetBytes((short)(value * 32767)));
                            }
                        }

                        timeElapsedInTicks -= timeBetweenSamplesInTicks;
                        previousCounterAtStart += timeBetweenSamplesInTicks;
                    }
                }
            }

        }

        private void DataAvailableEventFire(object stateInfo)
        {
            lock (buffer)
            {
                if (buffer.Count == 0) return;

                DataAvailable?.Invoke(this, new WaveInEventArgs(buffer.ToArray(), buffer.Count));

                buffer.Clear();
            }
        }

        public virtual double?[] GetValues()
        {
            return channels.Select(ch => ch.GetValue()).ToArray();
        }
    }
}
