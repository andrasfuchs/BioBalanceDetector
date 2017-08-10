using BBDDriver.Models.Source;
using FFTWSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BBDDriver.Models.Filter
{
    public enum FFTPlanningRigor { Estimate, Measure, Patient }
    public enum FFTOutputFormat { Raw, RealImaginaryPair, RealOnly, Magnitude, FrequencyMagnitudePair }

    public class FFTWFilterSettings : ChannelFilterSettings
    {
        public int FFTSampleCount { get; set; }
        public bool IsBackward { get; set; }
        public FFTPlanningRigor PlanningRigor { get; set; }
        public int Timeout { get; set; }
        public bool IsRealTime { get; set; }
        public FFTOutputFormat OutputFormat { get; set; }
    }

    public class FFTWFilter : BaseChannelFilter<FFTWFilterSettings>
    {
        private int dataToProcess = 0;

        private fftwf_plan plan = null;
        private float[] complexInput;
        private float[] complexOutput;

        fftwf_complexarray mfin;
        fftwf_complexarray mfout;

        private float[] timeDomainData;
        private float[] frequencyDomainData;

        private volatile bool isProcessing = false;

        public int TimeoutCount { get; private set; }
        public int OutputBlockSize { get; private set; }
        public float FrequencyStep { get; private set; }
        public float ValidFrequencyMin { get; private set; }
        public float ValidFrequencyMax { get; private set; }

        protected override void InputDataChanged(object sender, DataChangedEventArgs e)
        {
            if (plan == null) return;

            if (this.OutputBlockSize > this.Input.BufferSize)
            {
                throw new Exception($"FFTWFilter's block size ({this.OutputBlockSize}) is bugger than the buffer size of the input ({this.Input.BufferSize}). Decrease the block size or increase the buffer size!");
            }

            this.FrequencyStep = (float)Input.SamplesPerSecond / settings.FFTSampleCount;
            this.ValidFrequencyMin = this.FrequencyStep;
            this.ValidFrequencyMax = (float)Input.SamplesPerSecond / 2.0f;

            lock (plan)
            {
                if (settings.IsRealTime)
                {
                    dataToProcess = settings.FFTSampleCount;
                }
                else
                {
                    dataToProcess += e.DataCount;
                }

                if (dataToProcess < settings.FFTSampleCount) return;

                timeDomainData = e.Channel.GetData(settings.FFTSampleCount, e.Position - (dataToProcess - settings.FFTSampleCount));

                dataToProcess -= settings.FFTSampleCount;
            }

            if (isProcessing && settings.IsRealTime) return;

            int waitTime = 0;
            while (isProcessing)
            {
                Thread.Sleep(100);
                waitTime += 100;

                if (waitTime > settings.Timeout)
                {
                    // we don't have time, let's skip this data
                    TimeoutCount++;
                    return;
                }
            }

            // fill the complexInput after the plan
            for (int i = 0; i < settings.FFTSampleCount; i++)
            {
                complexInput[i * 2] = timeDomainData[i];
            }

            // execute the plan
            isProcessing = true;
            mfin.SetData(complexInput);
            Task.Run(() => plan.Execute()).ContinueWith(t => FFTDone(t));
        }

        private void FFTDone(Task task)
        {
            complexOutput = mfout.GetData_Float();

            if (settings.OutputFormat == FFTOutputFormat.Raw)
            {
                this.Output.AppendData(complexOutput);
            }
            else if (settings.OutputFormat == FFTOutputFormat.RealImaginaryPair)
            {
                float[] realAndImaginary = new float[complexOutput.Length / 2];

                for (int i = 0; i < complexOutput.Length / 2; i++)
                {
                    realAndImaginary[i] = complexOutput[i];
                }

                this.Output.AppendData(realAndImaginary);
            }
            else if (settings.OutputFormat == FFTOutputFormat.RealOnly)
            {
                float[] realOnly = new float[complexOutput.Length / 4];

                for (int i = 0; i < complexOutput.Length / 4; i++)
                {
                    realOnly[i] = complexOutput[i * 2];
                }

                this.Output.AppendData(realOnly);
            }
            else if ((settings.OutputFormat == FFTOutputFormat.Magnitude) || (settings.OutputFormat == FFTOutputFormat.FrequencyMagnitudePair))
            {
                float[] magnitude = new float[complexOutput.Length / 4];

                // we need to skip the first value because that's always invalid (see ValidFrequencyMin)
                for (int i = 1; i < complexOutput.Length / 4; i++)
                {
                    float real = complexOutput[i * 2 + 0];
                    float im = complexOutput[i * 2 + 1];

                    //  Get the magnitude of the complex number sqrt((real * real) + (im * im))
                    magnitude[i] = (float)Math.Sqrt(real * real + im * im) / (settings.FFTSampleCount / 2);
                    if (magnitude[i] > 1.0f) magnitude[i] = 1.0f;
                }

                if (settings.OutputFormat == FFTOutputFormat.Magnitude)
                {
                    this.Output.AppendData(magnitude);
                }
                else
                {
                    float[] frequencyAndMagnitude = new float[magnitude.Length * 2];
                    for (int i = 1; i < magnitude.Length; i++)
                    {
                        frequencyAndMagnitude[i * 2 + 0] = this.FrequencyStep * i;
                        frequencyAndMagnitude[i * 2 + 1] = magnitude[i];
                    }

                    this.Output.AppendData(frequencyAndMagnitude);
                }
            }
            else
            {
                throw new Exception($"FFT output format '{settings.OutputFormat}' is not supported.");
            }

            isProcessing = false;
        }

        protected override void OnSettingsChanged(SettingsChangedEventArgs e)
        {
            while (isProcessing) Thread.Sleep(100);

            int n = settings.FFTSampleCount;

            timeDomainData = new float[settings.FFTSampleCount];
            frequencyDomainData = new float[settings.FFTSampleCount];

            // n*2 because we are dealing with complex numbers
            complexInput = new float[n * 2];
            complexOutput = new float[n * 2];

            try
            {
                mfin = new fftwf_complexarray(complexInput);
                mfout = new fftwf_complexarray(complexOutput);
            }
            catch (DllNotFoundException)
            {
                Console.WriteLine("DllNotFoundException: You must copy the native FFTW DLLs (libfftw3-3.dll, libfftw3f-3.dll, libfftw3l-3.dll) into the working directory.");
                throw;
            }
            catch (BadImageFormatException)
            {
                Console.WriteLine("BadImageFormatException: This normally means, that you're loading a 32bit DLL into a 64bit process or vice versa.");
                throw;
            }

            plan = fftwf_plan.dft_1d(n, mfin, mfout, 
                settings.IsBackward ? fftw_direction.Backward : fftw_direction.Forward, 
                settings.PlanningRigor == FFTPlanningRigor.Estimate ? fftw_flags.Estimate :
                settings.PlanningRigor == FFTPlanningRigor.Measure ? fftw_flags.Measure : 
                settings.PlanningRigor == FFTPlanningRigor.Patient ? fftw_flags.Patient :
                fftw_flags.Estimate);

            switch (settings.OutputFormat)
            {
                case FFTOutputFormat.Raw:
                    this.OutputBlockSize = settings.FFTSampleCount * 2;
                    break;
                case FFTOutputFormat.RealImaginaryPair:
                    this.OutputBlockSize = settings.FFTSampleCount;
                    break;
                case FFTOutputFormat.RealOnly:
                    this.OutputBlockSize = settings.FFTSampleCount / 2;
                    break;
                case FFTOutputFormat.Magnitude:
                    this.OutputBlockSize = settings.FFTSampleCount / 2;
                    break;
                case FFTOutputFormat.FrequencyMagnitudePair:
                    this.OutputBlockSize = settings.FFTSampleCount;
                    break;
                default:
                    throw new ArgumentException("The FFT's output format is not supported.", "FFTOutputFormat");
            }
        }
    }
}
