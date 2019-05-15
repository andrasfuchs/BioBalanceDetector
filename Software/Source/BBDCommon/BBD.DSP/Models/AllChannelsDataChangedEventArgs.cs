using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBD.DSP.Models
{
    public delegate void AllChannelsDataChangedEventHandler(object sender, AllChannelsDataChangedEventArgs e);

    public class AllChannelsDataChangedEventArgs : EventArgs
    {
        public DataChangedEventArgs[] DataChanges;

        public AllChannelsDataChangedEventArgs(DataChangedEventArgs[] dataChanges)
        {
            this.DataChanges = dataChanges;
        }
    }
}
