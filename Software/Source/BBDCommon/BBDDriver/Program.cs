using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows;
using System.Web.Http.SelfHost;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using BBDDriver.Models.Source;
using BBDDriver.Models.Output;
using BBDDriver.Models;
using System.Diagnostics;
using BBDDriver.Helpers;
using BBDDriver.Models.Filter;

namespace BBDDriver
{
    public class Program
    {
        private static string workingDirectory = @"c:\Work\BioBalanceDetector\Recordings\";

        private static string arduinoPort = "COM3";

        private static float[,,] recentValues;
        private static float recentChangesSensitivity = 1.00f;
        private static float columnMin = 0.0f;
        private static float columnMax = 1.0f;
        private static float frequencyStep = 1.0f;

        private static int recentDiffsIndex = 0;
        private static float[] recentDiffs = new float[32];

        private static DateTime firstActivity;

        private static IDisposable waveSourceDevice = null;
        private static MultiChannelInput<IDataChannel> waveSource = null;

        private static WaveFileOutput wfo;
        private static SimpleVTSFileOutput vtsfo;

        private static StringBuilder consoleSB;
        private static int consoleRefreshAtY;

        private static VisualOutput vo = null;
        private static DateTime lastRefreshVisualOutput = DateTime.UtcNow;

        private static int waveDataOverflowWarningCount = 0;
        private static long waveFileBytesWritten = 0;
        private static int vtsDataOverflowWarningCount = 0;
        private static long vtsFileBytesWritten = 0;

        private static string sessionId;

        public enum DataDisplayModes { None, NormalizedWaveform, FilteredSpectogram, DominanceMatrix, GoertzelValues }

        public static string SessionId
        {
            get
            {
                if (sessionId == null)
                {
                    sessionId = "BBD_" + DateTime.UtcNow.ToString("yyyyMMdd_HHmmss_") + RandomString(4);
                }

                return sessionId;
            }
        }

        public static void StartSession(DataDisplayModes mode)
        {
            consoleSB = new StringBuilder();

            //Console.SetOut(File.AppendText($"{workingDirectory}{SessionId}.log"));
            Console.WriteLine($"Session '{SessionId}' starts at {DateTime.Now.ToString("yyyy-MM-dd HH\\:mm\\:sszzz")} ({DateTime.UtcNow.ToString("yyyy-MM-dd HH\\:mm\\:ss")} UTC)");

            try
            {
                Console.Write($"Checking if Mercury-16 is connected as PHDC USB device... ");
                BBDMercury16Input bbdMercury16Input = new BBDMercury16Input();
                waveSourceDevice = bbdMercury16Input;
                waveSource = bbdMercury16Input;
                Console.WriteLine("OK");
            }
            catch (System.IO.IOException)
            {
                Console.WriteLine("not connected");
            }

            if (waveSource == null)
            {
                try
                {
                    Console.Write($"Checking if Arduino is connected on port '{arduinoPort}', expecting The 8x8 Matrix as data-source... ");
                    waveSource = new BBD8x8MatrixInput(arduinoPort, 64);
                    Console.WriteLine("OK");
                }
                catch (System.IO.IOException)
                {
                    Console.WriteLine("not connected");
                }
            }

            if (waveSource == null)
            {
                Console.Write($"No hardware source was detected, so creating an 8kHz 16ch sine signal generator as data-source... ");
                waveSource = new SineInput(8000, 16);
                Console.WriteLine("OK");
            }

            int fftSize = 1024 * 16;
            frequencyStep = (float)waveSource.SamplesPerSecond / fftSize;

            MultiChannelInput<IDataChannel> normalizedSource = FilterManager.ApplyFilters(waveSource, new NormalizeFilter() { Settings = new NormalizeFilterSettings() { Enabled = true, SampleCount = waveSource.SamplesPerSecond * 3, Gain = 10.0f } });
            //MultiChannelInput<IDataChannel> filteredSource = FilterManager.ApplyFilters(waveSource, new ByPassFilter() { Settings = new ByPassFilterSettings() { Enabled = true } });
            //MultiChannelInput<IDataChannel> filteredSource = FilterManager.ApplyFilters(waveSource, new FillFilter() { Settings = new FillFilterSettings() { Enabled = true, ValueToFillWith = 0.75f } });

            //MultiChannelInput<IDataChannel> averagedSource = FilterManager.ApplyFilters(normalizedSource, new MovingAverageFilter() { Settings = new MovingAverageFilterSettings() { Enabled = true, InputDataDimensions = 2, MovingAverageLength = 10 } });

            wfo = new WaveFileOutput(normalizedSource, $"{workingDirectory}{SessionId}.wav");
            wfo.DataWritten += Wfo_DataWritten;

            //vtkfo = new VTKFileOutput(filteredSource, $"{workingDirectory}{SessionId}.vts");
            //vtkfo.DataWritten += Wfo_DataWritten;

            //vtsfo = new SimpleVTSFileOutput(filteredSource, $"{workingDirectory}{SessionId}.vts", true);
            //vtsfo.DataWritten += Vtsfo_DataWritten;

            MultiChannelInput<IDataChannel> filteredSource = null;

            switch (mode)
            {
                case DataDisplayModes.NormalizedWaveform:
                    peakToPeakValues = null;
                    vo = new VisualOutput(normalizedSource, 25, VisualOutputMode.Waveform);                    
                    break;
                case DataDisplayModes.FilteredSpectogram:
                    filteredSource = FilterManager.ApplyFilters(normalizedSource, new FFTWFilter() { Settings = new FFTWFilterSettings() { Enabled = true, FFTSampleCount = fftSize, IsBackward = false, PlanningRigor = FFTPlanningRigor.Estimate, IsRealTime = true, Timeout = 300, OutputFormat = FFTOutputFormat.Magnitude, BufferSize = fftSize * 16 } });
                    vo = new VisualOutput(filteredSource, 25, VisualOutputMode.Spectrum);
                    break;
                case DataDisplayModes.DominanceMatrix:
                    filteredSource = FilterManager.ApplyFilters(normalizedSource, new FFTWFilter() { Settings = new FFTWFilterSettings() { Enabled = true, FFTSampleCount = fftSize, IsBackward = false, PlanningRigor = FFTPlanningRigor.Estimate, IsRealTime = true, Timeout = 300, OutputFormat = FFTOutputFormat.FrequencyMagnitudePair } });
                    MultiChannelInput<IDataChannel> thresholdedSource = FilterManager.ApplyFilters(filteredSource, new ThresholdFilter() { Settings = new ThresholdFilterSettings() { Enabled = true, MinValue = 0.002f, MaxValue = Single.MaxValue } });
                    vo = new VisualOutput(thresholdedSource, 25, VisualOutputMode.DominanceMatrix);
                    break;
                case DataDisplayModes.GoertzelValues:
                    vo = new VisualOutput(waveSource, 25, VisualOutputMode.GoertzelTable);
                    break;
            }

            if (vo != null)
            {
                vo.RefreshVisualOutput += Vo_RefreshVisualOutput;
            }

            firstActivity = DateTime.UtcNow;

            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.SetBufferSize(200, 500);
            Console.SetWindowSize(200, 63);
            consoleRefreshAtY = Console.CursorTop;
        }

        private static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public static void StopSession()
        {
            if (vo != null)
            {
                vo.RefreshVisualOutput -= Vo_RefreshVisualOutput;
            }

            if (wfo != null)
            {
                wfo.Dispose();
            }

            if (waveSourceDevice != null)
            {
                waveSourceDevice.Dispose();
            }

            sessionId = null;
        }

        private static void Vtsfo_DataWritten(object sender, DataWrittenEventArgs e)
        {
            vtsFileBytesWritten = e.TotalDataWritten;
            vtsDataOverflowWarningCount = e.DataOverflowWarningCount;
        }

        private static void Wfo_DataWritten(object sender, DataWrittenEventArgs e)
        {
            waveFileBytesWritten = e.TotalDataWritten;
            waveDataOverflowWarningCount = e.DataOverflowWarningCount;
        }

        private static void Vo_RefreshVisualOutput(object sender, RefreshVisualOutputEventArgs e)
        {
            if ((!e.ChangedSinceLastRefresh) && (e.Mode != VisualOutputMode.GoertzelTable)) return;

            if (lastRefreshVisualOutput.AddMilliseconds(20) > DateTime.UtcNow) return;
            lastRefreshVisualOutput = DateTime.UtcNow;

            lock (SessionId)
            {
                try
                {
                    int minimumAverageSamples = (e.Dimensions == null ? 1 : (e.Dimensions[1] / Console.WindowWidth) + 1);

                    string consoleOutput = "";
                    if (e.Mode == VisualOutputMode.Matrix)
                    {
                        consoleOutput = GenerateMatrixOutput(e, false);
                    }
                    else if (e.Mode == VisualOutputMode.Waveform)
                    {
                        //consoleOutput = GenerateWaveformOutput(e, minimumAverageSamples, 4, 13);
                        consoleOutput = GenerateWaveformOutput(e, minimumAverageSamples, 8, 6);
                    }
                    else if (e.Mode == VisualOutputMode.Spectrum)
                    {
                        consoleOutput = GenerateSpectrumOutput(e, minimumAverageSamples, 4, 13);
                    }
                    else if (e.Mode == VisualOutputMode.DominanceMatrix)
                    {
                        consoleOutput = GenerateDominanceMatrixOutput(e, 1, false);
                    }
                    else if (e.Mode == VisualOutputMode.GoertzelTable)
                    {
                        consoleOutput = GenerateGoertzelTableOutput(e);
                    }

                    Console.Title = GenerateConsoleTitle();
                    Console.SetCursorPosition(0, consoleRefreshAtY);
                    Console.Write(consoleOutput);
                }
                catch (System.Net.Http.HttpRequestException)
                {
                    Console.WriteLine("Communication error.");
                }
                catch (System.Threading.Tasks.TaskCanceledException)
                {
                    Console.WriteLine("Thread was cancelled.");
                }
            }
        }

        private static string GenerateSpectrumOutput(RefreshVisualOutputEventArgs e, int avgSampleCount, int maxChannelCount, int height)
        {
            consoleSB.Clear();

            float chColumnMax = 0.0f;
            for (int chIndex = 0; chIndex < Math.Min(maxChannelCount, e.Dimensions[0]); chIndex++)
            {
                int columnCount = e.Dimensions[1] / avgSampleCount;
                float[] columnAvgs = new float[columnCount];

                float dominantValue = 0.0f;
                float dominantIndex = 0.0f;

                for (int x = 0; x < columnCount; x++)
                {
                    for (int sampleIndex = 0; sampleIndex < avgSampleCount; sampleIndex++)
                    {
                        int valueIndex = x * avgSampleCount + sampleIndex;
                        float value = e.Values[chIndex, valueIndex, 0];
                        columnAvgs[x] += value;

                        if (value > dominantValue)
                        {
                            dominantValue = value;
                            dominantIndex = valueIndex;
                        }
                    }
                    columnAvgs[x] /= avgSampleCount;
                    if (columnAvgs[x] > chColumnMax) chColumnMax = columnAvgs[x];
                }

                consoleSB.AppendLine($"=== channel {chIndex} ------ value range on y-axis: [{columnMin.ToString("0.0000")}->{columnMax.ToString("0.0000")}], dominant value: {dominantValue.ToString("0.0000")} " + (dominantValue < 0.01f ? "                  " : $"at {(frequencyStep * (dominantIndex - 0.5f)).ToString("#,##0.0")}Hz-{(frequencyStep * (dominantIndex + 0.5f)).ToString("#,##0.0")}Hz"));
                for (int y = height - 1; y >= 0; y--)
                {
                    float maxValue = columnMin + (((columnMax - columnMin) / height) * (y + 1));
                    float minValue = columnMin + (((columnMax - columnMin) / height) * (y));

                    char[] line = new char[columnCount];
                    for (int x = 0; x < columnCount; x++)
                    {
                        line[x] = (columnAvgs[x] <= minValue ? ' ' : '|');

                        if ((chIndex == 0) && (y == height - 1) && (line[x] == '|')) line[x] = '|';

                        if ((columnAvgs[x] > 0) && (columnAvgs[x] >= minValue) && (columnAvgs[x] <= maxValue)) line[x] = '^';
                    }
                    consoleSB.Append(line);
                    consoleSB.AppendLine();
                }
            }

            if (chColumnMax > columnMax) columnMax *= 1.1f;
            if (chColumnMax < columnMax * 0.6f) columnMax *= 0.9f;

            return consoleSB.ToString();

        }


        private static int peakToPeakValueCounter;
        private static float[][] peakToPeakValues;

        private static string GenerateWaveformOutput(RefreshVisualOutputEventArgs e, int avgSampleCount, int maxChannelCount, int height)
        {
            if (peakToPeakValues == null)
            {
                peakToPeakValues = new float[e.Dimensions[0]][];
            }
            peakToPeakValueCounter = (peakToPeakValueCounter + 1) % (25 * 3);

            consoleSB.Clear();
            for (int chIndex = 0; chIndex < Math.Min(maxChannelCount, e.Dimensions[0]); chIndex++)
            {
                int columnCount = e.Dimensions[1] / avgSampleCount;
                float[] columnMins = Enumerable.Repeat<float>(Single.MaxValue, columnCount).ToArray();
                float[] columnMaxs = Enumerable.Repeat<float>(Single.MinValue, columnCount).ToArray();
                float[] columnAvgs = new float[columnCount];

                for (int x = 0; x < columnCount; x++)
                {
                    for (int sampleIndex = 0; sampleIndex < avgSampleCount; sampleIndex++)
                    {
                        float value = e.Values[chIndex, e.Dimensions[1] - ((x + 1) * avgSampleCount) + sampleIndex, 0];

                        if (value < columnMins[x]) columnMins[x] = value;
                        if (value > columnMaxs[x]) columnMaxs[x] = value;
                        columnAvgs[x] += value;
                    }
                    columnAvgs[x] /= avgSampleCount;
                }

                float peakToPeakValue = (columnMaxs.Max() - columnMins.Min());
                if (peakToPeakValues[chIndex] == null)
                {
                    peakToPeakValues[chIndex] = new float[25 * 3];
                }
                peakToPeakValues[chIndex][peakToPeakValueCounter] = peakToPeakValue;

                consoleSB.AppendLine($"=== channel {chIndex} (min: {columnMins.Min().ToString("+0.0000;-0.0000; 0.0000")}, max: {columnMaxs.Max().ToString("+0.0000;-0.0000; 0.0000")}, peak-to-peak: {peakToPeakValue.ToString("0.0000")} [avg: {peakToPeakValues[chIndex].Average().ToString("0.0000")}, {peakToPeakValues[chIndex].Min().ToString("0.0000")} - {peakToPeakValues[chIndex].Max().ToString("0.0000")}])       ");
                for (int y = height - 1; y >= 0; y--)
                {
                    float maxValue = ((2.0f / height) * (y + 1)) - 1.0f;
                    float minValue = ((2.0f / height) * (y)) - 1.0f;

                    char[] line = new char[columnCount];
                    for (int x = 0; x < columnCount; x++)
                    {
                        line[x] = ' ';
                        if ((0 >= minValue) && (0 <= maxValue)) line[x] = '-';
                        if ((columnMaxs[x] >= minValue) && (columnMins[x] <= maxValue)) line[x] = '|';
                        //if ((columnMins[x] >= minValue) && (columnMins[x] <= maxValue)) line[x] = '-';
                        //if ((columnMaxs[x] >= minValue) && (columnMaxs[x] <= maxValue)) line[x] = '+';
                        if ((columnAvgs[x] >= minValue) && (columnAvgs[x] <= maxValue)) line[x] = '*';
                    }
                    consoleSB.Append(line);
                    consoleSB.AppendLine();
                }
            }

            return consoleSB.ToString();
        }

        private static string GenerateConsoleTitle()
        {
            // Console Title
            double timeElapsed = (DateTime.UtcNow - firstActivity).TotalSeconds;
            string consoleTitle = $"{SessionId}";
            if (waveSource is BBD8x8MatrixInput)
            {
                BBD8x8MatrixInput bbdInput = (BBD8x8MatrixInput)waveSource;

                consoleTitle += $" - {(bbdInput.COMPortBytesReceived / timeElapsed / 1024).ToString("0.00")} kbytes/sec - {((double)bbdInput.LLCommandReceived / timeElapsed).ToString("0.00")} fps";
            }
            if (waveSource is BBDMercury16Input)
            {
                BBDMercury16Input bbdInput = (BBDMercury16Input)waveSource;

                lock (bbdInput.BenchmarkEntries)
                {
                    if (bbdInput.BenchmarkEntries.Count > 0)
                    {
                        double time = (bbdInput.BenchmarkEntries.Max(be => be.TimeStamp) - bbdInput.BenchmarkEntries.Min(be => be.TimeStamp)).TotalSeconds;
                        double speed = (time <= 0 ? 0 : bbdInput.BenchmarkEntries.Sum(be => be.BytesTransferred) / time);
                        int samples = bbdInput.BenchmarkEntries.Sum(be => be.SamplesTransferred);
                        consoleTitle += $" - {(speed / 1024).ToString("0.00")} kbytes/sec - {(samples / time).ToString("#,0")} Hz";
                    }
                }
            }
            if (recentValues != null)
            {
                consoleTitle += $" - sensibility { recentChangesSensitivity.ToString("0.0000")}";
            }

            if (wfo != null)
            {
                consoleTitle += $" - wav file: {(waveFileBytesWritten / 1024).ToString("#,0")} kbytes (jitter: {wfo.BufferJitter} samples, overflow warnings: {waveDataOverflowWarningCount})";
            }

            if (vtsfo != null)
            {
                consoleTitle += $" - vts file: {(vtsFileBytesWritten / 1024).ToString("#,0")} kbytes (overflow warnings: {vtsDataOverflowWarningCount})";
            }

            consoleTitle += "     ";

            return consoleTitle;
        }

        private static string GenerateMatrixOutput(RefreshVisualOutputEventArgs e, bool showChanges)
        {
            string[,,] valueStrings = new string[e.Dimensions[0], e.Dimensions[1], e.Dimensions[2]];
            double sum = 0;

            for (int z = 0; z < e.Dimensions[2]; z++)
            {
                for (int y = 0; y < e.Dimensions[1]; y++)
                {
                    for (int x = 0; x < e.Dimensions[0]; x++)
                    {
                        float value = e.Values[x, y, z];

                        // A, show values
                        valueStrings[x, y, z] = value.ToString(" +0.0000; -0.0000");


                        // B, show changes
                        if ((showChanges) && (recentValues != null))
                        {
                            if ((e.Values[x, y, z] == 0) || (recentValues[x, y, z] == 0))
                            {
                                valueStrings[x, y, z] = "o";
                            }
                            else
                            {
                                float diff = e.Values[x, y, z] - recentValues[x, y, z];
                                recentDiffsIndex = (recentDiffsIndex + 1) % recentDiffs.Length;
                                recentDiffs[recentDiffsIndex] = (diff < 0 ? -diff : diff);
                                float diffAverage = recentDiffs.Average();

                                if (diff > 3 * diffAverage)
                                {
                                    valueStrings[x, y, z] = "+++";
                                }
                                else if (diff > 2 * diffAverage)
                                {
                                    valueStrings[x, y, z] = "++";
                                }
                                else if (diff > 1 * diffAverage)
                                {
                                    valueStrings[x, y, z] = "+";
                                }

                                if (diff < -1 * diffAverage)
                                {
                                    valueStrings[x, y, z] = "-";
                                }
                                else if (diff < -2 * diffAverage)
                                {
                                    valueStrings[x, y, z] = "--";
                                }
                                else if (diff < -3 * diffAverage)
                                {
                                    valueStrings[x, y, z] = "---";
                                }

                                if ((diff > -1 * diffAverage) && (diff < diffAverage))
                                {
                                    valueStrings[x, y, z] = "";
                                }
                            }
                        }

                        sum += value;
                    }
                }
            }

            sum /= e.Dimensions[0] * e.Dimensions[1] * e.Dimensions[2];

            // Console Content
            consoleSB.Clear();
            consoleSB.AppendLine($"Sum: {sum.ToString(" +0.0000; -0.0000")}");
            for (int z = 0; z < e.Dimensions[2]; z++)
            {
                consoleSB.AppendLine($"=== layer {z}");
                consoleSB.Append("+");
                for (int x = 0; x < e.Dimensions[0]; x++)
                {
                    consoleSB.Append("--------+");
                }
                consoleSB.AppendLine();

                for (int y = 0; y < e.Dimensions[1]; y++)
                {
                    consoleSB.Append("|");
                    for (int x = 0; x < e.Dimensions[0]; x++)
                    {
                        consoleSB.Append(String.Format("{0,8}|", valueStrings[x, y, z]));
                    }
                    consoleSB.AppendLine();

                    consoleSB.Append("+");
                    for (int x = 0; x < e.Dimensions[0]; x++)
                    {
                        consoleSB.Append("--------+");
                    }
                    consoleSB.AppendLine();
                }
            }

            if (showChanges)
            {
                if ((recentValues == null) || (recentValues.GetLength(0) != e.Values.GetLength(0)) || (recentValues.GetLength(1) != e.Values.GetLength(1)) || (recentValues.GetLength(2) != e.Values.GetLength(2)))
                {
                    recentValues = new float[e.Values.GetLength(0), e.Values.GetLength(1), e.Values.GetLength(2)];
                }

                for (int i = 0; i < e.Values.GetLength(0); i++)
                {
                    for (int j = 0; j < e.Values.GetLength(1); j++)
                    {
                        for (int k = 0; k < e.Values.GetLength(2); k++)
                        {
                            recentValues[i, j, k] = e.Values[i, j, k];
                        }
                    }
                }
            }

            return consoleSB.ToString();
        }

        private static string GenerateDominanceMatrixOutput(RefreshVisualOutputEventArgs e, int maxLayers, bool colorDisplay)
        {
            string[,,] valueStrings = new string[e.Dimensions[0], e.Dimensions[1], e.Dimensions[2] / 2];
            ConsoleColor[,,] valueColors = new ConsoleColor[e.Dimensions[0], e.Dimensions[1], e.Dimensions[2] / 2];

            for (int z = 0; z < e.Dimensions[2] / 2; z++)
            {
                for (int y = 0; y < e.Dimensions[1]; y++)
                {
                    for (int x = 0; x < e.Dimensions[0]; x++)
                    {
                        float frequency = e.Values[x, y, z * 2 + 0];
                        float value = e.Values[x, y, z * 2 + 1];

                        if (colorDisplay)
                        {
                            byte[] rgb = ColorConverter.HSL2RGB(frequency / 2000.0f, 0.5, 0.5);
                            valueColors[x, y, z] = ColorConverter.ClosestConsoleColor(rgb[0], rgb[1], rgb[2]);
                        }

                        valueStrings[x, y, z] = $"{frequency.ToString("0.0")}Hz ({value.ToString(" +0.0000; -0.0000")})";
                    }
                }
            }

            // Console Content
            consoleSB.Clear();

            int displayLayers = Math.Min(maxLayers, e.Dimensions[2] / 2);
            for (int z = 0; z < displayLayers; z++)
            {
                PrintDominanceMatrixUnit(consoleSB, $"=== top {z+1} frequency", true, colorDisplay);
                PrintDominanceMatrixUnit(consoleSB, "+", false, colorDisplay);
                for (int x = 0; x < e.Dimensions[0]; x++)
                {
                    PrintDominanceMatrixUnit(consoleSB, "------------------------+", false, colorDisplay);
                }
                PrintDominanceMatrixUnit(consoleSB, null, true, colorDisplay);

                for (int y = 0; y < e.Dimensions[1]; y++)
                {
                    PrintDominanceMatrixUnit(consoleSB, "|", false, colorDisplay);
                    for (int x = 0; x < e.Dimensions[0]; x++)
                    {
                        PrintDominanceMatrixUnit(consoleSB, String.Format("{0,24}|", valueStrings[x, y, z]), false, colorDisplay, valueColors[x, y, z]);
                    }
                    PrintDominanceMatrixUnit(consoleSB, null, true, colorDisplay);

                    PrintDominanceMatrixUnit(consoleSB, "+", false, colorDisplay);
                    for (int x = 0; x < e.Dimensions[0]; x++)
                    {
                        PrintDominanceMatrixUnit(consoleSB, "------------------------+", false, colorDisplay);
                    }
                    PrintDominanceMatrixUnit(consoleSB, null, true, colorDisplay);
                }
            }

            return consoleSB.ToString();
        }

        private static string GenerateGoertzelTableOutput(RefreshVisualOutputEventArgs e)
        {
            BBDMercury16Input bbdMercury16Input = waveSource as BBDMercury16Input;

            // Console Content
            consoleSB.Clear();

            if (bbdMercury16Input != null)
            {
                for (int c = 0; c < bbdMercury16Input.GoertzelOutputs.Length; c++)
                {
                    if ((bbdMercury16Input.GoertzelOutputs[c] == null) || (bbdMercury16Input.GoertzelOutputs[c].Count % 3 != 0)) continue;

                    lock (bbdMercury16Input.GoertzelOutputs[c])
                    {
                        consoleSB.AppendLine($"---- channel {c+1}");
                        float[] relevantValues = bbdMercury16Input.GoertzelOutputs[c].Where((x, i) => (i % 3 == 0) && (i > bbdMercury16Input.GoertzelOutputs[c].Count - 28)).ToArray();
                        consoleSB.AppendLine($"- freq: {String.Format("{0, 10:##0.00}", bbdMercury16Input.GoertzelFrequency01)} Hz    values: {String.Join(" |", relevantValues.Select(v => String.Format("{0, 8:##0.00}", Math.Max(Math.Min(v, 9999.99), -9999.99))))}          ");
                        relevantValues = bbdMercury16Input.GoertzelOutputs[c].Where((x, i) => (i % 3 == 1) && (i > bbdMercury16Input.GoertzelOutputs[c].Count - 28)).ToArray();
                        consoleSB.AppendLine($"- freq: {String.Format("{0, 10:##0.00}", bbdMercury16Input.GoertzelFrequency02)} Hz    values: {String.Join(" |", relevantValues.Select(v => String.Format("{0, 8:##0.00}", Math.Max(Math.Min(v, 9999.99), -9999.99))))}          ");
                        relevantValues = bbdMercury16Input.GoertzelOutputs[c].Where((x, i) => (i % 3 == 2) && (i > bbdMercury16Input.GoertzelOutputs[c].Count - 28)).ToArray();
                        consoleSB.AppendLine($"- freq: {String.Format("{0, 10:##0.00}", bbdMercury16Input.GoertzelFrequency03)} Hz    values: {String.Join(" |", relevantValues.Select(v => String.Format("{0, 8:##0.00}", Math.Max(Math.Min(v, 9999.99), -9999.99))))}          ");
                        consoleSB.AppendLine();
                    }
                }
            }

            return consoleSB.ToString();
        }

        private static void PrintDominanceMatrixUnit(StringBuilder consoleSB, string s, bool addNewLine = false, bool colorDisplay = false, ConsoleColor? color = null)
        {
            if (!colorDisplay)
            {
                if (addNewLine)
                {
                    consoleSB.AppendLine(s);
                }
                else
                {
                    consoleSB.Append(s);
                }
            } else
            {
                ConsoleColor cc = Console.ForegroundColor;
                if (color.HasValue) Console.ForegroundColor = color.Value;

                if (addNewLine)
                {
                    Console.WriteLine(s);
                }
                else
                {
                    Console.Write(s);
                }

                Console.ForegroundColor = cc;
            }
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
