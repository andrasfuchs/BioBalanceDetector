using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SleepLogger
{
    public class SleepLoggerConfig
    {
        public AD2Config AD2 { get; }
        public PostprocessingConfig Postprocessing { get; }

        public SleepLoggerConfig(IConfigurationRoot config)
        {
            AD2 = new AD2Config()
            {
                Samplerate = config["AD2:Samplerate"].EndsWith("k") ? Int32.Parse(config["AD2:Samplerate"][0..^1]) * 1024 : Int32.Parse(config["AD2:Samplerate"]),
                SignalGeneratorChannel = config["AD2:SignalGeneratorChannel"] == "W1" ? 0 : config["AD2:SignalGeneratorChannel"] == "W2" ? 1 : 255,
                SignalGeneratorHz = Single.Parse(config["AD2:SignalGeneratorHz"]),
                SignalGeneratorVolt = Single.Parse(config["AD2:SignalGeneratorVolt"]),
            };

            Postprocessing = new PostprocessingConfig()
            {
                IntervalSeconds = Single.Parse(config["Postprocessing:IntervalSeconds"]),
                FFTSize = config["Postprocessing:FFTSize"].EndsWith("k") ? Int32.Parse(config["Postprocessing:FFTSize"][0..^1]) * 1024 : Int32.Parse(config["Postprocessing:FFTSize"]),
                SaveAsWAV = Boolean.Parse(config["Postprocessing:SaveAsWAV"]),
                SaveAsFFT = Boolean.Parse(config["Postprocessing:SaveAsFFT"]),
                SaveAsCompressedFFT = Boolean.Parse(config["Postprocessing:SaveAsCompressedFFT"]),
                SaveAsPNG = Boolean.Parse(config["Postprocessing:SaveAsPNG"]),
                MagnitudeThreshold = Single.Parse(config["Postprocessing:MagnitudeThreshold"]),
            };
        }
    }

    public class AD2Config
    {
        /// <summary>
        /// Number of samples per second
        /// </summary>
        public int Samplerate { get; set; }
        /// <summary>
        /// Signal generation channel (W1 or W2)
        /// </summary>
        public byte SignalGeneratorChannel { get; set; }
        /// <summary>
        /// Generated signal frequency
        /// </summary>
        public float SignalGeneratorHz { get; set; }
        /// <summary>
        /// Peak amplitude of the generated signal
        /// </summary>
        public float SignalGeneratorVolt { get; set; }
    }

    public class PostprocessingConfig
    {
        public float IntervalSeconds { get; set; }
        /// <summary>
        /// FFT size
        /// </summary>
        public int FFTSize { get; set; }
        public bool SaveAsWAV { get; set; }
        public bool SaveAsFFT { get; set; }
        public bool SaveAsCompressedFFT { get; set; }
        public bool SaveAsPNG { get; set; }
        public float MagnitudeThreshold { get; set; }
    }
}
