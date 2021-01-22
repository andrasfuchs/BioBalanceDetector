using NWaves.Operations;
using NWaves.Signals;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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
        private float[] _magnitudeData { get; set; }
        public float[] MagnitudeData {
            get
            {
                return _magnitudeData;
            }
            set
            {
                if (_magnitudeData != null)
                {
                    throw new Exception("MagnitudeData can be set only once and it was already yet before.");
                }

                _magnitudeData = value;
            }
        }        

        public FftData Resample(float frequencyStep)
        {
            if (this.FirstFrequency > 0)
            {
                throw new Exception("The FirstFrequency must be 0 if you want to use the MagnitudesPerHz property.");
            }

            if (this.FrequencyStep > frequencyStep)
            {
                throw new Exception("The FrequencyStep of the FFT dataset must be smallar then the frequency step that we resample to.");
            }

            int maxFrequency = (int)Math.Ceiling(this.LastFrequency);

            var recalculatedValues = new List<float>();

            int i = 0;
            while (this.FrequencyStep * i < this.LastFrequency)
            {
                i++;
                if (frequencyStep * recalculatedValues.Count < i * this.FrequencyStep)
                {
                    recalculatedValues.Add(0);
                }

                recalculatedValues[^1] += _magnitudeData[i];
            }

            return new FftData()
            {
                CaptureTime = this.CaptureTime,
                Duration = this.Duration,
                FirstFrequency = this.FirstFrequency,
                LastFrequency = frequencyStep * recalculatedValues.Count,
                FrequencyStep = frequencyStep,
                FftSize = recalculatedValues.Count,
                MagnitudeData = recalculatedValues.ToArray()
            };
        }

        public FftData() { }

        internal static void SaveAs(FftData fftData, string pathToFile, bool compress)
        {
            pathToFile = pathToFile.Substring(0, pathToFile.Length - Path.GetExtension(pathToFile).Length);
            string filename = Path.GetFileNameWithoutExtension(pathToFile);
            string fftDataJson = JsonSerializer.Serialize(fftData, new JsonSerializerOptions() { WriteIndented = true });

            if (compress)
            {
                using (FileStream zipToOpen = new FileStream($"{pathToFile}.zip", FileMode.Create))
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create))
                    {
                        ZipArchiveEntry fftFileEntry = archive.CreateEntry($"{filename}.fft");
                        using (StreamWriter writer = new StreamWriter(fftFileEntry.Open()))
                        {
                            writer.Write(fftDataJson);
                        }
                    }
                }
            } else
            {
                File.WriteAllTextAsync($"{pathToFile}.fft", fftDataJson);
            }
        }

        internal static FftData LoadFrom(string pathToFile)
        {
            pathToFile = pathToFile.Substring(0, pathToFile.Length - Path.GetExtension(pathToFile).Length);
            string filename = Path.GetFileNameWithoutExtension(pathToFile);
            FftData fftData = null;

            if (File.Exists($"{pathToFile}.fft"))
            {
                fftData = JsonSerializer.Deserialize<FftData>(File.ReadAllText($"{pathToFile}.fft"));
            }
            
            if ((fftData == null) && (File.Exists($"{pathToFile}.zip")))
            {
                using (FileStream zipToOpen = new FileStream($"{pathToFile}.zip", FileMode.Open))
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
                    {                        
                        ZipArchiveEntry fftFileEntry = archive.GetEntry($"{filename}.fft");
                        using (StreamReader reader = new StreamReader(fftFileEntry.Open()))
                        {                            
                            fftData = JsonSerializer.Deserialize<FftData>(reader.ReadToEnd());
                        }
                    }
                }
            }

            if (fftData == null)
            {
                throw new Exception($"Could not open the .fft or .zip file for '{pathToFile}'.");
            }

            return fftData;
        }
    }
}
