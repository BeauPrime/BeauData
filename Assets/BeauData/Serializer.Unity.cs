/*
 * Copyright (C) 2017 - 2019. Filament Games, LLC. All rights reserved.
 * Author:  Alex Beauchesne
 * Date:    3 May 2017
 * 
 * File:    Serializer.Unity.cs
 * Purpose: Serializer for Unity objects.
 */

using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        private void SafeDispose<T>(ref T ioData) where T : UnityEngine.Object
        {
            if (ioData != null)
            {
                #if UNITY_EDITOR
                if (UnityEditor.EditorApplication.isPlaying)
                {
                    UnityEngine.Object.Destroy(ioData);
                }
                else
                {
                    UnityEngine.Object.DestroyImmediate(ioData);
                }
                #else
                UnityEngine.Object.Destroy(ioData);
                #endif
                ioData = null;
            }
        }

        private void SafeDispose<T>(ref T[] ioArray) where T : UnityEngine.Object
        {
            if (ioArray != null)
            {
                for (int i = ioArray.Length - 1; i >= 0; --i)
                    SafeDispose(ref ioArray[i]);
            }
            ioArray = null;
        }

        private void SafeRecreate<T>(ref T[] ioArray) where T : UnityEngine.Object
        {
            if (ioArray != null)
            {
                for (int i = ioArray.Length - 1; i >= 0; --i)
                    SafeDispose(ref ioArray[i]);
                System.Array.Resize(ref ioArray, 0);
            }
            else
            {
                ioArray = new T[0];
            }
        }

        private void SafeDispose<T>(ref List<T> ioArray) where T : UnityEngine.Object
        {
            if (ioArray != null)
            {
                for (int i = ioArray.Count - 1; i >= 0; --i)
                {
                    T obj = ioArray[i];
                    SafeDispose(ref obj);
                }
                ioArray.Clear();
            }
            ioArray = null;
        }

        private void SafeRecreate<T>(ref List<T> ioArray) where T : UnityEngine.Object
        {
            if (ioArray != null)
            {
                for (int i = ioArray.Count - 1; i >= 0; --i)
                {
                    T obj = ioArray[i];
                    SafeDispose(ref obj);
                }
                ioArray.Clear();
            }
            else
            {
                ioArray = new List<T>();
            }
        }

        private void SafeRecreate<T>(ref List<T> ioArray, int inCapacity) where T : UnityEngine.Object
        {
            if (ioArray != null)
            {
                for (int i = ioArray.Count - 1; i >= 0; --i)
                {
                    T obj = ioArray[i];
                    SafeDispose(ref obj);
                }
                ioArray.Clear();
                if (ioArray.Capacity < inCapacity)
                    ioArray.Capacity = inCapacity;
            }
            else
            {
                ioArray = new List<T>(inCapacity);
            }
        }

        private void SafeDispose<T>(ref HashSet<T> ioSet) where T : UnityEngine.Object
        {
            if (ioSet != null)
            {
                foreach (var obj in ioSet)
                {
                    T refObj = obj;
                    SafeDispose(ref refObj);
                }
                ioSet.Clear();
            }
            ioSet = null;
        }

        private void SafeRecreate<T>(ref HashSet<T> ioSet) where T : UnityEngine.Object
        {
            if (ioSet != null)
            {
                foreach (var obj in ioSet)
                {
                    T refObj = obj;
                    SafeDispose(ref refObj);
                }
                ioSet.Clear();
            }
            else
            {
                ioSet = new HashSet<T>();
            }
        }

        private void SafeDispose<T>(ref Dictionary<string, T> ioMap) where T : UnityEngine.Object
        {
            if (ioMap != null)
            {
                foreach (var value in ioMap.Values)
                {
                    T obj = value;
                    SafeDispose(ref obj);
                }
                ioMap.Clear();
            }
            ioMap = null;
        }

        private void SafeRecreate<T>(ref Dictionary<string, T> ioMap) where T : UnityEngine.Object
        {
            if (ioMap != null)
            {
                foreach (var value in ioMap.Values)
                {
                    T obj = value;
                    SafeDispose(ref obj);
                }
                ioMap.Clear();
            }
            else
            {
                ioMap = new Dictionary<string, T>();
            }
        }

        private void SafeDispose<T>(ref Dictionary<int, T> ioMap) where T : UnityEngine.Object
        {
            if (ioMap != null)
            {
                foreach (var value in ioMap.Values)
                {
                    T obj = value;
                    SafeDispose(ref obj);
                }
                ioMap.Clear();
            }
            ioMap = null;
        }

        private void SafeRecreate<T>(ref Dictionary<int, T> ioMap) where T : UnityEngine.Object
        {
            if (ioMap != null)
            {
                foreach (var value in ioMap.Values)
                {
                    T obj = value;
                    SafeDispose(ref obj);
                }
                ioMap.Clear();
            }
            else
            {
                ioMap = new Dictionary<int, T>();
            }
        }

        #region Read/Write

        #region Read

        /// <summary>
        /// Reads a value from the current node.
        /// </summary>
        private bool DoReadUnity<T>(int inIndex, ref T ioData, FieldOptions inOptions, ReadFunc<T> inReader) where T : UnityEngine.Object
        {
            bool bSuccess = BeginReadValue(inIndex);

            if (IsMissing())
            {
                if ((inOptions & FieldOptions.Optional) != 0)
                {
                    SafeDispose(ref ioData);
                    bSuccess = true;
                }
                else
                {
                    bSuccess &= false;
                }
            }
            else if (IsNull())
            {
                SafeDispose(ref ioData);
                bSuccess = true;
            }
            else
            {
                bSuccess &= inReader(ref ioData);
            }
            EndObject();

            return bSuccess;
        }

        /// <summary>
        /// Reads a value from the current node.
        /// </summary>
        private bool DoReadUnity<T>(string inKey, ref T ioData, FieldOptions inOptions, ReadFunc<T> inReader) where T : UnityEngine.Object
        {
            bool bSuccess = BeginReadValue(inKey);

            if (IsMissing())
            {
                if ((inOptions & FieldOptions.Optional) != 0)
                {
                    SafeDispose(ref ioData);
                    bSuccess = true;
                }
                else
                {
                    bSuccess &= false;
                }
            }
            else if (IsNull())
            {
                SafeDispose(ref ioData);
                bSuccess = true;
            }
            else
            {
                bSuccess &= inReader(ref ioData);
            }
            EndObject();

            return bSuccess;
        }

        #endregion

        #region Write

        /// <summary>
        /// Writes a value onto the current array.
        /// </summary>
        private void DoWriteUnity<T>(ref T ioData, FieldOptions inOptions, WriteFunc<T> inWriter) where T : UnityEngine.Object
        {
            if (ioData == null)
            {
                WriteNull();
                return;
            }

            BeginWriteValue();
            inWriter(ref ioData);
            EndValue();
        }

        /// <summary>
        /// Writes a value into the current object.
        /// </summary>
        private void DoWriteUnity<T>(string inKey, ref T ioData, FieldOptions inOptions, WriteFunc<T> inWriter) where T : UnityEngine.Object
        {
            if (ioData == null)
            {
                if ((inOptions & FieldOptions.Optional) == 0 || RequiresExplicitNull())
                    WriteNull(inKey);
                return;
            }

            BeginWriteValue(inKey, inOptions);
            inWriter(ref ioData);
            EndValue();
        }

        #endregion

        #endregion

        #region Serialize

        /// <summary>
        /// Serializes a value on the current object.
        /// </summary>
        private void DoSerializeUnity<T>(string inKey, ref T ioData, FieldOptions inOptions, ReadFunc<T> inReader, WriteFunc<T> inWriter) where T : UnityEngine.Object
        {
            if (IsReading)
            {
                bool bSuccess = DoReadUnity<T>(inKey, ref ioData, inOptions, inReader);

                if (!bSuccess)
                    AddErrorMessage("Unable to read value '{0}'.", inKey);

                return;
            }

            if (ioData == null)
            {
                if ((inOptions & FieldOptions.Optional) == 0 || RequiresExplicitNull())
                    WriteNull(inKey);
            }
            else
            {
                DoWriteUnity<T>(inKey, ref ioData, inOptions, inWriter);
            }
        }

        #endregion

        #region Array

        /// <summary>
        /// Serializes an array on the current object.
        /// </summary>
        private void DoArrayUnity<T>(string inKey, ref List<T> ioArray, FieldOptions inOptions, ReadFunc<T> inReader, WriteFunc<T> inWriter) where T : UnityEngine.Object
        {
            if (IsReading)
            {
                bool bSuccess = BeginReadArray(inKey);
                {
                    if (IsMissing())
                    {
                        if ((inOptions & FieldOptions.Optional) != 0)
                        {
                            SafeDispose(ref ioArray);
                        }
                        else
                        {
                            bSuccess = false;
                        }
                    }
                    else if (IsNull())
                    {
                        SafeDispose(ref ioArray);
                    }
                    else
                    {
                        int nodeCount = GetChildCount();
                        SafeRecreate(ref ioArray, nodeCount);

                        for (int i = 0; i < nodeCount; ++i)
                        {
                            T obj = default(T);
                            bSuccess &= DoReadUnity(i, ref obj, FieldOptions.None, inReader);
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
                    DoWriteUnity<T>(ref obj, FieldOptions.None, inWriter);
                }
                EndArray();
            }
        }

        /// <summary>
        /// Serializes an array on the current object.
        /// </summary>
        private void DoArrayUnity<T>(string inKey, ref T[] ioArray, FieldOptions inOptions, ReadFunc<T> inReader, WriteFunc<T> inWriter) where T : UnityEngine.Object
        {
            if (IsReading)
            {
                bool bSuccess = BeginReadArray(inKey);
                {
                    if (IsMissing())
                    {
                        if ((inOptions & FieldOptions.Optional) != 0)
                        {
                            SafeDispose(ref ioArray);
                        }
                        else
                        {
                            bSuccess = false;
                        }
                    }
                    else if (IsNull())
                    {
                        SafeDispose(ref ioArray);
                    }
                    else
                    {
                        int nodeCount = GetChildCount();
                        if (ioArray != null)
                        {
                            // Make sure to dispose the objects cut off by the new length
                            for (int i = nodeCount; i < ioArray.Length; ++i)
                                SafeDispose(ref ioArray[i]);
                            System.Array.Resize(ref ioArray, nodeCount);
                        }
                        else
                            ioArray = new T[nodeCount];

                        for (int i = 0; i < nodeCount; ++i)
                        {
                            T obj = default(T);
                            bSuccess &= DoReadUnity(i, ref obj, FieldOptions.None, inReader);
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
                    DoWriteUnity<T>(ref obj, FieldOptions.None, inWriter);
                }
                EndArray();
            }
        }

        #endregion

        #region Set

        /// <summary>
        /// Serializes a set on the current object.
        /// </summary>
        private void DoSetUnity<T>(string inKey, ref HashSet<T> ioSet, FieldOptions inOptions, ReadFunc<T> inReader, WriteFunc<T> inWriter) where T : UnityEngine.Object
        {
            if (IsReading)
            {
                bool bSuccess = BeginReadArray(inKey);
                {
                    if (IsMissing())
                    {
                        if ((inOptions & FieldOptions.Optional) != 0)
                        {
                            SafeDispose(ref ioSet);
                        }
                        else
                        {
                            bSuccess = false;
                        }
                    }
                    else if (IsNull())
                    {
                        SafeDispose(ref ioSet);
                    }
                    else
                    {
                        int nodeCount = GetChildCount();
                        SafeRecreate(ref ioSet);

                        for (int i = 0; i < nodeCount; ++i)
                        {
                            T obj = default(T);
                            bSuccess &= DoReadUnity(i, ref obj, FieldOptions.None, inReader);
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
                foreach(var item in ioSet)
                {
                    T obj = item;
                    DoWriteUnity<T>(ref obj, FieldOptions.None, inWriter);
                }
                EndArray();
            }
        }

        #endregion

        #region Map

        /// <summary>
        /// Serializes a map on the current object.
        /// </summary>
        private void DoMapUnity<T>(string inKey, ref Dictionary<string, T> ioMap, FieldOptions inOptions, ReadFunc<T> inReader, WriteFunc<T> inWriter) where T : UnityEngine.Object
        {
            if (IsReading)
            {
                bool bSuccess = BeginReadArray(inKey);
                {
                    if (IsMissing())
                    {
                        if ((inOptions & FieldOptions.Optional) != 0)
                        {
                            SafeDispose(ref ioMap);
                        }
                        else
                        {
                            bSuccess = false;
                        }
                    }
                    else if (IsNull())
                    {
                        SafeDispose(ref ioMap);
                    }
                    else
                    {
                        int nodeCount = GetChildCount();
                        SafeRecreate(ref ioMap);

                        for (int i = 0; i < nodeCount; ++i)
                        {
                            bSuccess &= BeginReadObject(i);
                            {
                                string key = null;
                                bSuccess &= DoRead(MAP_KEY, ref key, FieldOptions.None, Read_String_Cached ?? (Read_String_Cached = Read_String));

                                T obj = default(T);
                                bSuccess &= DoReadUnity(MAP_VALUE, ref obj, FieldOptions.None, inReader);

                                ioMap.Add(key, obj);
                            }
                            EndObject();
                        }
                    }
                }
                EndArray();

                if (!bSuccess)
                    AddErrorMessage("Unable to read map '{0}'.", inKey);

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
                    DoWriteUnity(MAP_VALUE, ref obj, FieldOptions.None, inWriter);

                    EndObject();
                }
                EndArray();
            }
        }

        /// <summary>
        /// Serializes a map on the current object.
        /// </summary>
        private void DoMapUnity<T>(string inKey, ref Dictionary<int, T> ioMap, FieldOptions inOptions, ReadFunc<T> inReader, WriteFunc<T> inWriter) where T : UnityEngine.Object
        {
            if (IsReading)
            {
                bool bSuccess = BeginReadArray(inKey);
                {
                    if (IsMissing())
                    {
                        if ((inOptions & FieldOptions.Optional) != 0)
                        {
                            SafeDispose(ref ioMap);
                        }
                        else
                        {
                            bSuccess = false;
                        }
                    }
                    else if (IsNull())
                    {
                        SafeDispose(ref ioMap);
                    }
                    else
                    {
                        int nodeCount = GetChildCount();
                        SafeRecreate(ref ioMap);

                        for (int i = 0; i < nodeCount; ++i)
                        {
                            bSuccess &= BeginReadObject(i);
                            {
                                int key = default(int);
                                bSuccess &= DoRead(MAP_KEY, ref key, FieldOptions.None, Read_Int32_Cached ?? (Read_Int32_Cached = Read_Int32));

                                T obj = default(T);
                                bSuccess &= DoReadUnity(MAP_VALUE, ref obj, FieldOptions.None, inReader);

                                ioMap.Add(key, obj);
                            }
                            EndObject();
                        }
                    }
                }
                EndArray();

                if (!bSuccess)
                    AddErrorMessage("Unable to read map '{0}'.", inKey);

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
                    DoWriteUnity(MAP_VALUE, ref obj, FieldOptions.None, inWriter);

                    EndObject();
                }
                EndArray();
            }
        }

        #endregion
    }
}