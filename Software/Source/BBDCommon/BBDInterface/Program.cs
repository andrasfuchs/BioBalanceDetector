using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBDInterface
{
    class Program
    {
        private static BBDDriver.Program.DataDisplayModes currentMode = BBDDriver.Program.DataDisplayModes.None;

        static void Main(string[] args)
        {
            ConsoleKeyInfo cki = new ConsoleKeyInfo();
            currentMode = BBDDriver.Program.DataDisplayModes.GoertzelValues;

            while ((cki.Key != ConsoleKey.Q) && (cki.Key != ConsoleKey.Escape))
            {
                Console.Clear();
                BBDDriver.Program.StartSession(currentMode);
                cki = Console.ReadKey();
                BBDDriver.Program.StopSession();

                if (cki.Key == ConsoleKey.D0)
                {
                    currentMode = BBDDriver.Program.DataDisplayModes.None;
                }

                if (cki.Key == ConsoleKey.D1)
                {
                    currentMode = BBDDriver.Program.DataDisplayModes.NormalizedWaveform;
                }

                if (cki.Key == ConsoleKey.D2)
                {
                    currentMode = BBDDriver.Program.DataDisplayModes.FilteredSpectogram;
                }

                if (cki.Key == ConsoleKey.D3)
                {
                    currentMode = BBDDriver.Program.DataDisplayModes.DominanceMatrix;
                }

                if (cki.Key == ConsoleKey.D4)
                {
                    currentMode = BBDDriver.Program.DataDisplayModes.GoertzelValues;
                }
            }
        }
    }
}
