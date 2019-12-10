using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        #region Read/Write

        protected abstract bool Read_Int32(ref System.Int32 ioData);
        protected abstract void Write_Int32(ref System.Int32 ioData);

        private ReadFunc<System.Int32> Read_Int32_Cached;
        private WriteFunc<System.Int32> Write_Int32_Cached;

        #endregion // Read/Write

        #region Basic

        public void Serialize(string inKey, ref System.Int32 ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.Int32>(inKey, ref ioData, inOptions,
                Read_Int32_Cached ?? (Read_Int32_Cached = Read_Int32),
                Write_Int32_Cached ?? (Write_Int32_Cached = Write_Int32));
        }

        public void Serialize(string inKey, ref System.Int32 ioData, System.Int32 inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.Int32>(inKey, ref ioData, inDefault, inOptions,
                Read_Int32_Cached ?? (Read_Int32_Cached = Read_Int32),
                Write_Int32_Cached ?? (Write_Int32_Cached = Write_Int32));
        }

        public void Array(string inKey, ref List<System.Int32> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.Int32>(inKey, ref ioArray, inOptions,
                Read_Int32_Cached ?? (Read_Int32_Cached = Read_Int32),
                Write_Int32_Cached ?? (Write_Int32_Cached = Write_Int32));
        }

        public void Array(string inKey, ref System.Int32[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.Int32>(inKey, ref ioArray, inOptions,
                Read_Int32_Cached ?? (Read_Int32_Cached = Read_Int32),
                Write_Int32_Cached ?? (Write_Int32_Cached = Write_Int32));
        }

        public void Set(string inKey, ref HashSet<System.Int32> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoSet<System.Int32>(inKey, ref ioSet, inOptions,
                Read_Int32_Cached ?? (Read_Int32_Cached = Read_Int32),
                Write_Int32_Cached ?? (Write_Int32_Cached = Write_Int32));
        }

        public void Map(string inKey, ref Dictionary<string, System.Int32> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.Int32>(inKey, ref ioMap, inOptions,
                Read_Int32_Cached ?? (Read_Int32_Cached = Read_Int32),
                Write_Int32_Cached ?? (Write_Int32_Cached = Write_Int32));
        }

        public void Map(string inKey, ref Dictionary<int, System.Int32> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.Int32>(inKey, ref ioMap, inOptions,
                Read_Int32_Cached ?? (Read_Int32_Cached = Read_Int32),
                Write_Int32_Cached ?? (Write_Int32_Cached = Write_Int32));
        }
    
        #endregion // Basic

        #region Proxy

        public void Int32Proxy<ProxyType>(string inKey, ref ProxyType ioData, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Int32>
        {
            DoProxy<ProxyType, System.Int32>(inKey, ref ioData, inOptions,
                Read_Int32_Cached ?? (Read_Int32_Cached = Read_Int32),
                Write_Int32_Cached ?? (Write_Int32_Cached = Write_Int32));
        }

        public void Int32Proxy<ProxyType>(string inKey, ref ProxyType ioData, ProxyType inDefault, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Int32>
        {
            DoProxy<ProxyType, System.Int32>(inKey, ref ioData, inDefault, inOptions,
                Read_Int32_Cached ?? (Read_Int32_Cached = Read_Int32),
                Write_Int32_Cached ?? (Write_Int32_Cached = Write_Int32));
        }

        public void Int32ProxyArray<ProxyType>(string inKey, ref List<ProxyType> ioArray, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Int32>
        {
            DoProxyArray<ProxyType, System.Int32>(inKey, ref ioArray, inOptions,
                Read_Int32_Cached ?? (Read_Int32_Cached = Read_Int32),
                Write_Int32_Cached ?? (Write_Int32_Cached = Write_Int32));
        }

        public void Int32ProxyArray<ProxyType>(string inKey, ref ProxyType[] ioArray, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Int32>
        {
            DoProxyArray<ProxyType, System.Int32>(inKey, ref ioArray, inOptions,
                Read_Int32_Cached ?? (Read_Int32_Cached = Read_Int32),
                Write_Int32_Cached ?? (Write_Int32_Cached = Write_Int32));
        }

        public void Int32ProxySet<ProxyType>(string inKey, ref HashSet<ProxyType> ioSet, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Int32>
        {
            DoProxySet<ProxyType, System.Int32>(inKey, ref ioSet, inOptions,
                Read_Int32_Cached ?? (Read_Int32_Cached = Read_Int32),
                Write_Int32_Cached ?? (Write_Int32_Cached = Write_Int32));
        }

        public void Int32ProxyMap<ProxyType>(string inKey, ref Dictionary<string, ProxyType> ioMap, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Int32>
        {
            DoProxyMap<ProxyType, System.Int32>(inKey, ref ioMap, inOptions,
                Read_Int32_Cached ?? (Read_Int32_Cached = Read_Int32),
                Write_Int32_Cached ?? (Write_Int32_Cached = Write_Int32));
        }

        public void Int32ProxyMap<ProxyType>(string inKey, ref Dictionary<int, ProxyType> ioMap, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Int32>
        {
            DoProxyMap<ProxyType, System.Int32>(inKey, ref ioMap, inOptions,
                Read_Int32_Cached ?? (Read_Int32_Cached = Read_Int32),
                Write_Int32_Cached ?? (Write_Int32_Cached = Write_Int32));
        }
    
        #endregion // Proxy
    }
}
