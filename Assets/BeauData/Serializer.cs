/*
 * Copyright (C) 2017 - 2019. Filament Games, LLC. All rights reserved.
 * Author:  Alex Beauchesne
 * Date:    3 May 2017
 * 
 * File:    Serializer.cs
 * Purpose: Base class for serialization interface.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BeauData
{
    public abstract partial class Serializer : IDisposable
    {
        private const string MAP_KEY = "key";
        private const string MAP_VALUE = "value";

        protected Serializer()
        {
            ObjectVersion = 1;
        }

        public bool IsReading { get; private set; }
        public bool IsWriting { get { return !IsReading; } }

        /// <summary>
        /// Version number for the currently serializing ISerializedObject.
        /// </summary>
        public ushort ObjectVersion { get; private set; }

        /// <summary>
        /// Returns if this serializer is binary.
        /// </summary>
        public virtual bool IsBinary()
        {
            return false;
        }

        /// <summary>
        /// Disposes of any owned resources.
        /// </summary>
        public abstract void Dispose();

        #region Group abstraction

        protected abstract bool RequiresExplicitNull();

        protected abstract void BeginWriteArray(string inKey);
        protected abstract bool BeginReadArray(string inKey);
        protected abstract void EndArray();

        protected abstract void BeginReadRoot(string inRootName);
        protected abstract void BeginWriteRoot(string inRootName);
        protected abstract void EndRoot();

        protected abstract void BeginWriteObject();
        protected abstract void BeginWriteObject(string inKey);
        protected abstract bool BeginReadObject(int inIndex);
        protected abstract bool BeginReadObject(string inKey);
        protected abstract void EndObject();

        protected abstract void BeginWriteValue();
        protected abstract void BeginWriteValue(string inKey, FieldOptions inOptions);
        protected abstract bool BeginReadValue(int inIndex);
        protected abstract bool BeginReadValue(string inKey);
        protected abstract void EndValue();

        protected abstract void WriteNull(string inKey);
        protected abstract void WriteNull();

        protected abstract bool IsNull();
        protected abstract bool IsMissing();

        protected abstract int GetChildCount();
        protected abstract void DeclareChildCount(int inCount);

        protected delegate bool ReadFunc<T>(ref T ioData);
        protected delegate void WriteFunc<T>(ref T ioData);

        #endregion

        #region Read/Write

        #region Read

        /// <summary>
        /// Reads a value from the current node.
        /// </summary>
        private bool DoRead<T>(int inIndex, ref T ioData, FieldOptions inOptions, ReadFunc<T> inReader)
        {
            bool bSuccess = BeginReadValue(inIndex);

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
                bSuccess &= inReader(ref ioData);
            }
            EndObject();

            return bSuccess;
        }

        /// <summary>
        /// Reads a value from the current node, substituting the default if needed.
        /// </summary>
        private bool DoRead<T>(int inIndex, ref T ioData, T inDefault, FieldOptions inOptions, ReadFunc<T> inReader)
        {
            bool bSuccess = BeginReadValue(inIndex);

            if (IsMissing() || IsNull())
            {
                ioData = inDefault;
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
        private bool DoRead<T>(string inKey, ref T ioData, FieldOptions inOptions, ReadFunc<T> inReader)
        {
            bool bSuccess = BeginReadValue(inKey);

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
                bSuccess &= inReader(ref ioData);
            }
            EndObject();

            return bSuccess;
        }

        /// <summary>
        /// Reads a value from the current node, substituting the default if needed.
        /// </summary>
        private bool DoRead<T>(string inKey, ref T ioData, T inDefault, FieldOptions inOptions, ReadFunc<T> inReader)
        {
            bool bSuccess = BeginReadValue(inKey);

            if (IsMissing() || IsNull())
            {
                ioData = inDefault;
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
        private void DoWrite<T>(ref T ioData, FieldOptions inOptions, WriteFunc<T> inWriter)
        {
            if (ioData != null)
            {
                BeginWriteValue();
                inWriter(ref ioData);
                EndValue();
            }
            else
            {
                WriteNull();
            }
        }

        /// <summary>
        /// Writes a value onto the current array, substituting null if default.
        /// </summary>
        private void DoWrite<T>(ref T ioData, T inDefault, FieldOptions inOptions, WriteFunc<T> inWriter)
        {
            if (ioData != null && !ioData.Equals(inDefault))
            {
                BeginWriteValue();
                inWriter(ref ioData);
                EndValue();
            }
            else
            {
                WriteNull();
            }
        }

        /// <summary>
        /// Writes a value into the current object.
        /// </summary>
        private void DoWrite<T>(string inKey, ref T ioData, FieldOptions inOptions, WriteFunc<T> inWriter)
        {
            if (ioData != null)
            {
                BeginWriteValue(inKey, inOptions);
                inWriter(ref ioData);
                EndValue();
            }
            else if (RequiresExplicitNull())
            {
                WriteNull(inKey);
            }
        }

        /// <summary>
        /// Writes a value into the current object, substituting null if default.
        /// </summary>
        private void DoWrite<T>(string inKey, ref T ioData, T inDefault, FieldOptions inOptions, WriteFunc<T> inWriter)
        {
            if (ioData != null && !ioData.Equals(inDefault))
            {
                BeginWriteValue(inKey, inOptions);
                inWriter(ref ioData);
                EndValue();
            }
            else if (RequiresExplicitNull())
            {
                WriteNull(inKey);
            }
        }

        #endregion

        #endregion

        #region Serialize

        /// <summary>
        /// Serializes a value on the current object.
        /// </summary>
        private void DoSerialize<T>(string inKey, ref T ioData, FieldOptions inOptions, ReadFunc<T> inReader, WriteFunc<T> inWriter)
        {
            if (IsReading)
            {
                bool bSuccess = DoRead<T>(inKey, ref ioData, inOptions, inReader);

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
                DoWrite<T>(inKey, ref ioData, inOptions, inWriter);
            }
        }

        /// <summary>
        /// Serializes a value on the current object with the given defaults.
        /// </summary>
        private void DoSerialize<T>(string inKey, ref T ioData, T inDefault, FieldOptions inOptions, ReadFunc<T> inReader, WriteFunc<T> inWriter)
        {
            if (IsReading)
            {
                bool bSuccess = DoRead<T>(inKey, ref ioData, inDefault, inOptions, inReader);

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
                DoWrite<T>(inKey, ref ioData, inDefault, inOptions, inWriter);
            }
        }

        #endregion

        #region Array

        /// <summary>
        /// Serializes an array on the current object.
        /// </summary>
        private void DoArray<T>(string inKey, ref List<T> ioArray, FieldOptions inOptions, ReadFunc<T> inReader, WriteFunc<T> inWriter)
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
                            bSuccess &= DoRead(i, ref obj, FieldOptions.None, inReader);
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
                    DoWrite<T>(ref obj, FieldOptions.None, inWriter);
                }
                EndArray();
            }
        }

        /// <summary>
        /// Serializes an array on the current object.
        /// </summary>
        private void DoArray<T>(string inKey, ref T[] ioArray, FieldOptions inOptions, ReadFunc<T> inReader, WriteFunc<T> inWriter)
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
                            bSuccess = true;
                        }
                        else
                        {
                            bSuccess = false;
                        }
                    }
                    else if (IsNull())
                    {
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
                            bSuccess &= DoRead(i, ref obj, FieldOptions.None, inReader);
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
                    DoWrite<T>(ref obj, FieldOptions.None, inWriter);
                }
                EndArray();
            }
        }

        #endregion

        #region Set

        /// <summary>
        /// Serializes a set on the current object.
        /// </summary>
        private void DoSet<T>(string inKey, ref HashSet<T> ioArray, FieldOptions inOptions, ReadFunc<T> inReader, WriteFunc<T> inWriter)
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
                    }
                    else
                    {
                        int nodeCount = GetChildCount();
                        if (ioArray != null)
                        {
                            ioArray.Clear();
                        }
                        else
                            ioArray = new HashSet<T>();

                        for (int i = 0; i < nodeCount; ++i)
                        {
                            T obj = default(T);
                            bSuccess &= DoRead(i, ref obj, FieldOptions.None, inReader);
                            ioArray.Add(obj);
                        }
                    }
                }
                EndArray();

                if (!bSuccess)
                    AddErrorMessage("Unable to read set '{0}'.", inKey);

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
                foreach (var item in ioArray)
                {
                    T obj = item;
                    DoWrite<T>(ref obj, FieldOptions.None, inWriter);
                }
                EndArray();
            }
        }

        #endregion

        #region Map

        /// <summary>
        /// Serializes a map on the current object.
        /// </summary>
        private void DoMap<T>(string inKey, ref Dictionary<string, T> ioMap, FieldOptions inOptions, ReadFunc<T> inReader, WriteFunc<T> inWriter)
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
                                bSuccess &= DoRead(MAP_KEY, ref key, FieldOptions.None, this.Read_String);

                                T obj = default(T);
                                bSuccess &= DoRead(MAP_VALUE, ref obj, FieldOptions.None, inReader);

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
                    DoWrite(MAP_VALUE, ref obj, FieldOptions.None, inWriter);

                    EndObject();
                }
                EndArray();
            }
        }

        /// <summary>
        /// Serializes a map on the current object.
        /// </summary>
        private void DoMap<T>(string inKey, ref Dictionary<int, T> ioMap, FieldOptions inOptions, ReadFunc<T> inReader, WriteFunc<T> inWriter)
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
                                bSuccess &= DoRead(MAP_KEY, ref key, FieldOptions.None, this.Read_Int32);

                                T obj = default(T);
                                bSuccess &= DoRead(MAP_VALUE, ref obj, FieldOptions.None, inReader);

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
                    DoWrite(MAP_VALUE, ref obj, FieldOptions.None, inWriter);

                    EndObject();
                }
                EndArray();
            }
        }

        #endregion

        #region Root

        /// <summary>
        /// Reads this object from the given serializer.
        /// </summary>
        private void Read<T>(ref T ioObject) where T : ISerializedObject
        {
            IsReading = true;

            BeginReadRoot(typeof(T).FullName);
            Read_Object<T>(ref ioObject);
            EndRoot();
        }

        /// <summary>
        /// Writes this object to the given serializer.
        /// </summary>
        private void Write<T>(ref T ioObject) where T : ISerializedObject
        {
            IsReading = false;

            BeginWriteRoot(typeof(T).FullName);
            Write_Object<T>(ref ioObject);
            EndRoot();
        }

        #endregion

        #region Group

        /// <summary>
        /// Begins serializing a group of data.
        /// </summary>
        public void BeginGroup(string inKey)
        {
            if (IsReading)
            {
                bool bSuccess = BeginReadObject(inKey);
                if (!bSuccess)
                    AddErrorMessage("Unable to read group '{0}'.", inKey);
            }
            else
                BeginWriteObject(inKey);
        }

        /// <summary>
        /// Ends serialization of the previously declared group.
        /// </summary>
        public void EndGroup()
        {
            EndObject();
        }

        #endregion

        #region Output

        internal abstract string AsString(OutputOptions inOptions = OutputOptions.None);
        internal abstract void AsStream(Stream inStream, OutputOptions inOptions = OutputOptions.None);

        #endregion

        #region Error Checking

        private StringBuilder m_ErrorString = new StringBuilder();

        /// <summary>
        /// Indicates if serialization encountered any errors.
        /// </summary>
        public bool HasErrors { get; private set; }

        /// <summary>
        /// Summary of serialization errors.
        /// </summary>
        public string Errors
        {
            get { return m_ErrorString.ToString(); }
        }

        /// <summary>
        /// Adds an error message.
        /// </summary>
        protected void AddErrorMessage(string inMessage, params object[] inArgs)
        {
            HasErrors = true;

            if (m_ErrorString.Length > 0)
                m_ErrorString.Append('\n');
            m_ErrorString.AppendFormat(inMessage, inArgs);
        }

        #endregion
    }
}