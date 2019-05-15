using BBD.DSP.Models.Source;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BBD.DSP.Models.Output
{
    public class BinaryFileOutput : FileOutput
    {
        private List<byte> buffer = new List<byte>();

        public BinaryFileOutput(MultiChannelInput<IDataChannel> mci, string path) : base(mci, path)
        {
        }

        protected override void WriteDataToBuffer(float[][] dataToWrite, int dataCount)
        {
            lock (buffer)
            {
                for (int j = 0; j < dataCount; j++)
                {
                    for (int i = 0; i < channelDataChanges.Count; i++)
                    {
                        buffer.AddRange(ConvertData(dataToWrite[i][j]));
                    }
                }
            }
        }

        protected override void WriteDataFromBuffer(object stateInfo)
        {
            byte[] dataToWrite = null;
            lock (buffer)
            {
                if (buffer.Count == 0) return;
                dataToWrite = buffer.ToArray();
                buffer.Clear();
            }

            AppendData(dataToWrite);

            OnDataWritten(this, new DataWrittenEventArgs(filename, dataToWrite.Length, bytesWritten, this.DataOverflowWarningCount));
        }

        protected virtual void AppendData(byte[] dataToWrite)
        {
            throw new NotImplementedException();
        }
    }
}
