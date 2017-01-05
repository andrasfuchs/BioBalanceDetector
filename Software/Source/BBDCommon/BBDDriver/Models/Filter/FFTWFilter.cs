using BBDDriver.Models.Input;
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

    public class FFTWFilterSettings : ChannelFilterSettings
    {
        public int FFTSampleCount { get; set; }
        public bool IsBackward { get; set; }
        public FFTPlanningRigor PlanningRigor { get; set; }
        public int Timeout { get; set; }
        public bool IsRealTime { get; set; }
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

        private bool isProcessing = false;

        public int TimeoutCount { get; private set; }

        protected override void InputDataChanged(object sender, DataChangedEventArgs e)
        {
            if (plan == null) return;

            lock (plan)
            {
                if (settings.IsRealTime)
                {
                    dataToProcess = settings.FFTSampleCount;
                } else
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
            for (int i=0; i<settings.FFTSampleCount; i++)
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
            // convert the complex data
            complexOutput = mfout.GetData_Float();
            for (int i = 0; i < settings.FFTSampleCount; i++)
            {
                frequencyDomainData[i] = complexOutput[i * 2];
            }

            this.Output.AppendData(frequencyDomainData);
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

            mfin = new fftwf_complexarray(complexInput);
            mfout = new fftwf_complexarray(complexOutput);

            plan = fftwf_plan.dft_1d(n, mfin, mfout, settings.IsBackward ? fftw_direction.Backward : fftw_direction.Forward, fftw_flags.Measure);
        }
    }
}
