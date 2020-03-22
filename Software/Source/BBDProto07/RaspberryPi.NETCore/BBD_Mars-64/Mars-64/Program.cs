using System;
using System.Threading;
using System.Device.Spi;
using System.Diagnostics;
using Bbd.Mars64.Iot.Device.Ad7193;

namespace Bbd.Mars64.AD7193Sample
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

			//InitUnosquare();

			// set SPI bus ID: 0
			// AD7193 CS Pin: 1
			SpiConnectionSettings settings = new SpiConnectionSettings(0, 1);
			settings.ClockFrequency = Ad7193.MaximumSpiFrequency;
			settings.Mode = SpiMode.Mode3;
			SpiDevice ad7193SpiDevice = SpiDevice.Create(settings);


			ad7193 = new Ad7193(ad7193SpiDevice);

			Console.WriteLine($"-- Resetting and calibrating AD7193.");
			ad7193.Reset();
			ad7193.SetPGAGain(Ad7193.Gain.X1);
			ad7193.Calibrate();
			ad7193.SetPsuedoDifferentialInputs(false);
			ad7193.AppendStatusRegisterToData = true;
			ad7193.JitterCorrection = true;

			ad7193.SetChannel(Ad7193.Channel.CH00 | Ad7193.Channel.CH01);
			ad7193.AdcValueReceived += Ad7193_AdcValueReceived;
			//ad7193.StartContinuousConversion();
			ad7193.StartSingleConversion();

			while (true)
			{
				if (ad7193.HasErrors)
				{
					Console.WriteLine();
					Console.WriteLine("!! ERROR !!");
					Console.WriteLine();
					Console.WriteLine($"AD7193 status: {ad7193.Status}");
					Console.WriteLine($"AD7193 mode: {ad7193.Mode}");
					Console.WriteLine($"AD7193 config: {ad7193.Config}");
					Console.WriteLine();
					Thread.Sleep(5000);
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

				Console.WriteLine($"ADC value on channel {adcValue.Channel}: {adcValue.Voltage.ToString("0.0000").PadLeft(9)} V [{adcValue.Raw.ToString("N0").PadLeft(13)}] | sample rate: {sps.ToString("N1")} SPS");
			}
		}

		private static void WaitForDebugger()
		{
			int i = 0;
			Console.WriteLine("Waiting for the debugger to attach... ");
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
 