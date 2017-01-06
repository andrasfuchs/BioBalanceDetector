using BBDDriver.Models.Source;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBDDriver.Models.Filter
{
    public class ByPassFilterSettings : ChannelFilterSettings
    {
    }

    public class ByPassFilter : BaseChannelFilter<ByPassFilterSettings>
    {
        protected override void InputDataChanged(object sender, DataChangedEventArgs e)
        {
            float[] newData = e.Channel.GetData(e.DataCount);

            this.Output.AppendData(newData);
        }
    }
}
