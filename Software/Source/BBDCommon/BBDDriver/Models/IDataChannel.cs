using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoMultiplexerServer
{
    public interface IDataChannel
    {
        double? GetValue(long startTimeInTicks, long durationInTicks);
        double? GetValue(long startTimeInTicks);
        double? GetValue();
    }
}
