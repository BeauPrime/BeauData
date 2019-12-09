using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        public void Serialize(string inKey, ref UnityEngine.Rect ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoStruct<UnityEngine.Rect>(inKey, ref ioData, inOptions, Serialize_Rect);
        }

        public void Serialize(string inKey, ref UnityEngine.Rect ioData, UnityEngine.Rect inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoStruct<UnityEngine.Rect>(inKey, ref ioData, inDefault, inOptions, Serialize_Rect);
        }

        public void Array(string inKey, ref List<UnityEngine.Rect> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructArray<UnityEngine.Rect>(inKey, ref ioArray, inOptions, Serialize_Rect);
        }

        public void Array(string inKey, ref UnityEngine.Rect[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructArray<UnityEngine.Rect>(inKey, ref ioArray, inOptions, Serialize_Rect);
        }

        public void Set(string inKey, ref HashSet<UnityEngine.Rect> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructSet<UnityEngine.Rect>(inKey, ref ioSet, inOptions, Serialize_Rect);
        }

        public void Map(string inKey, ref Dictionary<string, UnityEngine.Rect> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructMap<UnityEngine.Rect>(inKey, ref ioMap, inOptions, Serialize_Rect);
        }

        public void Map(string inKey, ref Dictionary<int, UnityEngine.Rect> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructMap<UnityEngine.Rect>(inKey, ref ioMap, inOptions, Serialize_Rect);
        }
    }
}
