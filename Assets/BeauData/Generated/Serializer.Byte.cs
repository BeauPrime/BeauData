using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        #region Read/Write

        protected abstract bool Read_Byte(ref System.Byte ioData);
        protected abstract void Write_Byte(ref System.Byte ioData);

        private ReadFunc<System.Byte> Read_Byte_Cached;
        private WriteFunc<System.Byte> Write_Byte_Cached;

        #endregion // Read/Write

        #region Basic

        public void Serialize(string inKey, ref System.Byte ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.Byte>(inKey, ref ioData, inOptions,
                Read_Byte_Cached ?? (Read_Byte_Cached = Read_Byte),
                Write_Byte_Cached ?? (Write_Byte_Cached = Write_Byte));
        }

        public void Serialize(string inKey, ref System.Byte ioData, System.Byte inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.Byte>(inKey, ref ioData, inDefault, inOptions,
                Read_Byte_Cached ?? (Read_Byte_Cached = Read_Byte),
                Write_Byte_Cached ?? (Write_Byte_Cached = Write_Byte));
        }

        public void Array(string inKey, ref List<System.Byte> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.Byte>(inKey, ref ioArray, inOptions,
                Read_Byte_Cached ?? (Read_Byte_Cached = Read_Byte),
                Write_Byte_Cached ?? (Write_Byte_Cached = Write_Byte));
        }

        public void Array(string inKey, ref System.Byte[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.Byte>(inKey, ref ioArray, inOptions,
                Read_Byte_Cached ?? (Read_Byte_Cached = Read_Byte),
                Write_Byte_Cached ?? (Write_Byte_Cached = Write_Byte));
        }

        public void Set(string inKey, ref HashSet<System.Byte> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoSet<System.Byte>(inKey, ref ioSet, inOptions,
                Read_Byte_Cached ?? (Read_Byte_Cached = Read_Byte),
                Write_Byte_Cached ?? (Write_Byte_Cached = Write_Byte));
        }

        public void Map(string inKey, ref Dictionary<string, System.Byte> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.Byte>(inKey, ref ioMap, inOptions,
                Read_Byte_Cached ?? (Read_Byte_Cached = Read_Byte),
                Write_Byte_Cached ?? (Write_Byte_Cached = Write_Byte));
        }

        public void Map(string inKey, ref Dictionary<int, System.Byte> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.Byte>(inKey, ref ioMap, inOptions,
                Read_Byte_Cached ?? (Read_Byte_Cached = Read_Byte),
                Write_Byte_Cached ?? (Write_Byte_Cached = Write_Byte));
        }
    
        #endregion // Basic

        #region Proxy

        public void ByteProxy<ProxyType>(string inKey, ref ProxyType ioData, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Byte>
        {
            DoProxy<ProxyType, System.Byte>(inKey, ref ioData, inOptions,
                Read_Byte_Cached ?? (Read_Byte_Cached = Read_Byte),
                Write_Byte_Cached ?? (Write_Byte_Cached = Write_Byte));
        }

        public void ByteProxy<ProxyType>(string inKey, ref ProxyType ioData, ProxyType inDefault, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Byte>
        {
            DoProxy<ProxyType, System.Byte>(inKey, ref ioData, inDefault, inOptions,
                Read_Byte_Cached ?? (Read_Byte_Cached = Read_Byte),
                Write_Byte_Cached ?? (Write_Byte_Cached = Write_Byte));
        }

        public void ByteProxyArray<ProxyType>(string inKey, ref List<ProxyType> ioArray, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Byte>
        {
            DoProxyArray<ProxyType, System.Byte>(inKey, ref ioArray, inOptions,
                Read_Byte_Cached ?? (Read_Byte_Cached = Read_Byte),
                Write_Byte_Cached ?? (Write_Byte_Cached = Write_Byte));
        }

        public void ByteProxyArray<ProxyType>(string inKey, ref ProxyType[] ioArray, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Byte>
        {
            DoProxyArray<ProxyType, System.Byte>(inKey, ref ioArray, inOptions,
                Read_Byte_Cached ?? (Read_Byte_Cached = Read_Byte),
                Write_Byte_Cached ?? (Write_Byte_Cached = Write_Byte));
        }

        public void ByteProxySet<ProxyType>(string inKey, ref HashSet<ProxyType> ioSet, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Byte>
        {
            DoProxySet<ProxyType, System.Byte>(inKey, ref ioSet, inOptions,
                Read_Byte_Cached ?? (Read_Byte_Cached = Read_Byte),
                Write_Byte_Cached ?? (Write_Byte_Cached = Write_Byte));
        }

        public void ByteProxyMap<ProxyType>(string inKey, ref Dictionary<string, ProxyType> ioMap, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Byte>
        {
            DoProxyMap<ProxyType, System.Byte>(inKey, ref ioMap, inOptions,
                Read_Byte_Cached ?? (Read_Byte_Cached = Read_Byte),
                Write_Byte_Cached ?? (Write_Byte_Cached = Write_Byte));
        }

        public void ByteProxyMap<ProxyType>(string inKey, ref Dictionary<int, ProxyType> ioMap, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Byte>
        {
            DoProxyMap<ProxyType, System.Byte>(inKey, ref ioMap, inOptions,
                Read_Byte_Cached ?? (Read_Byte_Cached = Read_Byte),
                Write_Byte_Cached ?? (Write_Byte_Cached = Write_Byte));
        }
    
        #endregion // Proxy
    }
}
