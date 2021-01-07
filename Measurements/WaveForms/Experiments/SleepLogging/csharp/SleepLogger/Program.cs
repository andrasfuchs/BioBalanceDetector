using NWaves.Audio;
using NWaves.Signals;
using NWaves.Transforms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SleepLogger
{
	class Program
	{
		/// <summary>
		/// Number of samples per buffer
		/// </summary>
		static int bufferSize;
		/// <summary>
		/// Number of samples per second
		/// </summary>
		const int samplerate = 2048;
		/// <summary>
		/// Audio signal channel
		/// </summary>
		const int signalGeneratorChannel = 1;		//W2
		/// <summary>
		/// Default audio signal frequency
		/// </summary>
		const double signalGeneratorFrequency = 36;
		/// <summary>
		/// Audio signal amplitude
		/// </summary>
		const double signalGeneratorAmplitude = 0.95;					// W2 oscillated between -0.95V and +0.95V

		static float inputAmplification = short.MaxValue / 1.0f;		// WAV file ranges from -1000 mV to +1000 mV
		
		static bool terminateAcquisition = false;

		static List<float> samples = new List<float>();
		static double[] voltData;

		static void Main(string[] args)
		{
			Dwf.FDwfGetVersion(out string dwfVersion);
			Console.WriteLine($"DWF Version: {dwfVersion}");

			Console.WriteLine("Opening first device");
			Dwf.FDwfDeviceOpen(-1, out int dwfHandle);

			if (dwfHandle == Dwf.hdwfNone)
			{
				Dwf.FDwfGetLastErrorMsg(out string lastError);
				Console.WriteLine($"Failed to open device: {lastError}");
				return;
			}

			Dwf.FDwfAnalogInBufferSizeInfo(dwfHandle, out int bufferSizeMinimum, out int bufferSizeMaximum);
			bufferSize = Math.Min(bufferSizeMaximum, samplerate);
			Console.WriteLine($"Device buffer size range: {bufferSizeMinimum} - {bufferSizeMaximum} samples, set to {bufferSize}.");
			voltData = new double[bufferSize];

			//set up acquisition
			Dwf.FDwfAnalogInFrequencySet(dwfHandle, samplerate);
			Dwf.FDwfAnalogInBufferSizeSet(dwfHandle, bufferSize);
			Dwf.FDwfAnalogInChannelEnableSet(dwfHandle, 0, 1);
			Dwf.FDwfAnalogInChannelRangeSet(dwfHandle, 0, 5.0);

			// set up signal generation
			Dwf.FDwfAnalogOutNodeEnableSet(dwfHandle, signalGeneratorChannel, Dwf.AnalogOutNodeCarrier, 1);            
			Dwf.FDwfAnalogOutNodeFunctionSet(dwfHandle, signalGeneratorChannel, Dwf.AnalogOutNodeCarrier, Dwf.funcSine);
			Dwf.FDwfAnalogOutNodeFrequencySet(dwfHandle, signalGeneratorChannel, Dwf.AnalogOutNodeCarrier, signalGeneratorFrequency);
			Dwf.FDwfAnalogOutNodeAmplitudeSet(dwfHandle, signalGeneratorChannel, Dwf.AnalogOutNodeCarrier, signalGeneratorAmplitude);

			Console.WriteLine($"Generating sine wave @{signalGeneratorFrequency}Hz...");
			Dwf.FDwfAnalogOutConfigure(dwfHandle, signalGeneratorChannel, 1);

			//wait at least 2 seconds for the offset to stabilize
			Thread.Sleep(2000);

			//start aquisition
			Console.WriteLine("Starting oscilloscope");
			Dwf.FDwfAnalogInConfigure(dwfHandle, 0, 1);

			Console.WriteLine($"Recording data @{samplerate}Hz, press Ctrl+C to stop...");

			Console.CancelKeyPress += Console_CancelKeyPress;

			while (!terminateAcquisition)
			{
				while (true)
				{
					Dwf.FDwfAnalogInStatus(dwfHandle, 1, out byte sts);

					if (sts == Dwf.DwfStateDone)
						break;

					Thread.Sleep(100);
				}


				Dwf.FDwfAnalogInStatusData(dwfHandle, 0, voltData, bufferSize);     //get channel 1 data CH1
				samples.AddRange(voltData.Select(vd => (float)vd));
				Console.Write(".");

				if (samples.Count >= samplerate * 5)
				{
					//generate a signal
					var signal = new DiscreteSignal(samplerate, samples.ToArray(), true);
					samples.Clear();

					Task.Run(() =>
					{
						var fft = new RealFft(2048);
						var powerSpectrum = fft.PowerSpectrum(signal, normalize: false);

						
						string filename = $"AD2_{DateTime.Now.ToString("yyyy-MM-dd_HHmmss")}";

						//save it the FFT to a binary file
						File.WriteAllTextAsync(filename + ".fft", JsonSerializer.Serialize(powerSpectrum));

						//and save samples to a WAV file
						FileStream waveFileStream = new FileStream(filename + ".wav", FileMode.Create);

						signal.Amplify(inputAmplification);
						WaveFile waveFile = new WaveFile(signal, 16);
						waveFile.SaveTo(waveFileStream, false);

						waveFileStream.Close();
						Console.Write("|");
					}
					);
				}
			}

			Console.WriteLine();
			Console.WriteLine("Acquisition done");

			Dwf.FDwfDeviceCloseAll();
		}

		private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
		{
			terminateAcquisition = true;
			e.Cancel = true;
		}
	}
}
