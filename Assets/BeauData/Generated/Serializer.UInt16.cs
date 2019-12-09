using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        protected abstract bool Read_UInt16(ref System.UInt16 ioData);
        protected abstract void Write_UInt16(ref System.UInt16 ioData);

        private ReadFunc<System.UInt16> Read_UInt16_Cached;
        private WriteFunc<System.UInt16> Write_UInt16_Cached;

        public void Serialize(string inKey, ref System.UInt16 ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.UInt16>(inKey, ref ioData, inOptions,
                Read_UInt16_Cached ?? (Read_UInt16_Cached = Read_UInt16),
                Write_UInt16_Cached ?? (Write_UInt16_Cached = Write_UInt16));
        }

        public void Serialize(string inKey, ref System.UInt16 ioData, System.UInt16 inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.UInt16>(inKey, ref ioData, inDefault, inOptions,
                Read_UInt16_Cached ?? (Read_UInt16_Cached = Read_UInt16),
                Write_UInt16_Cached ?? (Write_UInt16_Cached = Write_UInt16));
        }

        public void Array(string inKey, ref List<System.UInt16> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.UInt16>(inKey, ref ioArray, inOptions,
                Read_UInt16_Cached ?? (Read_UInt16_Cached = Read_UInt16),
                Write_UInt16_Cached ?? (Write_UInt16_Cached = Write_UInt16));
        }

        public void Array(string inKey, ref System.UInt16[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.UInt16>(inKey, ref ioArray, inOptions,
                Read_UInt16_Cached ?? (Read_UInt16_Cached = Read_UInt16),
                Write_UInt16_Cached ?? (Write_UInt16_Cached = Write_UInt16));
        }

        public void Set(string inKey, ref HashSet<System.UInt16> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoSet<System.UInt16>(inKey, ref ioSet, inOptions,
                Read_UInt16_Cached ?? (Read_UInt16_Cached = Read_UInt16),
                Write_UInt16_Cached ?? (Write_UInt16_Cached = Write_UInt16));
        }

        public void Map(string inKey, ref Dictionary<string, System.UInt16> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.UInt16>(inKey, ref ioMap, inOptions,
                Read_UInt16_Cached ?? (Read_UInt16_Cached = Read_UInt16),
                Write_UInt16_Cached ?? (Write_UInt16_Cached = Write_UInt16));
        }

        public void Map(string inKey, ref Dictionary<int, System.UInt16> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.UInt16>(inKey, ref ioMap, inOptions,
                Read_UInt16_Cached ?? (Read_UInt16_Cached = Read_UInt16),
                Write_UInt16_Cached ?? (Write_UInt16_Cached = Write_UInt16));
        }
    }
}
