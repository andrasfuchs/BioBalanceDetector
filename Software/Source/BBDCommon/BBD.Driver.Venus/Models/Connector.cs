using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Device.Gpio;
using System.Globalization;
using System.Threading;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;

namespace BBD.Driver.Venus.Models
{
    public class Connector
    {
        int j = 0;
        char[] progress = new char[4] { '|', '/', '-', '\\' };

        public void Main(string[] args)
        {
            Console.WriteLine("Running .NET Core 2.2 apps on NOOBS/Ubuntu");
            Console.WriteLine("1, Install .NET Core 2.2 SDK ARM32 on Pi [https://dotnet.microsoft.com/download/dotnet-core/2.2]");
            Console.WriteLine("2, Set up remote debuging [https://github.com/OmniSharp/omnisharp-vscode/wiki/Remote-Debugging-On-Linux-Arm]");
            Console.WriteLine("3, Create a SAMBA share [https://www.raspberrypi.org/magpi/samba-file-server/]");
            Console.WriteLine("4, Build for Raspberry Pi -> dotnet publish -c debug -r \"linux-arm\"");
            Console.WriteLine("5, Sync with Pi -> use WinSCP's Commands/Keep Remote Directory up to Date function");
            Console.WriteLine("6, Run the program on Pi -> SSH on Pi -> ./myHelloWorldApp");
            Console.WriteLine("7, Attach debugger to the process on Pi -> SSH on Pi -> ./myHelloWorldApp");
            Console.WriteLine();
            Console.WriteLine("Running .NET Core 2.2 apps on Windows 10 IoT");
            Console.WriteLine("1, Add to the .csproj file: <RuntimeIdentifiers>win10-arm;linux-arm</RuntimeIdentifiers>");
            Console.WriteLine("2, Expose U drive on Win10 IoT as \\\\192.168.1.x\\c$ for Everyone");
            Console.WriteLine("3, Create a publishing profile to publish the project output to the network share");
            Console.WriteLine("4, Select the self-contained deployment mode");
            Console.WriteLine();
            Console.WriteLine("Web access: https://192.168.1.36:44333/");
            Console.WriteLine("API access: https://192.168.1.36:44333/api/");
            Console.WriteLine();

            //Console.WriteLine("Waiting 10 seconds for the debugger to be attached...");
            //Thread.Sleep(10000);


            //LedBlinking();

            ADS1115ReadValues(0x01, 0x48, 0x00);
        }

        private void LedBlinkingWiringPi()
        {
            if (string.IsNullOrWhiteSpace(Pi.Info.OperatingSystem.ToString()))
            {
                Console.WriteLine("ERROR: You must run this app on a Raspberry Pi");
                return;
            }

            Console.WriteLine("Raspberry Pi OS: " + Pi.Info.OperatingSystem);

            Pi.Init<BootstrapWiringPi>();

            var blinkingPin = Pi.Gpio[17];
            blinkingPin.PinMode = GpioPinDriveMode.Output;

            while (true)
            {
                blinkingPin.Write(j % 2 == 0);
                Console.Write(progress[j++ % 4]);
                System.Threading.Thread.Sleep(100);
            }
        }

        private void LedBlinkingNative()
        {
            System.Device.Gpio.GpioController controller = new System.Device.Gpio.GpioController();
            controller.OpenPin(17);
            controller.SetPinMode(17, PinMode.Output);

            while (true)
            {
                controller.Write(17, j % 2 == 0);
                Console.Write(progress[j++ % 4]);
                System.Threading.Thread.Sleep(100);
            }
        }

        private void ADS1115ReadValues(byte busId, byte i2cAddress, int channel)
        {
            // Start with default values
            int config = ADS1115.ADS1115_REG_CONFIG_CQUE_NONE | // Disable the comparator (default val)
                            ADS1115.ADS1115_REG_CONFIG_CLAT_NONLAT | // Non-latching (default val)
                            ADS1115.ADS1115_REG_CONFIG_CPOL_ACTVLOW | // Alert/Rdy active low   (default val)
                            ADS1115.ADS1115_REG_CONFIG_CMODE_TRAD | // Traditional comparator (default val)
                            ADS1115.ADS1115_REG_CONFIG_DR_860SPS |
                            ADS1115.ADS1115_REG_CONFIG_MODE_CONTIN;

            // Set PGA/voltage range
            config |= ADS1115.ADS1115_REG_CONFIG_PGA_2_048V;

            // Set single-ended input channel
            switch (channel)
            {
                case (-2):
                    config |= ADS1115.ADS1115_REG_CONFIG_MUX_DIFF_2_3;
                    break;
                case (-1):
                    config |= ADS1115.ADS1115_REG_CONFIG_MUX_DIFF_0_1;
                    break;
                case (0):
                    config |= ADS1115.ADS1115_REG_CONFIG_MUX_SINGLE_0;
                    break;
                case (1):
                    config |= ADS1115.ADS1115_REG_CONFIG_MUX_SINGLE_1;
                    break;
                case (2):
                    config |= ADS1115.ADS1115_REG_CONFIG_MUX_SINGLE_2;
                    break;
                case (3):
                    config |= ADS1115.ADS1115_REG_CONFIG_MUX_SINGLE_3;
                    break;
            }

            // Set 'start single-conversion' bit
            config |= ADS1115.ADS1115_REG_CONFIG_OS_SINGLE;

            byte[] configDataChunk = new byte[] { ADS1115.ADS1115_REG_POINTER_CONFIG, (byte)(config >> 8), (byte)(config & 0xFF) };
            byte[] convertDataChunk = new byte[] { ADS1115.ADS1115_REG_POINTER_CONVERT };

            ADS1115ReadValuesNative(busId, i2cAddress, configDataChunk, convertDataChunk);
            ADS1115ReadValuesWiringPi(busId, i2cAddress, configDataChunk, convertDataChunk);
        }

        private void ADS1115ReadValuesNative(byte busId, byte i2cAddress, byte[] configDataChunk, byte[] convertDataChunk)
        {
            System.Device.I2c.I2cConnectionSettings address = new System.Device.I2c.I2cConnectionSettings(busId, i2cAddress);
            System.Device.I2c.Drivers.UnixI2cDevice ads1115 = new System.Device.I2c.Drivers.UnixI2cDevice(address);
            var buffer = new byte[2];

            // Write config register to the ADC
            ads1115.Write(configDataChunk);

            ads1115.WriteByte(ADS1115.ADS1115_REG_POINTER_CONFIG);
            ads1115.Read(buffer);
            //var currentConfig = ToWord(buffer);
            //Console.WriteLine($"New configuration: {currentConfig.ToString("X")}");

            DateTime firstDataRead = DateTime.UtcNow;
            while (true)
            {
                // Read the conversion results
                ads1115.Write(convertDataChunk);
                ads1115.Read(buffer);
                ushort result = ToWord(buffer);
                float voltage = (float)(result < 32768 ? result : -(65536 - result)) * 2.048f / 65536.0f;

                j++;
                if (j % 1000 == 0)
                {
                    Console.WriteLine(String.Format("| ADC value: {0,5} V | sample rate: {1,7} SPS |", voltage.ToString("N", CultureInfo.InvariantCulture), ((double)j / (DateTime.UtcNow - firstDataRead).TotalSeconds).ToString("N")));
                }
            }
        }

        private ushort ToWord(byte[] buffer)
        {
            return (ushort)((buffer[1] << 8) + buffer[0]);
        }

        private void ADS1115ReadValuesWiringPi(byte busId, byte i2cAddress, byte[] configDataChunk, byte[] convertDataChunk)
        {
            Pi.Init<BootstrapWiringPi>();

            II2CDevice ads1115 = Pi.I2C.AddDevice(i2cAddress);

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
        }
    }
}
