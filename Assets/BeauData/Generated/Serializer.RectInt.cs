using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        static private TypeUtility.TypeSerializerDelegate<UnityEngine.RectInt> Serialize_RectInt_Cached;

        public void Serialize(string inKey, ref UnityEngine.RectInt ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoStruct<UnityEngine.RectInt>(inKey, ref ioData, inOptions,
                Serialize_RectInt_Cached ?? (Serialize_RectInt_Cached = Serialize_RectInt));
        }

        public void Serialize(string inKey, ref UnityEngine.RectInt ioData, UnityEngine.RectInt inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoStruct<UnityEngine.RectInt>(inKey, ref ioData, inDefault, inOptions,
                Serialize_RectInt_Cached ?? (Serialize_RectInt_Cached = Serialize_RectInt));
        }

        public void Array(string inKey, ref List<UnityEngine.RectInt> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructArray<UnityEngine.RectInt>(inKey, ref ioArray, inOptions,
                Serialize_RectInt_Cached ?? (Serialize_RectInt_Cached = Serialize_RectInt));
        }

        public void Array(string inKey, ref UnityEngine.RectInt[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructArray<UnityEngine.RectInt>(inKey, ref ioArray, inOptions,
                Serialize_RectInt_Cached ?? (Serialize_RectInt_Cached = Serialize_RectInt));
        }

        public void Set(string inKey, ref HashSet<UnityEngine.RectInt> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructSet<UnityEngine.RectInt>(inKey, ref ioSet, inOptions,
                Serialize_RectInt_Cached ?? (Serialize_RectInt_Cached = Serialize_RectInt));
        }

        public void Map(string inKey, ref Dictionary<string, UnityEngine.RectInt> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructMap<UnityEngine.RectInt>(inKey, ref ioMap, inOptions,
                Serialize_RectInt_Cached ?? (Serialize_RectInt_Cached = Serialize_RectInt));
        }

        public void Map(string inKey, ref Dictionary<int, UnityEngine.RectInt> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructMap<UnityEngine.RectInt>(inKey, ref ioMap, inOptions,
                Serialize_RectInt_Cached ?? (Serialize_RectInt_Cached = Serialize_RectInt));
        }
    }
}
