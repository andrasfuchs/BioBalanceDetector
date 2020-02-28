using System;
using System.Linq;
using System.Threading;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;

namespace Mars_64
{
	class Program
	{
		static void Main(string[] args)
		{
			if (string.IsNullOrWhiteSpace(Pi.Info.OperatingSystem.ToString()))
			{
				Console.WriteLine("ERROR: You must run this app on a Raspberry Pi");
				return;
			}

			Console.WriteLine("Raspberry Pi OS: " + Pi.Info.OperatingSystem);

			Pi.Init<BootstrapWiringPi>();

			int i = 0;
			Console.WriteLine("Waiting for the debugger to attach... ");
			while (true)
			{
				Console.WriteLine(++i + " ");
				if (System.Diagnostics.Debugger.IsAttached) break; 
				System.Threading.Thread.Sleep(1000); 
			}
			Console.WriteLine();


			IGpioPin ad7193_CE0 = Pi.Gpio[BcmPin.Gpio08];
			ad7193_CE0.PinMode = GpioPinDriveMode.Output;


			Pi.Spi.Channel0Frequency = PI_3_SPI_SPEED_MIN;
			ISpiChannel ad7193 = Pi.Spi.Channel0;

			Console.WriteLine("Speed of SPI channel 0: " + (Pi.Spi.Channel0Frequency / 1024.0).ToString("0.00") + " kHz");


			// init
			while (true)
			{
				var buffer = ad7193_get_register_value(ad7193, AD7193_REG_ID, 1, ad7193_CE0);

				Console.WriteLine("returned: " + String.Join(',', buffer.Select(b => b.ToString())));

				if ((buffer[0] & AD7193_ID_MASK) == ID_AD7193)
				{
					break;
				}

				Thread.Sleep(100);
			}
/*


			// select channel


			// do a single conversion


			// calculate voltage value


			// display the value

			/*
						var configWord = (ushort)((configDataChunk[2] << 8) + configDataChunk[1]);

						// Write config register to the ADC
						//var currentConfig = ads1115.ReadAddressWord(ADS1115.ADS1115_REG_POINTER_CONFIG);
						//Console.WriteLine($"Setting original configuration ({currentConfig.ToString("X")}) to {config.ToString("X")}");

						ads1115.WriteAddressWord(configDataChunk[0], configWord);

						//currentConfig = ads1115.ReadAddressWord(ADS1115.ADS1115_REG_POINTER_CONFIG);
						//Console.WriteLine($"New configuration: {currentConfig.ToString("X")}");

						DateTime firstDataRead = DateTime.UtcNow;
						while (true)
						{
							// Read the conversion results                
							ushort result = (ushort)(ads1115.ReadAddressWord(convertDataChunk[0]));
							float voltage = (float)(result < 32768 ? result : -(65536 - result)) * 2.048f / 65536.0f;

							j++;
							if (j % 1000 == 0)
							{
								Console.WriteLine(String.Format("| ADC value: {0,5} V | sample rate: {1,7} SPS |", voltage.ToString("N", CultureInfo.InvariantCulture), ((double)j / (DateTime.UtcNow - firstDataRead).TotalSeconds).ToString("N")));
							}
						}
			*/
		}


		private const int PI_3_SPI_SPEED_MIN		= 320000;
		private const int PI_3_SPI_SPEED_MAX		= 20480000;

		/* AD7193 Register Map */
		private const byte AD7193_REG_COMM = 0x00;		// Communications Register (WO, 8-bit)
		private const byte AD7193_REG_DATA = 0x03;		// Data Register           (RO, 24/32-bit)
		private const byte AD7193_REG_ID = 0x04;        // ID Register             (RO, 8-bit)

		/* Communications Register Bit Designations (AD7193_REG_COMM) */
		private const byte AD7193_COMM_WRITE		= (0 << 6);
		private const byte AD7193_COMM_READ			= (1 << 6);

		/* ID Register Bit Designations (AD7193_REG_ID) */
		private const byte ID_AD7193				= 0x02;
		private const byte AD7193_ID_MASK			= 0x0F;



		private static byte[] ad7193_get_register_value(ISpiChannel ad7193_dev, byte register_address, byte bytes_number, IGpioPin cs)
		{
			//byte[] buffer = new byte[bytes_number + 5];
			//byte[] buffer = { 0x08, 0x28, 0x00, 0x60, 0x00, 0x00, 0x00, 0x00, 0x00 };
			byte[] buffer = { 0x08, 0x28, 0x00, 0x60, 0x00, 0x00, 0x58, 0x00, 0x00, 0x00, 0x00 };

			//buffer[0] = (byte)(AD7193_COMM_READ | ((register_address & 0x07) << 3));

			buffer = ad7193_dev.SendReceive(buffer);

			return buffer;
		}

	}
}
