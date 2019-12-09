using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        public void Serialize(string inKey, ref UnityEngine.Vector2Int ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoStruct<UnityEngine.Vector2Int>(inKey, ref ioData, inOptions, Serialize_Vector2Int);
        }

        public void Serialize(string inKey, ref UnityEngine.Vector2Int ioData, UnityEngine.Vector2Int inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoStruct<UnityEngine.Vector2Int>(inKey, ref ioData, inDefault, inOptions, Serialize_Vector2Int);
        }

        public void Array(string inKey, ref List<UnityEngine.Vector2Int> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructArray<UnityEngine.Vector2Int>(inKey, ref ioArray, inOptions, Serialize_Vector2Int);
        }

        public void Array(string inKey, ref UnityEngine.Vector2Int[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructArray<UnityEngine.Vector2Int>(inKey, ref ioArray, inOptions, Serialize_Vector2Int);
        }

        public void Set(string inKey, ref HashSet<UnityEngine.Vector2Int> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructSet<UnityEngine.Vector2Int>(inKey, ref ioSet, inOptions, Serialize_Vector2Int);
        }

        public void Map(string inKey, ref Dictionary<string, UnityEngine.Vector2Int> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructMap<UnityEngine.Vector2Int>(inKey, ref ioMap, inOptions, Serialize_Vector2Int);
        }

        public void Map(string inKey, ref Dictionary<int, UnityEngine.Vector2Int> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructMap<UnityEngine.Vector2Int>(inKey, ref ioMap, inOptions, Serialize_Vector2Int);
        }
    }
}
