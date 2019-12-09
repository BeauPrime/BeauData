using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        protected abstract bool Read_String(ref System.String ioData);
        protected abstract void Write_String(ref System.String ioData);

        private ReadFunc<System.String> Read_String_Cached;
        private WriteFunc<System.String> Write_String_Cached;

        public void Serialize(string inKey, ref System.String ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.String>(inKey, ref ioData, inOptions,
                Read_String_Cached ?? (Read_String_Cached = Read_String),
                Write_String_Cached ?? (Write_String_Cached = Write_String));
        }

        public void Serialize(string inKey, ref System.String ioData, System.String inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.String>(inKey, ref ioData, inDefault, inOptions,
                Read_String_Cached ?? (Read_String_Cached = Read_String),
                Write_String_Cached ?? (Write_String_Cached = Write_String));
        }

        public void Array(string inKey, ref List<System.String> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.String>(inKey, ref ioArray, inOptions,
                Read_String_Cached ?? (Read_String_Cached = Read_String),
                Write_String_Cached ?? (Write_String_Cached = Write_String));
        }

        public void Array(string inKey, ref System.String[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.String>(inKey, ref ioArray, inOptions,
                Read_String_Cached ?? (Read_String_Cached = Read_String),
                Write_String_Cached ?? (Write_String_Cached = Write_String));
        }

        public void Set(string inKey, ref HashSet<System.String> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoSet<System.String>(inKey, ref ioSet, inOptions,
                Read_String_Cached ?? (Read_String_Cached = Read_String),
                Write_String_Cached ?? (Write_String_Cached = Write_String));
        }

        public void Map(string inKey, ref Dictionary<string, System.String> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.String>(inKey, ref ioMap, inOptions,
                Read_String_Cached ?? (Read_String_Cached = Read_String),
                Write_String_Cached ?? (Write_String_Cached = Write_String));
        }

        public void Map(string inKey, ref Dictionary<int, System.String> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.String>(inKey, ref ioMap, inOptions,
                Read_String_Cached ?? (Read_String_Cached = Read_String),
                Write_String_Cached ?? (Write_String_Cached = Write_String));
        }
    }
}
