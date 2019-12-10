using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        private bool Read_Custom<T>(ref T inObject, TypeUtility.TypeSerializerDelegate<T> inSerializer)
        {
            int prevErrorLength = m_ErrorString.Length;
            if (inObject == null)
                inObject = (T) TypeUtility.Instantiate(typeof(T), this);
            inSerializer(ref inObject, this);
            return m_ErrorString.Length == prevErrorLength;
        }

        private void Write_Custom<T>(ref T inObject, TypeUtility.TypeSerializerDelegate<T> inSerializer)
        {
            inSerializer(ref inObject, this);
        }

        private void DoCustom<T>(string inKey, ref T ioData, FieldOptions inOptions, TypeUtility.TypeSerializerDelegate<T> inSerializer)
        {
            if (IsReading)
            {
                bool bSuccess = DoReadCustom<T>(inKey, ref ioData, inOptions, inSerializer);

                if (!bSuccess)
                    AddErrorMessage("Unable to read struct '{0}'.", inKey);
                return;
            }

            DoWriteCustom<T>(inKey, ref ioData, inOptions, inSerializer);
        }

        private void DoCustom<T>(string inKey, ref T ioData, T inDefault, FieldOptions inOptions, TypeUtility.TypeSerializerDelegate<T> inSerializer)
        {
            if (IsReading)
            {
                bool bSuccess = DoReadCustom<T>(inKey, ref ioData, inDefault, inOptions, inSerializer);

                if (!bSuccess)
                    AddErrorMessage("Unable to read struct '{0}'.", inKey);
                return;
            }

            DoWriteCustom<T>(inKey, ref ioData, inDefault, inOptions, inSerializer);
        }

        private void DoCustomArray<T>(string inKey, ref List<T> ioArray, FieldOptions inOptions, TypeUtility.TypeSerializerDelegate<T> inSerializer)
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
                            ioArray = new List<T>(nodeCount);

                        for (int i = 0; i < nodeCount; ++i)
                        {
                            T obj = default(T);
                            bSuccess &= DoReadCustom(i, ref obj, FieldOptions.None, inSerializer);
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
                    T obj = ioArray[i];
                    DoWriteCustom<T>(ref obj, inSerializer);
                }
                EndArray();
            }
        }

        private void DoCustomArray<T>(string inKey, ref T[] ioArray, FieldOptions inOptions, TypeUtility.TypeSerializerDelegate<T> inSerializer)
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
                            ioArray = new T[nodeCount];

                        for (int i = 0; i < nodeCount; ++i)
                        {
                            T obj = default(T);
                            bSuccess &= DoReadCustom(i, ref obj, FieldOptions.None, inSerializer);
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
                    T obj = ioArray[i];
                    DoWriteCustom<T>(ref obj, inSerializer);
                }
                EndArray();
            }
        }

        private void DoCustomSet<T>(string inKey, ref HashSet<T> ioSet, FieldOptions inOptions, TypeUtility.TypeSerializerDelegate<T> inSerializer)
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
                            ioSet = new HashSet<T>();

                        for (int i = 0; i < nodeCount; ++i)
                        {
                            T obj = default(T);
                            bSuccess &= DoReadCustom(i, ref obj, FieldOptions.None, inSerializer);
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
                    T obj = item;
                    DoWriteCustom<T>(ref obj, inSerializer);
                }
                EndArray();
            }
        }

        private void DoCustomMap<T>(string inKey, ref Dictionary<string, T> ioMap, FieldOptions inOptions, TypeUtility.TypeSerializerDelegate<T> inSerializer)
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
                            ioMap = new Dictionary<string, T>(nodeCount);

                        for (int i = 0; i < nodeCount; ++i)
                        {
                            bSuccess &= BeginReadObject(i);
                            {
                                string key = null;
                                bSuccess &= DoRead(MAP_KEY, ref key, FieldOptions.None, Read_String_Cached ?? (Read_String_Cached = Read_String));

                                T obj = default(T);
                                bSuccess &= DoReadCustom(MAP_VALUE, ref obj, FieldOptions.None, inSerializer);

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

                    T obj = keyValue.Value;
                    DoWriteCustom(MAP_VALUE, ref obj, FieldOptions.None, inSerializer);

                    EndObject();
                }
                EndArray();
            }
        }

        private void DoCustomMap<T>(string inKey, ref Dictionary<int, T> ioMap, FieldOptions inOptions, TypeUtility.TypeSerializerDelegate<T> inSerializer)
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
                            ioMap = new Dictionary<int, T>(nodeCount);

                        for (int i = 0; i < nodeCount; ++i)
                        {
                            bSuccess &= BeginReadObject(i);
                            {
                                int key = default(int);
                                bSuccess &= DoRead(MAP_KEY, ref key, FieldOptions.None, Read_Int32_Cached ?? (Read_Int32_Cached = Read_Int32));

                                T obj = default(T);
                                bSuccess &= DoReadCustom(MAP_VALUE, ref obj, FieldOptions.None, inSerializer);

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

                    T obj = keyValue.Value;
                    DoWriteCustom(MAP_VALUE, ref obj, FieldOptions.None, inSerializer);

                    EndObject();
                }
                EndArray();
            }
        }

        #region Read/Write

        /// <summary>
        /// Reads an object from the current node.
        /// </summary>
        private bool DoReadCustom<T>(int inIndex, ref T ioData, FieldOptions inOptions, TypeUtility.TypeSerializerDelegate<T> inSerializer)
        {
            bool bSuccess = BeginReadObject(inIndex);

            if (IsMissing())
            {
                if ((inOptions & FieldOptions.Optional) != 0)
                {
                    ioData = default(T);
                    bSuccess = true;
                }
                else
                {
                    bSuccess &= false;
                }
            }
            else if (IsNull())
            {
                ioData = default(T);
                bSuccess = true;
            }
            else
            {
                bSuccess &= Read_Custom(ref ioData, inSerializer);
            }
            EndObject();

            return bSuccess;
        }

        /// <summary>
        /// Reads an object from the current node.
        /// </summary>
        private bool DoReadCustom<T>(int inIndex, ref T ioData, T inDefault, FieldOptions inOptions, TypeUtility.TypeSerializerDelegate<T> inSerializer)
        {
            bool bSuccess = BeginReadObject(inIndex);

            if (IsMissing() || IsNull())
            {
                ioData = inDefault;
                bSuccess = true;
            }
            else
            {
                bSuccess &= Read_Custom(ref ioData, inSerializer);
            }
            EndObject();

            return bSuccess;
        }

        /// <summary>
        /// Reads an object from the current node.
        /// </summary>
        private bool DoReadCustom<T>(string inKey, ref T ioData, FieldOptions inOptions, TypeUtility.TypeSerializerDelegate<T> inSerializer)
        {
            bool bSuccess = BeginReadObject(inKey);

            if (IsMissing())
            {
                if ((inOptions & FieldOptions.Optional) != 0)
                {
                    ioData = default(T);
                    bSuccess = true;
                }
                else
                {
                    bSuccess &= false;
                }
            }
            else if (IsNull())
            {
                ioData = default(T);
                bSuccess = true;
            }
            else
            {
                bSuccess &= Read_Custom(ref ioData, inSerializer);
            }
            EndObject();

            return bSuccess;
        }

        /// <summary>
        /// Reads an object from the current node.
        /// </summary>
        private bool DoReadCustom<T>(string inKey, ref T ioData, T inDefault, FieldOptions inOptions, TypeUtility.TypeSerializerDelegate<T> inSerializer)
        {
            bool bSuccess = BeginReadObject(inKey);

            if (IsMissing() || IsNull())
            {
                ioData = inDefault;
                bSuccess = true;
            }
            else
            {
                bSuccess &= Read_Custom(ref ioData, inSerializer);
            }
            EndObject();

            return bSuccess;
        }

        /// <summary>
        /// Writes an object onto the current array.
        /// </summary>
        private void DoWriteCustom<T>(ref T ioData, TypeUtility.TypeSerializerDelegate<T> inSerializer)
        {
            if (ioData != null)
            {
                BeginWriteObject();
                Write_Custom(ref ioData, inSerializer);
                EndValue();
            }
            else
            {
                WriteNull();
            }
        }

        /// <summary>
        /// Writes an object onto the current array.
        /// </summary>
        private void DoWriteCustom<T>(ref T ioData, T inDefault, TypeUtility.TypeSerializerDelegate<T> inSerializer)
        {
            if (ioData != null && !ioData.Equals(inDefault))
            {
                BeginWriteObject();
                Write_Custom(ref ioData, inSerializer);
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
        private void DoWriteCustom<T>(string inKey, ref T ioData, FieldOptions inOptions, TypeUtility.TypeSerializerDelegate<T> inSerializer)
        {
            if (ioData != null)
            {
                BeginWriteObject(inKey);
                Write_Custom(ref ioData, inSerializer);
                EndValue();
            }
            else if ((inOptions & FieldOptions.Optional) == 0 || RequiresExplicitNull())
            {
                WriteNull(inKey);
            }
        }

        /// <summary>
        /// Writes an object into the current object.
        /// </summary>
        private void DoWriteCustom<T>(string inKey, ref T ioData, T inDefault, FieldOptions inOptions, TypeUtility.TypeSerializerDelegate<T> inSerializer)
        {
            if (ioData != null && !ioData.Equals(inDefault))
            {
                BeginWriteObject(inKey);
                Write_Custom(ref ioData, inSerializer);
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