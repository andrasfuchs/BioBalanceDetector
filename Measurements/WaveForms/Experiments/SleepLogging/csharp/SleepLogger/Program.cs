using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        /// Number of samples per buffer
        /// </summary>
        static int bufferSize;

        static float inputAmplification = short.MaxValue / 1.0f;        // WAV file ranges from -1000 mV to +1000 mV

        static bool terminateAcquisition = false;

        static List<float> samples = new List<float>();
        static double[] voltData;

        static SleepLoggerConfig config;

        static void Main(string[] args)
        {
            // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", false)
                .Build();        

            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("SleepLogger.Program", LogLevel.Debug)
                    .AddConsole();
            });
            ILogger logger = loggerFactory.CreateLogger<Program>();


            logger.LogInformation("Bio Balance Detector Sleep Logger v0.2 (2021-01-12)");

            try
            {
                config = new SleepLoggerConfig(configuration);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return;
            }

            dwf.FDwfGetVersion(out string dwfVersion);
            logger.LogInformation($"DWF Version: {dwfVersion}");

            logger.LogInformation("Opening first device");
            dwf.FDwfDeviceOpen(-1, out int dwfHandle);

            while (dwfHandle == dwf.hdwfNone)
            {
                dwf.FDwfGetLastErrorMsg(out string lastError);
                logger.LogInformation($"Failed to open device: {lastError.TrimEnd()}. Retrying in 30 seconds.");

                Thread.Sleep(30000);

                dwf.FDwfDeviceOpen(-1, out dwfHandle);
            }

            dwf.FDwfAnalogInBufferSizeInfo(dwfHandle, out int bufferSizeMinimum, out int bufferSizeMaximum);
            bufferSize = Math.Min(bufferSizeMaximum, config.AD2.Samplerate);
            logger.LogInformation($"Device buffer size range: {bufferSizeMinimum} - {bufferSizeMaximum} samples, set to {bufferSize}.");
            voltData = new double[bufferSize];

            //set up acquisition
            dwf.FDwfAnalogInFrequencySet(dwfHandle, config.AD2.Samplerate);
            dwf.FDwfAnalogInBufferSizeSet(dwfHandle, bufferSize);
            dwf.FDwfAnalogInChannelEnableSet(dwfHandle, 0, 1);
            dwf.FDwfAnalogInChannelRangeSet(dwfHandle, 0, 5.0);
            dwf.FDwfAnalogInAcquisitionModeSet(dwfHandle, dwf.acqmodeRecord);
            dwf.FDwfAnalogInRecordLengthSet(dwfHandle, -1);

            // set up signal generation
            dwf.FDwfAnalogOutNodeEnableSet(dwfHandle, config.AD2.SignalGeneratorChannel, dwf.AnalogOutNodeCarrier, 1);
            dwf.FDwfAnalogOutNodeFunctionSet(dwfHandle, config.AD2.SignalGeneratorChannel, dwf.AnalogOutNodeCarrier, dwf.funcSine);
            dwf.FDwfAnalogOutNodeFrequencySet(dwfHandle, config.AD2.SignalGeneratorChannel, dwf.AnalogOutNodeCarrier, config.AD2.SignalGeneratorHz);
            dwf.FDwfAnalogOutNodeAmplitudeSet(dwfHandle, config.AD2.SignalGeneratorChannel, dwf.AnalogOutNodeCarrier, config.AD2.SignalGeneratorVolt);

            logger.LogInformation($"Generating sine wave @{config.AD2.SignalGeneratorHz} Hz...");
            dwf.FDwfAnalogOutConfigure(dwfHandle, config.AD2.SignalGeneratorChannel, 1);

            //wait at least 2 seconds for the offset to stabilize
            Thread.Sleep(2000);

            //start aquisition
            logger.LogInformation("Starting oscilloscope");
            dwf.FDwfAnalogInConfigure(dwfHandle, 0, 1);

            logger.LogInformation($"Recording data @{config.AD2.Samplerate} Hz, press Ctrl+C to stop...");

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
                    logger.LogInformation($"Data error! lost:{cLost}, corrupted:{cCorrupted}");
                    skipBuffer = true;
                }

                dwf.FDwfAnalogInStatusData(dwfHandle, 0, voltData, cAvailable);     //get channel 1 data chunk
                cSamples += cAvailable;

                samples.AddRange(voltData.Take(cAvailable).Select(vd => (float)vd));

                if (samples.Count % (config.AD2.Samplerate / cAvailable) == 0)
                {
                    //Console.Write(".");
                }

                if (samples.Count >= config.AD2.Samplerate * config.Postprocessing.IntervalSeconds)
                {
                    //generate a signal
                    var signal = new DiscreteSignal(config.AD2.Samplerate, samples.ToArray(), true);
                    samples.Clear();

                    bufferIndex++;
                    if (!skipBuffer)
                    {
                        Task.Run(() =>
                        {
                            int bi = bufferIndex;

                            var fftData = new FftData()
                            {
                                CaptureTime = DateTime.Now,
                                Duration = config.Postprocessing.IntervalSeconds,
                                FirstFrequency = 0,
                                LastFrequency = config.AD2.Samplerate / 2,
                                FftSize = config.Postprocessing.FFTSize,
                            };

                            Stopwatch sw = Stopwatch.StartNew();
                            string filename = Path.Combine($"{fftData.CaptureTime.ToString("yyyy-MM-dd")}",$"AD2_{fftData.CaptureTime.ToString("yyyy-MM-dd_HHmmss")}");
                            if (!Directory.Exists(fftData.CaptureTime.ToString("yyyy-MM-dd")))
                            {
                                Directory.CreateDirectory(fftData.CaptureTime.ToString("yyyy-MM-dd"));
                            }

                            if (config.Postprocessing.SaveAsWAV)
                            {
                                sw.Restart();
                                //and save samples to a WAV file
                                FileStream waveFileStream = new FileStream($"{filename}.wav", FileMode.Create);

                                signal.Amplify(inputAmplification);
                                WaveFile waveFile = new WaveFile(signal, 16);
                                waveFile.SaveTo(waveFileStream, false);

                                waveFileStream.Close();

                                sw.Stop();
                                logger.LogInformation($"#{bi.ToString("0000")} Save as WAV completed in {sw.ElapsedMilliseconds} ms.");
                            }

                            sw.Restart();
                            int targetSamplerate = config.Postprocessing.FFTSize;
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

                            while (signal.SamplingRate >= config.Postprocessing.FFTSize)
                            {
                                var fft = new RealFft(config.Postprocessing.FFTSize);
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
                            logger.LogInformation($"#{bi.ToString("0000")} Signal processing completed in {sw.ElapsedMilliseconds} ms.");

                            var powerSpectrum = new DiscreteSignal((int)(targetSamplerate / 2 / config.Postprocessing.IntervalSeconds), concatenatedPowerSpectrum);
                            fftData.FrequencyStep = ((float)config.AD2.Samplerate / 2) / (powerSpectrum.Samples.Length - 1);
                            fftData.MagnitudeData = powerSpectrum.Samples;

                            float averageMagnitude = powerSpectrum.Samples.Take(100).Average();
                            if (averageMagnitude < config.Postprocessing.MagnitudeThreshold)
                            {
                                logger.LogWarning($"#{bi.ToString("0000")} Signal magnitude is too low: {averageMagnitude}");
                                return;
                            }

                            if (config.Postprocessing.SaveAsFFT)
                            {
                                //save it the FFT to a JSON file
                                sw.Restart();
                                File.WriteAllTextAsync($"{filename}.fft", JsonSerializer.Serialize(fftData));
                                sw.Stop();
                                logger.LogInformation($"#{bi.ToString("0000")} Save as FFT completed in {sw.ElapsedMilliseconds} ms.");
                            }

                            if (config.Postprocessing.SaveAsPNG)
                            {
                                //save a PNG with the values
                                sw.Restart();
                                SaveSignalAsPng($"{filename}_200kHz.png", fftData, 200000, 100, 1080, 15);
                                //SaveSignalAsPng($"{filename}_1kHz.png", fftData, 1000, 1, 1080, 1);
                                sw.Stop();
                                logger.LogInformation($"#{bi.ToString("0000")} Save as PNG completed in {sw.ElapsedMilliseconds} ms.");
                            }
                        }
                        );
                    }
                    skipBuffer = false;
                }
            }

            logger.LogInformation("Acquisition done");

            dwf.FDwfDeviceCloseAll();
        }

        private static void SaveSignalAsPng(string filename, FftData fftData, int maxSamples, double sampleAmplification = 100.0, int height = 1080, float resoltionScaleDownFactor = 15.0f)
        {
            const int maxPngWidth = 20000;
            Font font = new Font("Georgia", 10.0f * resoltionScaleDownFactor);

            // convert samples into log scale (dBm)
            //var samples = signal.Samples.Select(s => Scale.ToDecibel(s)).ToArray();
            var samples = fftData.MagnitudeData.Take(maxSamples).ToArray();
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
                        int valueToShow = (int)Math.Min((samples[dataPointIndex] - valueTreshold) * sampleAmplification, height);

                        graphics.DrawLine(pen, new Point(i, bottomLine), new Point(i, bottomLine - valueToShow));
                    }
                }

                if (((r - 1) * width) / 1000 > 0)
                {
                    graphics.DrawString($"{((r - 1) * width) / 1000} kHz", font, Brushes.White, new PointF(100.0f, bottomLine - (height * 0.75f)));
                }
                graphics.DrawString($"{(r * width) / 1000} kHz", font, Brushes.White, new PointF(width - (75.0f * resoltionScaleDownFactor), bottomLine - (height * 0.75f)));
            }

            graphics.DrawString($"{filename}", font, Brushes.White, new PointF(width - (350.0f * resoltionScaleDownFactor), 10.0f));

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
