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

        private float[] _magnitudesPerHz;
        public float[] MagnitudesPerHz 
        { 
            get
            {
                if (this.FirstFrequency > 0)
                {
                    throw new Exception("The FirstFrequency must be 0 if you want to use the MagnitudesPerHz property.");
                }

                if (_magnitudesPerHz == null)
                {
                    int maxFrequency = (int)Math.Ceiling(this.LastFrequency);

                    _magnitudesPerHz = new float[maxFrequency + 1];

                    _magnitudesPerHz[0] = MagnitudeData[0];
                    _magnitudesPerHz[^1] = MagnitudeData[^1];
                    int nextDataIndex = 1;
                    for (int i = 1; i < maxFrequency; i++)
                    {
                        while (this.FrequencyStep * nextDataIndex < i)
                        {
                            nextDataIndex++;
                        }

                        float distanceFromPrevious = i - (this.FrequencyStep * (nextDataIndex - 1));
                        float distanceFromNext = (this.FrequencyStep * nextDataIndex) - i;
                        _magnitudesPerHz[i] = (distanceFromNext * this.MagnitudeData[nextDataIndex - 1] + distanceFromPrevious * this.MagnitudeData[nextDataIndex]) / (distanceFromPrevious + distanceFromNext);
                    }
                }

                return _magnitudesPerHz;
            }
        }

        public FftData() { }

        internal static void SaveAs(FftData fftData, string pathToFile, bool compress)
        {
            pathToFile = pathToFile.Substring(0, pathToFile.Length - Path.GetExtension(pathToFile).Length);
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
