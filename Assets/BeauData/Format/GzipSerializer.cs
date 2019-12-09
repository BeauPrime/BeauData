/*
 * Copyright (C) 2017 - 2019. Filament Games, LLC. All rights reserved.
 * Author:  Alex Beauchesne
 * Date:    3 May 2017
 * 
 * File:    GzipSerializer.cs
 * Purpose: Serializes to a gziped binary format.
 */

using System;
using System.IO;
#if UNITY_WEBGL
using System.IO.Compression;
#else
using Ionic.Zlib;
#endif

namespace BeauData.Format
{
    internal sealed class GZIPSerializer : BinarySerializer
    {
        public new const string FILE_EXTENSION = "fbgz";
        public new const string FILE_CONTENT_PREFIX = "FBGZ";

        static private readonly byte[] FILE_CONTENT_PREFIX_BYTES = new byte[]
        {
            (byte) 70, (byte) 66, (byte) 71, (byte) 90
        };

        private const byte FILE_CONTENT_TYPE_STREAM = (byte) 0x21;
        private const string FILE_CONTENT_PREFIX_BASE64 = "FBGZ\"";
        private const byte FILE_CONTENT_TYPE_BASE64 = (byte) 0x22;

        private GZipStream m_Gzip;

        public GZIPSerializer() : base()
        { }

        internal GZIPSerializer(MemoryStream inStream) : base(inStream)
        { }

        public override void Dispose()
        {
            if (m_Gzip != null)
            {
                m_Gzip.Dispose();
                m_Gzip = null;
            }

            base.Dispose();
        }

        #region Group Abstraction

        protected override void BeginReadRoot(string inRootName)
        {
            byte[] prefix = new byte[FILE_CONTENT_PREFIX_BYTES.Length];
            m_Stream.Read(prefix, 0, prefix.Length);
            for (int i = 0; i < prefix.Length; ++i)
            {
                if (prefix[i] != FILE_CONTENT_PREFIX_BYTES[i])
                {
                    AddErrorMessage("File format doesn't match - expected {0} at start", FILE_CONTENT_PREFIX);
                    break;
                }
            }

            byte streamType = (byte) m_Stream.ReadByte();
            if (streamType == FILE_CONTENT_TYPE_STREAM)
            {
                m_Gzip = new GZipStream(m_Stream, CompressionMode.Decompress);
                m_Reader = new BinaryReader(m_Gzip);

                ReadNextFrame();
            }
            else if (streamType == FILE_CONTENT_TYPE_BASE64)
            {
                //Decompress from base64
                int bufferLength = (int) (m_Stream.Length - m_Stream.Position);
                byte[] buffer = Convert.FromBase64String(System.Text.Encoding.UTF8.GetString(m_Stream.GetBuffer(), (int) m_Stream.Position, bufferLength));

                if (m_Reader != null)
                {
                    m_Reader.Close();
                }
                m_Stream.Close();

                m_Stream = new MemoryStream(buffer);
                m_Gzip = new GZipStream(m_Stream, CompressionMode.Decompress);
                m_Reader = new BinaryReader(m_Gzip);

                ReadNextFrame();
            }
            else
            {
                m_Stream.Seek(4, SeekOrigin.Begin);

                m_Gzip = new GZipStream(m_Stream, CompressionMode.Decompress);
                m_Reader = new BinaryReader(m_Gzip);

                ReadNextFrame();
            }

            if (m_Current.Type == FieldType.UINT16)
            {
                SerializerVersion = (ushort) m_Current.Value;
                ReadNextFrame();
            }
        }

        protected override void BeginWriteRoot(string inRootName)
        {
            m_Gzip = new GZipStream(m_Stream, CompressionMode.Compress);
            m_Writer = new BinaryWriter(m_Gzip);

            ushort currentVersion = SerializerVersion;
            Write_UInt16(ref currentVersion);

            BeginWriteObject();
        }

        #endregion

        #region Output

        internal override void AsStream(Stream inStream, OutputOptions inOptions = OutputOptions.None)
        {
            m_Gzip.Flush();
            m_Gzip.Close();

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