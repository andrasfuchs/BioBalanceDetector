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
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xabe.FFmpeg;

namespace SleepLogger
{
    class Program
    {
        static string versionString = "v0.4 (2021-01-20)";

        /// <summary>
        /// Number of samples per buffer
        /// </summary>
        static int bufferSize;

        static float inputAmplification = short.MaxValue / 1.0f;        // WAV file ranges from -1000 mV to +1000 mV

        static bool terminateAcquisition = false;

        static List<float> samples = new List<float>();
        static double[] voltData;

        static List<float> maxValues = new List<float>();

        static IConfigurationRoot configuration;
        static SleepLoggerConfig config;

        static void Main(string[] args)
        {
            // Build configuration
            configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", false)
                .Build();

            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", Enum.Parse<LogLevel>(configuration["Logging:LogLevel:Default"]))
                    .AddFilter("System", Enum.Parse<LogLevel>(configuration["Logging:LogLevel:Default"]))
                    .AddFilter("SleepLogger.Program", Enum.Parse<LogLevel>(configuration["Logging:LogLevel:SleepLogger.Program"]))
                    .AddConfiguration(configuration)
                    .AddConsole();
            });
            ILogger logger = loggerFactory.CreateLogger<Program>();

            Console.WriteLine($"Bio Balance Detector Sleep Logger {versionString}");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("--video <FFT data directory>        Generates a PNG images and an MP4 video from FFT data");
            Console.WriteLine();

            logger.LogInformation($"Bio Balance Detector Sleep Logger {versionString}");
            try
            {
                configuration.Reload();
                config = new SleepLoggerConfig(configuration);
            }
            catch (Exception ex)
            {
                logger.LogError($"There was a problem with the configuration file. {ex.Message}");
                return;
            }

            Console.CancelKeyPress += Console_CancelKeyPress;

            if (args.Length > 1)
            {
                if (args[0] == "--video")
                {
                    string foldername = args[1];

                    if (Directory.Exists(foldername))
                    {
                        foreach (string filename in Directory.GetFiles(foldername))
                        {
                            //foldername = Path.GetFullPath(filename).Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)[^2];
                            string pathToFile = Path.GetFullPath(filename);

                            if ((Path.GetExtension(filename) != ".fft") && (Path.GetExtension(filename) != ".zip"))
                            {
                                continue;
                            }

                            try
                            {
                                FftData fftData = FftData.LoadFrom($"{pathToFile}");

                                string filenameWithoutExtension = Path.Combine(foldername, Path.GetFileNameWithoutExtension(pathToFile));
                                string filenameComplete = $"{filenameWithoutExtension}_{SimplifyNumber(config.Postprocessing.SaveAsPNG.RangeX)}Hz_{SimplifyNumber(config.Postprocessing.SaveAsPNG.RangeY)}V.png";

                                if (File.Exists(filenameComplete))
                                {
                                    logger.LogWarning($"{filenameComplete} already exists.");
                                    continue;
                                }

                                SaveSignalAsPng(filenameComplete, fftData, config.Postprocessing.SaveAsPNG);
                                logger.LogInformation($"{filenameComplete} was generated successfully.");
                            }
                            catch (Exception ex)
                            {
                                logger.LogWarning($"There was an error while generating PNG for '{Path.GetFileName(pathToFile)}': {ex.Message}");
                            }
                        }

                        string mp4Filename = $"{foldername}.mp4";
                        logger.LogInformation($"Generating MP4 video file '{mp4Filename}'");
                        try
                        {
                            FFmpeg.Conversions.New()
                                .SetInputFrameRate(12)
                                .BuildVideoFromImages(Directory.GetFiles(foldername, "*.png").OrderBy(fn => fn))
                                .SetFrameRate(12)
                                .SetPixelFormat(PixelFormat.yuv420p)
                                .SetOutput(mp4Filename)
                                .Start()
                                .Wait();
                        }
                        catch (Exception ex)
                        {
                            logger.LogError($"There was an error while generating MP4 video file '{mp4Filename}': {ex.Message}");
                        }
                    }
                    return;
                }
            }

            dwf.FDwfGetVersion(out string dwfVersion);
            logger.LogInformation($"DWF Version: {dwfVersion}");

            int dwfHandle = InitializeAD2(logger, config);

            int cSamples = 0;
            int bufferIndex = 1;
            bool skipBuffer = false;

            while (!terminateAcquisition)
            {
                while (true)
                {
                    logger.LogTrace($"FDwfAnalogInStatus begin | dwfHandle:{dwfHandle}");
                    dwf.FDwfAnalogInStatus(dwfHandle, 1, out byte sts);
                    logger.LogTrace($"FDwfAnalogInStatus end   | sts:{sts}");

                    if (!((cSamples == 0) && ((sts == dwf.DwfStateConfig) || (sts == dwf.DwfStatePrefill) || (sts == dwf.DwfStateArmed))))
                        break;

                    logger.LogWarning($"We got into an unusual state! sts:{sts}");
                    Thread.Sleep(500);
                }

                logger.LogTrace($"FDwfAnalogInStatusRecord begin | dwfHandle:{dwfHandle}");
                dwf.FDwfAnalogInStatusRecord(dwfHandle, out int cAvailable, out int cLost, out int cCorrupted);
                logger.LogTrace($"FDwfAnalogInStatusRecord end   | cAvailable:{cAvailable:N0}, cLost:{cLost:N0}, cCorrupted:{cCorrupted:N0}");

                if (cAvailable == 0)
                {
                    logger.LogWarning($"Aqusition error! cAvailable: {cAvailable:N0}");
                    Thread.Sleep(500);
                    logger.LogInformation($"Reseting device...");
                    dwfHandle = InitializeAD2(logger, config);
                    continue;
                }

                if ((cLost > 0) || (cCorrupted > 0))
                {
                    logger.LogInformation($"Data error! cLost:{cLost:N0}, cCorrupted:{cCorrupted:N0}");
                    skipBuffer = true;
                }

                logger.LogTrace($"FDwfAnalogInStatusData begin | dwfHandle:{dwfHandle}, cAvailable:{cAvailable:N0}");
                dwf.FDwfAnalogInStatusData(dwfHandle, 0, voltData, cAvailable);     // get channel 1 data chunk
                logger.LogTrace($"FDwfAnalogInStatusData end   | voltData.Count:{voltData.Count():N0}");

                cSamples += cAvailable;

                if (config.Postprocessing.Enabled)
                {
                    samples.AddRange(voltData.Take(cAvailable).Select(vd => (float)vd));

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

                                logger.LogTrace($"Postprocessing thread #{bi:N0} begin");

                                var fftData = new FftData()
                                {
                                    CaptureTime = DateTime.Now,
                                    Duration = config.Postprocessing.IntervalSeconds,
                                    FirstFrequency = 0,
                                    LastFrequency = config.AD2.Samplerate / 2,
                                    FftSize = config.Postprocessing.FFTSize,
                                };

                                Stopwatch sw = Stopwatch.StartNew();
                                string foldername = $"{fftData.CaptureTime.ToString("yyyy-MM-dd")}";
                                string filename = $"AD2_{fftData.CaptureTime.ToString("yyyyMMdd_HHmmss")}";
                                string pathToFile = Path.Combine(foldername, filename);
                                if (!Directory.Exists(fftData.CaptureTime.ToString("yyyy-MM-dd")))
                                {
                                    Directory.CreateDirectory(fftData.CaptureTime.ToString("yyyy-MM-dd"));
                                }

                                if (config.Postprocessing.SaveAsWAV)
                                {
                                    sw.Restart();
                                    //and save samples to a WAV file
                                    FileStream waveFileStream = new FileStream($"{pathToFile}.wav", FileMode.Create);
                                    DiscreteSignal signalToSave = new DiscreteSignal(signal.SamplingRate, signal.Samples, true);
                                    signalToSave.Amplify(inputAmplification);
                                    WaveFile waveFile = new WaveFile(signalToSave, 16);
                                    waveFile.SaveTo(waveFileStream, false);

                                    waveFileStream.Close();

                                    sw.Stop();
                                    logger.LogInformation($"#{bi.ToString("0000")} Save as WAV completed in {sw.ElapsedMilliseconds:N0} ms.");
                                }

                                sw.Restart();
                                var fft = new RealFft(config.Postprocessing.FFTSize);
                                try
                                {
                                    fftData.MagnitudeData = fft.MagnitudeSpectrum(signal, normalize: true).Samples;
                                }
                                catch (IndexOutOfRangeException)
                                {
                                    logger.LogError($"The FFT size of {config.Postprocessing.FFTSize:N0} is too high for the sample rate of {config.AD2.Samplerate:N0}. Decrease the FFT size or increase the sampling rate.");
                                    terminateAcquisition = true;
                                    return;
                                }
                                // clear the 0th coefficient (DC component)
                                fftData.MagnitudeData[0] = 0;
                                sw.Stop();
                                logger.LogInformation($"#{bi.ToString("0000")} Signal processing completed in {sw.ElapsedMilliseconds:N0} ms.");

                                fftData.FrequencyStep = ((float)config.AD2.Samplerate / 2) / (fftData.MagnitudeData.Length - 1);

                                maxValues.Add(fftData.MagnitudeData.Max());
                                int maxIndex = Array.FindIndex(fftData.MagnitudeData, d => d == maxValues[maxValues.Count - 1]);
                                logger.LogInformation($"#{bi.ToString("0000")} The maximum magnitude is {maxValues[maxValues.Count - 1] * 1000 * 1000:N} µV at the index of #{maxIndex:N0} ({maxIndex * fftData.FrequencyStep:N} Hz).");
                                if (config.AD2.SignalGenerator.Enabled)
                                {
                                    logger.LogInformation($"#{bi.ToString("0000")} The magnitude at {config.AD2.SignalGenerator.Frequency} Hz is {fftData.MagnitudesPerHz[(int)config.AD2.SignalGenerator.Frequency] * 1000 * 1000:N} µV.");
                                }

                                if (config.Postprocessing.MagnitudeThreshold > 0)
                                {
                                    float averageMagnitude = fftData.MagnitudesPerHz[100..1000].Average();
                                    if (averageMagnitude < config.Postprocessing.MagnitudeThreshold)
                                    {
                                        logger.LogWarning($"#{bi.ToString("0000")} The average magnitude in the 100-1000 Hz range is too low: {averageMagnitude * 1000 * 1000:N0} µV. The threashold is {config.Postprocessing.MagnitudeThreshold * 1000 * 1000:N0} µV.");
                                        return;
                                    }
                                }

                                if (config.Postprocessing.SaveAsFFT)
                                {
                                    Task.Run(() =>
                                    {
                                        Stopwatch sw = Stopwatch.StartNew();

                                        //save it the FFT to a JSON file
                                        sw.Restart();
                                        FftData.SaveAs(fftData, $"{pathToFile}.fft", false);
                                        sw.Stop();
                                        logger.LogInformation($"#{bi.ToString("0000")} Save as FFT completed in {sw.ElapsedMilliseconds:N0} ms.");
                                    });
                                }

                                if (config.Postprocessing.SaveAsCompressedFFT)
                                {
                                    Task.Run(() =>
                                    {
                                        Stopwatch sw = Stopwatch.StartNew();

                                        //save it the FFT to a zipped JSON file
                                        sw.Restart();
                                        FftData.SaveAs(fftData, $"{pathToFile}.zip", true);
                                        sw.Stop();
                                        logger.LogInformation($"#{bi.ToString("0000")} Save as compressed FFT completed in {sw.ElapsedMilliseconds:N0} ms.");
                                    });
                                }

                                if (config.Postprocessing.SaveAsPNG.Enabled)
                                {
                                    Task.Run(() =>
                                    {
                                        Stopwatch sw = Stopwatch.StartNew();

                                        //save a PNG with the values
                                        sw.Restart();
                                        string filenameComplete = $"{pathToFile}_{SimplifyNumber(config.Postprocessing.SaveAsPNG.RangeX)}Hz_{SimplifyNumber(config.Postprocessing.SaveAsPNG.RangeY)}V.png";
                                        SaveSignalAsPng(filenameComplete, fftData, config.Postprocessing.SaveAsPNG);
                                        //SaveSignalAsPng($"{filename}_1kHz.png", fftData, 1000, 1, 1080, 1);
                                        sw.Stop();
                                        logger.LogInformation($"#{bi.ToString("0000")} Save as PNG completed in {sw.ElapsedMilliseconds:N0} ms.");
                                    });
                                }

                                logger.LogTrace($"Postprocessing thread #{bi:N0} end");
                            }
                            );
                        }
                        skipBuffer = false;
                    }
                }
            }

            logger.LogInformation("Acquisition done");

            dwf.FDwfDeviceCloseAll();
        }

        private static int InitializeAD2(ILogger logger, SleepLoggerConfig config)
        {
            logger.LogInformation("Opening first device");
            // Open the AD2 with the 2nd configuration with 16k analog-in buffer
            dwf.FDwfDeviceConfigOpen(-1, 1, out int dwfHandle);

            while ((dwfHandle == dwf.hdwfNone) && (!terminateAcquisition))
            {
                dwf.FDwfGetLastErrorMsg(out string lastError);
                logger.LogInformation($"Failed to open device: {lastError.TrimEnd()}. Retrying in 10 seconds.");

                Thread.Sleep(10000);

                dwf.FDwfDeviceConfigOpen(-1, 1, out dwfHandle);
            }

            dwf.FDwfAnalogInBufferSizeInfo(dwfHandle, out int bufferSizeMinimum, out int bufferSizeMaximum);
            bufferSize = Math.Min(bufferSizeMaximum, config.AD2.Samplerate);
            logger.LogInformation($"Device buffer size range: {bufferSizeMinimum:N0} - {bufferSizeMaximum:N0} samples, set to {bufferSize:N0}.");
            voltData = new double[bufferSize];

            //set up acquisition
            dwf.FDwfAnalogInFrequencySet(dwfHandle, config.AD2.Samplerate);
            dwf.FDwfAnalogInFrequencyGet(dwfHandle, out double realSamplerate);

            if (config.AD2.Samplerate != realSamplerate)
            {
                logger.LogWarning($"The sampling rate of {config.AD2.Samplerate:N} Hz is not supported, so the effective sampling rate was set to {realSamplerate:N} Hz. Native sampling rates for AD2 are the following: 1, 2, 4, 5, 8, 10, 16, 20, 25, 32, 40, 50, 64, 80, 100, 125, 128, 160, 200, 250, 256, 320, 400, 500, 625, 640, 800 Hz, 1, 1.25, 1.28, 1.6, 2, 2.5, 3.125, 3.2, 4, 5, 6.25, 6.4, 8, 10, 12.5, 15.625, 16, 20, 25, 31.250, 32, 40, 50, 62.5, 78.125, 80, 100, 125, 156.25, 160, 200, 250, 312.5, 390.625, 400, 500, 625, 781.25, 800 kHz, 1, 1.25, 1.5625, 2, 2.5, 3.125, 4, 5, 6.25, 10, 12.5, 20, 25, 50 and 100 MHz.");
            }

            dwf.FDwfAnalogInBufferSizeSet(dwfHandle, bufferSize);
            dwf.FDwfAnalogInChannelEnableSet(dwfHandle, 0, 1);
            dwf.FDwfAnalogInChannelRangeSet(dwfHandle, 0, 5.0);
            dwf.FDwfAnalogInAcquisitionModeSet(dwfHandle, dwf.acqmodeRecord);
            dwf.FDwfAnalogInRecordLengthSet(dwfHandle, -1);

            if (config.AD2.SignalGenerator.Enabled)
            {
                // set up signal generation
                dwf.FDwfAnalogOutNodeEnableSet(dwfHandle, config.AD2.SignalGenerator.Channel, dwf.AnalogOutNodeCarrier, 1);
                dwf.FDwfAnalogOutNodeFunctionSet(dwfHandle, config.AD2.SignalGenerator.Channel, dwf.AnalogOutNodeCarrier, dwf.funcSine);
                dwf.FDwfAnalogOutNodeFrequencySet(dwfHandle, config.AD2.SignalGenerator.Channel, dwf.AnalogOutNodeCarrier, config.AD2.SignalGenerator.Frequency);
                dwf.FDwfAnalogOutNodeAmplitudeSet(dwfHandle, config.AD2.SignalGenerator.Channel, dwf.AnalogOutNodeCarrier, config.AD2.SignalGenerator.Voltage);

                logger.LogInformation($"Generating sine wave at {config.AD2.SignalGenerator.Frequency:N} Hz...");
                dwf.FDwfAnalogOutConfigure(dwfHandle, config.AD2.SignalGenerator.Channel, 1);
            }

            //wait at least 2 seconds for the offset to stabilize
            Thread.Sleep(2000);

            //start aquisition
            logger.LogInformation("Starting oscilloscope");
            dwf.FDwfAnalogInConfigure(dwfHandle, 0, 1);

            logger.LogInformation($"Recording data at {SimplifyNumber(config.AD2.Samplerate)}Hz, press Ctrl+C to stop...");

            return dwfHandle;
        }

        private static void SaveSignalAsPng(string filename, FftData fftData, SaveAsPngConfig config)
        {
            Size targetResolution = new Size(config.TargetWidth, config.TargetHeight);
            float idealAspectRatio = (float)targetResolution.Width / (float)targetResolution.Height;

            float maxValue = fftData.MagnitudesPerHz.Max();
            int maxValueAtIndex = Array.IndexOf(fftData.MagnitudesPerHz, maxValue);

            // convert samples into log scale (dBm)
            //var samples = signal.Samples.Select(s => Scale.ToDecibel(s)).ToArray();
            var samples = fftData.MagnitudesPerHz.Take(config.RangeX).ToArray();
            
            int width = 0;
            float currentAspectRatio;
            int rowCount;
            do
            {
                width = Math.Min(width + config.RowWidthStepsSamples, config.RangeX);
                rowCount = Math.Max(1, (int)Math.Ceiling((double)config.RangeX / width));
                currentAspectRatio = (float)width / (rowCount * config.RowHeightPixels);
            } while ((currentAspectRatio < idealAspectRatio) && (width != config.RangeX));

            float resoltionScaleDownFactor = Math.Max((float)width / targetResolution.Width, (float)(config.RowHeightPixels * rowCount) / targetResolution.Height);

            //int originalSampleCount = samples.Length;
            //int onePercentCount = originalSampleCount / 100;
            //double[] filteredSamples = new double[originalSampleCount - 1];
            //skip the first value, its the total power
            //Array.Copy(samples, 1, filteredSamples, 0, originalSampleCount - 1);
            //Array.Sort(filteredSamples);
            //discard the top 1% and bottom 1% to calculate the scale
            //filteredSamples = filteredSamples.Take(originalSampleCount - onePercentCount).TakeLast(originalSampleCount - onePercentCount - onePercentCount).ToArray();

            Color bgColor = Color.LightGray;
            Brush lineColor = new SolidBrush(Color.DarkGray);

            Bitmap spectrumBitmap = new Bitmap(width, config.RowHeightPixels * rowCount, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Graphics graphics = Graphics.FromImage(spectrumBitmap);
            graphics.FillRectangle(new SolidBrush(bgColor), new Rectangle(0, 0, width, config.RowHeightPixels * rowCount));

            var pen = new Pen(lineColor, 1);
            float sampleAmplification = (1.0f / config.RangeY) * config.RowHeightPixels;

            for (int r = 1; r <= rowCount; r++)
            {
                int bottomLine = r * config.RowHeightPixels;

                for (int i = 0; i < width; i++)
                {
                    int dataPointIndex = (r - 1) * width + i;
                    if (dataPointIndex >= samples.Length) continue;

                    if (samples[dataPointIndex] > 0)
                    {
                        int valueToShow = (int)Math.Min(samples[dataPointIndex] * sampleAmplification, config.RowHeightPixels);

                        graphics.DrawLine(pen, new Point(i, bottomLine), new Point(i, bottomLine - valueToShow));
                    }
                }
            }

            int scaledDownWidth = (((int)(spectrumBitmap.Width / resoltionScaleDownFactor)) / 2) * 2;
            int scaledDownHeight = (((int)(spectrumBitmap.Height / resoltionScaleDownFactor)) / 2) * 2;
            var scaledDownBitmap = new Bitmap(spectrumBitmap, new Size(scaledDownWidth, scaledDownHeight));

            // TODO: add all DrawStrings here
            Font font = new Font("Georgia", 10.0f);
            graphics = Graphics.FromImage(scaledDownBitmap);
            graphics.DrawString($"{filename}", font, Brushes.White, new PointF(scaledDownWidth * 0.75f, 10.0f + font.Height * 0));
            graphics.DrawString($"Y range: {Math.Round(config.RangeY * 1000 * 1000):N0} µV", font, Brushes.White, new PointF(scaledDownWidth * 0.75f, 10.0f + font.Height * 1.1f));
            graphics.DrawString($"Max value: {Math.Round(maxValue * 1000 * 1000):N0} µV @ {maxValueAtIndex:N} Hz", font, Brushes.White, new PointF(scaledDownWidth * 0.75f, 10.0f + font.Height * 2.2f));
            for (int r = 1; r <= rowCount; r++)
            {
                int fromKHz = (r - 1) * width / 1000;
                int toKHz = r * width / 1000;
                int rowHeight = (scaledDownHeight / rowCount);
                int bottomLine = r * rowHeight;

                if (fromKHz > 0)
                {
                    graphics.DrawString($"{fromKHz:N} kHz", font, Brushes.White, new PointF(10.0f, bottomLine - (rowHeight * 0.25f)));
                }
                graphics.DrawString($"{toKHz:N} kHz", font, Brushes.White, new PointF(scaledDownWidth - 75.0f, bottomLine - (rowHeight * 0.25f)));
            }

            FileStream pngFile = new FileStream(filename, FileMode.Create);
            scaledDownBitmap.Save(pngFile, System.Drawing.Imaging.ImageFormat.Png);
            pngFile.Close();
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            terminateAcquisition = true;
            e.Cancel = true;
        }

        private static string SimplifyNumber(double n, string format = "0.###")
        {
            string[] postfixes = { "p", "n", "u", "m", "", "k", "M", "T", "P" };
            int postfixIndex = 4;

            double absValue = Math.Abs(n);

            while ((absValue > 1) && (absValue % 1000 == 0))
            {
                postfixIndex++;
                absValue /= 1000;
            }

            while (absValue < 1)
            {
                postfixIndex--;
                absValue *= 1000;
            }

            return absValue.ToString(format) + postfixes[postfixIndex];
        }

    }
}
