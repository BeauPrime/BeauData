/*
 * Copyright (C) 2017 - 2020. Autumn Beauchesne. All rights reserved.
 * Author:  Autumn Beauchesne
 * Date:    28 Nov 2018
 * 
 * File:    FourCC.cs
 * Purpose: Four-character code. Short, mnemonic identifier
 *          stored as an 32-bit integer for efficient comparisons.
 */

// -- START CONFIGURATION OPTIONS -- //

// Comment this out to disable unity-specific features
#define UNITY3D

// -- END CONFIGURATION OPTIONS -- //

#if UNITY_EDITOR || UNITY_DEVELOPMENT || DEVELOPMENT
#define DEBUG
#endif // UNITY_EDITOR || UNITY_DEVELOPMENT || DEVELOPMENT

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using BeauData.Packed;

namespace BeauData
{
    /// <summary>
    /// Four-character code. Encodes four characters into an integer for fast comparisons.
    /// Useful for unique, human-parsable type codes (file types, object types, layer names, etc)
    /// Valid characters are A-Z, 0-9, and !#$+-?_
    /// May be case insensitive depending on your configuration
    /// </summary>
    [Serializable, StructLayout(LayoutKind.Explicit, Size = 4)]
    public partial struct FourCC : IEquatable<FourCC>, IComparable<FourCC>
    {
        #region Consts

        private const int Size = 4;
        private const int MaxShift = (Size - 1) * 8;
        private const string EmptyString = "    ";

        /// <summary>
        /// Empty/null FourCC.
        /// </summary>
        static public readonly FourCC Zero = new FourCC();

        #endregion // Consts

        #if UNITY3D
        [UnityEngine.SerializeField]
        #endif // UNITY3D
        [FieldOffset(0)]
        private int m_Value;

        /// <summary>
        /// Creates a FourCC with the given value.
        /// </summary>
        public FourCC(int inValue)
        {
            m_Value = inValue;
        }

        #region Factory

        /// <summary>
        /// Parses the string into a FourCC.
        /// </summary>
        static public FourCC Parse(string inString)
        {
            int value = Pack(inString);
            return new FourCC(value);
        }

        /// <summary>
        /// Attempts to parse the given string into a FourCC.
        /// Returns if the string was valid, and outputs the resulting FourCC.
        /// </summary>
        static public bool TryParse(string inString, out FourCC outResult)
        {
            int value;
            bool bSuccess = TryPack(inString, out value);
            outResult = new FourCC(value);
            return bSuccess;
        }

        #endregion // Factory

        #region Public Unpacks

        /// <summary>
        /// Unpacks the FourCC into its component bytes.
        /// </summary>
        public void Unpack(out byte outA, out byte outB, out byte outC, out byte outD)
        {
            Unpack(m_Value, out outA, out outB, out outC, out outD);
        }

        #endregion // Public Unpacks

        #region Overrides

        /// <summary>
        /// Returns the string representation of the FourCC.
        /// </summary>
        public override string ToString()
        {
            unsafe
            {
                return Stringify(m_Value, false);
            }
        }

        /// <summary>
        /// Returns the string representation of the FourCC,
        /// optionally trimming any trailing spaces.
        /// </summary>
        public string ToString(bool inbTrimSpaces)
        {
            unsafe
            {
                return Stringify(m_Value, inbTrimSpaces);
            }
        }

        public override int GetHashCode()
        {
            return m_Value;
        }

        public override bool Equals(object obj)
        {
            if (obj is FourCC)
                return ((FourCC) obj).m_Value == m_Value;
            if (obj is int)
                return ((int) obj) == m_Value;
            return false;
        }

        public bool Equals(FourCC other)
        {
            return m_Value == other.m_Value;
        }

        public int CompareTo(FourCC other)
        {
            if (m_Value == other.m_Value)
                return 0;

            byte a1, b1, c1, d1,
            a2, b2, c2, d2;

            Unpack(m_Value, out a1, out b1, out c1, out d1);
            Unpack(other.m_Value, out a2, out b2, out c2, out d2);

            if (a1 < a2)
                return -1;
            if (a1 > a2)
                return 1;

            if (b1 < b2)
                return -1;
            if (b1 > b2)
                return 1;

            if (c1 < c2)
                return -1;
            if (c1 > c2)
                return 1;

            if (d1 < d2)
                return -1;
            if (d1 > d2)
                return 1;

            return 0;
        }

        static public bool operator ==(FourCC a, FourCC b)
        {
            return a.m_Value == b.m_Value;
        }

        static public bool operator !=(FourCC a, FourCC b)
        {
            return a.m_Value != b.m_Value;
        }

        static public explicit operator int(FourCC inFourCC)
        {
            return inFourCC.m_Value;
        }

        static public explicit operator FourCC(int inValue)
        {
            return new FourCC(inValue);
        }

        #endregion // Overrides

        #region Serialization

        static internal class Serialization
        {
            /*
             * Note on serialization:
             * FourCCs are encoded in big-endian order.
             * To ensure readability and compatibility,
             * they are always serialized and deserialized in big-endian order.
             * This means we write four bytes instead of a single integer value.
             */

            // Cached char array for streaming chars
            [ThreadStatic]
            static private char[] s_StreamChars;

            // Cached char array for streaming bytes
            [ThreadStatic]
            static private byte[] s_StreamBytes;

            static private char[] RequireStreamChars()
            {
                return s_StreamChars ?? (s_StreamChars = new char[Size]);
            }

            static private byte[] RequireStreamBytes()
            {
                return s_StreamBytes ?? (s_StreamBytes = new byte[Size]);
            }

            /// <summary>
            /// Writes the given FourCC to the given Stream.
            /// </summary>
            static internal void WriteTo(FourCC inFourCC, Stream inStream)
            {
                byte[] bytes = RequireStreamBytes();
                
                Unpack(inFourCC.m_Value, bytes);
                inStream.Write(bytes, 0, Size);
            }

            /// <summary>
            /// Writes the given FourCC to the given byte array.
            /// </summary>
            static internal void WriteTo(FourCC inFourCC, byte[] inBytes, int inOffset = 0)
            {
                byte[] bytes = RequireStreamBytes();

                Unpack(inFourCC.m_Value, bytes);
                Array.Copy(bytes, 0, inBytes, inOffset, Size);
            }

            /// <summary>
            /// Writes the given FourCC to the given TextWriter.
            /// Note that this will write spaces in place of empty bytes.
            /// </summary>
            static internal void WriteTo(FourCC inFourCC, TextWriter inWriter)
            {
                byte[] bytes = RequireStreamBytes();
                char[] chars = RequireStreamChars();

                Unpack(inFourCC.m_Value, bytes);
                for (int i = 0; i < Size; ++i)
                {
                    byte val = bytes[i];
                    chars[i] = (val == 0) ? CharUtils.PaddingChar : (char) val;
                }

                inWriter.Write(chars);
            }

            /// <summary>
            /// Writes the given FourCC to the given StringBuilder.
            /// Note that this will write spaces in place of empty bytes if trim is off.
            /// </summary>
            static internal void WriteTo(FourCC inFourCC, StringBuilder inBuilder, bool inbTrimSpaces)
            {
                int value = inFourCC.m_Value;

                if (value == 0)
                {
                    if (!inbTrimSpaces)
                    {
                        inBuilder.Append(EmptyString);
                    }

                    return;
                }

                char[] chars = RequireStreamChars();

                for(int i = 0; i < Size; ++i)
                {
                    char ch = (char) (value >> (MaxShift - i * 8) & 0xFF);
                    if (ch == 0)
                    {
                        if (inbTrimSpaces)
                        {
                            inBuilder.Append(chars, 0, i);
                            return;
                        }
                        ch = CharUtils.PaddingChar;
                    }

                    chars[i] = ch;
                }

                inBuilder.Append(chars, 0, Size);
            }

            /// <summary>
            /// Writes the given FourCC to the given BinaryWriter.
            /// </summary>
            static internal void WriteTo(FourCC inFourCC, BinaryWriter inWriter)
            {
                byte[] bytes = RequireStreamBytes();

                Unpack(inFourCC.m_Value, bytes);
                inWriter.Write(bytes);
            }

            /// <summary>
            /// Reads a FourCC from the given Stream.
            /// </summary>
            static internal FourCC ReadFrom(Stream inStream)
            {
                byte[] bytes = RequireStreamBytes();

                int bytesRead = inStream.Read(bytes, 0, Size);
                for (int i = bytesRead; i < Size; ++i)
                    bytes[i] = 0;

                int value = Pack(bytes);
                return new FourCC(value);
            }

            /// <summary>
            /// Reads a FourCC from the given byte array.
            /// </summary>
            static internal FourCC ReadFrom(byte[] inBytes, int inOffset = 0)
            {
                byte[] bytes = RequireStreamBytes();

                int numBytesRead = Math.Min(Size, inBytes.Length - inOffset);
                for (int i = 0; i < numBytesRead; ++i)
                    bytes[i] = inBytes[inOffset + i];
                for (int i = numBytesRead; i < Size; ++i)
                    bytes[i] = 0;

                int value = Pack(bytes);
                return new FourCC(value);
            }

            /// <summary>
            /// Reads a FourCC from the given TextReader.
            /// </summary>
            static internal FourCC ReadFrom(TextReader inReader)
            {
                char[] chars = RequireStreamChars();
                byte[] bytes = RequireStreamBytes();

                int charsRead = inReader.Read(chars, 0, Size);
                for (int i = 0; i < charsRead; ++i)
                {
                    char read = chars[i];
                    if (read == CharUtils.PaddingChar)
                        bytes[i] = 0;
                    else
                        bytes[i] = (byte) read;
                }
                for (int i = charsRead; i < Size; ++i)
                    bytes[i] = 0;

                int value = Pack(bytes);
                return new FourCC(value);
            }

            /// <summary>
            /// Reads a FourCC from the given BinaryReader.
            /// </summary>
            static internal FourCC ReadFrom(BinaryReader inReader)
            {
                byte[] bytes = RequireStreamBytes();

                int bytesRead = inReader.Read(bytes, 0, Size);
                for (int i = bytesRead; i < Size; ++i)
                    bytes[i] = 0;

                int value = Pack(bytes);
                return new FourCC(value);
            }
        }

        #endregion // Serialization

        #region Bytes

        /// <summary>
        /// Unpacks an integer value into four bytes in an array.
        /// </summary>
        static private void Unpack(int inValue, byte[] outBytes)
        {
            #if DEBUG
            if (outBytes == null)
                throw new ArgumentNullException("outBytes");
            else if (outBytes.Length != Size)
                throw new ArgumentException("Byte array must be length 4", "outBytes");
            #endif // DEBUG

            outBytes[0] = (byte) ((inValue >> 24) & 0xFF);
            outBytes[1] = (byte) ((inValue >> 16) & 0xFF);
            outBytes[2] = (byte) ((inValue >> 8) & 0xFF);
            outBytes[3] = (byte) ((inValue) & 0xFF);
        }

        /// <summary>
        /// Unpacks an integer value into four bytes in an array.
        /// </summary>
        static private unsafe void Unpack(int inValue, byte* outBytes)
        {
            #if DEBUG
            if (outBytes == null)
                throw new ArgumentNullException("outBytes");
            #endif // DEBUG

            outBytes[0] = (byte) ((inValue >> 24) & 0xFF);
            outBytes[1] = (byte) ((inValue >> 16) & 0xFF);
            outBytes[2] = (byte) ((inValue >> 8) & 0xFF);
            outBytes[3] = (byte) ((inValue) & 0xFF);
        }

        /// <summary>
        /// Unpacks an integer value into four bytes.
        /// </summary>
        static private void Unpack(int inValue, out byte outA, out byte outB, out byte outC, out byte outD)
        {
            outA = (byte) ((inValue >> 24) & 0xFF);
            outB = (byte) ((inValue >> 16) & 0xFF);
            outC = (byte) ((inValue >> 8) & 0xFF);
            outD = (byte) ((inValue) & 0xFF);
        }

        /// <summary>
        /// Packs the four bytes into an integer value.
        /// </summary>
        static private int Pack(byte inA, byte inB, byte inC, byte inD)
        {
            return (inA << 24) | (inB << 16) | (inC << 8) | (inD);
        }

        /// <summary>
        /// Packs the four bytes from the given array into an integer value.
        /// </summary>
        static private int Pack(byte[] inBytes)
        {
            #if DEBUG
            if (inBytes == null)
                throw new ArgumentNullException("inBytes");
            else if (inBytes.Length != Size)
                throw new ArgumentException("Byte array must be length 4", "outBytes");
            #endif // DEBUG

            return (inBytes[0] << 24) | (inBytes[1] << 16) | (inBytes[2] << 8) | (inBytes[3]);
        }

        /// <summary>
        /// Packs the four bytes from the given array into an integer value.
        /// </summary>
        static private unsafe int Pack(byte* inBytes)
        {
            #if DEBUG
            if (inBytes == null)
                throw new ArgumentNullException("inBytes");
            #endif // DEBUG

            return (inBytes[0] << 24) | (inBytes[1] << 16) | (inBytes[2] << 8) | (inBytes[3]);
        }

        #endregion // Bytes

        #region Utils

        /// <summary>
        /// Stringifies the given value, optionally trimming off empty space.
        /// </summary>
        static private unsafe string Stringify(int inValue, bool inbTrim)
        {
            if (inValue == 0)
                return inbTrim ? string.Empty : EmptyString;

            char* assembler = stackalloc char[Size];

            for(int i = 0; i < Size; ++i)
            {
                char ch = (char) (inValue >> (MaxShift - i * 8) & 0xFF);
                if (ch == 0)
                {
                    if (inbTrim)
                        return i == 0 ? string.Empty : new string(assembler, 0, i);

                    ch = CharUtils.PaddingChar;
                }

                assembler[i] = ch;
            }

            return new string(assembler, 0, Size);
        }

        /// <summary>
        /// Packs the string into an integer value.
        /// </summary>
        static private int Pack(string inString)
        {
            if (inString == null)
                return 0;

            int strLength = inString.Length;
            if (strLength <= 0)
                return 0;

            #if DEBUG
            if (strLength > Size)
                throw new FormatException("Cannot pack from a string longer than 4 characters");
            #endif // DEBUG

            byte a = CharUtils.MapCC(inString[0]);
            byte b = strLength < 2 ? (byte) 0 : CharUtils.MapCC(inString[1]);
            byte c = strLength < 3 ? (byte) 0 : CharUtils.MapCC(inString[2]);
            byte d = strLength < 4 ? (byte) 0 : CharUtils.MapCC(inString[3]);

            return Pack(a, b, c, d);
        }

        /// <summary>
        /// Attempts to pack the string into an integer value.
        /// Returns false if the string is poorly formatted.
        /// </summary>
        static private unsafe bool TryPack(string inString, out int outValue)
        {
            if (inString == null)
            {
                outValue = 0;
                return true;
            }

            int strLength = inString.Length;
            if (strLength <= 0)
            {
                outValue = 0;
                return true;
            }

            if (strLength > Size)
            {
                outValue = 0;
                return false;
            }

            byte* bytes = stackalloc byte[Size];
            bool bHasSpace = false;

            for(int i = 0; i < Size; ++i)
            {
                byte mapped;
                if (strLength <= i)
                {
                    mapped = 0;
                }
                else
                {
                    if (!CharUtils.TryMapCC(inString[i], out mapped))
                    {
                        outValue = 0;
                        return false;
                    }
                }
                
                if (mapped == 0)
                {
                    bHasSpace = true;
                }
                else
                {
                    if (bHasSpace)
                    {
                        outValue = 0;
                        return false;
                    }
                }

                bytes[i] = mapped;
            }

            outValue = Pack(bytes);
            return true;
        }

        #endregion // Utils

        #region Statics

        /// <summary>
        /// Writes a FourCC to the given byte array.
        /// </summary>
        static public void WriteTo(FourCC inFourCC, byte[] inBytes, int inOffset = 0)
        {
            FourCC.Serialization.WriteTo(inFourCC, inBytes, inOffset);
        }

        /// <summary>
        /// Writes a FourCC to the given stream.
        /// </summary>
        static public void WriteTo(FourCC inFourCC, Stream inStream)
        {
            FourCC.Serialization.WriteTo(inFourCC, inStream);
        }

        /// <summary>
        /// Reads a FourCC from the given byte array.
        /// </summary>
        static public FourCC ReadFrom(byte[] inBytes, int inOffset = 0)
        {
            return FourCC.Serialization.ReadFrom(inBytes, inOffset);
        }

        /// <summary>
        /// Reads a FourCC from the given stream.
        /// </summary>
        static public FourCC ReadFrom(Stream inStream)
        {
            return FourCC.Serialization.ReadFrom(inStream);
        }

        #endregion // Statics
    }

    /// <summary>
    /// Extension methods for dealing with FourCCs.
    /// </summary>
    static public class FourCCExtentions
    {
        #region Serialization

        /// <summary>
        /// Writes a FourCC value to this TextWriter.
        /// </summary>
        static public void Write(this TextWriter inWriter, FourCC inFourCC)
        {
            FourCC.Serialization.WriteTo(inFourCC, inWriter);
        }

        /// <summary>
        /// Writes a FourCC value to this BinaryWriter.
        /// </summary>
        static public void Write(this BinaryWriter inWriter, FourCC inFourCC)
        {
            FourCC.Serialization.WriteTo(inFourCC, inWriter);
        }

        /// <summary>
        /// Writes a FourCC value to this StringBuilder, optionally trimming spaces from the end.
        /// </summary>
        static public StringBuilder Append(this StringBuilder inBuilder, FourCC inFourCC, bool inbTrimSpaces = false)
        {
            FourCC.Serialization.WriteTo(inFourCC, inBuilder, inbTrimSpaces);
            return inBuilder;
        }

        /// <summary>
        /// Reads a FourCC value from this TextReader.
        /// </summary>
        static public FourCC ReadFourCC(this TextReader inReader)
        {
            return FourCC.Serialization.ReadFrom(inReader);
        }

        /// <summary>
        /// Reads a FourCC value from this BinaryReader.
        /// </summary>
        static public FourCC ReadFourCC(this BinaryReader inReader)
        {
            return FourCC.Serialization.ReadFrom(inReader);
        }

        #endregion // Serialization
    }

}