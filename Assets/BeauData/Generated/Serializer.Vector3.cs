using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        static private TypeUtility.TypeSerializerDelegate<UnityEngine.Vector3> Serialize_Vector3_Cached;

        public void Serialize(string inKey, ref UnityEngine.Vector3 ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoStruct<UnityEngine.Vector3>(inKey, ref ioData, inOptions,
                Serialize_Vector3_Cached ?? (Serialize_Vector3_Cached = Serialize_Vector3));
        }

        public void Serialize(string inKey, ref UnityEngine.Vector3 ioData, UnityEngine.Vector3 inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoStruct<UnityEngine.Vector3>(inKey, ref ioData, inDefault, inOptions,
                Serialize_Vector3_Cached ?? (Serialize_Vector3_Cached = Serialize_Vector3));
        }

        public void Array(string inKey, ref List<UnityEngine.Vector3> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructArray<UnityEngine.Vector3>(inKey, ref ioArray, inOptions,
                Serialize_Vector3_Cached ?? (Serialize_Vector3_Cached = Serialize_Vector3));
        }

        public void Array(string inKey, ref UnityEngine.Vector3[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructArray<UnityEngine.Vector3>(inKey, ref ioArray, inOptions,
                Serialize_Vector3_Cached ?? (Serialize_Vector3_Cached = Serialize_Vector3));
        }

        public void Set(string inKey, ref HashSet<UnityEngine.Vector3> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructSet<UnityEngine.Vector3>(inKey, ref ioSet, inOptions,
                Serialize_Vector3_Cached ?? (Serialize_Vector3_Cached = Serialize_Vector3));
        }

        public void Map(string inKey, ref Dictionary<string, UnityEngine.Vector3> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructMap<UnityEngine.Vector3>(inKey, ref ioMap, inOptions,
                Serialize_Vector3_Cached ?? (Serialize_Vector3_Cached = Serialize_Vector3));
        }

        public void Map(string inKey, ref Dictionary<int, UnityEngine.Vector3> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructMap<UnityEngine.Vector3>(inKey, ref ioMap, inOptions,
                Serialize_Vector3_Cached ?? (Serialize_Vector3_Cached = Serialize_Vector3));
        }
    }
}
