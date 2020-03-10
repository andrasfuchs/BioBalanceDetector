using System;
using System.Collections.Generic;
using System.Text;

namespace Mars_64.Iot.Device.Ad7193
{
    public class AdcValue
    {
        private byte? channel;
        public byte? Channel { get => channel; set => channel = value; }

        private uint raw;
        public uint Raw { get => raw; set => this.raw = value; }

        private float value;
        public float Value { get => value; set => this.value = value; }

        private DateTime time;
        public DateTime Time { get => time; set => time = value; }
    }
}
