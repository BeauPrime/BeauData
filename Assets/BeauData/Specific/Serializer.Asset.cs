/*
 * Copyright (C) 2017 - 2019. Filament Games, LLC. All rights reserved.
 * Author:  Alex Beauchesne
 * Date:    3 May 2017
 * 
 * File:    Serializer.Asset.cs
 * Purpose: Serializer for asset references.
 */

using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        #region Read/Write

        #region Read

        /// <summary>
        /// Reads a value from the current node.
        /// </summary>
        private bool DoReadAsset<T>(int inIndex, ref T ioData, FieldOptions inOptions) where T : class
        {
            bool bSuccess = BeginReadValue(inIndex);

            if (IsMissing())
            {
                if ((inOptions & FieldOptions.Optional) != 0)
                {
                    ioData = null;
                    bSuccess = true;
                }
                else
                {
                    bSuccess &= false;
                }
            }
            else if (IsNull())
            {
                ioData = null;
                bSuccess = true;
            }
            else
            {
                string id = string.Empty;
                bool bIdSuccess = Read_String(ref id);
                bSuccess &= bIdSuccess;

                if (!bIdSuccess)
                {
                    AddErrorMessage("No id present");
                }
                else if (Context == null)
                {
                    AddErrorMessage("No context available to resolve asset {0} with id {1}", typeof(T).FullName, id);
                }
                else
                {
                    bool bResolveSuccess = Context.TryResolveAsset(id, out ioData);
                    bSuccess &= bResolveSuccess;
                    if (!bResolveSuccess)
                    {
                        AddErrorMessage("Unable to resolve asset {0} with id {1}", typeof(T).FullName, id);
                    }
                }
            }
            EndValue();

            return bSuccess;
        }

        /// <summary>
        /// Reads a value from the current node.
        /// </summary>
        private bool DoReadAsset<T>(string inKey, ref T ioData, FieldOptions inOptions) where T : class
        {
            bool bSuccess = BeginReadValue(inKey);

            if (IsMissing())
            {
                if ((inOptions & FieldOptions.Optional) != 0)
                {
                    ioData = null;
                    bSuccess = true;
                }
                else
                {
                    bSuccess &= false;
                }
            }
            else if (IsNull())
            {
                ioData = null;
                bSuccess = true;
            }
            else
            {
                string id = string.Empty;
                bool bIdSuccess = Read_String(ref id);
                bSuccess &= bIdSuccess;

                if (!bIdSuccess)
                {
                    AddErrorMessage("No id present");
                }
                else if (Context == null)
                {
                    AddErrorMessage("No context available to resolve asset {0} with id {1}", typeof(T).FullName, id);
                }
                else
                {
                    bool bResolveSuccess = Context.TryResolveAsset(id, out ioData);
                    bSuccess &= bResolveSuccess;
                    if (!bResolveSuccess)
                    {
                        AddErrorMessage("Unable to resolve asset {0} with id {1}", typeof(T).FullName, id);
                    }
                }
            }
            EndValue();

            return bSuccess;
        }

        #endregion

        #region Write

        /// <summary>
        /// Writes a value onto the current array.
        /// </summary>
        private void DoWriteAsset<T>(ref T ioData, FieldOptions inOptions) where T : class
        {
            if (ioData == null)
            {
                WriteNull();
                return;
            }

            string id = string.Empty;
            if (Context == null)
            {
                AddErrorMessage("No context available to resolve asset refs");
            }
            else if (!Context.TryGetAssetId(ioData, out id) || id == null)
            {
                AddErrorMessage("Unable to get id for asset");
                id = string.Empty;
            }

            BeginWriteValue();
            Write_String(ref id);
            EndValue();
        }

        /// <summary>
        /// Writes a value into the current object.
        /// </summary>
        private void DoWriteAsset<T>(string inKey, ref T ioData, FieldOptions inOptions) where T : class
        {
            if (ioData == null)
            {
                if ((inOptions & FieldOptions.Optional) == 0 || RequiresExplicitNull())
                    WriteNull(inKey);
                return;
            }

            string id = string.Empty;
            if (Context == null)
            {
                AddErrorMessage("No context available to resolve asset refs");
            }
            else if (!Context.TryGetAssetId(ioData, out id) || id == null)
            {
                AddErrorMessage("Unable to get id for asset");
                id = string.Empty;
            }

            BeginWriteValue(inKey, inOptions);
            Write_String(ref id);
            EndValue();
        }

        #endregion

        #endregion

        #region Serialize

        public void AssetRef<T>(string inKey, ref T ioData, FieldOptions inOptions = FieldOptions.None) where T : class
        {
            if (IsReading)
            {
                bool bSuccess = DoReadAsset<T>(inKey, ref ioData, inOptions);

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
                DoWriteAsset<T>(inKey, ref ioData, inOptions);
            }
        }

        #endregion

        #region Array

        public void AssetRefArray<T>(string inKey, ref List<T> ioArray, FieldOptions inOptions = FieldOptions.None) where T : class
        {
            if (IsReading)
            {
                bool bSuccess = BeginReadArray(inKey);
                {
                    if (IsMissing())
                    {
                        if ((inOptions & FieldOptions.Optional) != 0)
                        {
                            ioArray = null;
                        }
                        else
                        {
                            bSuccess = false;
                        }
                    }
                    else if (IsNull())
                    {
                        ioArray = null;
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
                            bSuccess &= DoReadAsset(i, ref obj, FieldOptions.None);
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
                    DoWriteAsset<T>(ref obj, FieldOptions.None);
                }
                EndArray();
            }
        }

        public void AssetRefArray<T>(string inKey, ref T[] ioArray, FieldOptions inOptions = FieldOptions.None) where T : class
        {
            if (IsReading)
            {
                bool bSuccess = BeginReadArray(inKey);
                {
                    if (IsMissing())
                    {
                        if ((inOptions & FieldOptions.Optional) != 0)
                        {
                            ioArray = null;
                        }
                        else
                        {
                            bSuccess = false;
                        }
                    }
                    else if (IsNull())
                    {
                        ioArray = null;
                    }
                    else
                    {
                        int nodeCount = GetChildCount();
                        if (ioArray != null)
                        {
                            System.Array.Resize(ref ioArray, nodeCount);
                        }
                        else
                            ioArray = new T[nodeCount];

                        for (int i = 0; i < nodeCount; ++i)
                        {
                            T obj = default(T);
                            bSuccess &= DoReadAsset(i, ref obj, FieldOptions.None);
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
                    DoWriteAsset<T>(ref obj, FieldOptions.None);
                }
                EndArray();
            }
        }

        #endregion

        #region Set

        public void AssetRefSet<T>(string inKey, ref HashSet<T> ioSet, FieldOptions inOptions = FieldOptions.None) where T : class
        {
            if (IsReading)
            {
                bool bSuccess = BeginReadArray(inKey);
                {
                    if (IsMissing())
                    {
                        if ((inOptions & FieldOptions.Optional) != 0)
                        {
                            ioSet = null;
                        }
                        else
                        {
                            bSuccess = false;
                        }
                    }
                    else if (IsNull())
                    {
                        ioSet = null;
                    }
                    else
                    {
                        int nodeCount = GetChildCount();
                        if (ioSet != null)
                        {
                            ioSet.Clear();
                        }
                        else
                            ioSet = new HashSet<T>();

                        for (int i = 0; i < nodeCount; ++i)
                        {
                            T obj = default(T);
                            bSuccess &= DoReadAsset(i, ref obj, FieldOptions.None);
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
                    DoWriteAsset<T>(ref obj, FieldOptions.None);
                }
                EndArray();
            }
        }

        #endregion

        #region Map

        public void AssetRefMap<T>(string inKey, ref Dictionary<string, T> ioMap, FieldOptions inOptions = FieldOptions.None) where T : class
        {
            if (IsReading)
            {
                bool bSuccess = BeginReadArray(inKey);
                {
                    if (IsMissing())
                    {
                        if ((inOptions & FieldOptions.Optional) != 0)
                        {
                            ioMap = null;
                        }
                        else
                        {
                            bSuccess = false;
                        }
                    }
                    else if (IsNull())
                    {
                        ioMap = null;
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
                                bSuccess &= DoRead(MAP_KEY, ref key, FieldOptions.None, this.Read_String);

                                T obj = default(T);
                                bSuccess &= DoReadAsset(MAP_VALUE, ref obj, FieldOptions.None);

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
                    DoWriteAsset(MAP_VALUE, ref obj, FieldOptions.None);

                    EndObject();
                }
                EndArray();
            }
        }

        public void AssetRefMap<T>(string inKey, ref Dictionary<int, T> ioMap, FieldOptions inOptions = FieldOptions.None) where T : class
        {
            if (IsReading)
            {
                bool bSuccess = BeginReadArray(inKey);
                {
                    if (IsMissing())
                    {
                        if ((inOptions & FieldOptions.Optional) != 0)
                        {
                            ioMap = null;
                        }
                        else
                        {
                            bSuccess = false;
                        }
                    }
                    else if (IsNull())
                    {
                        ioMap = null;
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
                                bSuccess &= DoReadAsset(MAP_VALUE, ref obj, FieldOptions.None);

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
                    DoWriteAsset(MAP_VALUE, ref obj, FieldOptions.None);

                    EndObject();
                }
                EndArray();
            }
        }

        #endregion
    }
}