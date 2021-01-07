using System;
using System.Threading;

namespace SleepLogger
{
    class Program
    {
        const int buffersize = 4096;			// samples / buffer
        const int samplerate = 8000;			// samples / second
        const int signalgenhz = 80;

        static bool terminateAcquisition = false;
        static double[] rgdSamples = new double[buffersize];

        static void Main(string[] args)
        {
            dwf.FDwfGetVersion(out string dwfVersion);
            Console.WriteLine($"DWF Version: {dwfVersion}");

            Console.WriteLine("Opening first device");
            dwf.FDwfDeviceOpen(-1, out int hdwf);

            if (hdwf == dwf.hdwfNone)
            {
                dwf.FDwfGetLastErrorMsg(out string szerr);
                Console.WriteLine(szerr);
                Console.WriteLine("failed to open device");
                return;
            }

            dwf.FDwfAnalogInBufferSizeInfo(hdwf, out int cBufInt, out int cBufMax);
            Console.WriteLine($"Device buffer size: {cBufMax} samples");

            //set up acquisition
            dwf.FDwfAnalogInFrequencySet(hdwf, samplerate);
            dwf.FDwfAnalogInBufferSizeSet(hdwf, buffersize);
            dwf.FDwfAnalogInChannelEnableSet(hdwf, 0, 1);
            dwf.FDwfAnalogInChannelRangeSet(hdwf, 0, 5.0);

            // set up signal generation
            /*
            dwf.FDwfAnalogOutNodeEnableSet(hdwf, channel, AnalogOutNodeCarrier, c_bool(True))
            dwf.FDwfAnalogOutNodeFunctionSet(hdwf, channel, AnalogOutNodeCarrier, funcTriangle)					# ! this looks like a square wave
            dwf.FDwfAnalogOutNodeFrequencySet(hdwf, channel, AnalogOutNodeCarrier, c_double(signalgenhz))
            dwf.FDwfAnalogOutNodeAmplitudeSet(hdwf, channel, AnalogOutNodeCarrier, c_double(1.41))			# ! this doesn't really do anything
            dwf.FDwfAnalogOutNodeOffsetSet(hdwf, channel, AnalogOutNodeCarrier, c_double(1.41))

            print("Generating sine wave @" + str(signalgenhz) + "Hz...")
            dwf.FDwfAnalogOutConfigure(hdwf, channel, c_bool(True))

            #wait at least 2 seconds for the offset to stabilize
            time.sleep(2)

            */

            //get the proper file name
            DateTime starttime = DateTime.Now;
            string startfilename = $"AD2_{starttime.ToString("yyyy-MM-dd_HHmmss")}.wav";

            //open WAV file
            Console.WriteLine("Opening WAV file '" + startfilename + "'");
            NWaves.Signals.DiscreteSignal signal = new NWaves.Signals.DiscreteSignal(samplerate, buffersize);
            NWaves.Audio.WaveFile waveFile = new NWaves.Audio.WaveFile(signal, 32);

            //start aquisition
            Console.WriteLine("Starting oscilloscope");
            dwf.FDwfAnalogInConfigure(hdwf, 0, 1);

            Console.WriteLine($"Recording data @{samplerate}Hz, press Ctrl+C to stop...");

            int bufferCounter = 0;

            Console.CancelKeyPress += Console_CancelKeyPress;

            while (!terminateAcquisition)
            {
                while (!terminateAcquisition)
                {
                    dwf.FDwfAnalogInStatus(hdwf, 1, out byte sts);

                    if (sts == dwf.DwfStateDone)
                        break;

                    Thread.Sleep(100);
                }


                dwf.FDwfAnalogInStatusData(hdwf, 0, rgdSamples, buffersize);    //get channel 1 data CH1 - ! it looks like 2 channels get read here and only the second is the data of CH1
                dwf.FDwfAnalogInStatusData(hdwf, 1, rgdSamples, buffersize); 	//get channel 2 data CH2

                //waveWrite.writeframes(rgdSamples);
                //waveFile.SaveTo();
                bufferCounter += 1;
                Console.Write(".");
            }

            Console.WriteLine("Acquisition done");

            Console.WriteLine("Closing WAV file");
        //    waveWrite.close();

            dwf.FDwfDeviceCloseAll();

/*            
//rename the file so that we know both the start and end times from the filename
endtime = datetime.datetime.now();
            endfilename = "AD2_" + "{:04d}".format(starttime.year) + "{:02d}".format(starttime.month) + "{:02d}".format(starttime.day) + "_" + "{:02d}".format(starttime.hour) + "{:02d}".format(starttime.minute) + "{:02d}".format(starttime.second) + "-" + "{:02d}".format(endtime.hour) + "{:02d}".format(endtime.minute) + "{:02d}".format(endtime.second) + ".wav";

            print("Renaming file from '" + startfilename + "' to '" + endfilename + "'");
            os.rename(startfilename, endfilename);
*/


        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            terminateAcquisition = true;
        }
    }
}
