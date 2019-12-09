using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        public void Serialize(string inKey, ref UnityEngine.BoundsInt ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoStruct<UnityEngine.BoundsInt>(inKey, ref ioData, inOptions, Serialize_BoundsInt);
        }

        public void Serialize(string inKey, ref UnityEngine.BoundsInt ioData, UnityEngine.BoundsInt inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoStruct<UnityEngine.BoundsInt>(inKey, ref ioData, inDefault, inOptions, Serialize_BoundsInt);
        }

        public void Array(string inKey, ref List<UnityEngine.BoundsInt> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructArray<UnityEngine.BoundsInt>(inKey, ref ioArray, inOptions, Serialize_BoundsInt);
        }

        public void Array(string inKey, ref UnityEngine.BoundsInt[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructArray<UnityEngine.BoundsInt>(inKey, ref ioArray, inOptions, Serialize_BoundsInt);
        }

        public void Set(string inKey, ref HashSet<UnityEngine.BoundsInt> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructSet<UnityEngine.BoundsInt>(inKey, ref ioSet, inOptions, Serialize_BoundsInt);
        }

        public void Map(string inKey, ref Dictionary<string, UnityEngine.BoundsInt> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructMap<UnityEngine.BoundsInt>(inKey, ref ioMap, inOptions, Serialize_BoundsInt);
        }

        public void Map(string inKey, ref Dictionary<int, UnityEngine.BoundsInt> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructMap<UnityEngine.BoundsInt>(inKey, ref ioMap, inOptions, Serialize_BoundsInt);
        }
    }
}
