using System;
using System.Collections.Generic;
using System.Text;

namespace Mars_64.Iot.Device.Ad7193
{
    public class AdcValueReceivedEventArgs : EventArgs
    {
        public AdcValueReceivedEventArgs(AdcValue adcValue)
        {
            this.adcValue = adcValue;
        }
        private AdcValue adcValue;
        public AdcValue AdcValue
        {
            get { return adcValue; }
        }
    }
}
