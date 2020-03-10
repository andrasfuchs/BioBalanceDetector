using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Unosquare.RaspberryIO.Abstractions;

namespace Unosquare.RaspberryIO.Peripherals
{
    public class Ad7193
    {
        private readonly ISpiChannel spiChannel;
        private readonly int delayAfterCommand = 0;

        /*
        public const int AD7193_CS_PIN = 10;        // define the chipselect
        public const int AD7193_RDY_STATE = MISO;   // pin to watch for data ready state
        */

        /* AD7193 Register Map */
        public const byte AD7193_REG_COMM = 0;          // Communications Register (WO, 8-bit) 
        public const byte AD7193_REG_STAT = 0;          // Status Register         (RO, 8-bit)
        public const byte AD7193_REG_MODE = 1;          // Mode Register           (RW, 24-bit)
        public const byte AD7193_REG_CONF = 2;          // Configuration Register  (RW, 24-bit)
        public const byte AD7193_REG_DATA = 3;          // Data Register           (RO, 24/32-bit) 
        public const byte AD7193_REG_ID = 4;            // ID Register             (RO, 8-bit) 
        public const byte AD7193_REG_GPOCON = 5;        // GPOCON Register         (RW, 8-bit) 
        public const byte AD7193_REG_OFFSET = 6;        // Offset Register         (RW, 24-bit 
        public const byte AD7193_REG_FULLSCALE = 7;     // Full-Scale Register     (RW, 24-bit)

        /* Communications Register Bit Designations (AD7193_REG_COMM) */
        public const int AD7193_COMM_WEN = (1 << 7);    // Write Enable. 
        public const int AD7193_COMM_WRITE = (0 << 6);  // Write Operation.
        public const int AD7193_COMM_READ = (1 << 6);   // Read Operation.         
        public const int AD7193_COMM_CREAD = (1 << 2);  // Continuous Read of Data Register.

        // default register settings
        private ulong[] registerMap = { 0x00, 0x080060, 0x000117, 0x000000 };
        private byte[] registerSize = { 1, 3, 3, 3, 1, 1, 3, 3 };


        public Ad7193(ISpiChannel spiChannel)
        {
            this.spiChannel = spiChannel;
        }

        public void Reset()
        {
            //Debug.WriteLine("Resetting AD7193...");

            for (int i = 0; i < 6; i++)

            {
                spiChannel.Write(new byte[] { 0xFF });
            }

            Thread.Sleep(delayAfterCommand);
        }

        public void SetPGAGain(int gain)
        {
            Debug.WriteLine($"Setting PGA Gain to {gain}");

            ulong gainSetting;

            if (gain == 1) { gainSetting = 0x0; }
            else if (gain == 8) { gainSetting = 0x3; }
            else if (gain == 16) { gainSetting = 0x4; }
            else if (gain == 32) { gainSetting = 0x5; }
            else if (gain == 64) { gainSetting = 0x6; }
            else if (gain == 128) { gainSetting = 0x7; }
            else
            {
                Debug.WriteLine("ERROR - Invalid Gain Setting - no changes made.  Valid Gain settings are 1, 8, 16, 32, 64, 128");
                return;
            }

            byte regAddress = AD7193_REG_CONF;

            registerMap[regAddress] &= 0xFFFFF8;    //keep all bit values except gain bits
            registerMap[regAddress] |= gainSetting;

            SetRegisterValue(regAddress, registerMap[regAddress], registerSize[regAddress]);
        }
        /*
                void SetAveraging(int filterRate)
                {

                    Debug.WriteLine($"Setting Filter Rate Select Bits to {filterRate}");

                    if (filterRate > 0x3ff)
                    {
                        Debug.WriteLine("ERROR - Invalid Filter Rate Setting - no changes made.  Filter Rate is a 10-bit value");
                        return;
                    }

                    registerMap[1] &= 0xFFFC00;         //keep all bit values except filter setting bits
                    registerMap[1] |= filterRate;

                    SetRegisterValue(1, registerMap[1], registerSize[1], 1);

                }

                void AD7193::SetPsuedoDifferentialInputs(void)
                {
                    Serial.println("Switching from differential input to pseudo differential inputs...");

                    unsigned long psuedoBit = 0x040000;
                    registerMap[2] &= 0xFBFFFF;
                    registerMap[2] |= 0x040000;

                    SetRegisterValue(2, registerMap[2], registerSize[2], 1);

                    //Serial.print(" - on next register refresh, new Config Reg value will be: ");
                    //Serial.println(registerMap[2], HEX);
                }

                void AD7193::AppendStatusValuetoData(void)
                {
                    Serial.println("\nEnabling DAT_STA Bit (appends status register to data register when reading)");


                    registerMap[1] &= 0xEFFFFF; //keep all bit values except DAT_STA bit
                    registerMap[1] |= 0x100000;  // set DAT_STA to 1

                    SetRegisterValue(1, registerMap[1], registerSize[1], 1);

                    //Serial.print(" - New Mode Reg Value: ");
                    //Serial.println(registerMap[1], HEX);

                    registerSize[3] = 4; // change register size to 4, b/c status register is now appended
                }

                void AD7193::Calibrate(void)
                {
                    Serial.print("\nInitiate Internal Calibration, starting with Zero-scale calibration...");

                    // Begin Communication cycle, bring CS low manually
                    digitalWrite(AD7193_CS_PIN, LOW);
                    delay(100);

                    registerMap[1] &= 0x1FFFFF; //keep all bit values except Channel bits
                    registerMap[1] |= 0x800000; // internal zero scale calibration

                    SetRegisterValue(1, registerMap[1], 3, 0);  // overwriting previous MODE reg setting 

                    WaitForADC();
                    //delay(100);

                    Serial.print("\n\nNow full-scale calibration...");


                    registerMap[1] &= 0x1FFFFF; //keep all bit values except Channel bits
                    registerMap[1] |= 0xA00000; // internal full scale calibration

                    SetRegisterValue(1, registerMap[1], 3, 0);  // overwriting previous MODE reg setting 

                    WaitForADC();
                    //delay(100);

                    digitalWrite(AD7193_CS_PIN, HIGH);
                    delay(100);
                }

                void AD7193::WaitForADC(void)
                {
                    int breakTime = 0;

                    Serial.print("\nWaiting for Conversion");

                    while (1)
                    {
                        if (digitalRead(AD7193_RDY_STATE) == 0)
                        {      // Break if ready goes low
                            break;
                        }

                        if (breakTime > 5000)
                        {                       // Break after five seconds - avoids program hanging up
                            Serial.print("Data Ready never went low!");
                            break;
                        }

                        if (digitalRead(AD7193_RDY_STATE)) { Serial.print("."); }
                        delay(1);
                        breakTime = breakTime + 1;
                    }
                }

                void IntitiateSingleConversion()
                {
                    //Serial.print("    Initiate Single Conversion... (Device will go into low pwer mode when conversion complete)");

                    // Begin Communication cycle, bring CS low manually
                    digitalWrite(AD7193_CS_PIN, LOW);
                    delay(100);

                    registerMap[1] &= 0x1FFFFF; //keep all bit values except Channel bits
                    registerMap[1] |= 0x200000; // single conversion mode bits  

                    SetRegisterValue(1, registerMap[1], 3, 0);  // overwriting previous MODE reg setting 
                }

                ulong ReadADCData()
                {

                    unsigned char byteIndex = 0;
                    unsigned long buffer = 0;
                    unsigned char receiveBuffer = 0;
                    unsigned char dataLength = registerSize[3];  // data length depends on if Status register is appended to Data read - see AppendStatusValuetoData()

                    SPI.transfer(0x58);  // command to start read data

                    while (byteIndex < dataLength)
                    {
                        receiveBuffer = SPI.transfer(0);
                        buffer = (buffer << 8) + receiveBuffer;
                        byteIndex++;
                    }

                    return (buffer);
                }


                void AD7193::SetChannel(int channel)
                {

                    // generate Channel settings bits for Configuration write
                    unsigned long shiftvalue = 0x00000100;
                    unsigned long channelBits = shiftvalue << channel;

                    // Write Channel bits to Config register, keeping other bits as is
                    registerMap[2] &= 0xFC00FF; //keep all bit values except Channel bits
                    registerMap[2] |= channelBits;

                    // write channel selected to Configuration register
                    SetRegisterValue(2, registerMap[2], registerSize[2], 1);
                    delay(10);
                }

                unsigned long AD7193::ReadADCChannel(int channel)
                {

                    SetChannel(channel);

                    // write command to initial conversion
                    IntitiateSingleConversion();
                    //delay(100); // hardcoded wait time for data to be ready
                    // should scale the wait time by averaging

                    WaitForADC();

                    unsigned long ADCdata = ReadADCData();
                    delay(10);

                    // end communication cycle, bringing CS pin High manually 
                    digitalWrite(AD7193_CS_PIN, HIGH);
                    delay(10);

                    return (ADCdata);
                }



                float AD7193::DataToVoltage(long rawData)
                {
                    float voltage = 0;
                    char mGain = 0;
                    float mVref = 2.5;
                    char mPolarity = 0;

                    int PGASetting = registerMap[2] & 0x000007;  // keep only the PGA setting bits
                    int PGAGain;

                    if (PGASetting == 0)
                    {
                        PGAGain = 1;
                    }
                    else if (PGASetting == 3)
                    {
                        PGAGain = 8;
                    }
                    else if (PGASetting == 4)
                    {
                        PGAGain = 16;
                    }
                    else if (PGASetting == 5)
                    {
                        PGAGain = 32;
                    }
                    else if (PGASetting == 6)
                    {
                        PGAGain = 64;
                    }
                    else if (PGASetting == 7)
                    {
                        PGAGain = 128;
                    }
                    else
                    {
                        PGAGain = 1;
                    }


                    if (mPolarity == 1)
                    {
                        voltage = ((double)rawData / 16777216 / (1 << PGAGain)) * mVref;
                    }
                    if (mPolarity == 0)
                    {
                        voltage = (((float)rawData / (float)8388608) - (float)1) * (mVref / (float)PGAGain);
                    }


                    return (voltage);
                }

                // See "Tempature Sensor" section of AD7193 Datasheet - page 39
                float AD7193::TempSensorDataToDegC(unsigned long rawData)
                {
                    float degC = (float(rawData - 0x800000) / 2815) - 273;
                    float degF = (degC * 9 / 5) + 32;
                    return (degC);
                }
        */

        /*! Reads the value of a register. */
        public byte[] GetRegisterValue(byte registerAddress, byte bytesNumber)
        {
            byte commandByte;
            byte[] buffer = new byte[bytesNumber + 1];

            // set the command byte at the beginning of the spi datastream
            commandByte = (byte)(AD7193_COMM_READ | GetCommAddr(registerAddress));
            buffer[0] = commandByte;


            buffer = spiChannel.SendReceive(buffer);
            buffer = buffer[1..];

            Console.WriteLine($"Read Register - address: {registerAddress.ToString("X2")}, command: {commandByte.ToString("X2")}, received: {String.Join(' ', buffer.Select(x => x.ToString("X2")))}");
            Thread.Sleep(delayAfterCommand);

            return buffer;
        }

        /*! Writes data into a register. */
        public void SetRegisterValue(byte registerAddress, ulong registerValue, byte bytesNumber)
        {
            byte commandByte = 0;
            byte[] buffer = new byte[bytesNumber + 1];

            commandByte = (byte)(AD7193_COMM_WRITE | GetCommAddr(registerAddress));
            buffer[0] = commandByte;
            if (bytesNumber >= 1)
            {
                buffer[1] = (byte)((registerValue >> 0) & 0x000000FF);
            }
            if (bytesNumber >= 2)
            {
                buffer[2] = (byte)((registerValue >> 8) & 0x000000FF);
            }
            if (bytesNumber >= 3) 
            { 
                buffer[3] = (byte)((registerValue >> 16) & 0x000000FF);
            }
            if (bytesNumber >= 4)
            { 
                buffer[4] = (byte) ((registerValue >> 24) & 0x000000FF);
            }

            spiChannel.Write(buffer);
            buffer = buffer[1..];

            Console.WriteLine($"Write Register - address: {registerAddress.ToString("X2")}, command: {commandByte.ToString("X2")}, sent: {String.Join(' ', buffer.Select(x => x.ToString("X2")))}");
        }

        public void ReadRegisterMap()
        {
            Debug.WriteLine("Read All Register Values (helpful for troubleshooting)");
            Reset();
            GetRegisterValue(AD7193_REG_STAT, registerSize[AD7193_REG_STAT]);
            Reset();
            GetRegisterValue(AD7193_REG_MODE, registerSize[AD7193_REG_MODE]);
            Reset();
            GetRegisterValue(AD7193_REG_CONF, registerSize[AD7193_REG_CONF]);
            Reset();
            GetRegisterValue(AD7193_REG_DATA, registerSize[AD7193_REG_DATA]);
            Reset();
            GetRegisterValue(AD7193_REG_ID, registerSize[AD7193_REG_ID]); 
            Reset();
            GetRegisterValue(AD7193_REG_GPOCON, registerSize[AD7193_REG_GPOCON]);
            Reset();
            GetRegisterValue(AD7193_REG_OFFSET, registerSize[AD7193_REG_OFFSET]);
            Reset();
            GetRegisterValue(AD7193_REG_FULLSCALE, registerSize[AD7193_REG_FULLSCALE]);
            
            Thread.Sleep(delayAfterCommand);
        }

        private int GetCommAddr(int x)
        {
            return (((x) & 0x07) << 3);
        }
    }
}
