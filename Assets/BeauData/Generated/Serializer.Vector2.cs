using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        static private TypeUtility.TypeSerializerDelegate<UnityEngine.Vector2> Serialize_Vector2_Cached;

        public void Serialize(string inKey, ref UnityEngine.Vector2 ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoStruct<UnityEngine.Vector2>(inKey, ref ioData, inOptions,
                Serialize_Vector2_Cached ?? (Serialize_Vector2_Cached = Serialize_Vector2));
        }

        public void Serialize(string inKey, ref UnityEngine.Vector2 ioData, UnityEngine.Vector2 inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoStruct<UnityEngine.Vector2>(inKey, ref ioData, inDefault, inOptions,
                Serialize_Vector2_Cached ?? (Serialize_Vector2_Cached = Serialize_Vector2));
        }

        public void Array(string inKey, ref List<UnityEngine.Vector2> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructArray<UnityEngine.Vector2>(inKey, ref ioArray, inOptions,
                Serialize_Vector2_Cached ?? (Serialize_Vector2_Cached = Serialize_Vector2));
        }

        public void Array(string inKey, ref UnityEngine.Vector2[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructArray<UnityEngine.Vector2>(inKey, ref ioArray, inOptions,
                Serialize_Vector2_Cached ?? (Serialize_Vector2_Cached = Serialize_Vector2));
        }

        public void Set(string inKey, ref HashSet<UnityEngine.Vector2> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructSet<UnityEngine.Vector2>(inKey, ref ioSet, inOptions,
                Serialize_Vector2_Cached ?? (Serialize_Vector2_Cached = Serialize_Vector2));
        }

        public void Map(string inKey, ref Dictionary<string, UnityEngine.Vector2> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructMap<UnityEngine.Vector2>(inKey, ref ioMap, inOptions,
                Serialize_Vector2_Cached ?? (Serialize_Vector2_Cached = Serialize_Vector2));
        }

        public void Map(string inKey, ref Dictionary<int, UnityEngine.Vector2> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructMap<UnityEngine.Vector2>(inKey, ref ioMap, inOptions,
                Serialize_Vector2_Cached ?? (Serialize_Vector2_Cached = Serialize_Vector2));
        }
    }
}
