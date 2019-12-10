using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        #region Read/Write

        protected abstract bool Read_Double(ref System.Double ioData);
        protected abstract void Write_Double(ref System.Double ioData);

        private ReadFunc<System.Double> Read_Double_Cached;
        private WriteFunc<System.Double> Write_Double_Cached;

        #endregion // Read/Write

        #region Basic

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
    
        #endregion // Basic

        #region Proxy

        public void DoubleProxy<ProxyType>(string inKey, ref ProxyType ioData, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Double>
        {
            DoProxy<ProxyType, System.Double>(inKey, ref ioData, inOptions,
                Read_Double_Cached ?? (Read_Double_Cached = Read_Double),
                Write_Double_Cached ?? (Write_Double_Cached = Write_Double));
        }

        public void DoubleProxy<ProxyType>(string inKey, ref ProxyType ioData, ProxyType inDefault, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Double>
        {
            DoProxy<ProxyType, System.Double>(inKey, ref ioData, inDefault, inOptions,
                Read_Double_Cached ?? (Read_Double_Cached = Read_Double),
                Write_Double_Cached ?? (Write_Double_Cached = Write_Double));
        }

        public void DoubleProxyArray<ProxyType>(string inKey, ref List<ProxyType> ioArray, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Double>
        {
            DoProxyArray<ProxyType, System.Double>(inKey, ref ioArray, inOptions,
                Read_Double_Cached ?? (Read_Double_Cached = Read_Double),
                Write_Double_Cached ?? (Write_Double_Cached = Write_Double));
        }

        public void DoubleProxyArray<ProxyType>(string inKey, ref ProxyType[] ioArray, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Double>
        {
            DoProxyArray<ProxyType, System.Double>(inKey, ref ioArray, inOptions,
                Read_Double_Cached ?? (Read_Double_Cached = Read_Double),
                Write_Double_Cached ?? (Write_Double_Cached = Write_Double));
        }

        public void DoubleProxySet<ProxyType>(string inKey, ref HashSet<ProxyType> ioSet, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Double>
        {
            DoProxySet<ProxyType, System.Double>(inKey, ref ioSet, inOptions,
                Read_Double_Cached ?? (Read_Double_Cached = Read_Double),
                Write_Double_Cached ?? (Write_Double_Cached = Write_Double));
        }

        public void DoubleProxyMap<ProxyType>(string inKey, ref Dictionary<string, ProxyType> ioMap, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Double>
        {
            DoProxyMap<ProxyType, System.Double>(inKey, ref ioMap, inOptions,
                Read_Double_Cached ?? (Read_Double_Cached = Read_Double),
                Write_Double_Cached ?? (Write_Double_Cached = Write_Double));
        }

        public void DoubleProxyMap<ProxyType>(string inKey, ref Dictionary<int, ProxyType> ioMap, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Double>
        {
            DoProxyMap<ProxyType, System.Double>(inKey, ref ioMap, inOptions,
                Read_Double_Cached ?? (Read_Double_Cached = Read_Double),
                Write_Double_Cached ?? (Write_Double_Cached = Write_Double));
        }
    
        #endregion // Proxy
    }
}
