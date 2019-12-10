using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        #region Read/Write

        protected abstract bool Read_UInt32(ref System.UInt32 ioData);
        protected abstract void Write_UInt32(ref System.UInt32 ioData);

        private ReadFunc<System.UInt32> Read_UInt32_Cached;
        private WriteFunc<System.UInt32> Write_UInt32_Cached;

        #endregion // Read/Write

        #region Basic

        public void Serialize(string inKey, ref System.UInt32 ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.UInt32>(inKey, ref ioData, inOptions,
                Read_UInt32_Cached ?? (Read_UInt32_Cached = Read_UInt32),
                Write_UInt32_Cached ?? (Write_UInt32_Cached = Write_UInt32));
        }

        public void Serialize(string inKey, ref System.UInt32 ioData, System.UInt32 inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.UInt32>(inKey, ref ioData, inDefault, inOptions,
                Read_UInt32_Cached ?? (Read_UInt32_Cached = Read_UInt32),
                Write_UInt32_Cached ?? (Write_UInt32_Cached = Write_UInt32));
        }

        public void Array(string inKey, ref List<System.UInt32> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.UInt32>(inKey, ref ioArray, inOptions,
                Read_UInt32_Cached ?? (Read_UInt32_Cached = Read_UInt32),
                Write_UInt32_Cached ?? (Write_UInt32_Cached = Write_UInt32));
        }

        public void Array(string inKey, ref System.UInt32[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.UInt32>(inKey, ref ioArray, inOptions,
                Read_UInt32_Cached ?? (Read_UInt32_Cached = Read_UInt32),
                Write_UInt32_Cached ?? (Write_UInt32_Cached = Write_UInt32));
        }

        public void Set(string inKey, ref HashSet<System.UInt32> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoSet<System.UInt32>(inKey, ref ioSet, inOptions,
                Read_UInt32_Cached ?? (Read_UInt32_Cached = Read_UInt32),
                Write_UInt32_Cached ?? (Write_UInt32_Cached = Write_UInt32));
        }

        public void Map(string inKey, ref Dictionary<string, System.UInt32> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.UInt32>(inKey, ref ioMap, inOptions,
                Read_UInt32_Cached ?? (Read_UInt32_Cached = Read_UInt32),
                Write_UInt32_Cached ?? (Write_UInt32_Cached = Write_UInt32));
        }

        public void Map(string inKey, ref Dictionary<int, System.UInt32> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.UInt32>(inKey, ref ioMap, inOptions,
                Read_UInt32_Cached ?? (Read_UInt32_Cached = Read_UInt32),
                Write_UInt32_Cached ?? (Write_UInt32_Cached = Write_UInt32));
        }
    
        #endregion // Basic

        #region Proxy

        public void UInt32Proxy<ProxyType>(string inKey, ref ProxyType ioData, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.UInt32>
        {
            DoProxy<ProxyType, System.UInt32>(inKey, ref ioData, inOptions,
                Read_UInt32_Cached ?? (Read_UInt32_Cached = Read_UInt32),
                Write_UInt32_Cached ?? (Write_UInt32_Cached = Write_UInt32));
        }

        public void UInt32Proxy<ProxyType>(string inKey, ref ProxyType ioData, ProxyType inDefault, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.UInt32>
        {
            DoProxy<ProxyType, System.UInt32>(inKey, ref ioData, inDefault, inOptions,
                Read_UInt32_Cached ?? (Read_UInt32_Cached = Read_UInt32),
                Write_UInt32_Cached ?? (Write_UInt32_Cached = Write_UInt32));
        }

        public void UInt32ProxyArray<ProxyType>(string inKey, ref List<ProxyType> ioArray, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.UInt32>
        {
            DoProxyArray<ProxyType, System.UInt32>(inKey, ref ioArray, inOptions,
                Read_UInt32_Cached ?? (Read_UInt32_Cached = Read_UInt32),
                Write_UInt32_Cached ?? (Write_UInt32_Cached = Write_UInt32));
        }

        public void UInt32ProxyArray<ProxyType>(string inKey, ref ProxyType[] ioArray, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.UInt32>
        {
            DoProxyArray<ProxyType, System.UInt32>(inKey, ref ioArray, inOptions,
                Read_UInt32_Cached ?? (Read_UInt32_Cached = Read_UInt32),
                Write_UInt32_Cached ?? (Write_UInt32_Cached = Write_UInt32));
        }

        public void UInt32ProxySet<ProxyType>(string inKey, ref HashSet<ProxyType> ioSet, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.UInt32>
        {
            DoProxySet<ProxyType, System.UInt32>(inKey, ref ioSet, inOptions,
                Read_UInt32_Cached ?? (Read_UInt32_Cached = Read_UInt32),
                Write_UInt32_Cached ?? (Write_UInt32_Cached = Write_UInt32));
        }

        public void UInt32ProxyMap<ProxyType>(string inKey, ref Dictionary<string, ProxyType> ioMap, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.UInt32>
        {
            DoProxyMap<ProxyType, System.UInt32>(inKey, ref ioMap, inOptions,
                Read_UInt32_Cached ?? (Read_UInt32_Cached = Read_UInt32),
                Write_UInt32_Cached ?? (Write_UInt32_Cached = Write_UInt32));
        }

        public void UInt32ProxyMap<ProxyType>(string inKey, ref Dictionary<int, ProxyType> ioMap, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.UInt32>
        {
            DoProxyMap<ProxyType, System.UInt32>(inKey, ref ioMap, inOptions,
                Read_UInt32_Cached ?? (Read_UInt32_Cached = Read_UInt32),
                Write_UInt32_Cached ?? (Write_UInt32_Cached = Write_UInt32));
        }
    
        #endregion // Proxy
    }
}
