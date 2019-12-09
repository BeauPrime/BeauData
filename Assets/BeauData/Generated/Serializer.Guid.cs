using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        protected abstract bool Read_Guid(ref System.Guid ioData);
        protected abstract void Write_Guid(ref System.Guid ioData);

        private ReadFunc<System.Guid> Read_Guid_Cached;
        private WriteFunc<System.Guid> Write_Guid_Cached;

        public void Serialize(string inKey, ref System.Guid ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.Guid>(inKey, ref ioData, inOptions,
                Read_Guid_Cached ?? (Read_Guid_Cached = Read_Guid),
                Write_Guid_Cached ?? (Write_Guid_Cached = Write_Guid));
        }

        public void Serialize(string inKey, ref System.Guid ioData, System.Guid inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.Guid>(inKey, ref ioData, inDefault, inOptions,
                Read_Guid_Cached ?? (Read_Guid_Cached = Read_Guid),
                Write_Guid_Cached ?? (Write_Guid_Cached = Write_Guid));
        }

        public void Array(string inKey, ref List<System.Guid> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.Guid>(inKey, ref ioArray, inOptions,
                Read_Guid_Cached ?? (Read_Guid_Cached = Read_Guid),
                Write_Guid_Cached ?? (Write_Guid_Cached = Write_Guid));
        }

        public void Array(string inKey, ref System.Guid[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.Guid>(inKey, ref ioArray, inOptions,
                Read_Guid_Cached ?? (Read_Guid_Cached = Read_Guid),
                Write_Guid_Cached ?? (Write_Guid_Cached = Write_Guid));
        }

        public void Set(string inKey, ref HashSet<System.Guid> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoSet<System.Guid>(inKey, ref ioSet, inOptions,
                Read_Guid_Cached ?? (Read_Guid_Cached = Read_Guid),
                Write_Guid_Cached ?? (Write_Guid_Cached = Write_Guid));
        }

        public void Map(string inKey, ref Dictionary<string, System.Guid> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.Guid>(inKey, ref ioMap, inOptions,
                Read_Guid_Cached ?? (Read_Guid_Cached = Read_Guid),
                Write_Guid_Cached ?? (Write_Guid_Cached = Write_Guid));
        }

        public void Map(string inKey, ref Dictionary<int, System.Guid> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.Guid>(inKey, ref ioMap, inOptions,
                Read_Guid_Cached ?? (Read_Guid_Cached = Read_Guid),
                Write_Guid_Cached ?? (Write_Guid_Cached = Write_Guid));
        }
    }
}
