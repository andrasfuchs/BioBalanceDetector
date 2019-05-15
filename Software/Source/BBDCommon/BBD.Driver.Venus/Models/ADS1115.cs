using System;
using System.Collections.Generic;
using System.Text;

namespace BBD.Driver.Venus.Models
{
    /// <summary>
    /// This is a stripped down version of Adafruit's C++ ADS1x15 library (https://github.com/adafruit/Adafruit_ADS1X15)
    /// </summary>
    public class ADS1115
    {
        public static byte ADS1115_ADDRESS = 0x48;              // 1001 000 (ADDR = GND)
        public static byte ADS1115_CONVERSIONDELAY = 0x08;

        public static byte ADS1115_REG_POINTER_MASK = 0x03;
        public static byte ADS1115_REG_POINTER_CONVERT = 0x00;
        public static byte ADS1115_REG_POINTER_CONFIG = 0x01;
        public static byte ADS1115_REG_POINTER_LOWTHRESH = 0x02;
        public static byte ADS1115_REG_POINTER_HITHRESH = 0x03;

        public static ushort ADS1115_REG_CONFIG_OS_MASK = 0x8000;
        public static ushort ADS1115_REG_CONFIG_OS_SINGLE = 0x8000;  // Write: Set to start a single-conversion
        public static ushort ADS1115_REG_CONFIG_OS_BUSY = 0x0000;  // Read: Bit = 0 when conversion is in progress
        public static ushort ADS1115_REG_CONFIG_OS_NOTBUSY = 0x8000;  // Read: Bit = 1 when device is not performing a conversion

        public static ushort ADS1115_REG_CONFIG_MUX_MASK = 0x7000;
        public static ushort ADS1115_REG_CONFIG_MUX_DIFF_0_1 = 0x0000;  // Differential P = AIN0, N = AIN1 = default;
        public static ushort ADS1115_REG_CONFIG_MUX_DIFF_0_3 = 0x1000;  // Differential P = AIN0, N = AIN3
        public static ushort ADS1115_REG_CONFIG_MUX_DIFF_1_3 = 0x2000;  // Differential P = AIN1, N = AIN3
        public static ushort ADS1115_REG_CONFIG_MUX_DIFF_2_3 = 0x3000;  // Differential P = AIN2, N = AIN3
        public static ushort ADS1115_REG_CONFIG_MUX_SINGLE_0 = 0x4000;  // Single-ended AIN0
        public static ushort ADS1115_REG_CONFIG_MUX_SINGLE_1 = 0x5000;  // Single-ended AIN1
        public static ushort ADS1115_REG_CONFIG_MUX_SINGLE_2 = 0x6000;  // Single-ended AIN2
        public static ushort ADS1115_REG_CONFIG_MUX_SINGLE_3 = 0x7000;  // Single-ended AIN3

        public static ushort ADS1115_REG_CONFIG_PGA_MASK = 0x0E00;
        public static ushort ADS1115_REG_CONFIG_PGA_6_144V = 0x0000;  // +/-6.144V range = Gain 2/3
        public static ushort ADS1115_REG_CONFIG_PGA_4_096V = 0x0200;  // +/-4.096V range = Gain 1
        public static ushort ADS1115_REG_CONFIG_PGA_2_048V = 0x0400;  // +/-2.048V range = Gain 2 = default;
        public static ushort ADS1115_REG_CONFIG_PGA_1_024V = 0x0600;  // +/-1.024V range = Gain 4
        public static ushort ADS1115_REG_CONFIG_PGA_0_512V = 0x0800;  // +/-0.512V range = Gain 8
        public static ushort ADS1115_REG_CONFIG_PGA_0_256V = 0x0A00;  // +/-0.256V range = Gain 16

        public static ushort ADS1115_REG_CONFIG_MODE_MASK = 0x0100;
        public static ushort ADS1115_REG_CONFIG_MODE_CONTIN = 0x0000;  // Continuous conversion mode
        public static ushort ADS1115_REG_CONFIG_MODE_SINGLE = 0x0100;  // Power-down single-shot mode = default;

        public static ushort ADS1115_REG_CONFIG_DR_MASK = 0x00E0;
        public static ushort ADS1115_REG_CONFIG_DR_8SPS     = 0b00000000;
        public static ushort ADS1115_REG_CONFIG_DR_16SPS    = 0b00100000;
        public static ushort ADS1115_REG_CONFIG_DR_32SPS    = 0b01000000;
        public static ushort ADS1115_REG_CONFIG_DR_64SPS    = 0b01100000;
        public static ushort ADS1115_REG_CONFIG_DR_128SPS   = 0b10000000;
        public static ushort ADS1115_REG_CONFIG_DR_250SPS   = 0b10100000;
        public static ushort ADS1115_REG_CONFIG_DR_475SPS   = 0b11000000;
        public static ushort ADS1115_REG_CONFIG_DR_860SPS   = 0b11100000;

        public static ushort ADS1115_REG_CONFIG_CMODE_MASK = 0x0010;
        public static ushort ADS1115_REG_CONFIG_CMODE_TRAD = 0x0000;  // Traditional comparator with hysteresis = default;
        public static ushort ADS1115_REG_CONFIG_CMODE_WINDOW = 0x0010;  // Window comparator

        public static ushort ADS1115_REG_CONFIG_CPOL_MASK = 0x0008;
        public static ushort ADS1115_REG_CONFIG_CPOL_ACTVLOW = 0x0000;  // ALERT/RDY pin is low when active = default;
        public static ushort ADS1115_REG_CONFIG_CPOL_ACTVHI = 0x0008;  // ALERT/RDY pin is high when active

        public static ushort ADS1115_REG_CONFIG_CLAT_MASK = 0x0004;  // Determines if ALERT/RDY pin latches once asserted
        public static ushort ADS1115_REG_CONFIG_CLAT_NONLAT = 0x0000;  // Non-latching comparator = default;
        public static ushort ADS1115_REG_CONFIG_CLAT_LATCH = 0x0004;  // Latching comparator

        public static ushort ADS1115_REG_CONFIG_CQUE_MASK = 0x0003;
        public static ushort ADS1115_REG_CONFIG_CQUE_1CONV = 0x0000;  // Assert ALERT/RDY after one conversions
        public static ushort ADS1115_REG_CONFIG_CQUE_2CONV = 0x0001;  // Assert ALERT/RDY after two conversions
        public static ushort ADS1115_REG_CONFIG_CQUE_4CONV = 0x0002;  // Assert ALERT/RDY after four conversions
        public static ushort ADS1115_REG_CONFIG_CQUE_NONE = 0x0003;  // Disable the comparator and put ALERT/RDY in high state = default;
    }
}
