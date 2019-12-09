using System.Collections.Generic;
using UnityEngine;

namespace BeauData
{
    public abstract partial class Serializer
    {
        private bool Read_Color(ref UnityEngine.Color ioData)
        {
            if (!IsBinary())
            {
                string hexString = null;
                bool bSuccess = Read_String(ref hexString);
                if (bSuccess)
                    bSuccess = ColorUtility.TryParseHtmlString(hexString, out ioData);
                return bSuccess;
            }
            else
            {
                uint colorUint = 0;
                bool bSuccess = Read_UInt32(ref colorUint);
                if (bSuccess)
                    UintToColor(ref colorUint, ref ioData);
                return bSuccess;
            }
        }
        private void Write_Color(ref UnityEngine.Color ioData)
        {
            if (!IsBinary())
            {
                string hexString = "#" + ColorUtility.ToHtmlStringRGBA(ioData);
                Write_String(ref hexString);
            }
            else
            {
                uint colorUint = ColorToUint(ref ioData);
                Write_UInt32(ref colorUint);
            }
        }

        private ReadFunc<UnityEngine.Color> Read_Color_Cached;
        private WriteFunc<UnityEngine.Color> Write_Color_Cached;

        public void Serialize(string inKey, ref UnityEngine.Color ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<UnityEngine.Color>(inKey, ref ioData, inOptions,
                Read_Color_Cached ?? (Read_Color_Cached = Read_Color),
                Write_Color_Cached ?? (Write_Color_Cached = Write_Color));
        }

        public void Serialize(string inKey, ref UnityEngine.Color ioData, UnityEngine.Color inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<UnityEngine.Color>(inKey, ref ioData, inDefault, inOptions,
                Read_Color_Cached ?? (Read_Color_Cached = Read_Color),
                Write_Color_Cached ?? (Write_Color_Cached = Write_Color));
        }

        public void Array(string inKey, ref List<UnityEngine.Color> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<UnityEngine.Color>(inKey, ref ioArray, inOptions,
                Read_Color_Cached ?? (Read_Color_Cached = Read_Color),
                Write_Color_Cached ?? (Write_Color_Cached = Write_Color));
        }

        public void Array(string inKey, ref UnityEngine.Color[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<UnityEngine.Color>(inKey, ref ioArray, inOptions,
                Read_Color_Cached ?? (Read_Color_Cached = Read_Color),
                Write_Color_Cached ?? (Write_Color_Cached = Write_Color));
        }

        public void Set(string inKey, ref HashSet<UnityEngine.Color> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoSet<UnityEngine.Color>(inKey, ref ioSet, inOptions,
                Read_Color_Cached ?? (Read_Color_Cached = Read_Color),
                Write_Color_Cached ?? (Write_Color_Cached = Write_Color));
        }

        public void Map(string inKey, ref Dictionary<string, UnityEngine.Color> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<UnityEngine.Color>(inKey, ref ioMap, inOptions,
                Read_Color_Cached ?? (Read_Color_Cached = Read_Color),
                Write_Color_Cached ?? (Write_Color_Cached = Write_Color));
        }

        public void Map(string inKey, ref Dictionary<int, UnityEngine.Color> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<UnityEngine.Color>(inKey, ref ioMap, inOptions,
                Read_Color_Cached ?? (Read_Color_Cached = Read_Color),
                Write_Color_Cached ?? (Write_Color_Cached = Write_Color));
        }

        static private uint ColorToUint(ref Color inColor)
        {
            uint c = (uint)((byte)(inColor.r * 255) << 24)
                + (uint)((byte)(inColor.g * 255) << 16)
                + (uint)((byte)(inColor.b * 255) << 8)
                + (uint)((byte)(inColor.a * 255));
            return c;
        }

        static private void UintToColor(ref uint inValue, ref Color ioOutput)
        {
            ioOutput.a = (byte)(inValue & 0xFF) / 255f;
            ioOutput.b = (byte)((inValue >> 8) & 0xFF) / 255f;
            ioOutput.g = (byte)((inValue >> 16) & 0xFF) / 255f;
            ioOutput.r = (byte)((inValue >> 24) & 0xFF) / 255f;
        }
    }
}
