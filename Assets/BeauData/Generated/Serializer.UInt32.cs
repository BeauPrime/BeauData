using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        protected abstract bool Read_UInt32(ref System.UInt32 ioData);
        protected abstract void Write_UInt32(ref System.UInt32 ioData);

        private ReadFunc<System.UInt32> Read_UInt32_Cached;
        private WriteFunc<System.UInt32> Write_UInt32_Cached;

        public void Serialize(string inKey, ref System.UInt32 ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.UInt32>(inKey, ref ioData, inOptions,
                Read_UInt32_Cached ?? (Read_UInt32_Cached = Read_UInt32),
                Write_UInt32_Cached ?? (Write_UInt32_Cached = Write_UInt32));
        }

        public void Serialize(string inKey, ref System.UInt32 ioData, System.UInt32 inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.UInt32>(inKey, ref ioData, inDefault, inOptions,
                Read_UInt32_Cached ?? (Read_UInt32_Cached = Read_UInt32),
                Write_UInt32_Cached ?? (Write_UInt32_Cached = Write_UInt32));
        }

        public void Array(string inKey, ref List<System.UInt32> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.UInt32>(inKey, ref ioArray, inOptions,
                Read_UInt32_Cached ?? (Read_UInt32_Cached = Read_UInt32),
                Write_UInt32_Cached ?? (Write_UInt32_Cached = Write_UInt32));
        }

        public void Array(string inKey, ref System.UInt32[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.UInt32>(inKey, ref ioArray, inOptions,
                Read_UInt32_Cached ?? (Read_UInt32_Cached = Read_UInt32),
                Write_UInt32_Cached ?? (Write_UInt32_Cached = Write_UInt32));
        }

        public void Set(string inKey, ref HashSet<System.UInt32> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoSet<System.UInt32>(inKey, ref ioSet, inOptions,
                Read_UInt32_Cached ?? (Read_UInt32_Cached = Read_UInt32),
                Write_UInt32_Cached ?? (Write_UInt32_Cached = Write_UInt32));
        }

        public void Map(string inKey, ref Dictionary<string, System.UInt32> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.UInt32>(inKey, ref ioMap, inOptions,
                Read_UInt32_Cached ?? (Read_UInt32_Cached = Read_UInt32),
                Write_UInt32_Cached ?? (Write_UInt32_Cached = Write_UInt32));
        }

        public void Map(string inKey, ref Dictionary<int, System.UInt32> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.UInt32>(inKey, ref ioMap, inOptions,
                Read_UInt32_Cached ?? (Read_UInt32_Cached = Read_UInt32),
                Write_UInt32_Cached ?? (Write_UInt32_Cached = Write_UInt32));
        }
    }
}
