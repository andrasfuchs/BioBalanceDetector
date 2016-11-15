using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoMultiplexerServer
{
    public class SerialCommand
    {
        public string Command { get; set; }

        public string[] Parameters { get; set; }

        public byte[] ParametersRaw { get; set; }
    }
}
