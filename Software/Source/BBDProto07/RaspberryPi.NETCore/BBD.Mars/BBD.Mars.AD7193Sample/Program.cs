using System;
using System.Collections.Generic;
using System.Device.Spi;
using System.Diagnostics;
using System.Threading;
using Iot.Device.Ad7193;

namespace BBD.Mars.AD7193Sample
{
    internal class Program
    {
        private static Iot.Device.Ad7193.Ad7193 ad7193;
        private static DateTime firstDataRead;
        private static double lastChecked = 0;
        private static int lastCount = 0;
        private static int samplesTaken = 0;

        private static int udpSinkPayload = 1472;
        private static string[] udpSinkAddresses = { "127.0.0.1:8843", "192.168.1.36:8843", "192.168.1.30:8843" };

        private static List<NetCoreServer.UdpClient> udpSinks = new List<NetCoreServer.UdpClient>();
        private static List<List<byte>> udpSinkBuffers = new List<List<byte>>();

        public static void Main()
        {
            WaitForDebugger();

            // set SPI bus ID: 0
            // AD7193 CS Pin: 1
            SpiConnectionSettings settings = new SpiConnectionSettings(0, 1)
            {
                ClockFrequency = ((ISpiDeviceMetadata)Iot.Device.Ad7193.Ad7193.GetDeviceMetadata()).MaximumSpiFrequency,
                Mode = SpiMode.Mode3
            };
            SpiDevice ad7193SpiDevice = SpiDevice.Create(settings);

            ad7193 = new Iot.Device.Ad7193.Ad7193(ad7193SpiDevice);
            ad7193.OnValueReceived += Ad7193_AdcValueReceived;

            Console.WriteLine($"-- Resetting and calibrating AD7193.");
            ad7193.Reset();
            ad7193.PGAGain = Gain.X1;
            ad7193.Averaging = AveragingMode.Off;
            ad7193.InputMode = AnalogInputMode.EightPseudoDifferentialAnalogInputs;
            ad7193.AppendStatusRegisterToData = true;
            ad7193.JitterCorrection = true;
            ad7193.Filter = 0;

            Console.WriteLine($"AD7193 before calibration: offset={ad7193.Offset:X8}, full-scale={ad7193.FullScale:X8}");
            ad7193.Calibrate();
            Console.WriteLine($"AD7193  after calibration: offset={ad7193.Offset:X8}, full-scale={ad7193.FullScale:X8}");

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Starting 100 single conversions on CH0...");

            for (int i = 0; i < 100; i++)
            {
                ad7193.ReadSingleADCValue(Channel.CH00);
                Thread.Sleep(25);
            }

            Thread.Sleep(1000);

            StartUdpSinks(udpSinkAddresses);
            Thread.Sleep(1000);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Starting continuous conversion on CH0 and CH1...");
            //ad7193.StartContinuousConversion(Channel.CH00 | Channel.CH01);
            ad7193.StartContinuousConversion(Channel.CH00);

            int loopcounter = 0;
            while (true)
            {
                loopcounter++;
                if (ad7193.HasErrors || (loopcounter % 50 == 0))
                {
                    Console.WriteLine();
                    Console.WriteLine($"AD7193 status: {ad7193.RegisterToString(Register.Status)}");
                    Console.WriteLine($"AD7193 mode: {ad7193.RegisterToString(Register.Mode)}");
                    Console.WriteLine($"AD7193 config: {ad7193.RegisterToString(Register.Configuration)}");
                    Console.WriteLine();
                    Thread.Sleep(1500);
                }

                Thread.Sleep(250);
            }
        }

        private static void Ad7193_AdcValueReceived(object sender, Iot.Device.Ad7193.AdcValueReceivedEventArgs e)
        {
            if (firstDataRead == DateTime.MinValue)
            {
                firstDataRead = DateTime.UtcNow;
            }

            double secondsElapsed = (DateTime.UtcNow - firstDataRead).TotalSeconds;

            samplesTaken++;

            // show the results in every 0.25 seconds
            if (secondsElapsed - lastChecked > 0.25)
            {
                double sps = (double)(samplesTaken - lastCount) / (secondsElapsed - lastChecked);

                lastCount = samplesTaken;
                lastChecked = secondsElapsed;

                Iot.Device.Ad7193.AdcValue adcValue = e.AdcValue;

                Console.WriteLine($"Channel {adcValue.Channel,-2}: {adcValue.Voltage,-11:0.0000} V | {adcValue.Raw,-13:N0} | {sps,-9:N1} SPS");
            }

            for (int i = 0; i < udpSinks.Count; i++)
            {
                if (udpSinks[i] != null)
                {
                    udpSinkBuffers[i].AddRange(BitConverter.GetBytes((float)e.AdcValue.Voltage));
                    if (udpSinkBuffers[i].Count >= udpSinkPayload)
                    {
                        //Console.WriteLine($"Sending {udpSinkBuffers[i].Count} bytes of data to {udpSinks[i].Endpoint.Address}:{udpSinks[i].Endpoint.Port}...");
                        try
                        {
                            udpSinks[i].Send(udpSinkBuffers[i].ToArray());
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error on {udpSinks[i].Endpoint.Address}:{udpSinks[i].Endpoint.Port} UDP endpoint - {ex.Message}");
                            udpSinks[i].Reconnect();
                        }
                        udpSinkBuffers[i].Clear();
                    }
                }
            }

        }

        private static void WaitForDebugger()
        {
            int i = 0;
            Console.WriteLine("Waiting for the debugger to attach for 30 seconds... ");
            Console.WriteLine("(To attach the debugger in Visual Studio, press Ctrl+Alt+P, select SSH, set the IP address of the Raspberry Pi, enter your credentials, select the process, and click Attach. Press Shift+Alt+P next time.) ");
            while (true)
            {
                Console.WriteLine(++i + " ");
                if (Debugger.IsAttached)
                {
                    break;
                }

                Thread.Sleep(1000);

                if (i > 30)
                {
                    break;
                }
            }

            Console.WriteLine();
        }        

        private static void StartUdpSinks(string[] udpSinkAddresses)
        {
            udpSinks.Clear();

            foreach (string sinkAddress in udpSinkAddresses)
            {
                Console.WriteLine("Starting an UDP Sink to " + sinkAddress + "...");

                string address = sinkAddress.Split(':')[0];
                int port = Int32.Parse(sinkAddress.Split(':')[1]);

                var client = new NetCoreServer.UdpClient(address, port);
                client.OptionExclusiveAddressUse = false;
                client.OptionReuseAddress = false;
                client.OptionSendBufferSize = udpSinkPayload;
                client.Connect();
                
                udpSinks.Add(client);
                udpSinkBuffers.Add(new List<byte>());
                //udpSink.Start();
            }
        }
    }
}
