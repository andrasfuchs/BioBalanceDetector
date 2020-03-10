using System;
using System.Linq;
using System.Threading;
using System.Device.Spi;
using Iot.Device.Ad7193;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;
using Unosquare.PiGpio.ManagedModel;

namespace Mars_64
{
	class Program
	{
		private const int PI_3_SPI_SPEED_MIN = 320000;
		private const int PI_3_SPI_SPEED_MAX = 20480000;

		private static Ad7193 ad7193;

		static void Main(string[] args)
		{
			WaitForDebugger();

			//InitUnosquare();

			// set SPI bus ID: 0
			// AD7193 CS Pin: 1
			SpiConnectionSettings settings = new SpiConnectionSettings(0, 1);
			settings.Mode = SpiMode.Mode3;
			SpiDevice ad7193SpiDevice = SpiDevice.Create(settings);

			ad7193 = new Ad7193(ad7193SpiDevice);

			Console.WriteLine($"-- Resetting and calibrating AD7193.");
			ad7193.Reset();
			ad7193.Calibrate();
			ad7193.SetPGAGain(Ad7193.Gain.X1);
			ad7193.SetPsuedoDifferentialInputs(true);
			ad7193.AppendStatusRegisterToData = true;


			Ad7193.Channel ch = Ad7193.Channel.CH07;
			Console.WriteLine($"-- Setting channel to {ch}.");
			ad7193.SetChannel(ch);


			DateTime firstDataRead = DateTime.UtcNow;
			int samplesRead = 0;
			while (true)
			{
				while (ad7193.HasErrors)
				{
					Console.WriteLine("!! ERROR !!");
					Console.WriteLine($"AD7193 status: {ad7193.Status}");
					Console.WriteLine($"AD7193 mode: {ad7193.Mode}");
					Console.WriteLine($"AD7193 config: {ad7193.Config}");
					Thread.Sleep(500);
				}


				ad7193.StartSingleConversion();
				ad7193.WaitForADC();
				uint adcValue = ad7193.ReadADCValue();

				samplesRead++;
				if (samplesRead % 100 == 0)
				{
					Console.WriteLine();
					Console.WriteLine($"AD7193 status: {ad7193.Status}");
					Console.WriteLine($"AD7193 mode: {ad7193.Mode}");
					Console.WriteLine($"AD7193 config: {ad7193.Config}");
					Console.WriteLine($"ADC value on channel {ch}: {ad7193.ADCValueToVoltage(adcValue).ToString("0.0000")} V [{adcValue.ToString("N0")}] | sample rate: {((double)samplesRead / (DateTime.UtcNow - firstDataRead).TotalSeconds).ToString("N1")} SPS");
				}
			}
		}

		private static void WaitForDebugger()
		{
			int i = 0;
			Console.WriteLine("Waiting for the debugger to attach... ");
			while (true)
			{
				Console.WriteLine(++i + " ");
				if (System.Diagnostics.Debugger.IsAttached) break;
				System.Threading.Thread.Sleep(1000);
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
