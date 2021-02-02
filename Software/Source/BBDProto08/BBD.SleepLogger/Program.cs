﻿using Microsoft.Extensions.Configuration;
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

namespace BBD.SleepLogger
{
    class Program
    {
        static string versionString = "v0.6 (2021-01-30)";

        static Mutex mutex = new Mutex(true, "{79bb7f72-37bc-41ff-9014-ed8662659b52}");

        static string[] audioFrameworks = { "dshow", "alsa", "openal", "oss" };

        static string ffmpegAudioRecordingParameters = "-c:a aac -ac 1 -ar 44100 -ab 32k";
        static string ffmpegAudioProcessingSilenceRemove = "-af silenceremove=1:0:{SilenceThreshold}";
        static string ffmpegAudioProcessingNormalize = "-af loudnorm=I=-24.0:LRA=+11.0:tp=-2.0";

        /// <summary>
        /// Number of samples per buffer
        /// </summary>
        static int bufferSize;

        static float inputAmplification = short.MaxValue / 1.0f;        // WAV file ranges from -1000 mV to +1000 mV

        static bool terminateAcquisition = false;
        static DriveInfo spaceCheckDrive;

        static List<float> samples = new List<float>();
        static double[] voltData;

        static List<float> maxValues = new List<float>();

        static IConfigurationRoot configuration;
        static SleepLoggerConfig config;
        static Pen[] chartPens;

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
            Console.WriteLine("--video <FFT data directory>        Generate PNG images and an MP4 video from FFT data");
            Console.WriteLine("--mlcsv <FFT data directory>        Generate CSV file for machine learning from FFT data");
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

            if (!mutex.WaitOne(TimeSpan.Zero, true))
            {
                logger.LogError($"An instance of this app is already running on this machine.");
                return;
            }

            Console.CancelKeyPress += Console_CancelKeyPress;

            
            foreach (var d in DriveInfo.GetDrives())
            {
                if (Path.GetFullPath(AppendDataDir("")).ToLower().StartsWith(d.RootDirectory.FullName.ToLower()) && ((spaceCheckDrive == null) || (d.Name.Length > spaceCheckDrive.Name.Length)))
                {
                    spaceCheckDrive = d;
                }
            }


            List<string> ffmpegAudioNames = new List<string>();
            foreach (string audioFramework in audioFrameworks)
            {
                try
                {
                    var listDevices = FFmpeg.Conversions.New().Start($"-list_devices true -f {audioFramework} -i dummy").Result;
                }
                catch (Exception e)
                {
                    // this is expected
                    foreach (string audioDeviceDetails in e.Message.Split(Environment.NewLine).Where(ol => ol.StartsWith("[")))
                    {
                        string listedAudioFramework = audioDeviceDetails.Split(new char[] { '[', '@' }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                        string listedAudioDevice = audioDeviceDetails.Split(new char[] { ']', '\"' }, StringSplitOptions.RemoveEmptyEntries)[^1].Trim();

                        if (audioDeviceDetails.Contains("Alternative name") || !audioDeviceDetails.Contains("\"")) continue;

                        ffmpegAudioNames.Add($"{listedAudioFramework}/{listedAudioDevice}");
                    }
                }
            }

            if (config.AudioRecording.PreferredDevice.Split("/").Length != 2)
            {
                logger.LogWarning($"The preferred audio input device in the config file is not in the correct format. It should be in the '<ffmpeg audio framework>/<ffmpeg audio device>' format. Audio will not be recorded.");
                config.AudioRecording.Enabled = false;
            }

            if (config.AudioRecording.Enabled && !ffmpegAudioNames.Contains(config.AudioRecording.PreferredDevice))
            {
                logger.LogWarning($"The preferred audio input device was not listed by the operating system as a valid audio source. The listed audio inputs are: {String.Join(", ", ffmpegAudioNames)}.");
            }

            if (args.Length > 1)
            {
                if (args[0] == "--video")
                {
                    GenerateVideo(args[1], logger);
                    return;
                }

                if (args[0] == "--mlcsv")
                {
                    GenerateMLCSV(args[1], 1.0f, 45, logger);
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
                if ((spaceCheckDrive != null) && (spaceCheckDrive.AvailableFreeSpace < config.MinimumAvailableFreeSpace))
                {
                    logger.LogError($"There is not enough space on drive {spaceCheckDrive.Name}.  to continue. There must be at least {config.MinimumAvailableFreeSpace:N0} bytes of available free space at all times.");
                    break;
                }

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

                if ((config.Postprocessing.Enabled) || (config.AudioRecording.Enabled))
                {
                    samples.AddRange(voltData.Take(cAvailable).Select(vd => (float)vd));

                    if (samples.Count >= config.AD2.Samplerate * config.Postprocessing.IntervalSeconds)
                    {
                        DateTime captureTime = DateTime.Now;
                        string foldername = $"{captureTime.ToString("yyyy-MM-dd")}";
                        string filename = $"AD2_{captureTime.ToString("yyyyMMdd_HHmmss")}";
                        string pathToFile = Path.Combine(foldername, filename);
                        if (!Directory.Exists(AppendDataDir(captureTime.ToString("yyyy-MM-dd"))))
                        {
                            Directory.CreateDirectory(AppendDataDir(captureTime.ToString("yyyy-MM-dd")));
                        }

                        if (config.Postprocessing.Enabled)
                        {

                            //generate a signal
                            int sampleCount = samples.Count;
                            if (sampleCount < config.Postprocessing.FFTSize)
                            {
                                samples.AddRange(Enumerable.Repeat(0.0f, config.Postprocessing.FFTSize - sampleCount));
                            }
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
                                        CaptureTime = captureTime,
                                        Duration = config.Postprocessing.IntervalSeconds,
                                        FirstFrequency = 0,
                                        LastFrequency = config.AD2.Samplerate / 2,
                                        FftSize = config.Postprocessing.FFTSize,
                                    };

                                    Stopwatch sw = Stopwatch.StartNew();

                                    if (config.Postprocessing.SaveAsWAV)
                                    {
                                        sw.Restart();
                                    //and save samples to a WAV file
                                    FileStream waveFileStream = new FileStream(AppendDataDir($"{pathToFile}.wav"), FileMode.Create);
                                        DiscreteSignal signalToSave = new DiscreteSignal(signal.SamplingRate, signal.Samples.Take(sampleCount).ToArray(), true);
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
                                    //logger.LogInformation($"#{bi.ToString("0000")} signal.Samples.Length: {sampleCount:N0} | FFT size: {config.Postprocessing.FFTSize:N0}");
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

                                    fftData.FrequencyStep = ((float)fftData.LastFrequency) / (fftData.MagnitudeData.Length - 1);

                                    maxValues.Add(fftData.MagnitudeData.Max());
                                    int maxIndex = Array.FindIndex(fftData.MagnitudeData, d => d == maxValues[maxValues.Count - 1]);
                                    logger.LogInformation($"#{bi.ToString("0000")} The maximum magnitude is {maxValues[maxValues.Count - 1] * 1000 * 1000:N} µV at the index of #{maxIndex:N0} ({maxIndex * fftData.FrequencyStep:N} Hz).");
                                //if (config.AD2.SignalGenerator.Enabled)
                                //{
                                //    logger.LogInformation($"#{bi.ToString("0000")} The magnitude at {config.AD2.SignalGenerator.Frequency} Hz is {fftData.MagnitudesPer1p0Hz[(int)config.AD2.SignalGenerator.Frequency] * 1000 * 1000:N} µV.");
                                //}

                                //if (config.Postprocessing.MagnitudeThreshold > 0)
                                //{
                                //    float averageMagnitude = fftData.MagnitudesPer1p0Hz[100..1000].Average();
                                //    if (averageMagnitude < config.Postprocessing.MagnitudeThreshold)
                                //    {
                                //        logger.LogWarning($"#{bi.ToString("0000")} The average magnitude in the 100-1000 Hz range is too low: {averageMagnitude * 1000 * 1000:N0} µV. The threashold is {config.Postprocessing.MagnitudeThreshold * 1000 * 1000:N0} µV.");
                                //        return;
                                //    }
                                //}

                                if (config.Postprocessing.SaveAsFFT)
                                    {
                                        Task.Run(() =>
                                        {
                                            Stopwatch sw = Stopwatch.StartNew();

                                        //save it the FFT to a JSON file
                                        sw.Restart();
                                            FftData.SaveAs(fftData.Resample(0.1f), AppendDataDir($"{pathToFile}.fft"), false);
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
                                            FftData.SaveAs(fftData.Resample(0.1f), AppendDataDir($"{pathToFile}.zip"), true);
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
                                            SaveSignalAsPng(AppendDataDir(filenameComplete), fftData, config.Postprocessing.SaveAsPNG);
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

                        if (config.AudioRecording.Enabled)
                        {
                            Task.Run(() =>
                            {
                                TimeSpan tp = new TimeSpan(0, 0, (int)config.Postprocessing.IntervalSeconds);

                                string recFilename = AppendDataDir($"{pathToFile}_rec.{config.AudioRecording.OutputFormat}");
                                string silentFilename = AppendDataDir($"{pathToFile}_sr.{config.AudioRecording.OutputFormat}");
                                string finalFilename = AppendDataDir($"{pathToFile}.{config.AudioRecording.OutputFormat}");

                                string ffmpegAudioFramework = config.AudioRecording.PreferredDevice.Split("/")[0];
                                string ffmpegAudioDevice = config.AudioRecording.PreferredDevice.Split("/")[1];
                                string audioRecordingCommandLine = $"-f {ffmpegAudioFramework} -i audio=\"{ffmpegAudioDevice}\" {ffmpegAudioRecordingParameters} -t {tp} \"{recFilename}\"";
                                FFmpeg.Conversions.New().Start(audioRecordingCommandLine)
                                    .ContinueWith((Task<IConversionResult> cr) =>
                                    {
                                        Stopwatch sw = Stopwatch.StartNew();
                                        sw.Restart();

                                        string silenceRemoveCommandLine = $"-i {recFilename} {ffmpegAudioProcessingSilenceRemove.Replace("{SilenceThreshold}", config.AudioRecording.SilenceThreshold)} {ffmpegAudioRecordingParameters} {silentFilename}";
                                        FFmpeg.Conversions.New().Start(silenceRemoveCommandLine).Wait();

                                        File.Delete(recFilename);

                                        if (new FileInfo(silentFilename).Length > 0)
                                        {
                                            string normalizeCommandLine = $"-i {silentFilename} {ffmpegAudioProcessingNormalize} {ffmpegAudioRecordingParameters} {finalFilename}";
                                            FFmpeg.Conversions.New().Start(normalizeCommandLine).Wait();

                                            sw.Stop();
                                            logger.LogInformation($"Processed audio recording saved as '{pathToFile}.{config.AudioRecording.OutputFormat}' in {sw.ElapsedMilliseconds:N0} ms.");
                                        }

                                        File.Delete(silentFilename);
                                    });
                            });
                        }
                    }
                }
            }

            logger.LogInformation("Acquisition done");

            dwf.FDwfDeviceCloseAll();
        }

        private static void GenerateVideo(string foldername, ILogger logger)
        {
            foreach (FftData fftData in EnumerateFFTDataInFolder(foldername))
            {
                try 
                { 
                    string filenameComplete = Path.Combine(foldername, $"{fftData.Filename}_{SimplifyNumber(config.Postprocessing.SaveAsPNG.RangeX)}Hz_{SimplifyNumber(config.Postprocessing.SaveAsPNG.RangeY)}V.png");

                    if (File.Exists(AppendDataDir(filenameComplete)))
                    {
                        logger.LogWarning($"{filenameComplete} already exists.");
                        continue;
                    }

                    SaveSignalAsPng(AppendDataDir(filenameComplete), fftData, config.Postprocessing.SaveAsPNG);
                    logger.LogInformation($"{filenameComplete} was generated successfully.");
                }
                catch (Exception ex)
                {
                    logger.LogWarning($"There was an error while generating PNG for '{fftData.Filename}': {ex.Message}");
                }
            }

            string mp4Filename = $"{foldername}.mp4";
            logger.LogInformation($"Generating MP4 video file '{mp4Filename}'");
            try
            {
                FFmpeg.Conversions.New()
                    .SetInputFrameRate(12)
                    .BuildVideoFromImages(Directory.GetFiles(AppendDataDir(foldername), "*.png").OrderBy(fn => fn))
                    .SetFrameRate(12)
                    .SetPixelFormat(PixelFormat.yuv420p)
                    .SetOutput(AppendDataDir(mp4Filename))
                    .Start()
                    .Wait();
            }
            catch (Exception ex)
            {
                logger.LogError($"There was an error while generating MP4 video file '{mp4Filename}': {ex.Message}");
            }
        }

        private static void GenerateMLCSV(string foldername, float fftStep, int fftCount, ILogger logger)
        {
            StringBuilder sb = new StringBuilder();

            if (Directory.Exists(AppendDataDir(foldername)))
            {
                foreach (FftData fftData in EnumerateFFTDataInFolder(foldername))
                {
                    logger.LogInformation($"Adding {fftData.Filename} to the machine learning CSV file.");
                    sb.Append(String.Join(",", fftData.Resample(fftStep).MagnitudeData.Take(fftCount)));
                    sb.AppendLine("," + String.Join("+", fftData.Tags));
                }

                string csvFilename = $"{foldername}.csv";
                logger.LogInformation($"Generating machine learning CSV file '{csvFilename}'");
                try
                {
                    File.WriteAllText(AppendDataDir(csvFilename), sb.ToString());
                }
                catch (Exception ex)
                {
                    logger.LogError($"There was an error while generating CSV file '{csvFilename}': {ex.Message}");
                }
            }
        }

        static private IEnumerable<FftData> EnumerateFFTDataInFolder(string foldername, string[] applyTags = null)
        {
            if (applyTags == null) applyTags = Array.Empty<string>();

            if (Directory.Exists(AppendDataDir(foldername)))
            {
                foreach (string filename in Directory.GetFiles(AppendDataDir(foldername)).OrderBy(n => n))
                {
                    string pathToFile = Path.GetFullPath(filename);

                    if ((Path.GetExtension(filename) != ".fft") && (Path.GetExtension(filename) != ".zip"))
                    {
                        continue;
                    }

                    var fftData = FftData.LoadFrom($"{pathToFile}");
                    fftData.Filename = Path.GetFileNameWithoutExtension(pathToFile);
                    fftData.Tags = applyTags;

                    yield return fftData;
                }

                foreach (string directoryName in Directory.GetDirectories(AppendDataDir(foldername)).OrderBy(n => n))
                {
                    var tags = new List<string>(applyTags);
                    string newTag = Path.GetFileName(directoryName);
                    if (newTag.StartsWith("#"))
                    {
                        tags.Add(newTag[1..]);
                    }

                    foreach (var fftData in EnumerateFFTDataInFolder(directoryName, tags.ToArray()))
                    {
                        yield return fftData;
                    }
                }
            }
        }

        private static string AppendDataDir(string filename)
        {
            if (!String.IsNullOrWhiteSpace(config.DataDirectory))
            {
                return Path.Combine(config.DataDirectory, filename);
            }
            else
            {
                return Path.Combine(Directory.GetCurrentDirectory(), filename);
            }
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

            fftData = fftData.Resample(1.0f);
            string tags = String.Join(", ", fftData.Tags ?? Array.Empty<string>());

            float maxValue = fftData.MagnitudeData.Max();
            int maxValueAtIndex = Array.IndexOf(fftData.MagnitudeData, maxValue);

            // convert samples into log scale (dBm)
            //var samples = signal.Samples.Select(s => Scale.ToDecibel(s)).ToArray();
            var samples = fftData.MagnitudeData.Take(config.RangeX).ToArray();
            
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

            Bitmap spectrumBitmap = new Bitmap(width, config.RowHeightPixels * rowCount, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Graphics graphics = Graphics.FromImage(spectrumBitmap);
            graphics.FillRectangle(new SolidBrush(bgColor), new Rectangle(0, 0, width, config.RowHeightPixels * rowCount));

            if (chartPens == null)
            {
                chartPens = new Pen[11];
                for (int i = 0; i <= 10; i++)
                {
                    chartPens[i] = new Pen(new SolidBrush(ColorInterpolator.InterpolateBetween(Color.DarkGray, Color.DarkRed, ((double)i / 10.0) / 2.0)), 1.0f);
                }
            }

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
                        int penIndex = (int)Math.Min(chartPens.Length - 1, (((samples[dataPointIndex] * sampleAmplification) / (float)valueToShow) - 1.0f) * 0.5);

                        graphics.DrawLine(chartPens[penIndex], new Point(i, bottomLine), new Point(i, bottomLine - valueToShow));
                    }
                }
            }

            int scaledDownWidth = (((int)(spectrumBitmap.Width / resoltionScaleDownFactor)) / 2) * 2;
            int scaledDownHeight = (((int)(spectrumBitmap.Height / resoltionScaleDownFactor)) / 2) * 2;
            var scaledDownBitmap = new Bitmap(spectrumBitmap, new Size(scaledDownWidth, scaledDownHeight));

            Font font = new Font("Georgia", 10.0f);
            graphics = Graphics.FromImage(scaledDownBitmap);
            graphics.DrawString($"{filename}", font, Brushes.White, new PointF(scaledDownWidth * 0.75f, 10.0f + font.Height * 0));
            graphics.DrawString($"{tags}", font, Brushes.White, new PointF(scaledDownWidth * 0.75f, 10.0f + font.Height * 1.1f));
            graphics.DrawString($"Y range: {Math.Round(config.RangeY * 1000 * 1000):N0} µV", font, Brushes.White, new PointF(scaledDownWidth * 0.75f, 10.0f + font.Height * 2.2f));
            graphics.DrawString($"Max value: {Math.Round(maxValue * 1000 * 1000):N0} µV @ {maxValueAtIndex:N} Hz", font, Brushes.White, new PointF(scaledDownWidth * 0.75f, 10.0f + font.Height * 3.3f));
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

            mutex.ReleaseMutex();
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
