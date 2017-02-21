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
        // warning: interface guid changes when the driver is regenerated (by zadig)
        private const string DEVICE_INTERFACE_GUID = "{45e6adab-0529-4678-bd5f-4b9fed0a9c40}";
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
            var deviceList = LibUsbDotNet.LibUsb.LibUsbDevice.AllDevices.ToArray();
            foreach (var deviceInfoLUDN in deviceList)
            {
                Console.WriteLine("libusb: " + deviceInfoLUDN.DeviceInterfaceGuids[0].ToString().ToLower() + " - " + deviceInfoLUDN.Device.Info.ProductString);
            }

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
            }

            // Find your device in the array
            usbDevice = new USBDevice(selectedDevice);
            usbInterface = usbDevice.Interfaces.Find(USBBaseClass.PersonalHealthcare);
            


            byte[] rawDataFromUSB = null; 
            Console.WriteLine();
            while (true)
            {
                usbInterface.OutPipe.Write(new byte[] 
                {
                    0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA,
                    0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA,
                    0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA,
                    0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA
                });
                
                rawDataFromUSB = ReadAllFromPipe(usbInterface.InPipe);
                if (rawDataFromUSB.Length > 0) Console.WriteLine(FormatRawData(rawDataFromUSB));

                Thread.Sleep(5000);
            }

            
            
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

        private byte[] ReadAllFromPipe(USBPipe pipe)
        {
            List<byte> result = new List<byte>();

            while (true)
            {
                var task = Task.Run(() => UsbPipeReaderMethod(pipe)); //you can pass parameters to the method as well
                if (task.Wait(TimeSpan.FromSeconds(0.1)))
                    result.Add(task.Result); //the method returns elegantly
                else
                    return result.ToArray(); //the method timed-out
            }
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
