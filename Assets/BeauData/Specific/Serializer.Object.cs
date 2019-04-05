using System;
using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        private const string TYPE_KEY = "__type";
        private const string VERSION_KEY = "__version";

        private bool Read_Object<T>(ref T inObject) where T : ISerializedObject
        {
            string typeName = null;
            bool typeSuccess = DoRead(TYPE_KEY, ref typeName, null, FieldOptions.PreferAttribute, Read_String);

            if (!typeSuccess)
                AddErrorMessage("Unable to read object type info");

            Type objectType = typeof(T);
            if (!string.IsNullOrEmpty(typeName))
                objectType = TypeUtility.NameToType(typeName);

            if (inObject == null || inObject.GetType().TypeHandle.Value != objectType.TypeHandle.Value)
            {
                inObject = (T)TypeUtility.Instantiate(objectType, this);
            }

            ushort version = 1;
            bool versionSuccess = DoRead(VERSION_KEY, ref version, (ushort)1, FieldOptions.PreferAttribute, Read_UInt16);
            
            if (!versionSuccess)
                AddErrorMessage("Unable to read object version info");

            ushort prevVersion = ObjectVersion;
            ObjectVersion = version;

            int prevErrorLength = m_ErrorString.Length;
            inObject.Serialize(this);

            ObjectVersion = prevVersion;
            return typeSuccess && versionSuccess && (m_ErrorString.Length == prevErrorLength);
        }

        private void Write_Object<T>(ref T inObject) where T : ISerializedObject
        {
            // Make sure to write out the subclass name if we need to
            if (typeof(T).TypeHandle.Value != inObject.GetType().TypeHandle.Value)
            {
                string typeName = TypeUtility.TypeToName(inObject.GetType());
                DoWrite(TYPE_KEY, ref typeName, FieldOptions.PreferAttribute, Write_String);
            }
            else if (RequiresExplicitNull())
            {
                WriteNull(TYPE_KEY);
            }

            ushort version = 1;
            if (inObject is ISerializedVersion)
                version = ((ISerializedVersion)inObject).Version;

            if (version != 1)
            {
                DoWrite(VERSION_KEY, ref version, FieldOptions.PreferAttribute, Write_UInt16);
            }
            else if (RequiresExplicitNull())
            {
                WriteNull(VERSION_KEY);
            }

            ushort prevVersion = ObjectVersion;
            ObjectVersion = version;

            inObject.Serialize(this);

            ObjectVersion = prevVersion;
        }

        public void Object<T>(string inKey, ref T ioData, FieldOptions inOptions = FieldOptions.None) where T : ISerializedObject
        {
            if (IsReading)
            {
                bool bSuccess = DoReadObject<T>(inKey, ref ioData, inOptions);

                if (!bSuccess)
                    AddErrorMessage("Unable to read object '{0}'.", inKey);
                return;
            }

            DoWriteObject<T>(inKey, ref ioData, inOptions);
        }

        public void ObjectArray<T>(string inKey, ref List<T> ioArray, FieldOptions inOptions = FieldOptions.None) where T : ISerializedObject
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
                            ioArray.Clear();
                        else
                            ioArray = new List<T>(nodeCount);

                        for (int i = 0; i < nodeCount; ++i)
                        {
                            T obj = default(T);
                            bSuccess &= DoReadObject(i, ref obj, FieldOptions.None);
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
                    DoWriteObject<T>(ref obj);
                }
                EndArray();
            }
        }

        public void ObjectArray<T>(string inKey, ref T[] ioArray, FieldOptions inOptions = FieldOptions.None) where T : ISerializedObject
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
                            bSuccess &= DoReadObject(i, ref obj, FieldOptions.None);
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
                    DoWriteObject<T>(ref obj);
                }
                EndArray();
            }
        }

        public void ObjectSet<T>(string inKey, ref HashSet<T> ioSet, FieldOptions inOptions = FieldOptions.None) where T : ISerializedObject
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
                            bSuccess &= DoReadObject(i, ref obj, FieldOptions.None);
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
                    DoWriteObject<T>(ref obj);
                }
                EndArray();
            }
        }

        public void ObjectMap<T>(string inKey, ref Dictionary<string, T> ioMap, FieldOptions inOptions = FieldOptions.None) where T : ISerializedObject
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
                                bSuccess &= DoReadObject(MAP_VALUE, ref obj, FieldOptions.None);

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
                    DoWriteObject(MAP_VALUE, ref obj, FieldOptions.None);

                    EndObject();
                }
                EndArray();
            }
        }

        public void ObjectMap<T>(string inKey, ref Dictionary<int, T> ioMap, FieldOptions inOptions = FieldOptions.None) where T : ISerializedObject
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
                                bSuccess &= DoReadObject(MAP_VALUE, ref obj, FieldOptions.None);

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
                    DoWriteObject(MAP_VALUE, ref obj, FieldOptions.None);

                    EndObject();
                }
                EndArray();
            }
        }

        #region Read/Write

        /// <summary>
        /// Reads an object from the current node.
        /// </summary>
        private bool DoReadObject<T>(int inIndex, ref T ioData, FieldOptions inOptions) where T : ISerializedObject
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
                bSuccess &= Read_Object(ref ioData);
            }
            EndObject();

            return bSuccess;
        }

        /// <summary>
        /// Reads an object from the current node.
        /// </summary>
        private bool DoReadObject<T>(string inKey, ref T ioData, FieldOptions inOptions) where T : ISerializedObject
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
                bSuccess &= Read_Object(ref ioData);
            }
            EndObject();

            return bSuccess;
        }

        /// <summary>
        /// Writes an object onto the current array.
        /// </summary>
        private void DoWriteObject<T>(ref T ioData) where T : ISerializedObject
        {
            if (ioData == null)
            {
                WriteNull();
                return;
            }
            BeginWriteObject();
            Write_Object(ref ioData);
            EndValue();
        }

        /// <summary>
        /// Writes an object into the current object.
        /// </summary>
        private void DoWriteObject<T>(string inKey, ref T ioData, FieldOptions inOptions) where T : ISerializedObject
        {
            if (ioData == null)
            {
                if ((inOptions & FieldOptions.Optional) == 0 || RequiresExplicitNull())
                    WriteNull(inKey);
                return;
            }

            BeginWriteObject(inKey);
            Write_Object(ref ioData);
            EndValue();
        }

        #endregion
    }
}
