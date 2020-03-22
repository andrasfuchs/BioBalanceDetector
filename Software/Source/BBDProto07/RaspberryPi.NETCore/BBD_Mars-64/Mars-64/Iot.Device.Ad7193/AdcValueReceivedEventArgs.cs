using System;

namespace Bbd.Mars64.Iot.Device.Ad7193
{
    public class AdcValueReceivedEventArgs : EventArgs
    {
        public AdcValueReceivedEventArgs(AdcValue adcValue)
        {
            this.adcValue = adcValue;
        }
        private readonly AdcValue adcValue;
        public AdcValue AdcValue
        {
            get { return adcValue; }
        }
    }
}
