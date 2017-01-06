using BBDDriver.Models.Source;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBDDriver.Models.Filter
{
    public class FillFilterSettings : ChannelFilterSettings
    {
        public float ValueToFillWith { get; set; }
    }

    public class FillFilter : BaseChannelFilter<FillFilterSettings>
    {
        protected override void InputDataChanged(object sender, DataChangedEventArgs e)
        {
            float[] newData = Enumerable.Repeat<float>(settings.ValueToFillWith, e.DataCount).ToArray();

            this.Output.AppendData(newData);
        }
    }
}
