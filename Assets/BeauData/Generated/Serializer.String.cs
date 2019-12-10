using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        #region Read/Write

        protected abstract bool Read_String(ref System.String ioData);
        protected abstract void Write_String(ref System.String ioData);

        private ReadFunc<System.String> Read_String_Cached;
        private WriteFunc<System.String> Write_String_Cached;

        #endregion // Read/Write

        #region Basic

        public void Serialize(string inKey, ref System.String ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.String>(inKey, ref ioData, inOptions,
                Read_String_Cached ?? (Read_String_Cached = Read_String),
                Write_String_Cached ?? (Write_String_Cached = Write_String));
        }

        public void Serialize(string inKey, ref System.String ioData, System.String inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.String>(inKey, ref ioData, inDefault, inOptions,
                Read_String_Cached ?? (Read_String_Cached = Read_String),
                Write_String_Cached ?? (Write_String_Cached = Write_String));
        }

        public void Array(string inKey, ref List<System.String> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.String>(inKey, ref ioArray, inOptions,
                Read_String_Cached ?? (Read_String_Cached = Read_String),
                Write_String_Cached ?? (Write_String_Cached = Write_String));
        }

        public void Array(string inKey, ref System.String[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.String>(inKey, ref ioArray, inOptions,
                Read_String_Cached ?? (Read_String_Cached = Read_String),
                Write_String_Cached ?? (Write_String_Cached = Write_String));
        }

        public void Set(string inKey, ref HashSet<System.String> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoSet<System.String>(inKey, ref ioSet, inOptions,
                Read_String_Cached ?? (Read_String_Cached = Read_String),
                Write_String_Cached ?? (Write_String_Cached = Write_String));
        }

        public void Map(string inKey, ref Dictionary<string, System.String> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.String>(inKey, ref ioMap, inOptions,
                Read_String_Cached ?? (Read_String_Cached = Read_String),
                Write_String_Cached ?? (Write_String_Cached = Write_String));
        }

        public void Map(string inKey, ref Dictionary<int, System.String> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.String>(inKey, ref ioMap, inOptions,
                Read_String_Cached ?? (Read_String_Cached = Read_String),
                Write_String_Cached ?? (Write_String_Cached = Write_String));
        }
    
        #endregion // Basic

        #region Proxy

        public void StringProxy<ProxyType>(string inKey, ref ProxyType ioData, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.String>
        {
            DoProxy<ProxyType, System.String>(inKey, ref ioData, inOptions,
                Read_String_Cached ?? (Read_String_Cached = Read_String),
                Write_String_Cached ?? (Write_String_Cached = Write_String));
        }

        public void StringProxy<ProxyType>(string inKey, ref ProxyType ioData, ProxyType inDefault, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.String>
        {
            DoProxy<ProxyType, System.String>(inKey, ref ioData, inDefault, inOptions,
                Read_String_Cached ?? (Read_String_Cached = Read_String),
                Write_String_Cached ?? (Write_String_Cached = Write_String));
        }

        public void StringProxyArray<ProxyType>(string inKey, ref List<ProxyType> ioArray, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.String>
        {
            DoProxyArray<ProxyType, System.String>(inKey, ref ioArray, inOptions,
                Read_String_Cached ?? (Read_String_Cached = Read_String),
                Write_String_Cached ?? (Write_String_Cached = Write_String));
        }

        public void StringProxyArray<ProxyType>(string inKey, ref ProxyType[] ioArray, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.String>
        {
            DoProxyArray<ProxyType, System.String>(inKey, ref ioArray, inOptions,
                Read_String_Cached ?? (Read_String_Cached = Read_String),
                Write_String_Cached ?? (Write_String_Cached = Write_String));
        }

        public void StringProxySet<ProxyType>(string inKey, ref HashSet<ProxyType> ioSet, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.String>
        {
            DoProxySet<ProxyType, System.String>(inKey, ref ioSet, inOptions,
                Read_String_Cached ?? (Read_String_Cached = Read_String),
                Write_String_Cached ?? (Write_String_Cached = Write_String));
        }

        public void StringProxyMap<ProxyType>(string inKey, ref Dictionary<string, ProxyType> ioMap, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.String>
        {
            DoProxyMap<ProxyType, System.String>(inKey, ref ioMap, inOptions,
                Read_String_Cached ?? (Read_String_Cached = Read_String),
                Write_String_Cached ?? (Write_String_Cached = Write_String));
        }

        public void StringProxyMap<ProxyType>(string inKey, ref Dictionary<int, ProxyType> ioMap, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.String>
        {
            DoProxyMap<ProxyType, System.String>(inKey, ref ioMap, inOptions,
                Read_String_Cached ?? (Read_String_Cached = Read_String),
                Write_String_Cached ?? (Write_String_Cached = Write_String));
        }
    
        #endregion // Proxy
    }
}
