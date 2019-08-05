using System;
using System.Collections.Generic;
using SerializedType = UnityEngine.Vector3Int;

namespace BeauData
{
    public abstract partial class Serializer
    {
        static private void Serialize_Vector3Int(ref SerializedType ioData, Serializer ioSerializer)
        {
            int x = ioData.x, y = ioData.y, z = ioData.z;

            ioSerializer.Serialize("x", ref x, FieldOptions.PreferAttribute);
            ioSerializer.Serialize("y", ref y, FieldOptions.PreferAttribute);
            ioSerializer.Serialize("z", ref z, FieldOptions.PreferAttribute);

            if (ioSerializer.IsReading)
            {
                ioData.x = x;
                ioData.y = y;
                ioData.z = z;
            }
        }

        public void Serialize(string inKey, ref SerializedType ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoStruct<SerializedType>(inKey, ref ioData, inOptions, Serialize_Vector3Int);
        }

        public void Serialize(string inKey, ref SerializedType ioData, SerializedType inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoStruct<SerializedType>(inKey, ref ioData, inDefault, inOptions, Serialize_Vector3Int);
        }

        public void Array(string inKey, ref List<SerializedType> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructArray<SerializedType>(inKey, ref ioArray, inOptions, Serialize_Vector3Int);
        }

        public void Array(string inKey, ref SerializedType[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructArray<SerializedType>(inKey, ref ioArray, inOptions, Serialize_Vector3Int);
        }

        public void Set(string inKey, ref HashSet<SerializedType> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructSet<SerializedType>(inKey, ref ioSet, inOptions, Serialize_Vector3Int);
        }

        public void Map(string inKey, ref Dictionary<string, SerializedType> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructMap<SerializedType>(inKey, ref ioMap, inOptions, Serialize_Vector3Int);
        }

        public void Map(string inKey, ref Dictionary<int, SerializedType> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructMap<SerializedType>(inKey, ref ioMap, inOptions, Serialize_Vector3Int);
        }
    }
}
