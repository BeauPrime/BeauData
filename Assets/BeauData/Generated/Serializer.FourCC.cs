using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        protected abstract bool Read_FourCC(ref BeauData.FourCC ioData);
        protected abstract void Write_FourCC(ref BeauData.FourCC ioData);

        private ReadFunc<BeauData.FourCC> Read_FourCC_Cached;
        private WriteFunc<BeauData.FourCC> Write_FourCC_Cached;

        public void Serialize(string inKey, ref BeauData.FourCC ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<BeauData.FourCC>(inKey, ref ioData, inOptions,
                Read_FourCC_Cached ?? (Read_FourCC_Cached = Read_FourCC),
                Write_FourCC_Cached ?? (Write_FourCC_Cached = Write_FourCC));
        }

        public void Serialize(string inKey, ref BeauData.FourCC ioData, BeauData.FourCC inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<BeauData.FourCC>(inKey, ref ioData, inDefault, inOptions,
                Read_FourCC_Cached ?? (Read_FourCC_Cached = Read_FourCC),
                Write_FourCC_Cached ?? (Write_FourCC_Cached = Write_FourCC));
        }

        public void Array(string inKey, ref List<BeauData.FourCC> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<BeauData.FourCC>(inKey, ref ioArray, inOptions,
                Read_FourCC_Cached ?? (Read_FourCC_Cached = Read_FourCC),
                Write_FourCC_Cached ?? (Write_FourCC_Cached = Write_FourCC));
        }

        public void Array(string inKey, ref BeauData.FourCC[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<BeauData.FourCC>(inKey, ref ioArray, inOptions,
                Read_FourCC_Cached ?? (Read_FourCC_Cached = Read_FourCC),
                Write_FourCC_Cached ?? (Write_FourCC_Cached = Write_FourCC));
        }

        public void Set(string inKey, ref HashSet<BeauData.FourCC> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoSet<BeauData.FourCC>(inKey, ref ioSet, inOptions,
                Read_FourCC_Cached ?? (Read_FourCC_Cached = Read_FourCC),
                Write_FourCC_Cached ?? (Write_FourCC_Cached = Write_FourCC));
        }

        public void Map(string inKey, ref Dictionary<string, BeauData.FourCC> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<BeauData.FourCC>(inKey, ref ioMap, inOptions,
                Read_FourCC_Cached ?? (Read_FourCC_Cached = Read_FourCC),
                Write_FourCC_Cached ?? (Write_FourCC_Cached = Write_FourCC));
        }

        public void Map(string inKey, ref Dictionary<int, BeauData.FourCC> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<BeauData.FourCC>(inKey, ref ioMap, inOptions,
                Read_FourCC_Cached ?? (Read_FourCC_Cached = Read_FourCC),
                Write_FourCC_Cached ?? (Write_FourCC_Cached = Write_FourCC));
        }
    }
}
