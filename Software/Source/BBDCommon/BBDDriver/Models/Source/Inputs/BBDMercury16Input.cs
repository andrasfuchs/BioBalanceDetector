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

namespace BBDDriver.Models.Source
{
    internal class BBDMercury16Input : MultiChannelInput<IDataChannel>, IDisposable
    {
        private const string DEVICE_GUID = "{f43f6f28-6c4a-4f5c-9fd4-5b95cc84e3a7}";
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

        public BBDMercury16Input(USBDeviceInfo selectedDevice = null) : base(8000, 8)
        {
            // string expectedDeviceID = $"USB\\VID_{DEVICE_VID}&PID_{DEVICE_PID}";

            // libusbK implementation
            //int deviceCount = 0;
            //KLST_DEVINFO_HANDLE deviceInfo;
            //LstK lst = new LstK(KLST_FLAG.NONE);

            //lst.Count(ref deviceCount);
            //while (lst.MoveNext(out deviceInfo))
            //{
            //    if (deviceInfo.DeviceID.StartsWith(expectedDeviceID) && (deviceInfo.DeviceDesc.Contains(DEVICE_DESC)))
            //    {
            //        this.deviceInfo = deviceInfo;
            //    }
            //}

            //if (this.deviceInfo.DeviceID == null)
            //{
            //    throw new System.IO.IOException("There is no BBD Mercury-16 connected on any of the USB ports.");
            //}

            //lst.Free();


            // DotNetLibUsb
            //var deviceList = LibUsbDotNet.LibUsb.LibUsbDevice.AllDevices.ToArray();
            //foreach (var deviceInfoLUDN in deviceList)
            //{
            //    if ((deviceInfoLUDN.Vid == DEVICE_VID) && (deviceInfoLUDN.Pid == DEVICE_PID) && (deviceInfoLUDN.Name.Contains(DEVICE_DESC)))
            //    {
            //        this.deviceInfo = deviceInfoLUDN;
            //    }
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
                USBDeviceInfo[] connectedDevices = GetAllConnectedDevices();

                if (connectedDevices.Length == 0) throw new System.IO.IOException("There is no BBD Mercury-16 connected on any of the USB ports.");

                selectedDevice = connectedDevices[0];
            }

            // Find your device in the array
            usbDevice = new USBDevice(selectedDevice);
            usbInterface = usbDevice.Interfaces.Find(USBBaseClass.PersonalHealthcare);

            // TODO: get the number of channels on the device and their sample rate
            int channelCount = 8;
            for (int i = 0; i < channelCount; i++)
            {
                this.SetChannel(i, new SinglePrecisionDataChannel(this.SamplesPerSecond, this.SamplesPerSecond * 5));
            }

            this.BufferSize = Math.Max(this.BufferSize, this.SamplesPerSecond * this.ChannelCount * 2 / targetFPS);

            Task usbPollTask = Task.Run(() =>
            {
                while (PollUSBData(this)) {}
            });
        }

        public static USBDeviceInfo[] GetAllConnectedDevices()
        {
            return USBDevice.GetDevices(DEVICE_GUID);
        }

        private bool PollUSBData(object stateInfo)
        {
            byte[] rawDataFromUSB = new byte[this.BufferSize];
            int readBytes = 0;

            try
            {
                readBytes = usbInterface.InPipe.Read(rawDataFromUSB);
            }
            catch
            {
                return false;
            }

            DataRateBenchmarkEntry drbe = new DataRateBenchmarkEntry() { TimeStamp = DateTime.UtcNow, IsRead = true, BytesTransferred = readBytes, SamplesTransferred = readBytes / 2 / channels.Length };

            Task processUsbDataTask = Task.Run(() =>
            {
                ProcessUSBData(rawDataFromUSB, readBytes, drbe);
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

                    floats[j] = (shortValue / 65536.0f) * ADCReferenceV;

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
