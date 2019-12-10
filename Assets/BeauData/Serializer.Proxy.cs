using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        private void DoProxy<ProxyType, InnerType>(string inKey, ref ProxyType ioData, FieldOptions inOptions, ReadFunc<InnerType> inReader, WriteFunc<InnerType> inWriter)
        where ProxyType : struct, ISerializedProxy<InnerType>
        {
            if (IsReading)
            {
                bool bSuccess = DoReadProxy<ProxyType, InnerType>(inKey, ref ioData, inOptions, inReader);

                if (!bSuccess)
                    AddErrorMessage("Unable to read proxy '{0}'.", inKey);
                return;
            }

            DoWriteProxy<ProxyType, InnerType>(inKey, ref ioData, inOptions, inWriter);
        }

        private void DoProxy<ProxyType, InnerType>(string inKey, ref ProxyType ioData, ProxyType inDefault, FieldOptions inOptions, ReadFunc<InnerType> inReader, WriteFunc<InnerType> inWriter)
        where ProxyType : struct, ISerializedProxy<InnerType>
        {
            if (IsReading)
            {
                bool bSuccess = DoReadProxy<ProxyType, InnerType>(inKey, ref ioData, inDefault, inOptions, inReader);

                if (!bSuccess)
                    AddErrorMessage("Unable to read proxy '{0}'.", inKey);
                return;
            }

            DoWriteProxy<ProxyType, InnerType>(inKey, ref ioData, inDefault, inOptions, inWriter);
        }

        private void DoProxyArray<ProxyType, InnerType>(string inKey, ref List<ProxyType> ioArray, FieldOptions inOptions, ReadFunc<InnerType> inReader, WriteFunc<InnerType> inWriter)
        where ProxyType : struct, ISerializedProxy<InnerType>
        {
            if (IsReading)
            {
                bool bSuccess = BeginReadArray(inKey);
                {
                    if (IsMissing())
                    {
                        if ((inOptions & FieldOptions.Optional) != 0)
                        {
                            if (ioArray != null)
                                ioArray.Clear();
                            ioArray = null;
                            bSuccess = true;
                        }
                        else
                        {
                            bSuccess = false;
                        }
                    }
                    else if (IsNull())
                    {
                        if (ioArray != null)
                            ioArray.Clear();
                        ioArray = null;
                        bSuccess = true;
                    }
                    else
                    {
                        int nodeCount = GetChildCount();
                        if (ioArray != null)
                        {
                            ioArray.Clear();
                            if (ioArray.Capacity < nodeCount)
                                ioArray.Capacity = nodeCount;
                        }
                        else
                            ioArray = new List<ProxyType>(nodeCount);

                        for (int i = 0; i < nodeCount; ++i)
                        {
                            ProxyType obj = default(ProxyType);
                            bSuccess &= DoReadProxy(i, ref obj, FieldOptions.None, inReader);
                            ioArray.Add(obj);
                        }
                    }
                }
                EndArray();

                if (!bSuccess)
                    AddErrorMessage("Unable to read array '{0}'.", inKey);

                return;
            }

            if (ioArray == null)
            {
                if ((inOptions & FieldOptions.Optional) == 0 || RequiresExplicitNull())
                    WriteNull(inKey);
            }
            else
            {
                BeginWriteArray(inKey);
                DeclareChildCount(ioArray.Count);
                for (int i = 0; i < ioArray.Count; ++i)
                {
                    ProxyType obj = ioArray[i];
                    DoWriteProxy(ref obj, inWriter);
                }
                EndArray();
            }
        }

        private void DoProxyArray<ProxyType, InnerType>(string inKey, ref ProxyType[] ioArray, FieldOptions inOptions, ReadFunc<InnerType> inReader, WriteFunc<InnerType> inWriter)
        where ProxyType : struct, ISerializedProxy<InnerType>
        {
            if (IsReading)
            {
                bool bSuccess = BeginReadArray(inKey);
                {
                    if (IsMissing())
                    {
                        if ((inOptions & FieldOptions.Optional) != 0)
                        {
                            if (ioArray != null)
                                System.Array.Resize(ref ioArray, 0);
                            ioArray = null;
                            bSuccess = true;
                        }
                        else
                        {
                            bSuccess = false;
                        }
                    }
                    else if (IsNull())
                    {
                        if (ioArray != null)
                            System.Array.Resize(ref ioArray, 0);
                        ioArray = null;
                        bSuccess = true;
                    }
                    else
                    {
                        int nodeCount = GetChildCount();
                        if (ioArray != null)
                            System.Array.Resize(ref ioArray, nodeCount);
                        else
                            ioArray = new ProxyType[nodeCount];

                        for (int i = 0; i < nodeCount; ++i)
                        {
                            ProxyType obj = default(ProxyType);
                            bSuccess &= DoReadProxy(i, ref obj, FieldOptions.None, inReader);
                            ioArray[i] = obj;
                        }
                    }
                }
                EndArray();

                if (!bSuccess)
                    AddErrorMessage("Unable to read array '{0}'.", inKey);

                return;
            }

            if (ioArray == null)
            {
                if ((inOptions & FieldOptions.Optional) == 0 || RequiresExplicitNull())
                    WriteNull(inKey);
            }
            else
            {
                BeginWriteArray(inKey);
                DeclareChildCount(ioArray.Length);
                for (int i = 0; i < ioArray.Length; ++i)
                {
                    ProxyType obj = ioArray[i];
                    DoWriteProxy(ref obj, inWriter);
                }
                EndArray();
            }
        }

        private void DoProxySet<ProxyType, InnerType>(string inKey, ref HashSet<ProxyType> ioSet, FieldOptions inOptions, ReadFunc<InnerType> inReader, WriteFunc<InnerType> inWriter)
        where ProxyType : struct, ISerializedProxy<InnerType>
        {
            if (IsReading)
            {
                bool bSuccess = BeginReadArray(inKey);
                {
                    if (IsMissing())
                    {
                        if ((inOptions & FieldOptions.Optional) != 0)
                        {
                            if (ioSet != null)
                                ioSet.Clear();
                            ioSet = null;
                            bSuccess = true;
                        }
                        else
                        {
                            bSuccess = false;
                        }
                    }
                    else if (IsNull())
                    {
                        if (ioSet != null)
                            ioSet.Clear();
                        ioSet = null;
                        bSuccess = true;
                    }
                    else
                    {
                        int nodeCount = GetChildCount();
                        if (ioSet != null)
                            ioSet.Clear();
                        else
                            ioSet = new HashSet<ProxyType>();

                        for (int i = 0; i < nodeCount; ++i)
                        {
                            ProxyType obj = default(ProxyType);
                            bSuccess &= DoReadProxy(i, ref obj, FieldOptions.None, inReader);
                            ioSet.Add(obj);
                        }
                    }
                }
                EndArray();

                if (!bSuccess)
                    AddErrorMessage("Unable to read array '{0}'.", inKey);

                return;
            }

            if (ioSet == null)
            {
                if ((inOptions & FieldOptions.Optional) == 0 || RequiresExplicitNull())
                    WriteNull(inKey);
            }
            else
            {
                BeginWriteArray(inKey);
                DeclareChildCount(ioSet.Count);
                foreach (var item in ioSet)
                {
                    ProxyType obj = item;
                    DoWriteProxy(ref obj, inWriter);
                }
                EndArray();
            }
        }

        private void DoProxyMap<ProxyType, InnerType>(string inKey, ref Dictionary<string, ProxyType> ioMap, FieldOptions inOptions, ReadFunc<InnerType> inReader, WriteFunc<InnerType> inWriter)
        where ProxyType : struct, ISerializedProxy<InnerType>
        {
            if (IsReading)
            {
                bool bSuccess = BeginReadArray(inKey);
                {
                    if (IsMissing())
                    {
                        if ((inOptions & FieldOptions.Optional) != 0)
                        {
                            if (ioMap != null)
                                ioMap.Clear();
                            ioMap = null;
                            bSuccess = true;
                        }
                        else
                        {
                            bSuccess = false;
                        }
                    }
                    else if (IsNull())
                    {
                        if (ioMap != null)
                            ioMap.Clear();
                        ioMap = null;
                        bSuccess = true;
                    }
                    else
                    {
                        int nodeCount = GetChildCount();
                        if (ioMap != null)
                            ioMap.Clear();
                        else
                            ioMap = new Dictionary<string, ProxyType>(nodeCount);

                        for (int i = 0; i < nodeCount; ++i)
                        {
                            bSuccess &= BeginReadObject(i);
                            {
                                string key = null;
                                bSuccess &= DoRead(MAP_KEY, ref key, FieldOptions.None, Read_String_Cached ?? (Read_String_Cached = Read_String));

                                ProxyType obj = default(ProxyType);
                                bSuccess &= DoReadProxy(MAP_VALUE, ref obj, FieldOptions.None, inReader);

                                ioMap.Add(key, obj);
                            }
                            EndObject();
                        }
                    }
                }
                EndArray();

                if (!bSuccess)
                    AddErrorMessage("Unable to read array '{0}'.", inKey);

                return;
            }

            if (ioMap == null)
            {
                if ((inOptions & FieldOptions.Optional) == 0 || RequiresExplicitNull())
                    WriteNull(inKey);
            }
            else
            {
                BeginWriteArray(inKey);
                DeclareChildCount(ioMap.Count);
                foreach (var keyValue in ioMap)
                {
                    BeginWriteObject();

                    string key = keyValue.Key;
                    DoWrite(MAP_KEY, ref key, FieldOptions.PreferAttribute, this.Write_String);

                    ProxyType obj = keyValue.Value;
                    DoWriteProxy(MAP_VALUE, ref obj, FieldOptions.None, inWriter);

                    EndObject();
                }
                EndArray();
            }
        }

        private void DoProxyMap<ProxyType, InnerType>(string inKey, ref Dictionary<int, ProxyType> ioMap, FieldOptions inOptions, ReadFunc<InnerType> inReader, WriteFunc<InnerType> inWriter)
        where ProxyType : struct, ISerializedProxy<InnerType>
        {
            if (IsReading)
            {
                bool bSuccess = BeginReadArray(inKey);
                {
                    if (IsMissing())
                    {
                        if ((inOptions & FieldOptions.Optional) != 0)
                        {
                            if (ioMap != null)
                                ioMap.Clear();
                            ioMap = null;
                            bSuccess = true;
                        }
                        else
                        {
                            bSuccess = false;
                        }
                    }
                    else if (IsNull())
                    {
                        if (ioMap != null)
                            ioMap.Clear();
                        ioMap = null;
                        bSuccess = true;
                    }
                    else
                    {
                        int nodeCount = GetChildCount();
                        if (ioMap != null)
                            ioMap.Clear();
                        else
                            ioMap = new Dictionary<int, ProxyType>(nodeCount);

                        for (int i = 0; i < nodeCount; ++i)
                        {
                            bSuccess &= BeginReadObject(i);
                            {
                                int key = default(int);
                                bSuccess &= DoRead(MAP_KEY, ref key, FieldOptions.None, Read_Int32_Cached ?? (Read_Int32_Cached = Read_Int32));

                                ProxyType obj = default(ProxyType);
                                bSuccess &= DoReadProxy(MAP_VALUE, ref obj, FieldOptions.None, inReader);

                                ioMap.Add(key, obj);
                            }
                            EndObject();
                        }
                    }
                }
                EndArray();

                if (!bSuccess)
                    AddErrorMessage("Unable to read array '{0}'.", inKey);

                return;
            }

            if (ioMap == null)
            {
                if ((inOptions & FieldOptions.Optional) == 0 || RequiresExplicitNull())
                    WriteNull(inKey);
            }
            else
            {
                BeginWriteArray(inKey);
                DeclareChildCount(ioMap.Count);
                foreach (var keyValue in ioMap)
                {
                    BeginWriteObject();

                    int key = keyValue.Key;
                    DoWrite(MAP_KEY, ref key, FieldOptions.PreferAttribute, this.Write_Int32);

                    ProxyType obj = keyValue.Value;
                    DoWriteProxy(MAP_VALUE, ref obj, FieldOptions.None, inWriter);

                    EndObject();
                }
                EndArray();
            }
        }

        #region Read/Write

        /// <summary>
        /// Reads an object from the current node.
        /// </summary>
        private bool DoReadProxy<ProxyType, InnerType>(int inIndex, ref ProxyType ioData, FieldOptions inOptions, ReadFunc<InnerType> inReader)
        where ProxyType : struct, ISerializedProxy<InnerType>
        {
            bool bSuccess = BeginReadValue(inIndex);

            if (IsMissing())
            {
                if ((inOptions & FieldOptions.Optional) != 0)
                {
                    ioData = default(ProxyType);
                    bSuccess = true;
                }
                else
                {
                    bSuccess &= false;
                }
            }
            else if (IsNull())
            {
                ioData = default(ProxyType);
                bSuccess = true;
            }
            else
            {
                InnerType innerData = default(InnerType);
                if (inReader(ref innerData))
                {
                    ISerializedProxy<InnerType> proxy = ioData;
                    proxy.SetProxyValue(innerData, Context);
                    ioData = (ProxyType) proxy;
                }
                else
                {
                    bSuccess = false;
                }
            }
            EndValue();

            return bSuccess;
        }

        /// <summary>
        /// Reads an object from the current node.
        /// </summary>
        private bool DoReadProxy<ProxyType, InnerType>(int inIndex, ref ProxyType ioData, ProxyType inDefault, FieldOptions inOptions, ReadFunc<InnerType> inReader)
        where ProxyType : struct, ISerializedProxy<InnerType>
        {
            bool bSuccess = BeginReadValue(inIndex);

            if (IsMissing() || IsNull())
            {
                ioData = inDefault;
                bSuccess = true;
            }
            else
            {
                InnerType innerData = default(InnerType);
                if (inReader(ref innerData))
                {
                    ISerializedProxy<InnerType> proxy = ioData;
                    proxy.SetProxyValue(innerData, Context);
                    ioData = (ProxyType) proxy;
                }
                else
                {
                    bSuccess = false;
                }
            }
            EndValue();

            return bSuccess;
        }

        /// <summary>
        /// Reads an object from the current node.
        /// </summary>
        private bool DoReadProxy<ProxyType, InnerType>(string inKey, ref ProxyType ioData, FieldOptions inOptions, ReadFunc<InnerType> inReader)
        where ProxyType : struct, ISerializedProxy<InnerType>
        {
            bool bSuccess = BeginReadValue(inKey);

            if (IsMissing())
            {
                if ((inOptions & FieldOptions.Optional) != 0)
                {
                    ioData = default(ProxyType);
                    bSuccess = true;
                }
                else
                {
                    bSuccess &= false;
                }
            }
            else if (IsNull())
            {
                ioData = default(ProxyType);
                bSuccess = true;
            }
            else
            {
                InnerType innerData = default(InnerType);
                if (inReader(ref innerData))
                {
                    ISerializedProxy<InnerType> proxy = ioData;
                    proxy.SetProxyValue(innerData, Context);
                    ioData = (ProxyType) proxy;
                }
                else
                {
                    bSuccess = false;
                }
            }
            EndValue();

            return bSuccess;
        }

        /// <summary>
        /// Reads an object from the current node.
        /// </summary>
        private bool DoReadProxy<ProxyType, InnerType>(string inKey, ref ProxyType ioData, ProxyType inDefault, FieldOptions inOptions, ReadFunc<InnerType> inReader)
        where ProxyType : struct, ISerializedProxy<InnerType>
        {
            bool bSuccess = BeginReadValue(inKey);

            if (IsMissing() || IsNull())
            {
                ioData = inDefault;
                bSuccess = true;
            }
            else
            {
                InnerType innerData = default(InnerType);
                if (inReader(ref innerData))
                {
                    ISerializedProxy<InnerType> proxy = ioData;
                    proxy.SetProxyValue(innerData, Context);
                    ioData = (ProxyType) proxy;
                }
                else
                {
                    bSuccess = false;
                }
            }
            EndValue();

            return bSuccess;
        }

        /// <summary>
        /// Writes an object onto the current array.
        /// </summary>
        private void DoWriteProxy<ProxyType, InnerType>(ref ProxyType ioData, WriteFunc<InnerType> inWriter)
        where ProxyType : struct, ISerializedProxy<InnerType>
        {
            BeginWriteValue();
            InnerType innerData = ioData.GetProxyValue(Context);
            inWriter(ref innerData);
            EndValue();
        }

        /// <summary>
        /// Writes an object onto the current array.
        /// </summary>
        private void DoWriteProxy<ProxyType, InnerType>(ref ProxyType ioData, ProxyType inDefault, WriteFunc<InnerType> inWriter)
        where ProxyType : struct, ISerializedProxy<InnerType>
        {
            if (!EqualityComparer<ProxyType>.Default.Equals(ioData, inDefault))
            {
                BeginWriteValue();
                InnerType innerData = ioData.GetProxyValue(Context);
                inWriter(ref innerData);
                EndValue();
            }
            else
            {
                WriteNull();
            }
        }

        /// <summary>
        /// Writes an object into the current object.
        /// </summary>
        private void DoWriteProxy<ProxyType, InnerType>(string inKey, ref ProxyType ioData, FieldOptions inOptions, WriteFunc<InnerType> inWriter)
        where ProxyType : struct, ISerializedProxy<InnerType>
        {
            BeginWriteValue(inKey, inOptions);
            InnerType innerData = ioData.GetProxyValue(Context);
            inWriter(ref innerData);
            EndValue();
        }

        /// <summary>
        /// Writes an object into the current object.
        /// </summary>
        private void DoWriteProxy<ProxyType, InnerType>(string inKey, ref ProxyType ioData, ProxyType inDefault, FieldOptions inOptions, WriteFunc<InnerType> inWriter)
        where ProxyType : struct, ISerializedProxy<InnerType>
        {
            if (!EqualityComparer<ProxyType>.Default.Equals(ioData, inDefault))
            {
                BeginWriteValue(inKey, inOptions);
                InnerType innerData = ioData.GetProxyValue(Context);
                inWriter(ref innerData);
                EndValue();
            }
            else if ((inOptions & FieldOptions.Optional) == 0 || RequiresExplicitNull())
            {
                WriteNull(inKey);
            }
        }

        #endregion
    }
}