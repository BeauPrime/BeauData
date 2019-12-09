using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        public void Serialize(string inKey, ref UnityEngine.Bounds ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoStruct<UnityEngine.Bounds>(inKey, ref ioData, inOptions, Serialize_Bounds);
        }

        public void Serialize(string inKey, ref UnityEngine.Bounds ioData, UnityEngine.Bounds inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoStruct<UnityEngine.Bounds>(inKey, ref ioData, inDefault, inOptions, Serialize_Bounds);
        }

        public void Array(string inKey, ref List<UnityEngine.Bounds> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructArray<UnityEngine.Bounds>(inKey, ref ioArray, inOptions, Serialize_Bounds);
        }

        public void Array(string inKey, ref UnityEngine.Bounds[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructArray<UnityEngine.Bounds>(inKey, ref ioArray, inOptions, Serialize_Bounds);
        }

        public void Set(string inKey, ref HashSet<UnityEngine.Bounds> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructSet<UnityEngine.Bounds>(inKey, ref ioSet, inOptions, Serialize_Bounds);
        }

        public void Map(string inKey, ref Dictionary<string, UnityEngine.Bounds> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructMap<UnityEngine.Bounds>(inKey, ref ioMap, inOptions, Serialize_Bounds);
        }

        public void Map(string inKey, ref Dictionary<int, UnityEngine.Bounds> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructMap<UnityEngine.Bounds>(inKey, ref ioMap, inOptions, Serialize_Bounds);
        }
    }
}
