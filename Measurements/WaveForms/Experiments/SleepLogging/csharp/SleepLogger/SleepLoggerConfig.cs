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
                Samplerate = (int)ParseNumber(config["AD2:Samplerate"]),
                SignalGeneratorChannel = config["AD2:SignalGeneratorChannel"] == "W1" ? 0 : config["AD2:SignalGeneratorChannel"] == "W2" ? 1 : 255,
                SignalGeneratorHz = ParseNumber(config["AD2:SignalGeneratorHz"]),
                SignalGeneratorVolt = ParseNumber(config["AD2:SignalGeneratorVolt"]),
            };

            Postprocessing = new PostprocessingConfig()
            {
                Enabled = Boolean.Parse(config["Postprocessing:Enabled"]),
                IntervalSeconds = ParseNumber(config["Postprocessing:IntervalSeconds"]),
                FFTSize = (int)ParseNumber(config["Postprocessing:FFTSize"], true),
                MagnitudeThreshold = ParseNumber(config["Postprocessing:MagnitudeThreshold"]),
                SaveAsWAV = Boolean.Parse(config["Postprocessing:SaveAsWAV"]),
                SaveAsFFT = Boolean.Parse(config["Postprocessing:SaveAsFFT"]),
                SaveAsCompressedFFT = Boolean.Parse(config["Postprocessing:SaveAsCompressedFFT"]),
                SaveAsPng = new SaveAsPngConfig()
                {
                    Enabled = Boolean.Parse(config["Postprocessing:SaveAsPNG:Enabled"]),
                    TargetWidth = Int32.Parse(config["Postprocessing:SaveAsPNG:TargetWidth"]),
                    TargetHeight = Int32.Parse(config["Postprocessing:SaveAsPNG:TargetHeight"]),
                    RangeVolt = ParseNumber(config["Postprocessing:SaveAsPNG:RangeVolt"]),
                    RowWidthStepsSamples = Int32.Parse(config["Postprocessing:SaveAsPNG:RowWidthStepsSamples"]),
                    RowHeightPixels = Int32.Parse(config["Postprocessing:SaveAsPNG:RowHeightPixels"]),
                },
            };
        }

        private float ParseNumber(string str, bool mode = false)
        {
            return str.EndsWith("k") ? Single.Parse(str[0..^1]) * (mode ? 1024 : 1000) : Single.Parse(str);
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
        public bool Enabled { get; set; }
        public float IntervalSeconds { get; set; }
        /// <summary>
        /// FFT size
        /// </summary>
        public int FFTSize { get; set; }
        public float MagnitudeThreshold { get; set; }
        public bool SaveAsWAV { get; set; }
        public bool SaveAsFFT { get; set; }
        public bool SaveAsCompressedFFT { get; set; }
        public SaveAsPngConfig SaveAsPng { get; set; }
    }

    public class SaveAsPngConfig
    {
        public bool Enabled { get; set; }
        public int TargetWidth { get; set; }
        public int TargetHeight { get; set; }
        public float RangeVolt { get; set; }
        public int RowWidthStepsSamples { get; set; }
        public int RowHeightPixels { get; set; }      
    }

}
