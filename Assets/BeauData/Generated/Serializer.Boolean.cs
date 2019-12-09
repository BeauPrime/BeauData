using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        protected abstract bool Read_Boolean(ref System.Boolean ioData);
        protected abstract void Write_Boolean(ref System.Boolean ioData);

        private ReadFunc<System.Boolean> Read_Boolean_Cached;
        private WriteFunc<System.Boolean> Write_Boolean_Cached;

        public void Serialize(string inKey, ref System.Boolean ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.Boolean>(inKey, ref ioData, inOptions,
                Read_Boolean_Cached ?? (Read_Boolean_Cached = Read_Boolean),
                Write_Boolean_Cached ?? (Write_Boolean_Cached = Write_Boolean));
        }

        public void Serialize(string inKey, ref System.Boolean ioData, System.Boolean inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.Boolean>(inKey, ref ioData, inDefault, inOptions,
                Read_Boolean_Cached ?? (Read_Boolean_Cached = Read_Boolean),
                Write_Boolean_Cached ?? (Write_Boolean_Cached = Write_Boolean));
        }

        public void Array(string inKey, ref List<System.Boolean> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.Boolean>(inKey, ref ioArray, inOptions,
                Read_Boolean_Cached ?? (Read_Boolean_Cached = Read_Boolean),
                Write_Boolean_Cached ?? (Write_Boolean_Cached = Write_Boolean));
        }

        public void Array(string inKey, ref System.Boolean[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.Boolean>(inKey, ref ioArray, inOptions,
                Read_Boolean_Cached ?? (Read_Boolean_Cached = Read_Boolean),
                Write_Boolean_Cached ?? (Write_Boolean_Cached = Write_Boolean));
        }

        public void Set(string inKey, ref HashSet<System.Boolean> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoSet<System.Boolean>(inKey, ref ioSet, inOptions,
                Read_Boolean_Cached ?? (Read_Boolean_Cached = Read_Boolean),
                Write_Boolean_Cached ?? (Write_Boolean_Cached = Write_Boolean));
        }

        public void Map(string inKey, ref Dictionary<string, System.Boolean> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.Boolean>(inKey, ref ioMap, inOptions,
                Read_Boolean_Cached ?? (Read_Boolean_Cached = Read_Boolean),
                Write_Boolean_Cached ?? (Write_Boolean_Cached = Write_Boolean));
        }

        public void Map(string inKey, ref Dictionary<int, System.Boolean> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.Boolean>(inKey, ref ioMap, inOptions,
                Read_Boolean_Cached ?? (Read_Boolean_Cached = Read_Boolean),
                Write_Boolean_Cached ?? (Write_Boolean_Cached = Write_Boolean));
        }
    }
}
