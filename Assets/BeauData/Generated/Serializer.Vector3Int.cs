using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        public void Serialize(string inKey, ref UnityEngine.Vector3Int ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoStruct<UnityEngine.Vector3Int>(inKey, ref ioData, inOptions, Serialize_Vector3Int);
        }

        public void Serialize(string inKey, ref UnityEngine.Vector3Int ioData, UnityEngine.Vector3Int inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoStruct<UnityEngine.Vector3Int>(inKey, ref ioData, inDefault, inOptions, Serialize_Vector3Int);
        }

        public void Array(string inKey, ref List<UnityEngine.Vector3Int> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructArray<UnityEngine.Vector3Int>(inKey, ref ioArray, inOptions, Serialize_Vector3Int);
        }

        public void Array(string inKey, ref UnityEngine.Vector3Int[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructArray<UnityEngine.Vector3Int>(inKey, ref ioArray, inOptions, Serialize_Vector3Int);
        }

        public void Set(string inKey, ref HashSet<UnityEngine.Vector3Int> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructSet<UnityEngine.Vector3Int>(inKey, ref ioSet, inOptions, Serialize_Vector3Int);
        }

        public void Map(string inKey, ref Dictionary<string, UnityEngine.Vector3Int> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructMap<UnityEngine.Vector3Int>(inKey, ref ioMap, inOptions, Serialize_Vector3Int);
        }

        public void Map(string inKey, ref Dictionary<int, UnityEngine.Vector3Int> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructMap<UnityEngine.Vector3Int>(inKey, ref ioMap, inOptions, Serialize_Vector3Int);
        }
    }
}
