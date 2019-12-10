using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        #region Read/Write

        protected abstract bool Read_Boolean(ref System.Boolean ioData);
        protected abstract void Write_Boolean(ref System.Boolean ioData);

        private ReadFunc<System.Boolean> Read_Boolean_Cached;
        private WriteFunc<System.Boolean> Write_Boolean_Cached;

        #endregion // Read/Write

        #region Basic

        public void Serialize(string inKey, ref System.Boolean ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.Boolean>(inKey, ref ioData, inOptions,
                Read_Boolean_Cached ?? (Read_Boolean_Cached = Read_Boolean),
                Write_Boolean_Cached ?? (Write_Boolean_Cached = Write_Boolean));
        }

        public void Serialize(string inKey, ref System.Boolean ioData, System.Boolean inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.Boolean>(inKey, ref ioData, inDefault, inOptions,
                Read_Boolean_Cached ?? (Read_Boolean_Cached = Read_Boolean),
                Write_Boolean_Cached ?? (Write_Boolean_Cached = Write_Boolean));
        }

        public void Array(string inKey, ref List<System.Boolean> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.Boolean>(inKey, ref ioArray, inOptions,
                Read_Boolean_Cached ?? (Read_Boolean_Cached = Read_Boolean),
                Write_Boolean_Cached ?? (Write_Boolean_Cached = Write_Boolean));
        }

        public void Array(string inKey, ref System.Boolean[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.Boolean>(inKey, ref ioArray, inOptions,
                Read_Boolean_Cached ?? (Read_Boolean_Cached = Read_Boolean),
                Write_Boolean_Cached ?? (Write_Boolean_Cached = Write_Boolean));
        }

        public void Set(string inKey, ref HashSet<System.Boolean> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoSet<System.Boolean>(inKey, ref ioSet, inOptions,
                Read_Boolean_Cached ?? (Read_Boolean_Cached = Read_Boolean),
                Write_Boolean_Cached ?? (Write_Boolean_Cached = Write_Boolean));
        }

        public void Map(string inKey, ref Dictionary<string, System.Boolean> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.Boolean>(inKey, ref ioMap, inOptions,
                Read_Boolean_Cached ?? (Read_Boolean_Cached = Read_Boolean),
                Write_Boolean_Cached ?? (Write_Boolean_Cached = Write_Boolean));
        }

        public void Map(string inKey, ref Dictionary<int, System.Boolean> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.Boolean>(inKey, ref ioMap, inOptions,
                Read_Boolean_Cached ?? (Read_Boolean_Cached = Read_Boolean),
                Write_Boolean_Cached ?? (Write_Boolean_Cached = Write_Boolean));
        }
    
        #endregion // Basic

        #region Proxy

        public void BooleanProxy<ProxyType>(string inKey, ref ProxyType ioData, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Boolean>
        {
            DoProxy<ProxyType, System.Boolean>(inKey, ref ioData, inOptions,
                Read_Boolean_Cached ?? (Read_Boolean_Cached = Read_Boolean),
                Write_Boolean_Cached ?? (Write_Boolean_Cached = Write_Boolean));
        }

        public void BooleanProxy<ProxyType>(string inKey, ref ProxyType ioData, ProxyType inDefault, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Boolean>
        {
            DoProxy<ProxyType, System.Boolean>(inKey, ref ioData, inDefault, inOptions,
                Read_Boolean_Cached ?? (Read_Boolean_Cached = Read_Boolean),
                Write_Boolean_Cached ?? (Write_Boolean_Cached = Write_Boolean));
        }

        public void BooleanProxyArray<ProxyType>(string inKey, ref List<ProxyType> ioArray, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Boolean>
        {
            DoProxyArray<ProxyType, System.Boolean>(inKey, ref ioArray, inOptions,
                Read_Boolean_Cached ?? (Read_Boolean_Cached = Read_Boolean),
                Write_Boolean_Cached ?? (Write_Boolean_Cached = Write_Boolean));
        }

        public void BooleanProxyArray<ProxyType>(string inKey, ref ProxyType[] ioArray, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Boolean>
        {
            DoProxyArray<ProxyType, System.Boolean>(inKey, ref ioArray, inOptions,
                Read_Boolean_Cached ?? (Read_Boolean_Cached = Read_Boolean),
                Write_Boolean_Cached ?? (Write_Boolean_Cached = Write_Boolean));
        }

        public void BooleanProxySet<ProxyType>(string inKey, ref HashSet<ProxyType> ioSet, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Boolean>
        {
            DoProxySet<ProxyType, System.Boolean>(inKey, ref ioSet, inOptions,
                Read_Boolean_Cached ?? (Read_Boolean_Cached = Read_Boolean),
                Write_Boolean_Cached ?? (Write_Boolean_Cached = Write_Boolean));
        }

        public void BooleanProxyMap<ProxyType>(string inKey, ref Dictionary<string, ProxyType> ioMap, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Boolean>
        {
            DoProxyMap<ProxyType, System.Boolean>(inKey, ref ioMap, inOptions,
                Read_Boolean_Cached ?? (Read_Boolean_Cached = Read_Boolean),
                Write_Boolean_Cached ?? (Write_Boolean_Cached = Write_Boolean));
        }

        public void BooleanProxyMap<ProxyType>(string inKey, ref Dictionary<int, ProxyType> ioMap, FieldOptions inOptions = FieldOptions.None) where ProxyType : struct, ISerializedProxy<System.Boolean>
        {
            DoProxyMap<ProxyType, System.Boolean>(inKey, ref ioMap, inOptions,
                Read_Boolean_Cached ?? (Read_Boolean_Cached = Read_Boolean),
                Write_Boolean_Cached ?? (Write_Boolean_Cached = Write_Boolean));
        }
    
        #endregion // Proxy
    }
}
