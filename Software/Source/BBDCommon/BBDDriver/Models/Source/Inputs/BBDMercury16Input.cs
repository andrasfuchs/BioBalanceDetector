using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using libusbK;
using LibUsbDotNet;
using MadWizard.WinUSBNet;
using System.Runtime.InteropServices;
using BBDDriver.Models.IEEE11073;

namespace BBDDriver.Models.Source
{
    internal class BBDMercury16Input : MultiChannelInput<IDataChannel>, IDisposable
    {
        // warning: interface guid changes when the driver is regenerated (by zadig)
        // the new DeviceGUID can be found in the .inf file in the 'C:\Users\{username}\usb_driver' folder.
        //private const string DEVICE_INTERFACE_GUID = "{ABA7BA71-CA9E-428F-813C-0B97D8205E31}"; // Sheldon
        private const string DEVICE_INTERFACE_GUID = "{1E04662A-6FF7-4864-B7B7-8BBA6658585F}"; // Mr. Pepper
        private const int DEVICE_VID = 0x03EB;
        private const int DEVICE_PID = 0x2405;
        private const string DEVICE_DESC = "Mercury-16";
        private const int targetFPS = 30;

        //private KLST_DEVINFO_HANDLE deviceInfo;
        //private LibUsbDotNet.Main.UsbRegistry deviceInfo;
        //private LibUsbDotNet.UsbEndpointReader reader;
        //private LibUsbDotNet.UsbEndpointWriter writer;

        public List<DataRateBenchmarkEntry> BenchmarkEntries = new List<DataRateBenchmarkEntry>();

        private USBDevice usbDevice;
        private USBInterface usbInterface;

        public int CPUClockSpeedMhz { get; set; } = 32;
        public int ADCSpeedkMhz { get; set; } = 2;
        public float ADCReferenceV { get; set; } = 1.0f;
        public int ADCGain { get; set; } = 1;
        public int SampleRatekHz { get; set; } = 8;
        public int BufferSize { get; set; } = 32 * 1024;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct CellSettings
        {
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 FirmwareVersion;

            [MarshalAs(UnmanagedType.U1)]
            public byte TestMode;

            // reset | enabled
            [MarshalAs(UnmanagedType.U1)]
            public byte DeviceStatus;

            // unknown | cell | organizer
            [MarshalAs(UnmanagedType.U1)]
            public byte DeviceType;

            // MPCM index
            [MarshalAs(UnmanagedType.U1)]
            public byte DeviceIndex;

            // same ID for the same type of chip (e.g. Atmel XMEGA256A4)
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 DeviceID;

            // unique serial for every chip
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 DeviceSerial;

            // system clock speed in Hz
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 SystemClock;

            // ADC clock speed in Hz
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 ADCClock;

            // is any of the ADCs enabled
            [MarshalAs(UnmanagedType.U1)]
            public bool ADCEnabled;

            // is ADC-A enabled
            [MarshalAs(UnmanagedType.U1)]
            public bool ADCAEnabled;

            // is ADC-B enabled
            [MarshalAs(UnmanagedType.U1)]
            public bool ADCBEnabled;

            // ADC reference
            [MarshalAs(UnmanagedType.U1)]
            public byte ADCReference;

            // ADC gain
            [MarshalAs(UnmanagedType.U1)]
            public byte ADCGain;

            [MarshalAs(UnmanagedType.U1)]
            public byte ADCBits;

            // sampling timer rate in Hz
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 SampleRate;

            // compensation of the timer (slightly changes the speed)
            [MarshalAs(UnmanagedType.I2)]
            public short SampleRateCompensation;

            // number of channels
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 ChannelCount;

            // is DAC enabled
            [MarshalAs(UnmanagedType.U1)]
            public bool DACEnabled;

            // is USB enabled
            [MarshalAs(UnmanagedType.U1)]
            public bool USBEnabled;

            [MarshalAs(UnmanagedType.U1)]
            public byte USBAddress;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 USBSpeed;

            // is USART enabled
            [MarshalAs(UnmanagedType.U1)]
            public bool USARTEnabled;

            // USART mode (1 - async, 2 - sync master, 3 - sync slave)
            [MarshalAs(UnmanagedType.U1)]
            public byte USARTMode;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 USARTSpeed;

            [MarshalAs(UnmanagedType.U1)]
            public bool GoertzelEnabled;

            [MarshalAs(UnmanagedType.R4)]
            public float GoertzelFrequency;

            [MarshalAs(UnmanagedType.U1)]
            public byte ADCValueBits;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 ADCValuePerPacket;

            [MarshalAs(UnmanagedType.U1)]
            public bool ADCValuePacketToUSB;

            [MarshalAs(UnmanagedType.U1)]
            public bool ADCValuePacketToUSART;

            [MarshalAs(UnmanagedType.U1)]
            public bool GoertzelPacketToUSB;

            [MarshalAs(UnmanagedType.U1)]
            public bool GoertzelPacketToUSART;
        }

        public BBDMercury16Input(USBDeviceInfo selectedDevice = null) : base(8000, 8)
        {
            string expectedDeviceID = $"USB\\VID_{DEVICE_VID}&PID_{DEVICE_PID}";

            // libusbK implementation
            //int deviceCount = 0;
            //KLST_DEVINFO_HANDLE deviceInfo;
            //LstK lst = new LstK(KLST_FLAG.NONE);

            //lst.Count(ref deviceCount);
            //while (lst.MoveNext(out deviceInfo))
            //{
            //    Console.WriteLine("libusbk: " + deviceInfo.DeviceInterfaceGUID.ToLower());
            //}

            //lst.Free();


            // DotNetLibUsb
            //var deviceList = LibUsbDotNet.LibUsb.LibUsbDevice.AllDevices.ToArray();
            //foreach (var deviceInfoLUDN in deviceList)
            //{
            //    Console.WriteLine("libusb: " + deviceInfoLUDN.DeviceInterfaceGuids[0].ToString().ToLower() + " - " + deviceInfoLUDN.Device.Info.ProductString);
            //}

            //if (this.deviceInfo == null)
            //{
            //    throw new System.IO.IOException("There is no BBD Mercury-16 connected on any of the USB ports.");
            //}

            //if (deviceInfo.Device.Open())
            //{
            //    reader = deviceInfo.Device.OpenEndpointReader(LibUsbDotNet.Main.ReadEndpointID.Ep01);
            //    writer = deviceInfo.Device.OpenEndpointWriter(LibUsbDotNet.Main.WriteEndpointID.Ep01);
            //}

            // WinUSB.NET
            if (selectedDevice == null)
            {
                // GUID is an example, specify your own unique GUID in the .inf file
                USBDeviceInfo[] connectedDevices = USBDevice.GetDevices(DEVICE_INTERFACE_GUID);

                if (connectedDevices.Length == 0) throw new System.IO.IOException("There is no BBD Mercury-16 connected on any of the USB ports.");

                selectedDevice = connectedDevices[0];

                //USBNotifier notifier = new USBNotifier(null, DEVICE_INTERFACE_GUID);
            }

            // Find your device in the array
            usbDevice = new USBDevice(selectedDevice);
            usbInterface = usbDevice.Interfaces.Find(USBBaseClass.PersonalHealthcare);


            var phdPacket = ReadIEEE11073Pakcet(usbInterface.InPipe);

            // stop streaming data
            usbInterface.OutPipe.Write(new byte[] { 0xF0, 0x01, 0x00, 0x00 });

            CellSettings cellSettings = new CellSettings();
            while (cellSettings.SystemClock == 0)
            {
                // get the cell settings
                usbInterface.OutPipe.Write(new byte[] { 0xF0, 0x03, 0x00, 0x00 });
                cellSettings = ReadPacket<CellSettings>(usbInterface.InPipe, 0xF004, 10);
            }

            // start streaming data
            usbInterface.OutPipe.Write(new byte[] { 0xF0, 0x02, 0x00, 0x00 });

            if ((cellSettings.ChannelCount != 8) && (cellSettings.ChannelCount != 16) && (cellSettings.ChannelCount != 24))
            {
                throw new Exception($"The number of channels reported by Mercury-16 ({cellSettings.ChannelCount}) is invalid!");
            }


            //this.SamplesPerSecond = cellSettings.SampleRate;
            for (int i = 0; i < cellSettings.ChannelCount; i++)
            {
                this.SetChannel(i, new SinglePrecisionDataChannel(this.SamplesPerSecond, this.SamplesPerSecond * 5));
            }

            this.BufferSize = Math.Max(this.BufferSize, this.SamplesPerSecond * this.ChannelCount * 2 / targetFPS);

            Task usbPollTask = Task.Run(() =>
            {
                while (PollUSBData(this)) {}
            });
        }

        private T ReadPacket<T>(USBPipe pipe, UInt16 choice = 0, int maxDiscardedPackets = 0)
        {
            T result = default(T);

            byte[] rawHeaderLE = new byte[4];
            usbInterface.InPipe.Read(rawHeaderLE);
            GCHandle handle = GCHandle.Alloc(rawHeaderLE, GCHandleType.Pinned);
            APDU apdu = (APDU)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(APDU));
            handle.Free();

            if (apdu.Length > 0)
            {
                byte[] rawBodyLE = new byte[apdu.Length];
                usbInterface.InPipe.Read(rawBodyLE);

                handle = GCHandle.Alloc(rawBodyLE, GCHandleType.Pinned);
                result = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
                handle.Free();
            }

            if ((choice != 0) && (apdu.Choice != choice) && (maxDiscardedPackets > 0))
            {
                // this is not the packet we are looking for
                return ReadPacket<T>(pipe, choice, --maxDiscardedPackets);
            }

            return result;
        }

        private DataPacket ReadPacketRaw(USBPipe pipe, UInt16 choice = 0, int maxDiscardedPackets = 0)
        {
            DataPacket result = new DataPacket();

            byte[] rawHeaderLE = new byte[4];
            usbInterface.InPipe.Read(rawHeaderLE);
            GCHandle handle = GCHandle.Alloc(rawHeaderLE, GCHandleType.Pinned);
            result.APDU = (APDU)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(APDU));
            handle.Free();

            if (result.APDU.Length > 0)
            {
                byte[] rawBodyLE = new byte[result.APDU.Length];
                usbInterface.InPipe.Read(rawBodyLE);
                result.RawData = rawBodyLE;
                result.Data = new DataAPDU();
            }

            if ((choice != 0) && (result.APDU.Choice != choice) && (maxDiscardedPackets > 0))
            {
                // this is not the packet we are looking for
                return ReadPacketRaw(pipe, choice, --maxDiscardedPackets);
            }

            return result;
        }

        private DataPacket ReadIEEE11073Pakcet(USBPipe pipe)
        {
            DataPacket result = new DataPacket();

            byte[] rawHeaderBE = new byte[4];
            usbInterface.InPipe.Read(rawHeaderBE);

            result.APDU = Tools.GetBigEndian<APDU>(rawHeaderBE);

            
            if (result.APDU.Length > 0)
            {
                byte[] rawBodyBE = new byte[result.APDU.Length];
                usbInterface.InPipe.Read(rawBodyBE);
                result.RawData = rawBodyBE;
                result.Data = new DataAPDU();

                if (result.APDU.Choice == 0xE700)
                {
                    result.Data = Tools.GetBigEndian<DataAPDU>(rawBodyBE);
                }
            }

            return result;
        }

        private string FormatRawData(byte[] rawDataFromUSB)
        {
            return "(" + rawDataFromUSB.Length + "): " + String.Join(" ", Array.ConvertAll(rawDataFromUSB, b => "0x" + b.ToString("X2")));
        }

        private byte UsbPipeReaderMethod(USBPipe pipe)
        {
            byte[] rawByte = new byte[1];
            pipe.Read(rawByte);
            return rawByte[0];            
        }

        [Obsolete]
        private byte[] ReadAllFromPipe(USBPipe pipe)
        {
            List<byte> result = new List<byte>();

            while (result.Count < 2 * 1024)
            {
                var task = Task.Run(() => UsbPipeReaderMethod(pipe)); //you can pass parameters to the method as well
                if (task.Wait(TimeSpan.FromSeconds(0.1)))
                    result.Add(task.Result); //the method returns elegantly
                else
                    break; //the method timed-out
            }

            return result.ToArray();
        }


        private bool PollUSBData(object stateInfo)
        {
            DataPacket dp = null;

            try
            {
                dp = ReadPacketRaw(usbInterface.InPipe, 0xF006, 10);
                if (dp.APDU.Choice != 0xF006) return false;
            }
            catch
            {
                return false;
            }

            DataRateBenchmarkEntry drbe = new DataRateBenchmarkEntry() { TimeStamp = DateTime.UtcNow, IsRead = true, BytesTransferred = dp.RawData.Length, SamplesTransferred = dp.RawData.Length / 2 / channels.Length };

            // cut the 32-bit device id
            byte[] channelData = new byte[dp.RawData.Length - 4];
            Array.Copy(dp.RawData, 4, channelData, 0, channelData.Length);

            Task processUsbDataTask = Task.Run(() =>
            {
                ProcessUSBData(channelData, channelData.Length, drbe);
            });

            lock (BenchmarkEntries)
            {
                BenchmarkEntries.Add(drbe);
                BenchmarkEntries.RemoveAll(be => be.TimeStamp < DateTime.UtcNow.AddSeconds(-5));
            }

            return true;
        }

        private void ProcessUSBData(byte[] rawDataFromUSB, int readBytes, DataRateBenchmarkEntry drbe)
        {
            if (readBytes == 0) return;

            ushort[] shorts = new ushort[readBytes / 2];
            for (int i = 0; i < shorts.Length; i++)
            {
                shorts[i] = System.BitConverter.ToUInt16(rawDataFromUSB, i * 2);
            }

            ushort prevShortValue = 0;

            float[] floats = new float[shorts.Length / channels.Length];
            for (int i = 0; i < channels.Length; i++)
            {
                if (channels[i] == null) continue;

                for (int j = 0; j < floats.Length; j++)
                {
                    ushort shortValue = shorts[j * channels.Length + i];
                    if (j == 0) prevShortValue = shortValue;


                    floats[j] = ((shortValue / 65536.0f) - 0.5f) * (ADCReferenceV * 2);



                    ushort valueChange = (ushort)Math.Abs(shortValue - prevShortValue);
                    if (valueChange > drbe.MaxJumpBetweenSampleValues) drbe.MaxJumpBetweenSampleValues = valueChange;

                    // 12 bit samples are shifted to left by 4 bits, so we need check if the change is higher than 256 << 4 = 4096
                    if (valueChange >= 4096) drbe.EightBitChangeOverflowCount++;
                    if ((j > 0) && (valueChange == 0)) drbe.SameValueWarningCount++;

                    prevShortValue = shortValue;
                }

                channels[i].AppendData(floats);
            }
        }

        public override float[] GetValues()
        {
            return base.GetValues();
        }

        void IDisposable.Dispose()
        {
        }
    }
}
