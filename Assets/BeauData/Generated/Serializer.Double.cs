using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        protected abstract bool Read_Double(ref System.Double ioData);
        protected abstract void Write_Double(ref System.Double ioData);

        private ReadFunc<System.Double> Read_Double_Cached;
        private WriteFunc<System.Double> Write_Double_Cached;

        public void Serialize(string inKey, ref System.Double ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.Double>(inKey, ref ioData, inOptions,
                Read_Double_Cached ?? (Read_Double_Cached = Read_Double),
                Write_Double_Cached ?? (Write_Double_Cached = Write_Double));
        }

        public void Serialize(string inKey, ref System.Double ioData, System.Double inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.Double>(inKey, ref ioData, inDefault, inOptions,
                Read_Double_Cached ?? (Read_Double_Cached = Read_Double),
                Write_Double_Cached ?? (Write_Double_Cached = Write_Double));
        }

        public void Array(string inKey, ref List<System.Double> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.Double>(inKey, ref ioArray, inOptions,
                Read_Double_Cached ?? (Read_Double_Cached = Read_Double),
                Write_Double_Cached ?? (Write_Double_Cached = Write_Double));
        }

        public void Array(string inKey, ref System.Double[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.Double>(inKey, ref ioArray, inOptions,
                Read_Double_Cached ?? (Read_Double_Cached = Read_Double),
                Write_Double_Cached ?? (Write_Double_Cached = Write_Double));
        }

        public void Set(string inKey, ref HashSet<System.Double> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoSet<System.Double>(inKey, ref ioSet, inOptions,
                Read_Double_Cached ?? (Read_Double_Cached = Read_Double),
                Write_Double_Cached ?? (Write_Double_Cached = Write_Double));
        }

        public void Map(string inKey, ref Dictionary<string, System.Double> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.Double>(inKey, ref ioMap, inOptions,
                Read_Double_Cached ?? (Read_Double_Cached = Read_Double),
                Write_Double_Cached ?? (Write_Double_Cached = Write_Double));
        }

        public void Map(string inKey, ref Dictionary<int, System.Double> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.Double>(inKey, ref ioMap, inOptions,
                Read_Double_Cached ?? (Read_Double_Cached = Read_Double),
                Write_Double_Cached ?? (Write_Double_Cached = Write_Double));
        }
    }
}
