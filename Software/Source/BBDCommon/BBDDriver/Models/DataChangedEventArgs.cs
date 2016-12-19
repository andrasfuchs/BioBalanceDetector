using BBDDriver.Models.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBDDriver.Models
{
    public delegate void DataChangedEventHandler(object sender, DataChangedEventArgs e);

    public class DataChangedEventArgs : EventArgs
    {
        public IDataChannel Channel;
        public int NewBufferPosition;
        public int NewDataCount;

        public DataChangedEventArgs(IDataChannel channel, int newBufferPosition, int newDataCount)
        {
            this.Channel = channel;
            this.NewBufferPosition = newBufferPosition;
            this.NewDataCount = newDataCount;
        }
    }
}
