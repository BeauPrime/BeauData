using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        protected abstract bool Read_Int64(ref System.Int64 ioData);
        protected abstract void Write_Int64(ref System.Int64 ioData);

        private ReadFunc<System.Int64> Read_Int64_Cached;
        private WriteFunc<System.Int64> Write_Int64_Cached;

        public void Serialize(string inKey, ref System.Int64 ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.Int64>(inKey, ref ioData, inOptions,
                Read_Int64_Cached ?? (Read_Int64_Cached = Read_Int64),
                Write_Int64_Cached ?? (Write_Int64_Cached = Write_Int64));
        }

        public void Serialize(string inKey, ref System.Int64 ioData, System.Int64 inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.Int64>(inKey, ref ioData, inDefault, inOptions,
                Read_Int64_Cached ?? (Read_Int64_Cached = Read_Int64),
                Write_Int64_Cached ?? (Write_Int64_Cached = Write_Int64));
        }

        public void Array(string inKey, ref List<System.Int64> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.Int64>(inKey, ref ioArray, inOptions,
                Read_Int64_Cached ?? (Read_Int64_Cached = Read_Int64),
                Write_Int64_Cached ?? (Write_Int64_Cached = Write_Int64));
        }

        public void Array(string inKey, ref System.Int64[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.Int64>(inKey, ref ioArray, inOptions,
                Read_Int64_Cached ?? (Read_Int64_Cached = Read_Int64),
                Write_Int64_Cached ?? (Write_Int64_Cached = Write_Int64));
        }

        public void Set(string inKey, ref HashSet<System.Int64> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoSet<System.Int64>(inKey, ref ioSet, inOptions,
                Read_Int64_Cached ?? (Read_Int64_Cached = Read_Int64),
                Write_Int64_Cached ?? (Write_Int64_Cached = Write_Int64));
        }

        public void Map(string inKey, ref Dictionary<string, System.Int64> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.Int64>(inKey, ref ioMap, inOptions,
                Read_Int64_Cached ?? (Read_Int64_Cached = Read_Int64),
                Write_Int64_Cached ?? (Write_Int64_Cached = Write_Int64));
        }

        public void Map(string inKey, ref Dictionary<int, System.Int64> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.Int64>(inKey, ref ioMap, inOptions,
                Read_Int64_Cached ?? (Read_Int64_Cached = Read_Int64),
                Write_Int64_Cached ?? (Write_Int64_Cached = Write_Int64));
        }
    }
}
