using NWaves.Audio;
using NWaves.Operations;
using NWaves.Signals;
using NWaves.Transforms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SleepLogger
{
    class Program
    {
        /// <summary>
        /// FFT size
        /// </summary>
        const int fftSize = 8192 * 8 * 8;

        /// <summary>
        /// Number of samples per buffer
        /// </summary>
        static int bufferSize;
        /// <summary>
        /// Number of samples per second
        /// </summary>
        const int samplerate = fftSize;
        /// <summary>
        /// Audio signal channel
        /// </summary>
        const int signalGeneratorChannel = 1;       //W2
        /// <summary>
        /// Default audio signal frequency
        /// </summary>
        const double signalGeneratorFrequency = 36;
        /// <summary>
        /// Audio signal amplitude
        /// </summary>
        const double signalGeneratorAmplitude = 0.95;                   // W2 oscillated between -0.95V and +0.95V

        static float inputAmplification = short.MaxValue / 1.0f;        // WAV file ranges from -1000 mV to +1000 mV

        static bool terminateAcquisition = false;

        static float processingFrequency = 5.0f;                        // process it every 5.0 seconds

        static bool saveAsWav = false;
        static bool saveAsFft = true;
        static bool saveAsPng = true;

        static List<float> samples = new List<float>();
        static double[] voltData;

        static void Main(string[] args)
        {
            dwf.FDwfGetVersion(out string dwfVersion);
            Console.WriteLine($"DWF Version: {dwfVersion}");

            Console.WriteLine("Opening first device");
            dwf.FDwfDeviceOpen(-1, out int dwfHandle);

            if (dwfHandle == dwf.hdwfNone)
            {
                dwf.FDwfGetLastErrorMsg(out string lastError);
                Console.WriteLine($"Failed to open device: {lastError}");
                return;
            }

            dwf.FDwfAnalogInBufferSizeInfo(dwfHandle, out int bufferSizeMinimum, out int bufferSizeMaximum);
            bufferSize = Math.Min(bufferSizeMaximum, samplerate);
            Console.WriteLine($"Device buffer size range: {bufferSizeMinimum} - {bufferSizeMaximum} samples, set to {bufferSize}.");
            voltData = new double[bufferSize];

            //set up acquisition
            dwf.FDwfAnalogInFrequencySet(dwfHandle, samplerate);
            dwf.FDwfAnalogInBufferSizeSet(dwfHandle, bufferSize);
            dwf.FDwfAnalogInChannelEnableSet(dwfHandle, 0, 1);
            dwf.FDwfAnalogInChannelRangeSet(dwfHandle, 0, 5.0);
            dwf.FDwfAnalogInAcquisitionModeSet(dwfHandle, dwf.acqmodeRecord);
            dwf.FDwfAnalogInRecordLengthSet(dwfHandle, -1);

            // set up signal generation
            dwf.FDwfAnalogOutNodeEnableSet(dwfHandle, signalGeneratorChannel, dwf.AnalogOutNodeCarrier, 1);
            dwf.FDwfAnalogOutNodeFunctionSet(dwfHandle, signalGeneratorChannel, dwf.AnalogOutNodeCarrier, dwf.funcSine);
            dwf.FDwfAnalogOutNodeFrequencySet(dwfHandle, signalGeneratorChannel, dwf.AnalogOutNodeCarrier, signalGeneratorFrequency);
            dwf.FDwfAnalogOutNodeAmplitudeSet(dwfHandle, signalGeneratorChannel, dwf.AnalogOutNodeCarrier, signalGeneratorAmplitude);

            Console.WriteLine($"Generating sine wave @{signalGeneratorFrequency}Hz...");
            dwf.FDwfAnalogOutConfigure(dwfHandle, signalGeneratorChannel, 1);

            //wait at least 2 seconds for the offset to stabilize
            Thread.Sleep(2000);

            //start aquisition
            Console.WriteLine("Starting oscilloscope");
            dwf.FDwfAnalogInConfigure(dwfHandle, 0, 1);

            Console.WriteLine($"Recording data @{samplerate}Hz, press Ctrl+C to stop...");

            Console.CancelKeyPress += Console_CancelKeyPress;

            int cSamples = 0;
            int bufferIndex = 1;
            bool skipBuffer = false;
            while (!terminateAcquisition)
            {
                while (true)
                {
                    dwf.FDwfAnalogInStatus(dwfHandle, 1, out byte sts);

                    if (!((cSamples == 0) && ((sts == dwf.DwfStateConfig) || (sts == dwf.DwfStatePrefill) || (sts == dwf.DwfStateArmed))))
                        break;

                    Thread.Sleep(50);
                }

                dwf.FDwfAnalogInStatusRecord(dwfHandle, out int cAvailable, out int cLost, out int cCorrupted);
                if (cAvailable == 0) continue;
                if ((cLost > 0) || (cCorrupted > 0))
                {
                    Console.WriteLine($"Data error! lost:{cLost}, corrupted:{cCorrupted}");
                    skipBuffer = true;
                }

                dwf.FDwfAnalogInStatusData(dwfHandle, 0, voltData, cAvailable);     //get channel 1 data chunk
                cSamples += cAvailable;

                samples.AddRange(voltData.Take(cAvailable).Select(vd => (float)vd));

                if (samples.Count % (samplerate / cAvailable) == 0)
                {
                    //Console.Write(".");
                }

                if (samples.Count >= samplerate * processingFrequency)
                {
                    //generate a signal
                    var signal = new DiscreteSignal(samplerate, samples.ToArray(), true);
                    samples.Clear();

                    bufferIndex++;
                    if (!skipBuffer)
                    {
                        Task.Run(() =>
                        {
                            int bi = bufferIndex;
                            DateTime now = DateTime.Now;

                            Stopwatch sw = Stopwatch.StartNew();
                            string filename = $"{now.ToString("yyyy-MM-dd")}\\AD2_{now.ToString("yyyy-MM-dd_HHmmss")}";
                            if (!Directory.Exists(now.ToString("yyyy-MM-dd")))
                            {
                                Directory.CreateDirectory(now.ToString("yyyy-MM-dd"));
                            }

                            if (saveAsWav)
                            {
                                sw.Restart();
                                //and save samples to a WAV file
                                FileStream waveFileStream = new FileStream($"{filename}.wav", FileMode.Create);

                                signal.Amplify(inputAmplification);
                                WaveFile waveFile = new WaveFile(signal, 16);
                                waveFile.SaveTo(waveFileStream, false);

                                waveFileStream.Close();

                                sw.Stop();
                                Console.WriteLine($"#{bi.ToString("0000")} Save as WAV completed in {sw.ElapsedMilliseconds} ms.");
                            }

                            sw.Restart();
                            int targetSamplerate = fftSize;
                            int sampleRateMultiplier = 1;

                            while (targetSamplerate < signal.SamplingRate)
                            {
                                targetSamplerate *= 2;
                                sampleRateMultiplier *= 2;
                            }
                            float[] concatenatedPowerSpectrum = new float[targetSamplerate / 2 + 1];

                            var resampler = new Resampler();
                            if (targetSamplerate != signal.SamplingRate)
                            {
                                signal = resampler.Resample(signal, targetSamplerate);
                            }

                            while (signal.SamplingRate >= fftSize)
                            {
                                var fft = new RealFft(fftSize);
                                var partialPowerSpectrum = fft.MagnitudeSpectrum(signal, normalize: false);

                                if (concatenatedPowerSpectrum[0] == 0)
                                {
                                    concatenatedPowerSpectrum[0] = partialPowerSpectrum[0];
                                }

                                for (int i = 1; i < partialPowerSpectrum.Length; i++)
                                {
                                    for (int j = 0; j < sampleRateMultiplier; j++)
                                    {
                                        concatenatedPowerSpectrum[(i - 1) * sampleRateMultiplier + j] = partialPowerSpectrum[i];
                                    }
                                }

                                signal = new DiscreteSignal(signal.SamplingRate / 2, signal.Samples.Where((value, index) => index % 2 == 0).ToArray());
                                sampleRateMultiplier /= 2;
                            }
                            sw.Stop();
                            Console.WriteLine($"#{bi.ToString("0000")} Signal processing completed in {sw.ElapsedMilliseconds} ms.");

                            var powerSpectrum = new DiscreteSignal((int)(targetSamplerate / 2 / processingFrequency), concatenatedPowerSpectrum);
                            if (saveAsFft)
                            {
                                sw.Restart();
                                //save it the FFT to a JSON file
                                File.WriteAllTextAsync($"{filename}.fft", JsonSerializer.Serialize(powerSpectrum));
                                sw.Stop();
                                Console.WriteLine($"#{bi.ToString("0000")} Save as FFT completed in {sw.ElapsedMilliseconds} ms.");
                            }

                            if (saveAsPng)
                            {
                                sw.Restart();
                                //save a PNG with the values
                                SaveSignalAsPng($"{filename}.png", powerSpectrum, 200000);
                                sw.Stop();
                                Console.WriteLine($"#{bi.ToString("0000")} Save as PNG completed in {sw.ElapsedMilliseconds} ms.");
                            }
                        }
                        );
                    }
                    skipBuffer = false;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Acquisition done");

            dwf.FDwfDeviceCloseAll();
        }

        private static void SaveSignalAsPng(string filename, DiscreteSignal signal, int maxSamples, int height = 1080)
        {
            const int maxPngWidth = 20000;
            const float resoltionScaleDownFactor = 15.0f;
            Font font = new Font("Georgia", 150.0f);

            // convert samples into log scale (dBm)
            //var samples = signal.Samples.Select(s => Scale.ToDecibel(s)).ToArray();
            var samples = signal.Samples.Take(maxSamples).ToArray();
            int width = Math.Min(maxPngWidth, samples.Length);
            int dataRows = Math.Max(1, (int)Math.Ceiling((double)samples.Length / maxPngWidth));

            int originalSampleCount = samples.Length;
            int onePercentCount = originalSampleCount / 100;
            double[] filteredSamples = new double[originalSampleCount - 1];
            //skip the first value, its the total power
            Array.Copy(samples, 1, filteredSamples, 0, originalSampleCount - 1);
            Array.Sort(filteredSamples);
            //discard the top 1% and bottom 1% to calculate the scale
            //filteredSamples = filteredSamples.Take(originalSampleCount - onePercentCount).TakeLast(originalSampleCount - onePercentCount - onePercentCount).ToArray();

            //double scale = height / (filteredSamples.Max() - filteredSamples.Min());
            //double scale = 4.0;             //this is good enough for the 2V peak-to-peak signal detection
            //double valueTreshold = -128;    //typical received signal power from a GPS satellite
            
            double scale = 100.0;          // for non-normalized
            //double scale = 2500000.0;     // for normalized
            double valueTreshold = 0;


            Color bgColor = Color.LightGray;
            Brush lineColor = new SolidBrush(Color.DarkGray);

            Bitmap spectrumBitmap = new Bitmap(width, height * dataRows, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Graphics graphics = Graphics.FromImage(spectrumBitmap);
            graphics.FillRectangle(new SolidBrush(bgColor), new Rectangle(0, 0, width, height * dataRows));

            var pen = new Pen(lineColor, 1);

            for (int r = 1; r <= dataRows; r++)
            {
                int bottomLine = r * height;

                for (int i = 0; i < width; i++)
                {
                    int dataPointIndex = (r - 1) * width + i;
                    if ((dataPointIndex >= samples.Length) || (dataPointIndex >= maxSamples)) continue;

                    if (samples[dataPointIndex] - valueTreshold > 0)
                    {
                        int valueToShow = (int)Math.Min((samples[dataPointIndex] - valueTreshold) * scale, height);

                        graphics.DrawLine(pen, new Point(i, bottomLine), new Point(i, bottomLine - valueToShow));
                    }
                }

                graphics.DrawString($"{((r - 1) * width) / 1000} kHz", font, Brushes.White, new PointF(100.0f, bottomLine - (height * 0.75f)));
                graphics.DrawString($"{(r * width) / 1000} kHz", font, Brushes.White, new PointF(width - 800.0f, bottomLine - (height * 0.75f)));
            }

            graphics.DrawString($"{filename}", font, Brushes.White, new PointF(width - 4000.0f, 10.0f));

            var scaledDownBitmap = new Bitmap(spectrumBitmap, new Size((int)(spectrumBitmap.Width / resoltionScaleDownFactor), (int)(spectrumBitmap.Height / resoltionScaleDownFactor)));

            FileStream pngFile = new FileStream(filename, FileMode.Create);
            scaledDownBitmap.Save(pngFile, System.Drawing.Imaging.ImageFormat.Png);
            pngFile.Close();
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            terminateAcquisition = true;
            e.Cancel = true;
        }
    }
}
