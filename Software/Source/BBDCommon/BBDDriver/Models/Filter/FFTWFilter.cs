using BBDDriver.Models.Input;
using FFTWSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBDDriver.Models.Filter
{
    public enum FFTPlanningRigor { Estimate, Measure, Patient }

    public class FFTWFilterSettings : ChannelFilterSettings
    {
        public int FFTSampleCount { get; set; }
        public bool IsBackward { get; set; }
        public FFTPlanningRigor PlanningRigor { get; set; }
    }

    public class FFTWFilter : BaseChannelFilter<FFTWFilterSettings>
    {
        private int dataToProcess = 0;

        private fftwf_plan plan = null;
        private float[] complexInput;
        private float[] complexOutput;

        protected override void InputDataChanged(object sender, DataChangedEventArgs e)
        {
            dataToProcess += e.DataCount;

            if (dataToProcess < settings.FFTSampleCount) return;

            float[] timeDomainData = e.Channel.GetData(settings.FFTSampleCount, e.Position - (dataToProcess - settings.FFTSampleCount));

            // TODO: fill the complexInput after the plan

            // execute the plan
            if (plan != null)
            {
                Task.Run(() => plan.Execute()).ContinueWith(t => FFTDone(t));                
            }
        }

        private void FFTDone(Task task)
        {      
            // TODO: convert the complex data
                  
            this.Output.AppendData(complexOutput);
        }

        protected override void OnSettingsChanged(SettingsChangedEventArgs e)
        {
            int n = settings.FFTSampleCount;

            // n*2 because we are dealing with complex numbers
            complexInput = new float[n * 2];
            complexOutput = new float[n * 2];

            fftwf_complexarray mfin = new fftwf_complexarray(complexInput);
            fftwf_complexarray mfout = new fftwf_complexarray(complexOutput);

            plan = fftwf_plan.dft_1d(n, mfin, mfout, settings.IsBackward ? fftw_direction.Backward : fftw_direction.Forward, fftw_flags.Measure);
        }
    }
}
