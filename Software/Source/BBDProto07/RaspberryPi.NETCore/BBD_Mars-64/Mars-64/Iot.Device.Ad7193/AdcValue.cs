namespace Bbd.Mars64.Iot.Device.Ad7193
{
    public class AdcValue
    {
        private byte channel;
        public byte Channel { get => channel; set => channel = value; }

        private uint raw;
        public uint Raw { get => raw; set => this.raw = value; }

        private float value;
        public float Voltage { get => value; set => this.value = value; }

        private long time;
        public long Time { get => time; set => time = value; }

        public override string ToString()
        {
            return $"Ch{Channel.ToString().PadLeft(5,' ')} {Voltage.ToString("0.0000").PadLeft(10, ' ')} V";
        }
    }
}
