using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        #region Read/Write

        protected abstract bool Read_Single(ref System.Single ioData);
        protected abstract void Write_Single(ref System.Single ioData);

        private ReadFunc<System.Single> Read_Single_Cached;
        private WriteFunc<System.Single> Write_Single_Cached;

        #endregion // Read/Write

        #region Basic

        public void Serialize(string inKey, ref System.Single ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.Single>(inKey, ref ioData, inOptions,
                Read_Single_Cached ?? (Read_Single_Cached = Read_Single),
                Write_Single_Cached ?? (Write_Single_Cached = Write_Single));
        }

        public void Serialize(string inKey, ref System.Single ioData, System.Single inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.Single>(inKey, ref ioData, inDefault, inOptions,
                Read_Single_Cached ?? (Read_Single_Cached = Read_Single),
                Write_Single_Cached ?? (Write_Single_Cached = Write_Single));
        }

        public void Array(string inKey, ref List<System.Single> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.Single>(inKey, ref ioArray, inOptions,
                Read_Single_Cached ?? (Read_Single_Cached = Read_Single),
                Write_Single_Cached ?? (Write_Single_Cached = Write_Single));
        }

        public void Array(string inKey, ref System.Single[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.Single>(inKey, ref ioArray, inOptions,
                Read_Single_Cached ?? (Read_Single_Cached = Read_Single),
                Write_Single_Cached ?? (Write_Single_Cached = Write_Single));
        }

        public void Set(string inKey, ref HashSet<System.Single> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoSet<System.Single>(inKey, ref ioSet, inOptions,
                Read_Single_Cached ?? (Read_Single_Cached = Read_Single),
                Write_Single_Cached ?? (Write_Single_Cached = Write_Single));
        }

        public void Map(string inKey, ref Dictionary<string, System.Single> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.Single>(inKey, ref ioMap, inOptions,
                Read_Single_Cached ?? (Read_Single_Cached = Read_Single),
                Write_Single_Cached ?? (Write_Single_Cached = Write_Single));
        }

        public void Map(string inKey, ref Dictionary<int, System.Single> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.Single>(inKey, ref ioMap, inOptions,
                Read_Single_Cached ?? (Read_Single_Cached = Read_Single),
                Write_Single_Cached ?? (Write_Single_Cached = Write_Single));
        }
    
        #endregion // Basic

        #region Proxy

        public void SingleProxy<ProxyType>(string inKey, ref ProxyType ioData, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Single>
        {
            DoProxy<ProxyType, System.Single>(inKey, ref ioData, inOptions,
                Read_Single_Cached ?? (Read_Single_Cached = Read_Single),
                Write_Single_Cached ?? (Write_Single_Cached = Write_Single));
        }

        public void SingleProxy<ProxyType>(string inKey, ref ProxyType ioData, ProxyType inDefault, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Single>
        {
            DoProxy<ProxyType, System.Single>(inKey, ref ioData, inDefault, inOptions,
                Read_Single_Cached ?? (Read_Single_Cached = Read_Single),
                Write_Single_Cached ?? (Write_Single_Cached = Write_Single));
        }

        public void SingleProxyArray<ProxyType>(string inKey, ref List<ProxyType> ioArray, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Single>
        {
            DoProxyArray<ProxyType, System.Single>(inKey, ref ioArray, inOptions,
                Read_Single_Cached ?? (Read_Single_Cached = Read_Single),
                Write_Single_Cached ?? (Write_Single_Cached = Write_Single));
        }

        public void SingleProxyArray<ProxyType>(string inKey, ref ProxyType[] ioArray, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Single>
        {
            DoProxyArray<ProxyType, System.Single>(inKey, ref ioArray, inOptions,
                Read_Single_Cached ?? (Read_Single_Cached = Read_Single),
                Write_Single_Cached ?? (Write_Single_Cached = Write_Single));
        }

        public void SingleProxySet<ProxyType>(string inKey, ref HashSet<ProxyType> ioSet, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Single>
        {
            DoProxySet<ProxyType, System.Single>(inKey, ref ioSet, inOptions,
                Read_Single_Cached ?? (Read_Single_Cached = Read_Single),
                Write_Single_Cached ?? (Write_Single_Cached = Write_Single));
        }

        public void SingleProxyMap<ProxyType>(string inKey, ref Dictionary<string, ProxyType> ioMap, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Single>
        {
            DoProxyMap<ProxyType, System.Single>(inKey, ref ioMap, inOptions,
                Read_Single_Cached ?? (Read_Single_Cached = Read_Single),
                Write_Single_Cached ?? (Write_Single_Cached = Write_Single));
        }

        public void SingleProxyMap<ProxyType>(string inKey, ref Dictionary<int, ProxyType> ioMap, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Single>
        {
            DoProxyMap<ProxyType, System.Single>(inKey, ref ioMap, inOptions,
                Read_Single_Cached ?? (Read_Single_Cached = Read_Single),
                Write_Single_Cached ?? (Write_Single_Cached = Write_Single));
        }
    
        #endregion // Proxy
    }
}
