using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BBD.DSP.Models.IEEE11073
{
    public class DataPacket
    {
        public APDU APDU;

        public byte[] RawData;

        public DataAPDU Data;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct APDU
    {
        public UInt16 Choice;

        public UInt16 Length;

        public override string ToString()
        {
            return $"0x{((int)Choice).ToString("X4")} ({Length} bytes)";
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DataAPDU
    {
        public UInt16 DataLength;

        public UInt16 InvokeID;

        public UInt16 Choice;

        public UInt16 Length;

        public override string ToString()
        {
            return $"0x{((int)Choice).ToString("X4")} ({Length} bytes)";
        }
    }

    public static class Tools
    {
        public enum Endianness
        {
            BigEndian,
            LittleEndian
        }

        public static T GetBigEndian<T>(byte[] data)
        {
            byte[] dataLE = Tools.MaybeAdjustEndianness(typeof(T), data, Tools.Endianness.BigEndian);
            GCHandle handle = GCHandle.Alloc(dataLE, GCHandleType.Pinned);
            T result = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();

            return result;
        }

        private static byte[] MaybeAdjustEndianness(Type type, byte[] data, Endianness endianness, int startOffset = 0)
        {
            if ((BitConverter.IsLittleEndian) == (endianness == Endianness.LittleEndian))
            {
                // nothing to change => return
                return data;
            }

            foreach (var field in type.GetFields())
            {
                var fieldType = field.FieldType;
                if (field.IsStatic)
                    // don't process static fields
                    continue;

                if (fieldType == typeof(string))
                    // don't swap bytes for strings
                    continue;

                var offset = Marshal.OffsetOf(type, field.Name).ToInt32();

                // handle enums
                if (fieldType.IsEnum)
                    fieldType = Enum.GetUnderlyingType(fieldType);

                // check for sub-fields to recurse if necessary
                var subFields = fieldType.GetFields().Where(subField => subField.IsStatic == false).ToArray();

                var effectiveOffset = startOffset + offset;

                if (effectiveOffset >= data.Length) continue;

                if (subFields.Length == 0)
                {
                    Array.Reverse(data, effectiveOffset, Marshal.SizeOf(fieldType));
                }
                else
                {
                    // recurse
                    MaybeAdjustEndianness(fieldType, data, endianness, effectiveOffset);
                }
            }

            return data;
        }

        internal static T BytesToStruct<T>(byte[] rawData, Endianness endianness) where T : struct
        {
            T result = default(T);

            MaybeAdjustEndianness(typeof(T), rawData, endianness);

            GCHandle handle = GCHandle.Alloc(rawData, GCHandleType.Pinned);

            try
            {
                IntPtr rawDataPtr = handle.AddrOfPinnedObject();
                result = (T)Marshal.PtrToStructure(rawDataPtr, typeof(T));
            }
            finally
            {
                handle.Free();
            }

            return result;
        }

        internal static byte[] StructToBytes<T>(T data, Endianness endianness) where T : struct
        {
            byte[] rawData = new byte[Marshal.SizeOf(data)];
            GCHandle handle = GCHandle.Alloc(rawData, GCHandleType.Pinned);
            try
            {
                IntPtr rawDataPtr = handle.AddrOfPinnedObject();
                Marshal.StructureToPtr(data, rawDataPtr, false);
            }
            finally
            {
                handle.Free();
            }

            MaybeAdjustEndianness(typeof(T), rawData, endianness);

            return rawData;
        }
    }
}
