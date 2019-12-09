using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        protected abstract bool Read_UInt64(ref System.UInt64 ioData);
        protected abstract void Write_UInt64(ref System.UInt64 ioData);

        private ReadFunc<System.UInt64> Read_UInt64_Cached;
        private WriteFunc<System.UInt64> Write_UInt64_Cached;

        public void Serialize(string inKey, ref System.UInt64 ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.UInt64>(inKey, ref ioData, inOptions,
                Read_UInt64_Cached ?? (Read_UInt64_Cached = Read_UInt64),
                Write_UInt64_Cached ?? (Write_UInt64_Cached = Write_UInt64));
        }

        public void Serialize(string inKey, ref System.UInt64 ioData, System.UInt64 inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.UInt64>(inKey, ref ioData, inDefault, inOptions,
                Read_UInt64_Cached ?? (Read_UInt64_Cached = Read_UInt64),
                Write_UInt64_Cached ?? (Write_UInt64_Cached = Write_UInt64));
        }

        public void Array(string inKey, ref List<System.UInt64> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.UInt64>(inKey, ref ioArray, inOptions,
                Read_UInt64_Cached ?? (Read_UInt64_Cached = Read_UInt64),
                Write_UInt64_Cached ?? (Write_UInt64_Cached = Write_UInt64));
        }

        public void Array(string inKey, ref System.UInt64[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.UInt64>(inKey, ref ioArray, inOptions,
                Read_UInt64_Cached ?? (Read_UInt64_Cached = Read_UInt64),
                Write_UInt64_Cached ?? (Write_UInt64_Cached = Write_UInt64));
        }

        public void Set(string inKey, ref HashSet<System.UInt64> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoSet<System.UInt64>(inKey, ref ioSet, inOptions,
                Read_UInt64_Cached ?? (Read_UInt64_Cached = Read_UInt64),
                Write_UInt64_Cached ?? (Write_UInt64_Cached = Write_UInt64));
        }

        public void Map(string inKey, ref Dictionary<string, System.UInt64> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.UInt64>(inKey, ref ioMap, inOptions,
                Read_UInt64_Cached ?? (Read_UInt64_Cached = Read_UInt64),
                Write_UInt64_Cached ?? (Write_UInt64_Cached = Write_UInt64));
        }

        public void Map(string inKey, ref Dictionary<int, System.UInt64> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.UInt64>(inKey, ref ioMap, inOptions,
                Read_UInt64_Cached ?? (Read_UInt64_Cached = Read_UInt64),
                Write_UInt64_Cached ?? (Write_UInt64_Cached = Write_UInt64));
        }
    }
}
