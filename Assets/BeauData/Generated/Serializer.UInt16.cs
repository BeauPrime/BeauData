using System;
using System.Collections.Generic;
using SerializedType = System.UInt16;

namespace BeauData
{
    public abstract partial class Serializer
    {
        protected abstract bool Read_UInt16(ref SerializedType ioData);
        protected abstract void Write_UInt16(ref SerializedType ioData);

        public void Serialize(string inKey, ref SerializedType ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<SerializedType>(inKey, ref ioData, inOptions, Read_UInt16, Write_UInt16);
        }

        public void Serialize(string inKey, ref SerializedType ioData, SerializedType inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<SerializedType>(inKey, ref ioData, inDefault, inOptions, Read_UInt16, Write_UInt16);
        }

        public void Array(string inKey, ref List<SerializedType> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<SerializedType>(inKey, ref ioArray, inOptions, Read_UInt16, Write_UInt16);
        }

        public void Array(string inKey, ref SerializedType[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<SerializedType>(inKey, ref ioArray, inOptions, Read_UInt16, Write_UInt16);
        }

        public void Set(string inKey, ref HashSet<SerializedType> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoSet<SerializedType>(inKey, ref ioSet, inOptions, Read_UInt16, Write_UInt16);
        }

        public void Map(string inKey, ref Dictionary<string, SerializedType> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<SerializedType>(inKey, ref ioMap, inOptions, Read_UInt16, Write_UInt16);
        }

        public void Map(string inKey, ref Dictionary<int, SerializedType> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<SerializedType>(inKey, ref ioMap, inOptions, Read_UInt16, Write_UInt16);
        }
    }
}
