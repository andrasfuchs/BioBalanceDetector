using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libusbK;

namespace BBDDriver.Models.Source
{
    internal class BBDMercury16Input : MultiChannelInput<IDataChannel>, IDisposable
    {
        private const string DEVICE_VID = "03EB";
        private const string DEVICE_PID = "2405";
        private const string DEVICE_DESC = "Mercury-16";

        private KLST_DEVINFO_HANDLE deviceInfo;

        public BBDMercury16Input() : base(8000, 16)
        {
            string expectedDeviceID = $"USB\\VID_{DEVICE_VID}&PID_{DEVICE_PID}";

            int deviceCount = 0;
            KLST_DEVINFO_HANDLE deviceInfo;
            LstK lst = new LstK(KLST_FLAG.NONE);

            lst.Count(ref deviceCount);
            while (lst.MoveNext(out deviceInfo))
            {
                if (deviceInfo.DeviceID.StartsWith(expectedDeviceID) && (deviceInfo.DeviceDesc.Contains(DEVICE_DESC)))
                {
                    this.deviceInfo = deviceInfo;
                }
            }

            if (this.deviceInfo.DeviceID == null)
            {
                throw new System.IO.IOException("There is no BBD Mercury-16 connected on any of the USB ports.");
            }

            lst.Free();
        }

        void IDisposable.Dispose()
        {
        }
    }
}
