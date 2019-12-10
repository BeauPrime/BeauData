using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        #region Read/Write

        protected abstract bool Read_Int64(ref System.Int64 ioData);
        protected abstract void Write_Int64(ref System.Int64 ioData);

        private ReadFunc<System.Int64> Read_Int64_Cached;
        private WriteFunc<System.Int64> Write_Int64_Cached;

        #endregion // Read/Write

        #region Basic

        public void Serialize(string inKey, ref System.Int64 ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.Int64>(inKey, ref ioData, inOptions,
                Read_Int64_Cached ?? (Read_Int64_Cached = Read_Int64),
                Write_Int64_Cached ?? (Write_Int64_Cached = Write_Int64));
        }

        public void Serialize(string inKey, ref System.Int64 ioData, System.Int64 inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.Int64>(inKey, ref ioData, inDefault, inOptions,
                Read_Int64_Cached ?? (Read_Int64_Cached = Read_Int64),
                Write_Int64_Cached ?? (Write_Int64_Cached = Write_Int64));
        }

        public void Array(string inKey, ref List<System.Int64> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.Int64>(inKey, ref ioArray, inOptions,
                Read_Int64_Cached ?? (Read_Int64_Cached = Read_Int64),
                Write_Int64_Cached ?? (Write_Int64_Cached = Write_Int64));
        }

        public void Array(string inKey, ref System.Int64[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.Int64>(inKey, ref ioArray, inOptions,
                Read_Int64_Cached ?? (Read_Int64_Cached = Read_Int64),
                Write_Int64_Cached ?? (Write_Int64_Cached = Write_Int64));
        }

        public void Set(string inKey, ref HashSet<System.Int64> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoSet<System.Int64>(inKey, ref ioSet, inOptions,
                Read_Int64_Cached ?? (Read_Int64_Cached = Read_Int64),
                Write_Int64_Cached ?? (Write_Int64_Cached = Write_Int64));
        }

        public void Map(string inKey, ref Dictionary<string, System.Int64> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.Int64>(inKey, ref ioMap, inOptions,
                Read_Int64_Cached ?? (Read_Int64_Cached = Read_Int64),
                Write_Int64_Cached ?? (Write_Int64_Cached = Write_Int64));
        }

        public void Map(string inKey, ref Dictionary<int, System.Int64> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.Int64>(inKey, ref ioMap, inOptions,
                Read_Int64_Cached ?? (Read_Int64_Cached = Read_Int64),
                Write_Int64_Cached ?? (Write_Int64_Cached = Write_Int64));
        }
    
        #endregion // Basic

        #region Proxy

        public void Int64Proxy<ProxyType>(string inKey, ref ProxyType ioData, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Int64>
        {
            DoProxy<ProxyType, System.Int64>(inKey, ref ioData, inOptions,
                Read_Int64_Cached ?? (Read_Int64_Cached = Read_Int64),
                Write_Int64_Cached ?? (Write_Int64_Cached = Write_Int64));
        }

        public void Int64Proxy<ProxyType>(string inKey, ref ProxyType ioData, ProxyType inDefault, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Int64>
        {
            DoProxy<ProxyType, System.Int64>(inKey, ref ioData, inDefault, inOptions,
                Read_Int64_Cached ?? (Read_Int64_Cached = Read_Int64),
                Write_Int64_Cached ?? (Write_Int64_Cached = Write_Int64));
        }

        public void Int64ProxyArray<ProxyType>(string inKey, ref List<ProxyType> ioArray, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Int64>
        {
            DoProxyArray<ProxyType, System.Int64>(inKey, ref ioArray, inOptions,
                Read_Int64_Cached ?? (Read_Int64_Cached = Read_Int64),
                Write_Int64_Cached ?? (Write_Int64_Cached = Write_Int64));
        }

        public void Int64ProxyArray<ProxyType>(string inKey, ref ProxyType[] ioArray, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Int64>
        {
            DoProxyArray<ProxyType, System.Int64>(inKey, ref ioArray, inOptions,
                Read_Int64_Cached ?? (Read_Int64_Cached = Read_Int64),
                Write_Int64_Cached ?? (Write_Int64_Cached = Write_Int64));
        }

        public void Int64ProxySet<ProxyType>(string inKey, ref HashSet<ProxyType> ioSet, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Int64>
        {
            DoProxySet<ProxyType, System.Int64>(inKey, ref ioSet, inOptions,
                Read_Int64_Cached ?? (Read_Int64_Cached = Read_Int64),
                Write_Int64_Cached ?? (Write_Int64_Cached = Write_Int64));
        }

        public void Int64ProxyMap<ProxyType>(string inKey, ref Dictionary<string, ProxyType> ioMap, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Int64>
        {
            DoProxyMap<ProxyType, System.Int64>(inKey, ref ioMap, inOptions,
                Read_Int64_Cached ?? (Read_Int64_Cached = Read_Int64),
                Write_Int64_Cached ?? (Write_Int64_Cached = Write_Int64));
        }

        public void Int64ProxyMap<ProxyType>(string inKey, ref Dictionary<int, ProxyType> ioMap, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Int64>
        {
            DoProxyMap<ProxyType, System.Int64>(inKey, ref ioMap, inOptions,
                Read_Int64_Cached ?? (Read_Int64_Cached = Read_Int64),
                Write_Int64_Cached ?? (Write_Int64_Cached = Write_Int64));
        }
    
        #endregion // Proxy
    }
}
