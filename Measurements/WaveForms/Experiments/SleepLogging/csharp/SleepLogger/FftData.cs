using NWaves.Signals;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json;
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

        internal static void SaveAs(FftData fftData, string pathToFile, bool compress)
        {
            pathToFile = pathToFile.Substring(0, pathToFile.Length - Path.GetExtension(pathToFile).Length == 0 ? 0 : Path.GetExtension(pathToFile).Length + 1);
            string filename = Path.GetFileNameWithoutExtension(pathToFile);
            string fftDataJson = JsonSerializer.Serialize(fftData);

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
