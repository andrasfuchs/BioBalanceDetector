using System;
using Unosquare.RaspberryIO;
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
            Console.Write("Waiting for debugger to attach... ");
            while (true)
            {
                Console.Write(++i + " ");
                if (System.Diagnostics.Debugger.IsAttached) break; 
                System.Threading.Thread.Sleep(1000); 
            }
            Console.WriteLine();
        }
    }
}
