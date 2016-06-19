using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoMultiplexerServer
{
    public interface IDataChannel
    {
        short? Get16BitSignedIntData(long startTimeInTicks, long durationInTicks);
    }
}
