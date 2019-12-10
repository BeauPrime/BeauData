using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        #region Read/Write

        protected abstract bool Read_Int16(ref System.Int16 ioData);
        protected abstract void Write_Int16(ref System.Int16 ioData);

        private ReadFunc<System.Int16> Read_Int16_Cached;
        private WriteFunc<System.Int16> Write_Int16_Cached;

        #endregion // Read/Write

        #region Basic

        public void Serialize(string inKey, ref System.Int16 ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.Int16>(inKey, ref ioData, inOptions,
                Read_Int16_Cached ?? (Read_Int16_Cached = Read_Int16),
                Write_Int16_Cached ?? (Write_Int16_Cached = Write_Int16));
        }

        public void Serialize(string inKey, ref System.Int16 ioData, System.Int16 inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.Int16>(inKey, ref ioData, inDefault, inOptions,
                Read_Int16_Cached ?? (Read_Int16_Cached = Read_Int16),
                Write_Int16_Cached ?? (Write_Int16_Cached = Write_Int16));
        }

        public void Array(string inKey, ref List<System.Int16> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.Int16>(inKey, ref ioArray, inOptions,
                Read_Int16_Cached ?? (Read_Int16_Cached = Read_Int16),
                Write_Int16_Cached ?? (Write_Int16_Cached = Write_Int16));
        }

        public void Array(string inKey, ref System.Int16[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.Int16>(inKey, ref ioArray, inOptions,
                Read_Int16_Cached ?? (Read_Int16_Cached = Read_Int16),
                Write_Int16_Cached ?? (Write_Int16_Cached = Write_Int16));
        }

        public void Set(string inKey, ref HashSet<System.Int16> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoSet<System.Int16>(inKey, ref ioSet, inOptions,
                Read_Int16_Cached ?? (Read_Int16_Cached = Read_Int16),
                Write_Int16_Cached ?? (Write_Int16_Cached = Write_Int16));
        }

        public void Map(string inKey, ref Dictionary<string, System.Int16> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.Int16>(inKey, ref ioMap, inOptions,
                Read_Int16_Cached ?? (Read_Int16_Cached = Read_Int16),
                Write_Int16_Cached ?? (Write_Int16_Cached = Write_Int16));
        }

        public void Map(string inKey, ref Dictionary<int, System.Int16> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.Int16>(inKey, ref ioMap, inOptions,
                Read_Int16_Cached ?? (Read_Int16_Cached = Read_Int16),
                Write_Int16_Cached ?? (Write_Int16_Cached = Write_Int16));
        }
    
        #endregion // Basic

        #region Proxy

        public void Int16Proxy<ProxyType>(string inKey, ref ProxyType ioData, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Int16>
        {
            DoProxy<ProxyType, System.Int16>(inKey, ref ioData, inOptions,
                Read_Int16_Cached ?? (Read_Int16_Cached = Read_Int16),
                Write_Int16_Cached ?? (Write_Int16_Cached = Write_Int16));
        }

        public void Int16Proxy<ProxyType>(string inKey, ref ProxyType ioData, ProxyType inDefault, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Int16>
        {
            DoProxy<ProxyType, System.Int16>(inKey, ref ioData, inDefault, inOptions,
                Read_Int16_Cached ?? (Read_Int16_Cached = Read_Int16),
                Write_Int16_Cached ?? (Write_Int16_Cached = Write_Int16));
        }

        public void Int16ProxyArray<ProxyType>(string inKey, ref List<ProxyType> ioArray, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Int16>
        {
            DoProxyArray<ProxyType, System.Int16>(inKey, ref ioArray, inOptions,
                Read_Int16_Cached ?? (Read_Int16_Cached = Read_Int16),
                Write_Int16_Cached ?? (Write_Int16_Cached = Write_Int16));
        }

        public void Int16ProxyArray<ProxyType>(string inKey, ref ProxyType[] ioArray, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Int16>
        {
            DoProxyArray<ProxyType, System.Int16>(inKey, ref ioArray, inOptions,
                Read_Int16_Cached ?? (Read_Int16_Cached = Read_Int16),
                Write_Int16_Cached ?? (Write_Int16_Cached = Write_Int16));
        }

        public void Int16ProxySet<ProxyType>(string inKey, ref HashSet<ProxyType> ioSet, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Int16>
        {
            DoProxySet<ProxyType, System.Int16>(inKey, ref ioSet, inOptions,
                Read_Int16_Cached ?? (Read_Int16_Cached = Read_Int16),
                Write_Int16_Cached ?? (Write_Int16_Cached = Write_Int16));
        }

        public void Int16ProxyMap<ProxyType>(string inKey, ref Dictionary<string, ProxyType> ioMap, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Int16>
        {
            DoProxyMap<ProxyType, System.Int16>(inKey, ref ioMap, inOptions,
                Read_Int16_Cached ?? (Read_Int16_Cached = Read_Int16),
                Write_Int16_Cached ?? (Write_Int16_Cached = Write_Int16));
        }

        public void Int16ProxyMap<ProxyType>(string inKey, ref Dictionary<int, ProxyType> ioMap, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Int16>
        {
            DoProxyMap<ProxyType, System.Int16>(inKey, ref ioMap, inOptions,
                Read_Int16_Cached ?? (Read_Int16_Cached = Read_Int16),
                Write_Int16_Cached ?? (Write_Int16_Cached = Write_Int16));
        }
    
        #endregion // Proxy
    }
}
