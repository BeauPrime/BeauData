using System;
using System.Collections.Generic;
using SerializedType = System.Int64;

namespace BeauData
{
    public abstract partial class Serializer
    {
        protected abstract bool Read_Int64(ref SerializedType ioData);
        protected abstract void Write_Int64(ref SerializedType ioData);

        public void Serialize(string inKey, ref SerializedType ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<SerializedType>(inKey, ref ioData, inOptions, Read_Int64, Write_Int64);
        }

        public void Serialize(string inKey, ref SerializedType ioData, SerializedType inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<SerializedType>(inKey, ref ioData, inDefault, inOptions, Read_Int64, Write_Int64);
        }

        public void Array(string inKey, ref List<SerializedType> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<SerializedType>(inKey, ref ioArray, inOptions, Read_Int64, Write_Int64);
        }

        public void Array(string inKey, ref SerializedType[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<SerializedType>(inKey, ref ioArray, inOptions, Read_Int64, Write_Int64);
        }

        public void Set(string inKey, ref HashSet<SerializedType> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoSet<SerializedType>(inKey, ref ioSet, inOptions, Read_Int64, Write_Int64);
        }

        public void Map(string inKey, ref Dictionary<string, SerializedType> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<SerializedType>(inKey, ref ioMap, inOptions, Read_Int64, Write_Int64);
        }

        public void Map(string inKey, ref Dictionary<int, SerializedType> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<SerializedType>(inKey, ref ioMap, inOptions, Read_Int64, Write_Int64);
        }
    }
}
