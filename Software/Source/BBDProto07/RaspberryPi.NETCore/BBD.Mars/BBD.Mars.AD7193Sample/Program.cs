﻿using System;
using System.Threading;
using System.Device.Spi;
using System.Diagnostics;
using BBD.Mars.Iot.Device.Ad7193;

namespace BBD.Mars.AD7193Sample
{
	class Program
	{
		private const int PI_3_SPI_SPEED_MIN = 320000;
		private const int PI_3_SPI_SPEED_MAX = 20480000;

		private static Ad7193 ad7193;
		private static DateTime firstDataRead;
		private static double lastChecked = 0;
		private static int lastCount = 0;
		private static int samplesTaken = 0;


		static void Main(string[] args)
		{
			WaitForDebugger();

			// set SPI bus ID: 0
			// AD7193 CS Pin: 1
			SpiConnectionSettings settings = new SpiConnectionSettings(0, 1);
			settings.ClockFrequency = Ad7193.MaximumSpiFrequency;
			settings.Mode = SpiMode.Mode3;
			SpiDevice ad7193SpiDevice = SpiDevice.Create(settings);


			ad7193 = new Ad7193(ad7193SpiDevice);
			ad7193.AdcValueReceived += Ad7193_AdcValueReceived;

			Console.WriteLine($"-- Resetting and calibrating AD7193.");
			ad7193.Reset();
			ad7193.PGAGain = Ad7193.Gain.X1;
			ad7193.Averaging = Ad7193.AveragingMode.Off;
			ad7193.InputMode = Ad7193.AnalogInputMode.EightPseudoDifferentialAnalogInputs;
			ad7193.AppendStatusRegisterToData = true;
			ad7193.JitterCorrection = true;
			ad7193.Filter = 0;

			Console.WriteLine($"AD7193 before calibration: offset={ad7193.Offset.ToString("X8")}, full-scale={ad7193.FullScale.ToString("X8")}");
			ad7193.Calibrate();
			Console.WriteLine($"AD7193  after calibration: offset={ad7193.Offset.ToString("X8")}, full-scale={ad7193.FullScale.ToString("X8")}");


			Console.WriteLine("Starting 100 single conversions on CH0...");
			ad7193.ActiveChannels = Ad7193.Channel.CH00;

			for (int i = 0; i < 100; i++)
			{
				ad7193.StartSingleConversion();
				ad7193.WaitForADC();
				ad7193.ReadADCValue();
				Thread.Sleep(25);
			}

			Thread.Sleep(1000);


			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine("Starting continuous conversion on CH0 and CH1...");
			ad7193.ActiveChannels = Ad7193.Channel.CH00 | Ad7193.Channel.CH01;
			ad7193.StartContinuousConversion();

			int loopcounter = 0;
			while (true)
			{
				loopcounter++;
				if (ad7193.HasErrors || (loopcounter % 50 == 0))
				{
					Console.WriteLine();
					Console.WriteLine($"AD7193 status: {ad7193.Status}");
					Console.WriteLine($"AD7193 mode: {ad7193.Mode}");
					Console.WriteLine($"AD7193 config: {ad7193.Config}");
					Console.WriteLine();
					Thread.Sleep(1500);
				}
				Thread.Sleep(250);
			}
		}

		private static void Ad7193_AdcValueReceived(object sender, Iot.Device.Ad7193.AdcValueReceivedEventArgs e)
		{
			if (firstDataRead == DateTime.MinValue) firstDataRead = DateTime.UtcNow;
			double secondsElapsed = (DateTime.UtcNow - firstDataRead).TotalSeconds;

			samplesTaken++;

			// show the results in every 0.25 seconds
			if (secondsElapsed - lastChecked > 0.25)
			{
				double sps = (double)(samplesTaken - lastCount) / (secondsElapsed - lastChecked);

				lastCount = samplesTaken;
				lastChecked = secondsElapsed;

				Iot.Device.Ad7193.AdcValue adcValue = e.AdcValue;

				Console.WriteLine($"Channel {adcValue.Channel.ToString().PadLeft(2)}: {adcValue.Voltage.ToString("0.0000").PadLeft(11)} V | {adcValue.Raw.ToString("N0").PadLeft(13)} | {sps.ToString("N1").PadLeft(9)} SPS");
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
				if (Debugger.IsAttached) break;
				Thread.Sleep(1000);

				if (i > 30) break;
			}
			Console.WriteLine();
		}
	}
}
 