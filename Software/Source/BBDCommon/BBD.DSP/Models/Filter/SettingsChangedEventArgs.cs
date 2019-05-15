using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBD.DSP.Models.Filter
{
    public delegate void SettingsChangedEventHandler(object sender, SettingsChangedEventArgs e);

    public class SettingsChangedEventArgs : EventArgs
    {
        public ChannelFilterSettings Settings;

        public SettingsChangedEventArgs(ChannelFilterSettings settings)
        {
            this.Settings = settings;
        }
    }
}
