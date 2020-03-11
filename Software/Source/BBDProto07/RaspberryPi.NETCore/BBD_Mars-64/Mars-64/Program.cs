using System;
using System.Linq;
using System.Threading;
using System.Device.Spi;
using Iot.Device.Ad7193;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;
using Unosquare.PiGpio.ManagedModel;
using System.Diagnostics;

namespace Mars_64
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

			InitAd7193(Ad7193.Channel.CH00, Ad7193.Gain.X1);

			uint readFrequency = 1200;

			ad7193.AdcValueReceived += Ad7193_AdcValueReceived;

			while (true)
			{
				if (readFrequency == 0)
				{
					ad7193.StartSingleConversion();
					ad7193.WaitForADC();
					ad7193.ReadADCValue();
				}
				else
				{
					if (samplesTaken == 0)
					{
						ad7193.StartContinuousConversion(readFrequency);
					}
					Thread.Sleep(250);
				}

				if (ad7193.HasErrors)
				{
					Console.WriteLine();
					Console.WriteLine("!! ERROR !!");
					ShowStatus();
					Console.WriteLine();
					Thread.Sleep(5000);
				}
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

		private static void ShowStatus()
		{
			Console.WriteLine();
			Console.WriteLine($"AD7193 status: {ad7193.Status}");
			Console.WriteLine($"AD7193 mode: {ad7193.Mode}");
			Console.WriteLine($"AD7193 config: {ad7193.Config}");
		}

		private static void InitAd7193(Ad7193.Channel ch, Ad7193.Gain gain)
		{
			Console.WriteLine($"-- Resetting and calibrating AD7193.");
			ad7193.Reset();
			ad7193.SetPGAGain(gain);
			ad7193.Calibrate();
			ad7193.SetPsuedoDifferentialInputs(false);
			ad7193.AppendStatusRegisterToData = true;
			ad7193.JitterCorrection = true;


			Console.WriteLine($"-- Setting channel to {ch} and gain to {gain}.");
			ad7193.SetChannel(ch);
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

		private static Unosquare.RaspberryIO.Peripherals.Ad7193 InitUnosquareAd7193()
		{
			if (string.IsNullOrWhiteSpace(Pi.Info.OperatingSystem.ToString()))
			{
				Console.WriteLine("ERROR: You must run this app on a Raspberry Pi");
				return null;
			}

			Console.WriteLine("Raspberry Pi OS: " + Pi.Info.OperatingSystem);

			Pi.Init<BootstrapWiringPi>();

			//IGpioPin ad7193_CE0 = Pi.Gpio[BcmPin.Gpio08];
			//ad7193_CE0.PinMode = GpioPinDriveMode.Output;

			Pi.Spi.Channel1Frequency = PI_3_SPI_SPEED_MIN;
			ISpiChannel ad7193_SPI = Pi.Spi.Channel1;

			Console.WriteLine("Speed of SPI channel 1: " + (Pi.Spi.Channel1Frequency / 1024.0).ToString("0.00") + " kHz");

			return new Unosquare.RaspberryIO.Peripherals.Ad7193(ad7193_SPI);
		}

	}
}
 