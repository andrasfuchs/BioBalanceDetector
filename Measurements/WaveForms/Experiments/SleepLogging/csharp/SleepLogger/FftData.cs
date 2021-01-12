using NWaves.Signals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleepLogger
{
    public class FftData
    {
        public DateTime CaptureTime { get; set; }
        public double Duration { get; set; }
        public float FirstFrequency { get; set; }
        public float LastFrequency { get; set; }
        public float FrequencyStep { get; set; }
        public int FftSize { get; set; }
        public float[] MagnitudeData { get; set; }

        public FftData() { }
        public FftData(DateTime captureTime, float firstFrequency, float lastFrequency, float frequencyStep, int fftSize, DiscreteSignal powerSpectrum)
        {
            this.CaptureTime = captureTime;
            this.Duration = powerSpectrum.Duration;
            this.FirstFrequency = firstFrequency;
            this.LastFrequency = lastFrequency;
            this.FrequencyStep = frequencyStep;
            this.FftSize = fftSize;
            this.MagnitudeData = powerSpectrum.Samples;
        }
    }
}
