using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BBD.DSP.Models
{
    public class HighResolutionTimer
    {
        private bool isPerfCounterSupported = false;
        private Int64 frequency = 0;
        private long startTime, stopTime;

        // Windows CE native library with QueryPerformanceCounter().
        private const string lib = "kernel32.dll";
        [DllImport(lib)]
        private static extern int QueryPerformanceCounter(ref Int64 count);
        [DllImport(lib)]
        private static extern int QueryPerformanceFrequency(ref Int64 frequency);

        public HighResolutionTimer()
        {
            startTime = 0;
            stopTime = 0;

            // Query the high-resolution timer only if it is supported.
            // A returned frequency of 1000 typically indicates that it is not
            // supported and is emulated by the OS using the same value that is
            // returned by Environment.TickCount.
            // A return value of 0 indicates that the performance counter is
            // not supported.
            int returnVal = QueryPerformanceFrequency(ref frequency);

            if (returnVal != 0 && frequency != 1000)
            {
                // The performance counter is supported.
                isPerfCounterSupported = true;
            }
            else
            {
                // The performance counter is not supported. Use
                // Environment.TickCount instead.
                frequency = 1000;
            }
        }

        public Int64 Frequency
        {
            get
            {
                return frequency;
            }
        }

        public Int64 Value
        {
            get
            {
                Int64 tickCount = 0;

                if (isPerfCounterSupported)
                {
                    // Get the value here if the counter is supported.
                    QueryPerformanceCounter(ref tickCount);
                    return tickCount;
                }
                else
                {
                    // Otherwise, use Environment.TickCount.
                    return (Int64)Environment.TickCount;
                }
            }
        }

        // Start the timer
        public void Start()
        {
            // lets do the waiting threads there work
            Thread.Sleep(0);

            startTime = this.Value;
        }

        // Stop the timer
        public void Stop()
        {
            stopTime = this.Value;
        }

        // Returns the duration of the timer (in seconds)
        public double Duration
        {
            get
            {
                return (double)(stopTime - startTime) / (double)frequency;
            }
        }

    }
}
