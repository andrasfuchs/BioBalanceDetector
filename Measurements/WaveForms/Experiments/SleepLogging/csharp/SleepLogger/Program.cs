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
        /// <summary>
        /// Number of samples per buffer
        /// </summary>
        static int bufferSize;

        static float inputAmplification = short.MaxValue / 1.0f;        // WAV file ranges from -1000 mV to +1000 mV

        static bool terminateAcquisition = false;

        static List<float> samples = new List<float>();
        static double[] voltData;

        static List<float> minValues = new List<float>();
        static List<float> maxValues = new List<float>();

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
                    .AddFilter("Microsoft", Enum.Parse<LogLevel>(configuration["Logging:LogLevel:Default"]))
                    .AddFilter("System", Enum.Parse<LogLevel>(configuration["Logging:LogLevel:Default"]))
                    .AddFilter("SleepLogger.Program", Enum.Parse<LogLevel>(configuration["Logging:LogLevel:SleepLogger.Program"]))
                    .AddConfiguration(configuration)
                    .AddConsole();
            });
            ILogger logger = loggerFactory.CreateLogger<Program>();


            logger.LogInformation("Bio Balance Detector Sleep Logger v0.3 (2021-01-13)");
            logger.LogInformation("");
            logger.LogInformation("Options:");
            logger.LogInformation("--generatepng <FFT data directory>        Generates a PNG images from FFT data");
            logger.LogInformation("");

            try
            {
                config = new SleepLoggerConfig(configuration);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return;
            }

            Console.CancelKeyPress += Console_CancelKeyPress;

            if (args.Length > 1)
            {
                if (args[0] == "--generatepng")
                {
                    string foldername = args[1];

                    if (Directory.Exists(foldername))
                    {
                        foreach (string filename in Directory.GetFiles(args[1]))
                        {
                            string pathToFile = Path.GetFullPath(filename);

                            if ((Path.GetExtension(filename) != ".fft") && (Path.GetExtension(filename) != ".zip"))
                            {
                                continue;
                            }

                            try
                            {
                                FftData fftData = FftData.LoadFrom($"{pathToFile}");

                                string filenameWithoutExtension = Path.Combine(foldername, Path.GetFileNameWithoutExtension(pathToFile));

                                if (File.Exists($"{filenameWithoutExtension}_200kHz.png"))
                                {
                                    logger.LogWarning($"{filenameWithoutExtension}_200kHz.png already exists.");
                                    continue;
                                }

                                SaveSignalAsPng($"{filenameWithoutExtension}_200kHz.png", fftData, config.Postprocessing.SaveAsPng, 60000);
                                logger.LogInformation($"{filenameWithoutExtension}_200kHz.png was generated successfully.");
                            }
                            catch (Exception ex)
                            {
                                logger.LogWarning($"There was an error while generating PNG for '{Path.GetFileName(pathToFile)}': {ex.Message}");
                            }
                        }

                        string mp4Filename = $"{Path.GetDirectoryName(foldername)}.mp4";
                        logger.LogInformation($"Generating MP4 video file '{mp4Filename}'");
                        try
                        {
                            FFmpeg.Conversions.New()
                                .SetInputFrameRate(12)
                                .BuildVideoFromImages(Directory.GetFiles(args[1], "*.png").OrderBy(fn => fn))
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
                logger.LogTrace($"FDwfAnalogInStatusRecord end   | cAvailable:{cAvailable}, cLost:{cLost}, cCorrupted:{cCorrupted}");

                if (cAvailable == 0)
                {
                    logger.LogWarning($"Aqusition error! cAvailable: {cAvailable}");
                    Thread.Sleep(500);
                    logger.LogInformation($"Reseting device...");
                    dwfHandle = InitializeAD2(logger, config);
                    continue;
                }

                if ((cLost > 0) || (cCorrupted > 0))
                {
                    logger.LogInformation($"Data error! cLost:{cLost}, cCorrupted:{cCorrupted}");
                    skipBuffer = true;
                }

                logger.LogTrace($"FDwfAnalogInStatusData begin | dwfHandle:{dwfHandle}, cAvailable:{cAvailable}");
                dwf.FDwfAnalogInStatusData(dwfHandle, 0, voltData, cAvailable);     // get channel 1 data chunk
                logger.LogTrace($"FDwfAnalogInStatusData end   | voltData.Count:{voltData.Count()}");

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

                                logger.LogTrace($"Postprocessing thread #{bi} begin");

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
                                string filename = $"AD2_{fftData.CaptureTime.ToString("yyyy-MM-dd_HHmmss")}";
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

                                var magnitudes = new DiscreteSignal((int)(targetSamplerate / 2 / config.Postprocessing.IntervalSeconds), concatenatedPowerSpectrum);
                                fftData.FrequencyStep = ((float)config.AD2.Samplerate / 2) / (magnitudes.Samples.Length - 1);
                                // normalize the data so that 1.0 means 1.0 V of peak to peak amplitude
                                fftData.MagnitudeData = magnitudes.Samples.Select(md => md / 130750.0f).ToArray();

                                float averageMagnitude = fftData.MagnitudeData.Take(1000).Average();
                                if (averageMagnitude < config.Postprocessing.MagnitudeThreshold)
                                {
                                    logger.LogWarning($"#{bi.ToString("0000")} The average magnitude for the first 1000 values is too low: {averageMagnitude * 1000 * 1000} µV");
                                    return;
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
                                        logger.LogInformation($"#{bi.ToString("0000")} Save as FFT completed in {sw.ElapsedMilliseconds} ms.");
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
                                        logger.LogInformation($"#{bi.ToString("0000")} Save as compressed FFT completed in {sw.ElapsedMilliseconds} ms.");
                                    });
                                }

                                if (config.Postprocessing.SaveAsPng.Enabled)
                                {
                                    Task.Run(() =>
                                    {
                                        Stopwatch sw = Stopwatch.StartNew();

                                        //save a PNG with the values
                                        sw.Restart();
                                        SaveSignalAsPng($"{pathToFile}_200kHz.png", fftData, config.Postprocessing.SaveAsPng, 200000);
                                        //SaveSignalAsPng($"{filename}_1kHz.png", fftData, 1000, 1, 1080, 1);
                                        sw.Stop();
                                        logger.LogInformation($"#{bi.ToString("0000")} Save as PNG completed in {sw.ElapsedMilliseconds} ms.");
                                    });
                                }

                                logger.LogTrace($"Postprocessing thread #{bi} end");
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
            dwf.FDwfDeviceOpen(-1, out int dwfHandle);

            while ((dwfHandle == dwf.hdwfNone) && (!terminateAcquisition))
            {
                dwf.FDwfGetLastErrorMsg(out string lastError);
                logger.LogInformation($"Failed to open device: {lastError.TrimEnd()}. Retrying in 10 seconds.");

                Thread.Sleep(10000);

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

            logger.LogInformation($"Generating sine wave at {config.AD2.SignalGeneratorHz} Hz...");
            dwf.FDwfAnalogOutConfigure(dwfHandle, config.AD2.SignalGeneratorChannel, 1);

            //wait at least 2 seconds for the offset to stabilize
            Thread.Sleep(2000);

            //start aquisition
            logger.LogInformation("Starting oscilloscope");
            dwf.FDwfAnalogInConfigure(dwfHandle, 0, 1);

            logger.LogInformation($"Recording data at {config.AD2.Samplerate} Hz, press Ctrl+C to stop...");

            return dwfHandle;
        }

        private static void SaveSignalAsPng(string filename, FftData fftData, SaveAsPngConfig config, int maxSamples)
        {
            Size targetResolution = new Size(config.TargetWidth, config.TargetHeight);
            float idealAspectRatio = (float)targetResolution.Width / (float)targetResolution.Height;

            maxValues.Add(fftData.MagnitudeData.Max());
            int index = Array.FindIndex(fftData.MagnitudeData, d => d == maxValues[maxValues.Count-1]);

            // convert samples into log scale (dBm)
            //var samples = signal.Samples.Select(s => Scale.ToDecibel(s)).ToArray();
            var samples = fftData.MagnitudeData.Take(maxSamples).ToArray();
            
            int width = 0;
            float currentAspectRatio;
            int rowCount;
            do
            {
                width = Math.Min(width + config.RowWidthStepsSamples, samples.Length);
                rowCount = Math.Max(1, (int)Math.Ceiling((double)samples.Length / width));
                currentAspectRatio = (float)width / (rowCount * config.RowHeightPixels);
            } while (currentAspectRatio < idealAspectRatio);

            float resoltionScaleDownFactor = Math.Max((float)width / targetResolution.Width, (float)(config.RowHeightPixels * rowCount) / targetResolution.Height);
            Font font = new Font("Georgia", 10.0f * resoltionScaleDownFactor);

            int originalSampleCount = samples.Length;
            int onePercentCount = originalSampleCount / 100;
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
            float sampleAmplification = (1.0f / config.RangeVolt) * config.RowHeightPixels;

            for (int r = 1; r <= rowCount; r++)
            {
                int bottomLine = r * config.RowHeightPixels;

                for (int i = 0; i < width; i++)
                {
                    int dataPointIndex = (r - 1) * width + i;
                    if ((dataPointIndex >= samples.Length) || (dataPointIndex >= maxSamples)) continue;

                    if (samples[dataPointIndex] > 0)
                    {
                        int valueToShow = (int)Math.Min(samples[dataPointIndex] * sampleAmplification, config.RowHeightPixels);

                        graphics.DrawLine(pen, new Point(i, bottomLine), new Point(i, bottomLine - valueToShow));
                    }
                }

                if (((r - 1) * width) / 1000 > 0)
                {
                    graphics.DrawString($"{((r - 1) * width) / 1000} kHz", font, Brushes.White, new PointF(100.0f, bottomLine - (config.RowHeightPixels * 0.75f)));
                }
                graphics.DrawString($"{(r * width) / 1000} kHz", font, Brushes.White, new PointF(width - (75.0f * resoltionScaleDownFactor), bottomLine - (config.RowHeightPixels * 0.75f)));
            }

            graphics.DrawString($"{filename}", font, Brushes.White, new PointF(width - (350.0f * resoltionScaleDownFactor), 10.0f));
            graphics.DrawString($"Y range: {Math.Round(config.RangeVolt * 1000 * 1000)} µV", font, Brushes.White, new PointF(width - (350.0f * resoltionScaleDownFactor), 30.0f + font.Height));


            int scaledDownWidth = (((int)(spectrumBitmap.Width / resoltionScaleDownFactor)) / 2) * 2;
            int scaledDownHeight = (((int)(spectrumBitmap.Height / resoltionScaleDownFactor)) / 2) * 2;
            var scaledDownBitmap = new Bitmap(spectrumBitmap, new Size(scaledDownWidth, scaledDownHeight));

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
