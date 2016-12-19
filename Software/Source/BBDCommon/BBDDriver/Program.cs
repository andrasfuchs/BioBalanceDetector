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
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NAudio.Wave;
using System.IO;
using BBDDriver.Models.Input;
using BBDDriver.Models.Output;
using BBDDriver.Models;
using System.Diagnostics;

namespace BBDDriver
{
    public class Program
    {
        private static string workingDirectory = @"c:\Work\BioBalanceDetector\Recordings\";

        private static string arduinoPort = "COM3";

        private static float[,,] recentValues;
        private static double recentChangesSensitivity = 1.00;

        private static DateTime firstActivity;

        private static MultiChannelInput<IDataChannel> waveSource = null;
        private static long waveFileBytesWritten = 0;

        private static string sessionId;

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

        public static void StartSession()
        {
            Console.SetOut(File.AppendText($"{workingDirectory}{SessionId}.log"));

            try
            {
                waveSource = new BBDArduinoInput(arduinoPort, 64);
            }
            catch (System.IO.IOException)
            {
                Console.WriteLine(String.Format("Arduino is not connected on port '{0}', creating random signal generator as data-source.", arduinoPort));

                // Arduino is not connected, let's create a 64-channel 16bit, 8kHz pseudo-source of data                
                waveSource = new SineInput(8000, 4);
            }

            WaveFileOutput wfo = new WaveFileOutput(waveSource, $"{workingDirectory}{SessionId}.wav");
            wfo.DataWritten += Wfo_DataWritten;

            VisualOutput vo = new VisualOutput(waveSource, 25, waveSource.ChannelCount);
            vo.RefreshVisualOutput += Vo_RefreshVisualOutput;

            firstActivity = DateTime.UtcNow;
        }

        private static void Wfo_DataWritten(object sender, DataWrittenEventArgs e)
        {
            waveFileBytesWritten = e.TotalDataWritten;
        }

        private static void Vo_RefreshVisualOutput(object sender, RefreshVisualOutputEventArgs e)
        {
            try
            {
                if (!e.ChangedSinceLastRefresh) return;

                lock (SessionId)
                {
                    string[,,] valueStrings = new string[e.Dimensions[0], e.Dimensions[1], e.Dimensions[2]];
                    double sum = 0;

                    int equalsCount = 0;

                    for (int z = 0; z < e.Dimensions[2]; z++)
                    {
                        for (int y = 0; y < e.Dimensions[1]; y++)
                        {
                            for (int x = 0; x < e.Dimensions[0]; x++)
                            {
                                float value = e.Values[x, y, z];

                                // A, show values
                                valueStrings[x, y, z] = value.ToString("0.00");

                                
                                // B, show changes
                                if (recentValues != null)
                                {
                                    if ((e.Values[x,y,z] == 0) || (recentValues[x, y, z] == 0))
                                    {
                                        valueStrings[x, y, z] = "o";
                                    }
                                    else
                                    {
                                        float diff = e.Values[x, y, z] - recentValues[x, y, z];

                                        if (diff > recentChangesSensitivity * recentChangesSensitivity * recentChangesSensitivity)
                                        {
                                            valueStrings[x, y, z] = "+++";
                                        }
                                        else if (diff > recentChangesSensitivity * recentChangesSensitivity)
                                        {
                                            valueStrings[x, y, z] = "++";
                                        }
                                        else if (diff > recentChangesSensitivity)
                                        {
                                            valueStrings[x, y, z] = "+";
                                        }

                                        if (diff < -recentChangesSensitivity)
                                        {
                                            valueStrings[x, y, z] = "-";
                                        }
                                        else if (diff < -recentChangesSensitivity * recentChangesSensitivity)
                                        {
                                            valueStrings[x, y, z] = "--";
                                        }
                                        else if (diff < -recentChangesSensitivity * recentChangesSensitivity * recentChangesSensitivity)
                                        {
                                            valueStrings[x, y, z] = "---";
                                        }

                                        if ((diff > -recentChangesSensitivity) && (diff < recentChangesSensitivity))
                                        {
                                            valueStrings[x, y, z] = "";
                                            equalsCount++;
                                        }
                                    }
                                }

                                sum += value;
                            }
                        }
                    }

                    sum /= e.Dimensions[0] * e.Dimensions[1] * e.Dimensions[2];

                    Console.WriteLine();
                    for (int z = 0; z < e.Dimensions[2]; z++)
                    {
                        Console.WriteLine($"---- layer {z} ----");
                        for (int y = 0; y < e.Dimensions[1]; y++)
                        {
                            Console.Write("|");
                            for (int x = 0; x < e.Dimensions[0]; x++)
                            {
                                Console.Write(String.Format("{0,5}|", valueStrings[x,y,z]));
                            }
                            Console.WriteLine();
                        }
                    }

                    double timeElapsed = (DateTime.UtcNow - firstActivity).TotalSeconds;
                    Console.Title = $"{SessionId} - sum: {sum.ToString("0.000")}";
                    if (waveSource is BBDArduinoInput)
                    {
                        BBDArduinoInput bbdInput = (BBDArduinoInput)waveSource;

                        Console.Title += $" - {(bbdInput.COMPortBytesReceived / timeElapsed / 1024).ToString("0.00")} kbytes/sec - {((double)bbdInput.LLCommandReceived / timeElapsed).ToString("0.00")} fps";
                    }
                    if (recentValues != null)
                    {
                        Console.Title += $" - sensibility { recentChangesSensitivity.ToString("0.0000")}";
                    }
                    Console.Title += $" - wav file: {(waveFileBytesWritten / 1024).ToString("#,0")} kbytes";


                    // comment this line to disable the feature
                    //recentValues = e.Values;
                    if (equalsCount > 60)
                    {
                        recentChangesSensitivity /= 2.0;
                    }

                    if (equalsCount < 4)
                    {
                        recentChangesSensitivity *= 2.0;
                    }
                }
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

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
