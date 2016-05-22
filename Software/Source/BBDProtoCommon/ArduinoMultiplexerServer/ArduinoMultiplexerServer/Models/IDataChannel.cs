using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoMultiplexerServer
{
    public interface IDataChannel
    {
        Int16 Get16BitSignedIntData(Int64 startTime, int duration);
    }
}
