using System;
using System.Collections.Generic;
using SerializedType = UnityEngine.Quaternion;

namespace BeauData
{
    public abstract partial class Serializer
    {
        static private void Serialize_Quaternion(ref SerializedType ioData, Serializer ioSerializer)
        {
            ioSerializer.Serialize("x", ref ioData.x, FieldOptions.PreferAttribute);
            ioSerializer.Serialize("y", ref ioData.y, FieldOptions.PreferAttribute);
            ioSerializer.Serialize("z", ref ioData.z, FieldOptions.PreferAttribute);
            ioSerializer.Serialize("w", ref ioData.w, FieldOptions.PreferAttribute);
        }

        public void Serialize(string inKey, ref SerializedType ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoStruct<SerializedType>(inKey, ref ioData, inOptions, Serialize_Quaternion);
        }

        public void Serialize(string inKey, ref SerializedType ioData, SerializedType inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoStruct<SerializedType>(inKey, ref ioData, inDefault, inOptions, Serialize_Quaternion);
        }

        public void Array(string inKey, ref List<SerializedType> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructArray<SerializedType>(inKey, ref ioArray, inOptions, Serialize_Quaternion);
        }

        public void Array(string inKey, ref SerializedType[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructArray<SerializedType>(inKey, ref ioArray, inOptions, Serialize_Quaternion);
        }

        public void Set(string inKey, ref HashSet<SerializedType> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructSet<SerializedType>(inKey, ref ioSet, inOptions, Serialize_Quaternion);
        }

        public void Map(string inKey, ref Dictionary<string, SerializedType> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructMap<SerializedType>(inKey, ref ioMap, inOptions, Serialize_Quaternion);
        }

        public void Map(string inKey, ref Dictionary<int, SerializedType> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructMap<SerializedType>(inKey, ref ioMap, inOptions, Serialize_Quaternion);
        }
    }
}
