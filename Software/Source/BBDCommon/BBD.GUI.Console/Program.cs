using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBD.GUI.Console
{
    class Program
    {
        private static BBD.DSP.Program.DataDisplayModes currentMode = BBD.DSP.Program.DataDisplayModes.None;

        static void Main(string[] args)
        {
            ConsoleKeyInfo cki = new ConsoleKeyInfo();
            currentMode = BBD.DSP.Program.DataDisplayModes.GoertzelValues;

            while ((cki.Key != ConsoleKey.Q) && (cki.Key != ConsoleKey.Escape))
            {
                System.Console.Clear();
                BBD.DSP.Program.StartSession(currentMode);
                cki = System.Console.ReadKey();
                BBD.DSP.Program.StopSession();

                if (cki.Key == ConsoleKey.D0)
                {
                    currentMode = BBD.DSP.Program.DataDisplayModes.None;
                }

                if (cki.Key == ConsoleKey.D1)
                {
                    currentMode = BBD.DSP.Program.DataDisplayModes.NormalizedWaveform;
                }

                if (cki.Key == ConsoleKey.D2)
                {
                    currentMode = BBD.DSP.Program.DataDisplayModes.FilteredSpectogram;
                }

                if (cki.Key == ConsoleKey.D3)
                {
                    currentMode = BBD.DSP.Program.DataDisplayModes.DominanceMatrix;
                }

                if (cki.Key == ConsoleKey.D4)
                {
                    currentMode = BBD.DSP.Program.DataDisplayModes.GoertzelValues;
                }
            }
        }
    }
}
