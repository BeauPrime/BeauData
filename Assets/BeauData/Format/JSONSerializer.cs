/*
 * Copyright (C) 2017 - 2020. Filament Games, LLC. All rights reserved.
 * Author:  Autumn Beauchesne
 * Date:    3 May 2017
 * 
 * File:    BinarySerializer.cs
 * Purpose: Serializes to a JSON format.
 */
using System;
using System.Collections.Generic;
using System.IO;

namespace BeauData.Format
{
    internal sealed class JSONSerializer : Serializer
    {
        public const string FILE_EXTENSION = "json";
        public const string FILE_CONTENT_PREFIX = "{";

        private Stack<JSON> m_Stack = new Stack<JSON>();
        private JSON m_Current;
        private JSON m_Root;

        public JSONSerializer()
        {
            m_Root = JSON.CreateObject();
        }

        internal JSONSerializer(JSON inRoot)
        {
            m_Root = inRoot;
        }

        public override void Dispose()
        {
            if (m_Stack != null)
            {
                m_Stack.Clear();
                m_Stack = null;
            }

            m_Current = m_Root = null;
        }

        #region Group Abstraction

        protected override void BeginReadRoot(string inRootName)
        {
            m_Stack.Clear();
            m_Current = m_Root;

            JSON serializerVersion = m_Current[SERIALIZER_VERSION_KEY];
            if (serializerVersion != null)
                SerializerVersion = serializerVersion.AsUShort;
            else
                SerializerVersion = VERSION_INITIAL;
        }

        protected override void BeginWriteRoot(string inRootName)
        {
            m_Stack.Clear();
            m_Current = m_Root;

            m_Current[SERIALIZER_VERSION_KEY].AsUShort = SerializerVersion;
        }

        protected override void EndRoot()
        {
            m_Stack.Clear();
            m_Current = m_Root;
        }

        protected override void BeginWriteArray(string inKey)
        {
            m_Stack.Push(m_Current);
            JSON newObj = JSON.CreateArray();
            m_Current[inKey] = newObj;
            m_Current = newObj;
        }

        protected override bool BeginReadArray(string inKey)
        {
            m_Stack.Push(m_Current);
            m_Current = m_Current[inKey];
            return m_Current.IsArray;
        }

        protected override void EndArray()
        {
            m_Current = m_Stack.Pop();
        }

        protected override void BeginWriteObject()
        {
            m_Stack.Push(m_Current);
            JSON newObj = JSON.CreateObject();
            m_Current.Add(newObj);
            m_Current = newObj;
        }

        protected override void BeginWriteObject(string inKey)
        {
            m_Stack.Push(m_Current);
            JSON newObj = JSON.CreateObject();
            m_Current[inKey] = newObj;
            m_Current = newObj;
        }

        protected override bool BeginReadObject(int inIndex)
        {
            m_Stack.Push(m_Current);
            m_Current = m_Current[inIndex];
            return m_Current.IsObject;
        }

        protected override bool BeginReadObject(string inKey)
        {
            m_Stack.Push(m_Current);
            m_Current = m_Current[inKey];
            return m_Current.IsObject;
        }

        protected override void EndObject()
        {
            m_Current = m_Stack.Pop();
        }

        protected override void BeginWriteValue()
        {
            m_Stack.Push(m_Current);
            JSON newObj = JSON.CreateNull();
            m_Current.Add(newObj);
            m_Current = newObj;
        }

        protected override void BeginWriteValue(string inKey, FieldOptions inOptions)
        {
            m_Stack.Push(m_Current);
            JSON newObj = JSON.CreateNull();
            m_Current[inKey] = newObj;
            m_Current = newObj;
        }

        protected override bool BeginReadValue(int inIndex)
        {
            m_Stack.Push(m_Current);
            m_Current = m_Current[inIndex];
            return true;
        }

        protected override bool BeginReadValue(string inKey)
        {
            m_Stack.Push(m_Current);
            m_Current = m_Current[inKey];
            return true;
        }

        protected override void EndValue()
        {
            m_Current = m_Stack.Pop();
        }

        protected override void WriteNull()
        {
            m_Current.Add(JSON.CreateNull());
        }

        protected override void WriteNull(string inKey)
        {
            m_Current[inKey] = JSON.CreateNull();
        }

        protected override bool IsMissing()
        {
            return m_Current.IsUndefined;
        }

        protected override bool IsNull()
        {
            return m_Current.IsNull;
        }

        protected override int GetChildCount()
        {
            return m_Current.Count;
        }

        protected override void DeclareChildCount(int inCount) { }

        #endregion

        #region Read/Write

        protected override bool RequiresExplicitNull() { return false; }

        // Boolean
        protected override bool Read_Boolean(ref bool ioData)
        {
            ioData = m_Current.AsBool;
            return m_Current.IsBool;
        }

        protected override void Write_Boolean(ref bool ioData)
        {
            m_Current.AsBool = ioData;
        }

        // Byte
        protected override bool Read_Byte(ref byte ioData)
        {
            ioData = (byte) m_Current.AsInt;
            return m_Current.IsNumber;
        }

        protected override void Write_Byte(ref byte ioData)
        {
            m_Current.AsInt = (int) ioData;
        }

        // ByteArray
        protected override bool Read_ByteArray(ref byte[] ioData)
        {
            string base64 = m_Current.AsString;
            try
            {
                ioData = System.Convert.FromBase64String(base64);
                return true;
            }
            catch (Exception e)
            {
                AddErrorMessage("Unable to convert string to base64: {0}", e.Message);
                return false;
            }
        }

        protected override void Write_ByteArray(ref byte[] ioData)
        {
            string base64 = Convert.ToBase64String(ioData);
            m_Current.AsString = base64;
        }

        // Double
        protected override bool Read_Double(ref double ioData)
        {
            ioData = m_Current.AsDouble;
            return m_Current.IsNumber;
        }

        protected override void Write_Double(ref double ioData)
        {
            m_Current.AsDouble = ioData;
        }

        // Guid
        protected override bool Read_Guid(ref Guid ioData)
        {
            ioData = new Guid(m_Current.AsString);
            return m_Current.IsString;
        }

        protected override void Write_Guid(ref Guid ioData)
        {
            m_Current.AsString = ioData.ToString();
        }

        // Int16
        protected override bool Read_Int16(ref short ioData)
        {
            ioData = m_Current.AsShort;
            return m_Current.IsNumber;
        }

        protected override void Write_Int16(ref short ioData)
        {
            m_Current.AsShort = ioData;
        }

        // Int32
        protected override bool Read_Int32(ref int ioData)
        {
            ioData = m_Current.AsInt;
            return m_Current.IsNumber;
        }

        protected override void Write_Int32(ref int ioData)
        {
            m_Current.AsInt = ioData;
        }

        // Int64
        protected override bool Read_Int64(ref long ioData)
        {
            ioData = m_Current.AsLong;
            return m_Current.IsNumber;
        }

        protected override void Write_Int64(ref long ioData)
        {
            m_Current.AsLong = ioData;
        }

        // Single
        protected override bool Read_Single(ref float ioData)
        {
            ioData = m_Current.AsFloat;
            return m_Current.IsNumber;
        }

        protected override void Write_Single(ref float ioData)
        {
            m_Current.AsFloat = ioData;
        }

        // String
        protected override bool Read_String(ref string ioData)
        {
            ioData = m_Current.AsString;
            return m_Current.IsString;
        }

        protected override void Write_String(ref string ioData)
        {
            m_Current.AsString = ioData;
        }

        // UInt16
        protected override bool Read_UInt16(ref ushort ioData)
        {
            ioData = m_Current.AsUShort;
            return m_Current.IsNumber;
        }

        protected override void Write_UInt16(ref ushort ioData)
        {
            m_Current.AsUShort = ioData;
        }

        // UInt32
        protected override bool Read_UInt32(ref uint ioData)
        {
            ioData = m_Current.AsUInt;
            return m_Current.IsNumber;
        }

        protected override void Write_UInt32(ref uint ioData)
        {
            m_Current.AsUInt = ioData;
        }

        // UInt64
        protected override bool Read_UInt64(ref ulong ioData)
        {
            ioData = m_Current.AsULong;
            return m_Current.IsNumber;
        }

        protected override void Write_UInt64(ref ulong ioData)
        {
            m_Current.AsULong = ioData;
        }

        // FourCC
        protected override bool Read_FourCC(ref FourCC ioData)
        {
            return FourCC.TryParse(m_Current.AsString, out ioData);
        }

        protected override void Write_FourCC(ref FourCC ioData)
        {
            m_Current.AsString = ioData.ToString(true);
        }

        #endregion

        #region Output

        internal override string AsString(OutputOptions inOptions = OutputOptions.None)
        {
            if ((inOptions & OutputOptions.Base64) != 0)
                return m_Root.ToBase64();
            if ((inOptions & OutputOptions.PrettyPrint) != 0)
                return m_Root.ToString(3);
            return m_Root.ToString();
        }

        internal override void AsStream(Stream inStream, OutputOptions inOptions = OutputOptions.None)
        {
            using(StreamWriter writer = new StreamWriter(inStream))
            {
                writer.Write(AsString(inOptions));
            }
        }

        #endregion
    }
}