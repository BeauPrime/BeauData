using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        protected abstract bool Read_Int16(ref System.Int16 ioData);
        protected abstract void Write_Int16(ref System.Int16 ioData);

        private ReadFunc<System.Int16> Read_Int16_Cached;
        private WriteFunc<System.Int16> Write_Int16_Cached;

        public void Serialize(string inKey, ref System.Int16 ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.Int16>(inKey, ref ioData, inOptions,
                Read_Int16_Cached ?? (Read_Int16_Cached = Read_Int16),
                Write_Int16_Cached ?? (Write_Int16_Cached = Write_Int16));
        }

        public void Serialize(string inKey, ref System.Int16 ioData, System.Int16 inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.Int16>(inKey, ref ioData, inDefault, inOptions,
                Read_Int16_Cached ?? (Read_Int16_Cached = Read_Int16),
                Write_Int16_Cached ?? (Write_Int16_Cached = Write_Int16));
        }

        public void Array(string inKey, ref List<System.Int16> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.Int16>(inKey, ref ioArray, inOptions,
                Read_Int16_Cached ?? (Read_Int16_Cached = Read_Int16),
                Write_Int16_Cached ?? (Write_Int16_Cached = Write_Int16));
        }

        public void Array(string inKey, ref System.Int16[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.Int16>(inKey, ref ioArray, inOptions,
                Read_Int16_Cached ?? (Read_Int16_Cached = Read_Int16),
                Write_Int16_Cached ?? (Write_Int16_Cached = Write_Int16));
        }

        public void Set(string inKey, ref HashSet<System.Int16> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoSet<System.Int16>(inKey, ref ioSet, inOptions,
                Read_Int16_Cached ?? (Read_Int16_Cached = Read_Int16),
                Write_Int16_Cached ?? (Write_Int16_Cached = Write_Int16));
        }

        public void Map(string inKey, ref Dictionary<string, System.Int16> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.Int16>(inKey, ref ioMap, inOptions,
                Read_Int16_Cached ?? (Read_Int16_Cached = Read_Int16),
                Write_Int16_Cached ?? (Write_Int16_Cached = Write_Int16));
        }

        public void Map(string inKey, ref Dictionary<int, System.Int16> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.Int16>(inKey, ref ioMap, inOptions,
                Read_Int16_Cached ?? (Read_Int16_Cached = Read_Int16),
                Write_Int16_Cached ?? (Write_Int16_Cached = Write_Int16));
        }
    }
}
