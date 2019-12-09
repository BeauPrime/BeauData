using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        public void Serialize(string inKey, ref UnityEngine.Quaternion ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoStruct<UnityEngine.Quaternion>(inKey, ref ioData, inOptions, Serialize_Quaternion);
        }

        public void Serialize(string inKey, ref UnityEngine.Quaternion ioData, UnityEngine.Quaternion inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoStruct<UnityEngine.Quaternion>(inKey, ref ioData, inDefault, inOptions, Serialize_Quaternion);
        }

        public void Array(string inKey, ref List<UnityEngine.Quaternion> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructArray<UnityEngine.Quaternion>(inKey, ref ioArray, inOptions, Serialize_Quaternion);
        }

        public void Array(string inKey, ref UnityEngine.Quaternion[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructArray<UnityEngine.Quaternion>(inKey, ref ioArray, inOptions, Serialize_Quaternion);
        }

        public void Set(string inKey, ref HashSet<UnityEngine.Quaternion> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructSet<UnityEngine.Quaternion>(inKey, ref ioSet, inOptions, Serialize_Quaternion);
        }

        public void Map(string inKey, ref Dictionary<string, UnityEngine.Quaternion> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructMap<UnityEngine.Quaternion>(inKey, ref ioMap, inOptions, Serialize_Quaternion);
        }

        public void Map(string inKey, ref Dictionary<int, UnityEngine.Quaternion> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructMap<UnityEngine.Quaternion>(inKey, ref ioMap, inOptions, Serialize_Quaternion);
        }
    }
}
