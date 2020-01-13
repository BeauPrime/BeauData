/*
 * Copyright (C) 2017 - 2020. Filament Games, LLC. All rights reserved.
 * Author:  Autumn Beauchesne
 * Date:    3 May 2017
 * 
 * File:    Serializer.Statics.cs
 * Purpose: Provides shortcuts for reading and writing ISerializedObjects.
 */

#if UNITY_2018_3_OR_NEWER
#define WWW_OBSOLETE
#endif // UNITY_2018_3_OR_NEWER

using System;
using System.IO;
using System.Xml;
using BeauData.Format;
using UnityEngine;
using UnityEngine.Networking;

namespace BeauData
{
    /// <summary>
    /// Serialization format and shortcuts.
    /// </summary>
    public abstract partial class Serializer
    {
        /// <summary>
        /// Supported file formats.
        /// </summary>
        public enum Format : byte
        {
            JSON = 0,
            XML = 1,
            Binary = 2,
            GZIP = 3,

            Unknown = 254,
            AutoDetect = 255
        }

        /// <summary>
        /// Default format for writing objects.
        /// </summary>
        static public Format DefaultWriteFormat = Format.JSON;

        #if WWW_OBSOLETE
        private const string WWW_OBSOLETE_MESSAGE = "WWW is obsolete. Refrain from using this call.";
        #endif // WWW_OBSOLETE

        #region Read

        #region TextAsset

        /// <summary>
        /// Reads an object from the given text asset.
        /// </summary>
        static public bool Read<T>(ref T ioObject, TextAsset inTextAsset, Format inFormat = Format.AutoDetect, ISerializerContext inContext = null) where T : ISerializedObject
        {
            if (inTextAsset == null)
            {
                Debug.LogError("[BeauData] Error when reading object: Provided TextAsset is null.");
                return false;
            }

            using(MemoryStream stream = GetStream(inTextAsset.bytes))
            {
                return Read<T>(ref ioObject, stream, inFormat, inContext);
            }
        }

        /// <summary>
        /// Reads an object from the given text asset.
        /// </summary>
        static public T Read<T>(TextAsset inAsset, Format inFormat = Format.AutoDetect, ISerializerContext inContext = null) where T : ISerializedObject
        {
            T obj = default(T);
            Read<T>(ref obj, inAsset, inFormat, inContext);
            return obj;
        }

        #endregion

        #region WWW

        // Disable obsolete WWW warning
        #pragma warning disable 612, 618

        /// <summary>
        /// Reads an object from the given WWW.
        /// </summary>
        #if WWW_OBSOLETE
        [Obsolete(WWW_OBSOLETE_MESSAGE)]
        #endif // WWW_OBSOLETE
        static public bool Read<T>(ref T ioObject, WWW inWWW, Format inFormat = Format.AutoDetect, ISerializerContext inContext = null) where T : ISerializedObject
        {
            if (!String.IsNullOrEmpty(inWWW.error))
            {
                Debug.LogError("[BeauData] Error when reading object WWW: " + inWWW.error);
                return false;
            }

            using(MemoryStream stream = GetStream(inWWW.bytes))
            {
                return Read<T>(ref ioObject, stream, inFormat, inContext);
            }
        }

        /// <summary>
        /// Reads an object from the given WWW.
        /// </summary>
        #if WWW_OBSOLETE
        [Obsolete(WWW_OBSOLETE_MESSAGE)]
        #endif // WWW_OBSOLETE
        static public T Read<T>(WWW inWWW, Format inFormat = Format.AutoDetect, ISerializerContext inContext = null) where T : ISerializedObject
        {
            T obj = default(T);
            Read<T>(ref obj, inWWW, inFormat, inContext);
            return obj;
        }

        // Restore obsolete WWW warning
        #pragma warning restore 612, 618

        #endregion

        #region UnityWebRequest

        /// <summary>
        /// Reads an object from the given UnityWebRequest.
        /// </summary>
        static public bool Read<T>(ref T ioObject, UnityWebRequest inWWW, Format inFormat = Format.AutoDetect, ISerializerContext inContext = null) where T : ISerializedObject
        {
            if (!String.IsNullOrEmpty(inWWW.error))
            {
                Debug.LogError("[BeauData] Error when reading object UnityWebRequest: " + inWWW.error);
                return false;
            }

            using(MemoryStream stream = GetStream(inWWW.downloadHandler.data))
            {
                return Read<T>(ref ioObject, stream, inFormat, inContext);
            }
        }

        /// <summary>
        /// Reads an object from the given UnityWebRequest.
        /// </summary>
        static public T Read<T>(UnityWebRequest inWWW, Format inFormat = Format.AutoDetect, ISerializerContext inContext = null) where T : ISerializedObject
        {
            T obj = default(T);
            Read<T>(ref obj, inWWW, inFormat, inContext);
            return obj;
        }

        #endregion

        #region String

        /// <summary>
        /// Reads an object from the given text.
        /// </summary>
        static public bool Read<T>(ref T ioObject, string inText, Format inFormat = Format.AutoDetect, ISerializerContext inContext = null) where T : ISerializedObject
        {
            if (string.IsNullOrEmpty(inText))
            {
                Debug.LogWarning("[BeauData] Error when reading object: Empty string");
                return false;
            }

            if (inFormat == Format.AutoDetect)
                inFormat = DetectFileFormatFromContents(inText);

            try
            {
                Serializer serializer;
                switch (inFormat)
                {
                    case Format.JSON:
                        {
                            JSON root = JSON.Parse(inText);
                            serializer = new JSONSerializer(root);
                            break;
                        }
                    case Format.XML:
                        {
                            XmlDocument document = new XmlDocument();
                            document.LoadXml(inText);
                            serializer = new XMLSerializer(document);
                            break;
                        }
                    case Format.Binary:
                        {
                            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(inText);
                            MemoryStream stream = GetStream(buffer);
                            serializer = new BinarySerializer(stream);
                            break;
                        }
                    case Format.GZIP:
                        {
                            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(inText);
                            MemoryStream stream = GetStream(buffer);
                            serializer = new GZIPSerializer(stream);
                            break;
                        }
                    default:
                        {
                            Debug.LogError("[BeauData] Unknown file format");
                            return false;
                        }
                }

                using(serializer)
                {
                    serializer.Read<T>(ref ioObject, inContext);
                    if (serializer.HasErrors)
                        Debug.LogError("[BeauData] Error when reading object:\n" + serializer.Errors);
                    return !serializer.HasErrors;
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                Debug.LogError("[BeauData] Exception when reading object");
                return false;
            }
        }

        /// <summary>
        /// Reads an object from the given text.
        /// </summary>
        static public T Read<T>(string inString, Format inFormat = Format.AutoDetect, ISerializerContext inContext = null) where T : ISerializedObject
        {
            T obj = default(T);
            Read<T>(ref obj, inString, inFormat, inContext);
            return obj;
        }

        #endregion

        #region Stream

        /// <summary>
        /// Reads an object from the given stream.
        /// </summary>
        static public bool Read<T>(ref T ioObject, Stream inStream, Format inFormat = Format.AutoDetect, ISerializerContext inContext = null) where T : ISerializedObject
        {
            if (inStream == null)
            {
                Debug.LogError("[BeauData] Error when reading object: Provided stream is null.");
                return false;
            }
            if (inStream.Length == 0)
            {
                Debug.LogWarning("[BeauData] Error when reading object: Empty stream");
                return false;
            }

            MemoryStream memoryStream = inStream is MemoryStream ? (MemoryStream) inStream : GetBytes(inStream);

            if (inFormat == Format.AutoDetect)
                inFormat = DetectFileFormatFromContents(memoryStream);

            memoryStream.Position = 0;

            try
            {
                Serializer serializer;
                switch (inFormat)
                {
                    case Format.JSON:
                        {
                            JSON root = JSON.Parse(System.Text.Encoding.UTF8.GetString(memoryStream.GetBuffer()));
                            serializer = new JSONSerializer(root);
                            break;
                        }
                    case Format.XML:
                        {
                            XmlDocument document = new XmlDocument();
                            document.LoadXml(System.Text.Encoding.UTF8.GetString(memoryStream.GetBuffer()));
                            serializer = new XMLSerializer(document);
                            break;
                        }
                    case Format.Binary:
                        {
                            serializer = new BinarySerializer(memoryStream);
                            break;
                        }
                    case Format.GZIP:
                        {
                            serializer = new GZIPSerializer(memoryStream);
                            break;
                        }
                    default:
                        {
                            Debug.LogError("[BeauData] Unknown file format");
                            return false;
                        }
                }

                using(serializer)
                {
                    serializer.Read<T>(ref ioObject, inContext);
                    if (serializer.HasErrors)
                        Debug.LogError("[BeauData] Error when reading object:\n" + serializer.Errors);
                    return !serializer.HasErrors;
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                Debug.LogError("[BeauData] Exception when reading object");
                return false;
            }
        }

        /// <summary>
        /// Reads an object from the given stream.
        /// </summary>
        static public T Read<T>(Stream inStream, Format inFormat = Format.AutoDetect, ISerializerContext inContext = null) where T : ISerializedObject
        {
            T obj = default(T);
            Read<T>(ref obj, inStream, inFormat, inContext);
            return obj;
        }

        #endregion

        #region Bytes

        /// <summary>
        /// Reads an object from the given byte array.
        /// </summary>
        static public bool Read<T>(ref T ioObject, byte[] inBytes, Format inFormat = Format.AutoDetect, ISerializerContext inContext = null) where T : ISerializedObject
        {
            return Read<T>(ref ioObject, GetStream(inBytes), inFormat, inContext);
        }

        /// <summary>
        /// Reads an object from the given byte array.
        /// </summary>
        static public T Read<T>(byte[] inBytes, Format inFormat = Format.AutoDetect, ISerializerContext inContext = null) where T : ISerializedObject
        {
            T obj = default(T);
            Read<T>(ref obj, inBytes, inFormat, inContext);
            return obj;
        }

        #endregion

        #region File

        /// <summary>
        /// Reads an object from a file.
        /// </summary>
        static public bool ReadFile<T>(ref T ioObject, string inFilePath, Format inFormat = Format.AutoDetect, ISerializerContext inContext = null) where T : ISerializedObject
        {
            string filePath = PathUtility.CorrectPath(inFilePath, inFormat);
            if (!File.Exists(filePath))
            {
                Debug.LogWarning("[BeauData] Error when reading object: File '" + filePath + "' does not exist.");
                return false;
            }

            using(FileStream stream = File.OpenRead(filePath))
            {
                return Read(ref ioObject, stream, inFormat, inContext);
            }
        }

        /// <summary>
        /// Reads an object from a file.
        /// </summary>
        static public T ReadFile<T>(string inFilePath, Format inFormat = Format.AutoDetect, ISerializerContext inContext = null) where T : ISerializedObject
        {
            T obj = default(T);
            ReadFile<T>(ref obj, inFilePath, inFormat, inContext);
            return obj;
        }

        #endregion

        #region Prefs

        /// <summary>
        /// Reads an object from PlayerPrefs.
        /// </summary>
        static public bool ReadPrefs<T>(ref T ioObject, string inKey, Format inFormat = Format.AutoDetect, ISerializerContext inContext = null) where T : ISerializedObject
        {
            return Read(ref ioObject, PlayerPrefs.GetString(inKey, string.Empty), inFormat, inContext);
        }

        /// <summary>
        /// Reads an object from PlayerPrefs.
        /// </summary>
        static public T ReadPrefs<T>(string inKey, Format inFormat = Format.AutoDetect, ISerializerContext inContext = null) where T : ISerializedObject
        {
            T obj = default(T);
            ReadPrefs<T>(ref obj, inKey, inFormat, inContext);
            return obj;
        }

        #endregion

        #endregion

        #region Write

        /// <summary>
        /// Writes an object to a string.
        /// </summary>
        static public string Write<T>(T inObject, OutputOptions inOptions = OutputOptions.None, Format inFormat = Format.AutoDetect, ISerializerContext inContext = null) where T : ISerializedObject
        {
            if (inFormat == Format.AutoDetect)
                inFormat = DefaultWriteFormat;

            using(Serializer serializer = CreateWriter(inFormat))
            {
                serializer.Write<T>(ref inObject, inContext);
                return serializer.AsString(inOptions);
            }
        }

        /// <summary>
        /// Writes an object to the given stream.
        /// </summary>
        static public void Write<T>(T inObject, Stream inStream, OutputOptions inOptions = OutputOptions.None, Format inFormat = Format.AutoDetect, ISerializerContext inContext = null) where T : ISerializedObject
        {
            if (inFormat == Format.AutoDetect)
                inFormat = DefaultWriteFormat;

            using(Serializer serializer = CreateWriter(inFormat))
            {
                serializer.Write<T>(ref inObject, inContext);
                serializer.AsStream(inStream, inOptions);
            }
        }

        /// <summary>
        /// Writes an object to a file.
        /// </summary>
        static public void WriteFile<T>(T inObject, string inFilePath, OutputOptions inOptions = OutputOptions.None, Format inFormat = Format.AutoDetect, ISerializerContext inContext = null) where T : ISerializedObject
        {
            string finalPath = PathUtility.CorrectPath(inFilePath, inFormat);
            using(FileStream stream = File.Open(finalPath, FileMode.Create))
            {
                Write(inObject, stream, inOptions, inFormat, inContext);
            }
        }

        /// <summary>
        /// Writes an object to PlayerPrefs.
        /// </summary>
        static public void WritePrefs<T>(T inObject, string inKey, OutputOptions inOptions = OutputOptions.None, Format inFormat = Format.AutoDetect, ISerializerContext inContext = null) where T : ISerializedObject
        {
            PlayerPrefs.SetString(inKey, Write(inObject, inOptions, inFormat));
        }

        // Creates a writer for the given format
        static private Serializer CreateWriter(Format inFormat)
        {
            switch (inFormat)
            {
                case Format.JSON:
                    return new JSONSerializer();
                case Format.XML:
                    return new XMLSerializer();
                case Format.Binary:
                    return new BinarySerializer();
                case Format.GZIP:
                    return new GZIPSerializer();
                default:
                    throw new Exception("Unknown format!");
            }
        }

        #endregion

        #region Format detection

        static private Format DetectFileFormatFromExtension(string inFilePath)
        {
            string extension = System.IO.Path.GetExtension(inFilePath);
            if (extension == null || extension.Length == 0)
                return Format.AutoDetect;

            if (extension[0] == '.')
            {
                extension = extension.Substring(1);
                if (extension.Length == 0)
                    return Format.AutoDetect;
            }

            if (extension == XMLSerializer.FILE_EXTENSION)
                return Format.XML;
            else if (extension == JSONSerializer.FILE_EXTENSION)
                return Format.JSON;
            else if (extension == BinarySerializer.FILE_EXTENSION)
                return Format.Binary;
            else if (extension == GZIPSerializer.FILE_EXTENSION)
                return Format.GZIP;
            else
                return Format.AutoDetect;
        }

        static private Format DetectFileFormatFromContents(string inFileContents)
        {
            int firstNonEmptyCharIndex = 0;
            while (firstNonEmptyCharIndex < inFileContents.Length)
            {
                if (!char.IsWhiteSpace(inFileContents[firstNonEmptyCharIndex]))
                    break;
                ++firstNonEmptyCharIndex;
            }

            if (MatchSubstring(inFileContents, JSONSerializer.FILE_CONTENT_PREFIX, firstNonEmptyCharIndex))
                return Format.JSON;
            else if (MatchSubstring(inFileContents, XMLSerializer.FILE_CONTENT_PREFIX, firstNonEmptyCharIndex))
                return Format.XML;
            else if (MatchSubstring(inFileContents, GZIPSerializer.FILE_CONTENT_PREFIX, firstNonEmptyCharIndex))
                return Format.GZIP;
            else if (MatchSubstring(inFileContents, BinarySerializer.FILE_CONTENT_PREFIX, firstNonEmptyCharIndex))
                return Format.Binary;

            return Format.Unknown;
        }

        static private Format DetectFileFormatFromContents(MemoryStream inFileContents)
        {
            inFileContents.Position = 0;

            int firstNonEmptyCharIndex = 0;
            while (firstNonEmptyCharIndex < inFileContents.Length)
            {
                char c = (char) inFileContents.ReadByte();
                if (!char.IsWhiteSpace(c))
                    break;
                ++firstNonEmptyCharIndex;
            }

            if (MatchSubstring(inFileContents, JSONSerializer.FILE_CONTENT_PREFIX, firstNonEmptyCharIndex))
                return Format.JSON;
            else if (MatchSubstring(inFileContents, XMLSerializer.FILE_CONTENT_PREFIX, firstNonEmptyCharIndex))
                return Format.XML;
            else if (MatchSubstring(inFileContents, GZIPSerializer.FILE_CONTENT_PREFIX, firstNonEmptyCharIndex))
                return Format.GZIP;
            else if (MatchSubstring(inFileContents, BinarySerializer.FILE_CONTENT_PREFIX, firstNonEmptyCharIndex))
                return Format.Binary;

            return Format.Unknown;
        }

        static private bool MatchSubstring(string inSource, string inTarget, int inStartIndex)
        {
            if (inStartIndex + inTarget.Length > inSource.Length)
                return false;

            for (int i = 0; i < inTarget.Length; ++i)
                if (inSource[inStartIndex + i] != inTarget[i])
                    return false;

            return true;
        }

        static private bool MatchSubstring(MemoryStream inStream, string inTarget, int inStartIndex)
        {
            if (inStartIndex + inTarget.Length > inStream.Length)
                return false;

            inStream.Position = inStartIndex;
            for (int i = 0; i < inTarget.Length; ++i)
                if (inStream.ReadByte() != inTarget[i])
                    return false;

            return true;
        }

        #endregion

        [ThreadStatic]
        static private byte[] READ_BUFFER;

        static private MemoryStream GetBytes(Stream inStream)
        {
            if (READ_BUFFER == null)
                READ_BUFFER = new byte[16 * 1024];

            MemoryStream memoryStream = new MemoryStream((int) inStream.Length);
            int read;
            while ((read = inStream.Read(READ_BUFFER, 0, READ_BUFFER.Length)) > 0)
                memoryStream.Write(READ_BUFFER, 0, read);
            return memoryStream;
        }

        static private MemoryStream GetStream(byte[] inBytes)
        {
            return new MemoryStream(inBytes, 0, inBytes.Length, false, true);
        }
    }
}