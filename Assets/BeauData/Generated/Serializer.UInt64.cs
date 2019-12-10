using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        #region Read/Write

        protected abstract bool Read_UInt64(ref System.UInt64 ioData);
        protected abstract void Write_UInt64(ref System.UInt64 ioData);

        private ReadFunc<System.UInt64> Read_UInt64_Cached;
        private WriteFunc<System.UInt64> Write_UInt64_Cached;

        #endregion // Read/Write

        #region Basic

        public void Serialize(string inKey, ref System.UInt64 ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.UInt64>(inKey, ref ioData, inOptions,
                Read_UInt64_Cached ?? (Read_UInt64_Cached = Read_UInt64),
                Write_UInt64_Cached ?? (Write_UInt64_Cached = Write_UInt64));
        }

        public void Serialize(string inKey, ref System.UInt64 ioData, System.UInt64 inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.UInt64>(inKey, ref ioData, inDefault, inOptions,
                Read_UInt64_Cached ?? (Read_UInt64_Cached = Read_UInt64),
                Write_UInt64_Cached ?? (Write_UInt64_Cached = Write_UInt64));
        }

        public void Array(string inKey, ref List<System.UInt64> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.UInt64>(inKey, ref ioArray, inOptions,
                Read_UInt64_Cached ?? (Read_UInt64_Cached = Read_UInt64),
                Write_UInt64_Cached ?? (Write_UInt64_Cached = Write_UInt64));
        }

        public void Array(string inKey, ref System.UInt64[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.UInt64>(inKey, ref ioArray, inOptions,
                Read_UInt64_Cached ?? (Read_UInt64_Cached = Read_UInt64),
                Write_UInt64_Cached ?? (Write_UInt64_Cached = Write_UInt64));
        }

        public void Set(string inKey, ref HashSet<System.UInt64> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoSet<System.UInt64>(inKey, ref ioSet, inOptions,
                Read_UInt64_Cached ?? (Read_UInt64_Cached = Read_UInt64),
                Write_UInt64_Cached ?? (Write_UInt64_Cached = Write_UInt64));
        }

        public void Map(string inKey, ref Dictionary<string, System.UInt64> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.UInt64>(inKey, ref ioMap, inOptions,
                Read_UInt64_Cached ?? (Read_UInt64_Cached = Read_UInt64),
                Write_UInt64_Cached ?? (Write_UInt64_Cached = Write_UInt64));
        }

        public void Map(string inKey, ref Dictionary<int, System.UInt64> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.UInt64>(inKey, ref ioMap, inOptions,
                Read_UInt64_Cached ?? (Read_UInt64_Cached = Read_UInt64),
                Write_UInt64_Cached ?? (Write_UInt64_Cached = Write_UInt64));
        }
    
        #endregion // Basic

        #region Proxy

        public void UInt64Proxy<ProxyType>(string inKey, ref ProxyType ioData, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.UInt64>
        {
            DoProxy<ProxyType, System.UInt64>(inKey, ref ioData, inOptions,
                Read_UInt64_Cached ?? (Read_UInt64_Cached = Read_UInt64),
                Write_UInt64_Cached ?? (Write_UInt64_Cached = Write_UInt64));
        }

        public void UInt64Proxy<ProxyType>(string inKey, ref ProxyType ioData, ProxyType inDefault, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.UInt64>
        {
            DoProxy<ProxyType, System.UInt64>(inKey, ref ioData, inDefault, inOptions,
                Read_UInt64_Cached ?? (Read_UInt64_Cached = Read_UInt64),
                Write_UInt64_Cached ?? (Write_UInt64_Cached = Write_UInt64));
        }

        public void UInt64ProxyArray<ProxyType>(string inKey, ref List<ProxyType> ioArray, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.UInt64>
        {
            DoProxyArray<ProxyType, System.UInt64>(inKey, ref ioArray, inOptions,
                Read_UInt64_Cached ?? (Read_UInt64_Cached = Read_UInt64),
                Write_UInt64_Cached ?? (Write_UInt64_Cached = Write_UInt64));
        }

        public void UInt64ProxyArray<ProxyType>(string inKey, ref ProxyType[] ioArray, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.UInt64>
        {
            DoProxyArray<ProxyType, System.UInt64>(inKey, ref ioArray, inOptions,
                Read_UInt64_Cached ?? (Read_UInt64_Cached = Read_UInt64),
                Write_UInt64_Cached ?? (Write_UInt64_Cached = Write_UInt64));
        }

        public void UInt64ProxySet<ProxyType>(string inKey, ref HashSet<ProxyType> ioSet, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.UInt64>
        {
            DoProxySet<ProxyType, System.UInt64>(inKey, ref ioSet, inOptions,
                Read_UInt64_Cached ?? (Read_UInt64_Cached = Read_UInt64),
                Write_UInt64_Cached ?? (Write_UInt64_Cached = Write_UInt64));
        }

        public void UInt64ProxyMap<ProxyType>(string inKey, ref Dictionary<string, ProxyType> ioMap, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.UInt64>
        {
            DoProxyMap<ProxyType, System.UInt64>(inKey, ref ioMap, inOptions,
                Read_UInt64_Cached ?? (Read_UInt64_Cached = Read_UInt64),
                Write_UInt64_Cached ?? (Write_UInt64_Cached = Write_UInt64));
        }

        public void UInt64ProxyMap<ProxyType>(string inKey, ref Dictionary<int, ProxyType> ioMap, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.UInt64>
        {
            DoProxyMap<ProxyType, System.UInt64>(inKey, ref ioMap, inOptions,
                Read_UInt64_Cached ?? (Read_UInt64_Cached = Read_UInt64),
                Write_UInt64_Cached ?? (Write_UInt64_Cached = Write_UInt64));
        }
    
        #endregion // Proxy
    }
}
