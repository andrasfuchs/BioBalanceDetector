using BBD.DSP.Models.Source;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBD.DSP.Models
{
    public delegate void DataWrittenEventHandler(object sender, DataWrittenEventArgs e);

    public class DataWrittenEventArgs : EventArgs
    {
        public string Path;
        public long DataWritten;
        public long TotalDataWritten;
        public int DataOverflowWarningCount;

        public DataWrittenEventArgs(string path, long dataWritten, long totalDataWritten, int dataOverflowWarningCount)
        {
            this.Path = path;
            this.DataWritten = dataWritten;
            this.TotalDataWritten = totalDataWritten;
            this.DataOverflowWarningCount = dataOverflowWarningCount;
        }
    }
}
