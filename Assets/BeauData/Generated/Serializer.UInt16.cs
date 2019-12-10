using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        #region Read/Write

        protected abstract bool Read_UInt16(ref System.UInt16 ioData);
        protected abstract void Write_UInt16(ref System.UInt16 ioData);

        private ReadFunc<System.UInt16> Read_UInt16_Cached;
        private WriteFunc<System.UInt16> Write_UInt16_Cached;

        #endregion // Read/Write

        #region Basic

        public void Serialize(string inKey, ref System.UInt16 ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.UInt16>(inKey, ref ioData, inOptions,
                Read_UInt16_Cached ?? (Read_UInt16_Cached = Read_UInt16),
                Write_UInt16_Cached ?? (Write_UInt16_Cached = Write_UInt16));
        }

        public void Serialize(string inKey, ref System.UInt16 ioData, System.UInt16 inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.UInt16>(inKey, ref ioData, inDefault, inOptions,
                Read_UInt16_Cached ?? (Read_UInt16_Cached = Read_UInt16),
                Write_UInt16_Cached ?? (Write_UInt16_Cached = Write_UInt16));
        }

        public void Array(string inKey, ref List<System.UInt16> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.UInt16>(inKey, ref ioArray, inOptions,
                Read_UInt16_Cached ?? (Read_UInt16_Cached = Read_UInt16),
                Write_UInt16_Cached ?? (Write_UInt16_Cached = Write_UInt16));
        }

        public void Array(string inKey, ref System.UInt16[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.UInt16>(inKey, ref ioArray, inOptions,
                Read_UInt16_Cached ?? (Read_UInt16_Cached = Read_UInt16),
                Write_UInt16_Cached ?? (Write_UInt16_Cached = Write_UInt16));
        }

        public void Set(string inKey, ref HashSet<System.UInt16> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoSet<System.UInt16>(inKey, ref ioSet, inOptions,
                Read_UInt16_Cached ?? (Read_UInt16_Cached = Read_UInt16),
                Write_UInt16_Cached ?? (Write_UInt16_Cached = Write_UInt16));
        }

        public void Map(string inKey, ref Dictionary<string, System.UInt16> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.UInt16>(inKey, ref ioMap, inOptions,
                Read_UInt16_Cached ?? (Read_UInt16_Cached = Read_UInt16),
                Write_UInt16_Cached ?? (Write_UInt16_Cached = Write_UInt16));
        }

        public void Map(string inKey, ref Dictionary<int, System.UInt16> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.UInt16>(inKey, ref ioMap, inOptions,
                Read_UInt16_Cached ?? (Read_UInt16_Cached = Read_UInt16),
                Write_UInt16_Cached ?? (Write_UInt16_Cached = Write_UInt16));
        }
    
        #endregion // Basic

        #region Proxy

        public void UInt16Proxy<ProxyType>(string inKey, ref ProxyType ioData, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.UInt16>
        {
            DoProxy<ProxyType, System.UInt16>(inKey, ref ioData, inOptions,
                Read_UInt16_Cached ?? (Read_UInt16_Cached = Read_UInt16),
                Write_UInt16_Cached ?? (Write_UInt16_Cached = Write_UInt16));
        }

        public void UInt16Proxy<ProxyType>(string inKey, ref ProxyType ioData, ProxyType inDefault, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.UInt16>
        {
            DoProxy<ProxyType, System.UInt16>(inKey, ref ioData, inDefault, inOptions,
                Read_UInt16_Cached ?? (Read_UInt16_Cached = Read_UInt16),
                Write_UInt16_Cached ?? (Write_UInt16_Cached = Write_UInt16));
        }

        public void UInt16ProxyArray<ProxyType>(string inKey, ref List<ProxyType> ioArray, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.UInt16>
        {
            DoProxyArray<ProxyType, System.UInt16>(inKey, ref ioArray, inOptions,
                Read_UInt16_Cached ?? (Read_UInt16_Cached = Read_UInt16),
                Write_UInt16_Cached ?? (Write_UInt16_Cached = Write_UInt16));
        }

        public void UInt16ProxyArray<ProxyType>(string inKey, ref ProxyType[] ioArray, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.UInt16>
        {
            DoProxyArray<ProxyType, System.UInt16>(inKey, ref ioArray, inOptions,
                Read_UInt16_Cached ?? (Read_UInt16_Cached = Read_UInt16),
                Write_UInt16_Cached ?? (Write_UInt16_Cached = Write_UInt16));
        }

        public void UInt16ProxySet<ProxyType>(string inKey, ref HashSet<ProxyType> ioSet, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.UInt16>
        {
            DoProxySet<ProxyType, System.UInt16>(inKey, ref ioSet, inOptions,
                Read_UInt16_Cached ?? (Read_UInt16_Cached = Read_UInt16),
                Write_UInt16_Cached ?? (Write_UInt16_Cached = Write_UInt16));
        }

        public void UInt16ProxyMap<ProxyType>(string inKey, ref Dictionary<string, ProxyType> ioMap, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.UInt16>
        {
            DoProxyMap<ProxyType, System.UInt16>(inKey, ref ioMap, inOptions,
                Read_UInt16_Cached ?? (Read_UInt16_Cached = Read_UInt16),
                Write_UInt16_Cached ?? (Write_UInt16_Cached = Write_UInt16));
        }

        public void UInt16ProxyMap<ProxyType>(string inKey, ref Dictionary<int, ProxyType> ioMap, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.UInt16>
        {
            DoProxyMap<ProxyType, System.UInt16>(inKey, ref ioMap, inOptions,
                Read_UInt16_Cached ?? (Read_UInt16_Cached = Read_UInt16),
                Write_UInt16_Cached ?? (Write_UInt16_Cached = Write_UInt16));
        }
    
        #endregion // Proxy
    }
}
