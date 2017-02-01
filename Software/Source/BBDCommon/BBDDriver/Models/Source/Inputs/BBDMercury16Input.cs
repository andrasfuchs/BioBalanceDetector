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

        //private KLST_DEVINFO_HANDLE deviceInfo;
        //private LibUsbDotNet.Main.UsbRegistry deviceInfo;
        //private LibUsbDotNet.UsbEndpointReader reader;
        //private LibUsbDotNet.UsbEndpointWriter writer;

        public List<DataRateBenchmarkEntry> BenchmarkEntries = new List<DataRateBenchmarkEntry>();

        private USBDevice usbDevice;
        private USBInterface usbInterface;

        private Timer refreshUSBInputTimer;

        public int CPUClockSpeedMhz { get; set; } = 32;
        public int ADCSampleRatekMhz { get; set; } = 100;
        public float ADCReferenceV { get; set; } = 1.0f;
        public int ADCGain { get; set; } = 1;

        public BBDMercury16Input(USBDeviceInfo selectedDevice = null) : base(1000, 8)
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
            int samplesPerSecond = 1000;
            for (int i = 0; i < channelCount; i++)
            {
                this.SetChannel(i, new SinglePrecisionDataChannel(samplesPerSecond, samplesPerSecond * 5));
            }

            refreshUSBInputTimer = new Timer(PollUSBData, this, 0, 50);
        }

        public static USBDeviceInfo[] GetAllConnectedDevices()
        {
            return USBDevice.GetDevices(DEVICE_GUID);
        }

        private void PollUSBData(object stateInfo)
        {
            byte[] rawDataFromUSB = new byte[1024 * 64];
            int readBytes = usbInterface.InPipe.Read(rawDataFromUSB);

            if (readBytes == 0) return;

            short[] shorts = new short[readBytes / 2];
            for (int i = 0; i < shorts.Length; i++)
            {
                shorts[i] = System.BitConverter.ToInt16(rawDataFromUSB, i * 2);
            }

            float[] floats = new float[shorts.Length / channels.Length];
            for (int i = 0; i < channels.Length; i++)
            {
                if (channels[i] == null) continue;

                for (int j = 0; j < floats.Length; j++)
                {
                    floats[j] = (shorts[j * channels.Length + i] / 65536.0f) * ADCReferenceV;
                }

                channels[i].AppendData(floats);
            }

            BenchmarkEntries.Add(new DataRateBenchmarkEntry() { TimeStamp = DateTime.UtcNow, IsRead = true, BytesTransferred = readBytes, SamplesTransferred = floats.Length });
            BenchmarkEntries.RemoveAll(drbe => drbe.TimeStamp < DateTime.UtcNow.AddSeconds(-5));
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
