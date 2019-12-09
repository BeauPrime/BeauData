using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        public void Serialize(string inKey, ref UnityEngine.Vector4 ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoStruct<UnityEngine.Vector4>(inKey, ref ioData, inOptions, Serialize_Vector4);
        }

        public void Serialize(string inKey, ref UnityEngine.Vector4 ioData, UnityEngine.Vector4 inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoStruct<UnityEngine.Vector4>(inKey, ref ioData, inDefault, inOptions, Serialize_Vector4);
        }

        public void Array(string inKey, ref List<UnityEngine.Vector4> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructArray<UnityEngine.Vector4>(inKey, ref ioArray, inOptions, Serialize_Vector4);
        }

        public void Array(string inKey, ref UnityEngine.Vector4[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructArray<UnityEngine.Vector4>(inKey, ref ioArray, inOptions, Serialize_Vector4);
        }

        public void Set(string inKey, ref HashSet<UnityEngine.Vector4> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructSet<UnityEngine.Vector4>(inKey, ref ioSet, inOptions, Serialize_Vector4);
        }

        public void Map(string inKey, ref Dictionary<string, UnityEngine.Vector4> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructMap<UnityEngine.Vector4>(inKey, ref ioMap, inOptions, Serialize_Vector4);
        }

        public void Map(string inKey, ref Dictionary<int, UnityEngine.Vector4> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructMap<UnityEngine.Vector4>(inKey, ref ioMap, inOptions, Serialize_Vector4);
        }
    }
}
