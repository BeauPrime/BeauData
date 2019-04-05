/*
 * Copyright (C) 2017 - 2019. Filament Games, LLC. All rights reserved.
 * Author:  Alex Beauchesne
 * Date:    3 May 2017
 * 
 * File:    BinarySerializer.cs
 * Purpose: Serializes to a binary format.
 */

using System;
using System.IO;

namespace BeauData.Format
{
    internal class BinarySerializer : Serializer
    {
        public const string FILE_EXTENSION = "fbin";
        public const string FILE_CONTENT_PREFIX = "FBIN";

        static private readonly byte[] FILE_CONTENT_PREFIX_BYTES = new byte[]
        {
            (byte) 70, (byte) 66, (byte) 73, (byte) 78
        };

        private const byte FILE_CONTENT_TYPE_STREAM = (byte) 0x21;
        private const string FILE_CONTENT_PREFIX_BASE64 = "FBIN\"";
        private const byte FILE_CONTENT_TYPE_BASE64 = (byte) 0x22;

        protected enum FieldType : byte
        {
            NULL = 0,

            BOOL = 1,
            BYTE = 2,
            DOUBLE = 3,
            INT16 = 4,
            INT32 = 5,
            INT64 = 6,
            SINGLE = 7,
            STRING = 8,
            UINT16 = 9,
            UINT32 = 10,
            UINT64 = 11,
            BINARY = 12,
            GUID = 13,
            FOURCC = 14,

            ARRAY = 80,
            OBJECT = 81,

            DEFAULT = 128
        }

        protected struct FrameData
        {
            public FieldType Type;
            public object Value;

            public void Set(FieldType inType, object inValue = null)
            {
                Type = inType;
                Value = inValue;
            }
        }

        protected FrameData m_Current;

        protected MemoryStream m_Stream;
        protected BinaryWriter m_Writer;
        protected BinaryReader m_Reader;

        internal BinarySerializer()
        {
            m_Stream = new MemoryStream();
        }

        internal BinarySerializer(MemoryStream inStream)
        {
            m_Stream = inStream;
        }

        public override bool IsBinary()
        {
            return true;
        }

        public override void Dispose()
        {
            if (m_Writer != null)
            {
                m_Writer.Close();
                m_Writer = null;
            }
            if (m_Reader != null)
            {
                m_Reader.Close();
                m_Reader = null;
            }

            if (m_Stream != null)
            {
                m_Stream.Dispose();
                m_Stream = null;
            }
        }

        protected void ReadNextFrame()
        {
            m_Current.Type = (FieldType) m_Reader.ReadByte();

            switch (m_Current.Type)
            {
                case FieldType.ARRAY:
                    m_Current.Value = m_Reader.ReadInt32();
                    break;

                case FieldType.BOOL:
                    m_Current.Value = m_Reader.ReadBoolean();
                    break;

                case FieldType.BYTE:
                    m_Current.Value = m_Reader.ReadByte();
                    break;

                case FieldType.DOUBLE:
                    m_Current.Value = m_Reader.ReadDouble();
                    break;

                case FieldType.GUID:
                    m_Current.Value = new Guid(m_Reader.ReadString());
                    break;

                case FieldType.INT16:
                    m_Current.Value = m_Reader.ReadInt16();
                    break;

                case FieldType.INT32:
                    m_Current.Value = m_Reader.ReadInt32();
                    break;

                case FieldType.INT64:
                    m_Current.Value = m_Reader.ReadInt64();
                    break;

                case FieldType.SINGLE:
                    m_Current.Value = m_Reader.ReadSingle();
                    break;

                case FieldType.STRING:
                    m_Current.Value = m_Reader.ReadString();
                    break;

                case FieldType.UINT16:
                    m_Current.Value = m_Reader.ReadUInt16();
                    break;

                case FieldType.UINT32:
                    m_Current.Value = m_Reader.ReadUInt32();
                    break;

                case FieldType.UINT64:
                    m_Current.Value = m_Reader.ReadUInt64();
                    break;

                case FieldType.FOURCC:
                    m_Current.Value = m_Reader.ReadFourCC();
                    break;

                case FieldType.BINARY:
                    int length = m_Reader.ReadInt32();
                    m_Current.Value = m_Reader.ReadBytes(length);
                    break;

                case FieldType.NULL:
                case FieldType.OBJECT:
                    break;

                default:
                    AddErrorMessage("Unknown binary object type {0}", m_Current.Type);
                    break;
            }
        }

        protected void ReadNextFrameIgnoreHeader(FieldType inType)
        {
            m_Current.Type = inType;

            switch (m_Current.Type)
            {
                case FieldType.ARRAY:
                    m_Current.Value = m_Reader.ReadInt32();
                    break;

                case FieldType.BOOL:
                    m_Current.Value = m_Reader.ReadBoolean();
                    break;

                case FieldType.BYTE:
                    m_Current.Value = m_Reader.ReadByte();
                    break;

                case FieldType.DOUBLE:
                    m_Current.Value = m_Reader.ReadDouble();
                    break;

                case FieldType.GUID:
                    m_Current.Value = new Guid(m_Reader.ReadString());
                    break;

                case FieldType.INT16:
                    m_Current.Value = m_Reader.ReadInt16();
                    break;

                case FieldType.INT32:
                    m_Current.Value = m_Reader.ReadInt32();
                    break;

                case FieldType.INT64:
                    m_Current.Value = m_Reader.ReadInt64();
                    break;

                case FieldType.SINGLE:
                    m_Current.Value = m_Reader.ReadSingle();
                    break;

                case FieldType.STRING:
                    m_Current.Value = m_Reader.ReadString();
                    break;

                case FieldType.UINT16:
                    m_Current.Value = m_Reader.ReadUInt16();
                    break;

                case FieldType.UINT32:
                    m_Current.Value = m_Reader.ReadUInt32();
                    break;

                case FieldType.UINT64:
                    m_Current.Value = m_Reader.ReadUInt64();
                    break;

                case FieldType.FOURCC:
                    m_Current.Value = m_Reader.ReadFourCC();
                    break;

                case FieldType.BINARY:
                    int length = m_Reader.ReadInt32();
                    m_Current.Value = m_Reader.ReadBytes(length);
                    break;

                case FieldType.NULL:
                case FieldType.OBJECT:
                    break;

                default:
                    AddErrorMessage("Unknown binary object type {0}", m_Current.Type);
                    break;
            }
        }

        protected void WriteTypeHeader(FieldType inType)
        {
            m_Current.Set(inType);
            m_Writer.Write((byte) inType);
        }

        protected void WriteTypeHeader<T>(FieldType inType, ref T ioData)
        {
            m_Current.Set(inType, ioData);
            m_Writer.Write((byte) inType);
        }

        protected bool ReadType<T>(FieldType inType, ref T ioData)
        {
            if (m_Current.Type != inType)
            {
                AddErrorMessage("Expected {0}, read {1}", inType, m_Current.Type);
                return false;
            }

            ioData = (T) m_Current.Value;
            return true;
        }

        #region Group abstraction

        protected override void BeginReadRoot(string inRootName)
        {
            m_Reader = new BinaryReader(m_Stream);

            byte[] prefix = m_Reader.ReadBytes(FILE_CONTENT_PREFIX_BYTES.Length);
            for (int i = 0; i < prefix.Length; ++i)
            {
                if (prefix[i] != FILE_CONTENT_PREFIX_BYTES[i])
                {
                    AddErrorMessage("File format doesn't match - expected {0} at start", FILE_CONTENT_PREFIX);
                    break;
                }
            }

            byte streamType = m_Reader.ReadByte();
            if (streamType == FILE_CONTENT_TYPE_STREAM)
            {
                ReadNextFrame();
            }
            else if (streamType == FILE_CONTENT_TYPE_BASE64)
            {
                //Decompress from base64
                int bufferLength = (int) (m_Stream.Length - m_Stream.Position);
                byte[] buffer = Convert.FromBase64String(System.Text.Encoding.UTF8.GetString(m_Stream.GetBuffer(), (int) m_Stream.Position, bufferLength));

                m_Reader.Close();
                m_Stream.Close();

                m_Stream = new MemoryStream(buffer);
                m_Reader = new BinaryReader(m_Stream);

                ReadNextFrame();
            }
            else
            {
                m_Stream.Seek(4, SeekOrigin.Begin);
                ReadNextFrame();
            }
        }

        protected override void BeginWriteRoot(string inRootName)
        {
            m_Writer = new BinaryWriter(m_Stream);

            BeginWriteObject();
        }

        protected override void EndRoot()
        {
            EndObject();
        }

        protected override void BeginWriteArray(string inKey)
        {
            WriteTypeHeader(FieldType.ARRAY);
        }

        protected override bool BeginReadArray(string inKey)
        {
            ReadNextFrame();
            return m_Current.Type == FieldType.ARRAY;
        }

        protected override void EndArray()
        {

        }

        protected override void BeginWriteObject()
        {
            WriteTypeHeader(FieldType.OBJECT);
        }

        protected override void BeginWriteObject(string inKey)
        {
            WriteTypeHeader(FieldType.OBJECT);
        }

        protected override bool BeginReadObject(int inIndex)
        {
            ReadNextFrame();
            return m_Current.Type == FieldType.OBJECT;
        }

        protected override bool BeginReadObject(string inKey)
        {
            ReadNextFrame();
            return m_Current.Type == FieldType.OBJECT;
        }

        protected override void EndObject() { }

        protected override void BeginWriteValue() { }

        protected override void BeginWriteValue(string inKey, FieldOptions inOptions) { }

        protected override bool BeginReadValue(int inIndex)
        {
            ReadNextFrame();
            return m_Current.Type != FieldType.OBJECT && m_Current.Type != FieldType.ARRAY;
        }

        protected override bool BeginReadValue(string inKey)
        {
            ReadNextFrame();
            return m_Current.Type != FieldType.OBJECT && m_Current.Type != FieldType.ARRAY;
        }

        protected override void EndValue() { }

        protected override void WriteNull()
        {
            WriteTypeHeader(FieldType.NULL);
        }

        protected override void WriteNull(string inKey)
        {
            WriteTypeHeader(FieldType.NULL);
        }

        protected override bool IsMissing()
        {
            return false;
        }

        protected override bool IsNull()
        {
            return m_Current.Type == FieldType.NULL;
        }

        protected override int GetChildCount()
        {
            if (m_Current.Type != FieldType.ARRAY)
            {
                AddErrorMessage("Attempting to read child count of non array type {0}", m_Current.Type);
                return 0;
            }
            return (int) m_Current.Value;
        }

        protected override void DeclareChildCount(int inCount)
        {
            m_Current.Value = inCount;
            m_Writer.Write(inCount);
        }

        #endregion

        #region Read/Write

        protected override bool RequiresExplicitNull() { return true; }

        // Boolean
        protected override bool Read_Boolean(ref bool ioData)
        {
            return ReadType(FieldType.BOOL, ref ioData);
        }

        protected override void Write_Boolean(ref bool ioData)
        {
            WriteTypeHeader(FieldType.BOOL, ref ioData);
            m_Writer.Write(ioData);
        }

        // Byte
        protected override bool Read_Byte(ref byte ioData)
        {
            return ReadType(FieldType.BYTE, ref ioData);
        }

        protected override void Write_Byte(ref byte ioData)
        {
            WriteTypeHeader(FieldType.BYTE, ref ioData);
            m_Writer.Write(ioData);
        }

        // ByteArray
        protected override bool Read_ByteArray(ref byte[] ioData)
        {
            return ReadType(FieldType.BINARY, ref ioData);
        }

        protected override void Write_ByteArray(ref byte[] ioData)
        {
            WriteTypeHeader(FieldType.BINARY, ref ioData);
            m_Writer.Write(ioData.Length);
            m_Writer.Write(ioData);
        }

        // Double
        protected override bool Read_Double(ref double ioData)
        {
            return ReadType(FieldType.DOUBLE, ref ioData);
        }

        protected override void Write_Double(ref double ioData)
        {
            WriteTypeHeader(FieldType.DOUBLE, ref ioData);
            m_Writer.Write(ioData);
        }

        //GUID
        protected override bool Read_Guid(ref Guid ioData)
        {
            return ReadType(FieldType.GUID, ref ioData);
        }

        protected override void Write_Guid(ref Guid ioData)
        {
            WriteTypeHeader(FieldType.GUID, ref ioData);
            m_Writer.Write(ioData.ToString());
        }

        // Int16
        protected override bool Read_Int16(ref short ioData)
        {
            return ReadType(FieldType.INT16, ref ioData);
        }

        protected override void Write_Int16(ref short ioData)
        {
            WriteTypeHeader(FieldType.INT16, ref ioData);
            m_Writer.Write(ioData);
        }

        // Int32
        protected override bool Read_Int32(ref int ioData)
        {
            return ReadType(FieldType.INT32, ref ioData);
        }

        protected override void Write_Int32(ref int ioData)
        {
            WriteTypeHeader(FieldType.INT32, ref ioData);
            m_Writer.Write(ioData);
        }

        // Int64
        protected override bool Read_Int64(ref long ioData)
        {
            return ReadType(FieldType.INT64, ref ioData);
        }

        protected override void Write_Int64(ref long ioData)
        {
            WriteTypeHeader(FieldType.INT64, ref ioData);
            m_Writer.Write(ioData);
        }

        // Single
        protected override bool Read_Single(ref float ioData)
        {
            return ReadType(FieldType.SINGLE, ref ioData);
        }

        protected override void Write_Single(ref float ioData)
        {
            WriteTypeHeader(FieldType.SINGLE, ref ioData);
            m_Writer.Write(ioData);
        }

        // String
        protected override bool Read_String(ref string ioData)
        {
            return ReadType(FieldType.STRING, ref ioData);
        }

        protected override void Write_String(ref string ioData)
        {
            WriteTypeHeader(FieldType.STRING, ref ioData);
            m_Writer.Write(ioData);
        }

        // Uint16
        protected override bool Read_UInt16(ref ushort ioData)
        {
            return ReadType(FieldType.UINT16, ref ioData);
        }

        protected override void Write_UInt16(ref ushort ioData)
        {
            WriteTypeHeader(FieldType.UINT16, ref ioData);
            m_Writer.Write(ioData);
        }

        // Uint32
        protected override bool Read_UInt32(ref uint ioData)
        {
            return ReadType(FieldType.UINT32, ref ioData);
        }

        protected override void Write_UInt32(ref uint ioData)
        {
            WriteTypeHeader(FieldType.UINT32, ref ioData);
            m_Writer.Write(ioData);
        }

        // Uint64
        protected override bool Read_UInt64(ref ulong ioData)
        {
            return ReadType(FieldType.UINT64, ref ioData);
        }

        protected override void Write_UInt64(ref ulong ioData)
        {
            WriteTypeHeader(FieldType.UINT64, ref ioData);
            m_Writer.Write(ioData);
        }

        // FourCC
        protected override bool Read_FourCC(ref FourCC ioData)
        {
            return ReadType(FieldType.FOURCC, ref ioData);
        }

        protected override void Write_FourCC(ref FourCC ioData)
        {
            WriteTypeHeader(FieldType.FOURCC, ref ioData);
            m_Writer.Write(ioData);
        }

        #endregion

        #region Output

        internal override void AsStream(Stream inStream, OutputOptions inOptions = OutputOptions.None)
        {
            m_Writer.Flush();
            m_Writer.Close();

            byte[] buffer = m_Stream.ToArray();
            inStream.Write(FILE_CONTENT_PREFIX_BYTES, 0, FILE_CONTENT_PREFIX_BYTES.Length);
            inStream.WriteByte(FILE_CONTENT_TYPE_STREAM);
            inStream.Write(buffer, 0, buffer.Length);
        }

        internal override string AsString(OutputOptions inOptions = OutputOptions.None)
        {
            m_Writer.Flush();
            m_Writer.Close();

            byte[] buffer = m_Stream.ToArray();
            return FILE_CONTENT_PREFIX_BASE64 + Convert.ToBase64String(buffer);
        }

        #endregion
    }
}