using BBD.DSP.Models.Source;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBD.DSP.Models
{
    public delegate void DataChangedEventHandler(object sender, DataChangedEventArgs e);

    public class DataChangedEventArgs : EventArgs
    {
        public IDataChannel Channel;
        public int Position;
        public int DataCount;

        public DataChangedEventArgs(IDataChannel channel, int newBufferPosition, int newDataCount)
        {
            this.Channel = channel;
            this.Position = newBufferPosition;
            this.DataCount = newDataCount;
        }
    }
}
