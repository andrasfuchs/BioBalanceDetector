using System;

namespace myHelloWorldApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

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
            Console.WriteLine("Raspberry Pi OS: " + Unosquare.RaspberryIO.Pi.Info.OperatingSystem);

            //Unosquare.RaspberryIO.Pi.Gpio[0].PinMode = Unosquare.RaspberryIO.Abstractions.GpioPinDriveMode.PwmOutput;
            //Unosquare.RaspberryIO.Pi.Gpio[1].PinMode = Unosquare.RaspberryIO.Abstractions.GpioPinDriveMode.PwmOutput;
            //Unosquare.RaspberryIO.Pi.Gpio[2].PinMode = Unosquare.RaspberryIO.Abstractions.GpioPinDriveMode.PwmOutput;
            //Unosquare.RaspberryIO.Pi.Gpio[3].PinMode = Unosquare.RaspberryIO.Abstractions.GpioPinDriveMode.PwmOutput;

            //Unosquare.RaspberryIO.Pi.Gpio[4].PinMode = Unosquare.RaspberryIO.Abstractions.GpioPinDriveMode.Input;

            while (true)
            {
                Console.Write(".");
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
