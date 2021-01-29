using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BBD.SleepLogger
{
    public class SleepLoggerConfig
    {
        public string DataDirectory { get; }
        public long MinimumAvailableFreeSpace { get; }
        public AD2Config AD2 { get; }
        public PostprocessingConfig Postprocessing { get; }
        public AudioRecordingConfig AudioRecording { get; }

        public SleepLoggerConfig(IConfigurationRoot config)
        {
            DataDirectory = config["DataDirectory"];
            MinimumAvailableFreeSpace = (long)ParseNumber(config["MinimumAvailableFreeSpace"]);

            AD2 = new AD2Config()
            {
                Samplerate = (int)ParseNumber(config["AD2:Samplerate"]),
                SignalGenerator = new SignalGeneratorConfig()
                {
                    Enabled = Boolean.Parse(config["AD2:SignalGenerator:Enabled"]),
                    Channel = config["AD2:SignalGenerator:Channel"] == "W1" ? 0 : config["AD2:SignalGenerator:Channel"] == "W2" ? 1 : 255,
                    Frequency = ParseNumber(config["AD2:SignalGenerator:Frequency"]),
                    Voltage = ParseNumber(config["AD2:SignalGenerator:Voltage"]),
                }
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
                SaveAsPNG = new SaveAsPngConfig()
                {
                    Enabled = Boolean.Parse(config["Postprocessing:SaveAsPNG:Enabled"]),
                    TargetWidth = Int32.Parse(config["Postprocessing:SaveAsPNG:TargetWidth"]),
                    TargetHeight = Int32.Parse(config["Postprocessing:SaveAsPNG:TargetHeight"]),
                    RangeX = (int)ParseNumber(config["Postprocessing:SaveAsPNG:RangeX"]),
                    RangeY = ParseNumber(config["Postprocessing:SaveAsPNG:RangeY"]),
                    RowWidthStepsSamples = (int)ParseNumber(config["Postprocessing:SaveAsPNG:RowWidthStepsSamples"]),
                    RowHeightPixels = Int32.Parse(config["Postprocessing:SaveAsPNG:RowHeightPixels"]),
                },
            };

            AudioRecording = new AudioRecordingConfig()
            {
                Enabled = Boolean.Parse(config["AudioRecording:Enabled"]),
                PreferredDevice = config["AudioRecording:PreferredDevice"],
                OutputFormat = config["AudioRecording:OutputFormat"],
                SilenceThreshold = config["AudioRecording:SilenceThreshold"]
            };
        }

        private float ParseNumber(string str, bool mode = false)
        {
            string[] postfixes = { "p", "n", "u", "m", "", "k", "M", "T", "P" };

            int numberIndex = 1;
            float numberPart = 0;
            while ((numberIndex <= str.Length) && (Single.TryParse(str.Substring(0, numberIndex), out float parsedNumberPart)))
            {
                numberIndex++;
                numberPart = parsedNumberPart;
            }

            string textPart = str.Substring(numberIndex - 1).Trim();

            int postfixIndex = 4;
            for (int i = 0; i < postfixes.Length; i++)
            {
                if ((postfixes[i] != "") && (textPart.StartsWith(postfixes[i])))
                {
                    postfixIndex = i;
                    break;
                }
            }

            while (postfixIndex < 4)
            {
                numberPart /= (mode ? 1024 : 1000);
                postfixIndex++;
            }

            while (postfixIndex > 4)
            {
                numberPart *= (mode ? 1024 : 1000);
                postfixIndex--;
            }

            return numberPart;
        }
    }

    public class AD2Config
    {
        /// <summary>
        /// Number of samples per second
        /// </summary>
        public int Samplerate { get; set; }
        public SignalGeneratorConfig SignalGenerator { get; set; }
    }

    public class SignalGeneratorConfig
    {
        public bool Enabled { get; set; }
        /// <summary>
        /// Signal generation channel (W1 or W2)
        /// </summary>
        public byte Channel { get; set; }
        /// <summary>
        /// Generated signal frequency
        /// </summary>
        public float Frequency { get; set; }
        /// <summary>
        /// Peak amplitude of the generated signal
        /// </summary>
        public float Voltage { get; set; }
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
        public SaveAsPngConfig SaveAsPNG { get; set; }
    }

    public class SaveAsPngConfig
    {
        public bool Enabled { get; set; }
        public int TargetWidth { get; set; }
        public int TargetHeight { get; set; }
        public float RangeY { get; set; }
        public int RangeX { get; set; }
        public int RowWidthStepsSamples { get; set; }
        public int RowHeightPixels { get; set; }      
    }

    public class AudioRecordingConfig
    {
        public bool Enabled { get; set; }
        public string PreferredDevice { get; set; }
        public string OutputFormat { get; set; }
        public string SilenceThreshold { get; set; }
    }

}
