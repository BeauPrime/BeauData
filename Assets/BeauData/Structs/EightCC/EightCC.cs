/*
 * Copyright (C) 2017 - 2020. Autumn Beauchesne. All rights reserved.
 * Author:  Autumn Beauchesne
 * Date:    8 April 2020
 * 
 * File:    EightCC.cs
 * Purpose: Eight-character code. Short, mnemonic identifier
 *          stored as a 64-bit integer for efficient comparisons.
 */

// -- START CONFIGURATION OPTIONS -- //

// Comment this out to disable unity-specific features
#define UNITY3D

// Comment this out to disable lowercase letters
// If enabled, uppercase and lowercase letters will be treated as distinct
// If disabled, lowercase letters will be cast to uppercase
#define CASE_SENSITIVE

// -- END CONFIGURATION OPTIONS -- //

#if UNITY_EDITOR || DEVELOPMENT_BUILD || DEVELOPMENT
#define DEBUG
#endif // UNITY_EDITOR || DEVELOPMENT_BUILD || DEVELOPMENT

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using BeauData.Packed;

namespace BeauData
{
    /// <summary>
    /// Eight-character code. Encodes eight characters into an int64 for fast comparisons.
    /// Useful for unique, human-parsable type codes (file types, object types, layer names, etc)
    /// Valid characters are A-Z, 0-9, and !#$+-?_
    /// May be case insensitive depending on your configuration
    /// </summary>
    [Serializable, StructLayout(LayoutKind.Explicit, Size = 8)]
    public partial struct EightCC : IEquatable<EightCC>, IComparable<EightCC>
    {
        #region Consts

        private const int Size = 8;
        private const int MaxShift = (Size - 1) * 8;
        private const string EmptyString = "        ";

        /// <summary>
        /// Empty/null EightCC.
        /// </summary>
        static public readonly EightCC Zero = new EightCC();

        #endregion // Consts

        #if UNITY3D
        [UnityEngine.SerializeField]
        #endif // UNITY3D
        [FieldOffset(0)]
        private long m_Value;

        /// <summary>
        /// Creates an EightCC with the given value.
        /// </summary>
        public EightCC(long inValue)
        {
            m_Value = inValue;
        }

        #region Factory

        /// <summary>
        /// Parses the string into an EightCC.
        /// </summary>
        static public EightCC Parse(string inString)
        {
            long value = Pack(inString);
            return new EightCC(value);
        }

        /// <summary>
        /// Attempts to parse the given string into an EightCC.
        /// Returns if the string was valid, and outputs the resulting EightCC.
        /// </summary>
        static public bool TryParse(string inString, out EightCC outResult)
        {
            long value;
            bool bSuccess = TryPack(inString, out value);
            outResult = new EightCC(value);
            return bSuccess;
        }

        #endregion // Factory

        #region Public Unpacks

        /// <summary>
        /// Unpacks the EightCC into its component bytes.
        /// </summary>
        public void Unpack(out byte outA, out byte outB, out byte outC, out byte outD, out byte outE, out byte outF, out byte outG, out byte outH)
        {
            Unpack(m_Value, out outA, out outB, out outC, out outD, out outE, out outF, out outG, out outH);
        }

        #endregion // Public Unpacks

        #region Overrides

        /// <summary>
        /// Returns the string representation of the EightCC.
        /// </summary>
        public override string ToString()
        {
            unsafe
            {
                return Stringify(m_Value, false);
            }
        }

        /// <summary>
        /// Returns the string representation of the EightCC,
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
            return (int) (m_Value >> 32) ^ (int) m_Value;
        }

        public override bool Equals(object obj)
        {
            if (obj is EightCC)
                return ((EightCC) obj).m_Value == m_Value;
            if (obj is long)
                return ((long) obj) == m_Value;
            return false;
        }

        public bool Equals(EightCC other)
        {
            return m_Value == other.m_Value;
        }

        public int CompareTo(EightCC other)
        {
            if (m_Value == other.m_Value)
                return 0;

            byte a1, b1, c1, d1, e1, f1, g1, h1,
            a2, b2, c2, d2, e2, f2, g2, h2;

            Unpack(m_Value, out a1, out b1, out c1, out d1, out e1, out f1, out g1, out h1);
            Unpack(other.m_Value, out a2, out b2, out c2, out d2, out e2, out f2, out g2, out h2);

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

            if (e1 < e2)
                return -1;
            if (e1 > e2)
                return 1;

            if (f1 < f2)
                return -1;
            if (f1 > f2)
                return 1;

            if (g1 < g2)
                return -1;
            if (g1 > g2)
                return 1;

            if (h1 < h2)
                return -1;
            if (h1 > h2)
                return 1;

            return 0;
        }

        static public bool operator ==(EightCC a, EightCC b)
        {
            return a.m_Value == b.m_Value;
        }

        static public bool operator !=(EightCC a, EightCC b)
        {
            return a.m_Value != b.m_Value;
        }

        static public explicit operator long(EightCC inEightCC)
        {
            return inEightCC.m_Value;
        }

        static public explicit operator EightCC(long inValue)
        {
            return new EightCC(inValue);
        }

        #endregion // Overrides

        #region Serialization

        static internal class Serialization
        {
            /*
             * Note on serialization:
             * EightCCs are encoded in big-endian order.
             * To ensure readability and compatibility,
             * they are always serialized and deserialized in big-endian order.
             * This means we write eight bytes instead of a single long value.
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
            /// Writes the given EightCC to the given Stream.
            /// </summary>
            static public void WriteTo(EightCC inEightCC, Stream inStream)
            {
                byte[] bytes = RequireStreamBytes();

                Unpack(inEightCC.m_Value, bytes);
                inStream.Write(bytes, 0, Size);
            }

            /// <summary>
            /// Writes the given EightCC to the given byte array.
            /// </summary>
            static public void WriteTo(EightCC inEightCC, byte[] inBytes, int inOffset = 0)
            {
                byte[] bytes = RequireStreamBytes();

                Unpack(inEightCC.m_Value, bytes);
                Array.Copy(bytes, 0, inBytes, inOffset, Size);
            }

            /// <summary>
            /// Writes the given EightCC to the given TextWriter.
            /// Note that this will write spaces in place of empty bytes.
            /// </summary>
            static public void WriteTo(EightCC inEightCC, TextWriter inWriter)
            {
                byte[] bytes = RequireStreamBytes();
                char[] chars = RequireStreamChars();

                Unpack(inEightCC.m_Value, bytes);
                for (int i = 0; i < Size; ++i)
                {
                    byte val = bytes[i];
                    chars[i] = (val == 0) ? CharUtils.PaddingChar : (char) val;
                }

                inWriter.Write(chars);
            }

            /// <summary>
            /// Writes the given EightCC to the given StringBuilder.
            /// Note that this will write spaces in place of empty bytes if trim is off.
            /// </summary>
            static public void WriteTo(EightCC inEightCC, StringBuilder inBuilder, bool inbTrimSpaces)
            {
                long value = inEightCC.m_Value;

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
            /// Writes the given EightCC to the given BinaryWriter.
            /// </summary>
            static public void WriteTo(EightCC inEightCC, BinaryWriter inWriter)
            {
                byte[] bytes = RequireStreamBytes();

                Unpack(inEightCC.m_Value, bytes);
                inWriter.Write(bytes);
            }

            /// <summary>
            /// Reads an EightCC from the given Stream.
            /// </summary>
            static public EightCC ReadFrom(Stream inStream)
            {
                byte[] bytes = RequireStreamBytes();

                int bytesRead = inStream.Read(bytes, 0, Size);
                for (int i = bytesRead; i < Size; ++i)
                    bytes[i] = 0;

                long value = Pack(bytes);
                return new EightCC(value);
            }

            /// <summary>
            /// Reads an EightCC from the given byte array.
            /// </summary>
            static public EightCC ReadFrom(byte[] inBytes, int inOffset = 0)
            {
                byte[] bytes = RequireStreamBytes();

                int numBytesRead = Math.Min(Size, inBytes.Length - inOffset);
                for (int i = 0; i < numBytesRead; ++i)
                    bytes[i] = inBytes[inOffset + i];
                for (int i = numBytesRead; i < Size; ++i)
                    bytes[i] = 0;

                long value = Pack(bytes);
                return new EightCC(value);
            }

            /// <summary>
            /// Reads an EightCC from the given TextReader.
            /// </summary>
            static public EightCC ReadFrom(TextReader inReader)
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

                long value = Pack(bytes);
                return new EightCC(value);
            }

            /// <summary>
            /// Reads an EightCC from the given BinaryReader.
            /// </summary>
            static public EightCC ReadFrom(BinaryReader inReader)
            {
                byte[] bytes = RequireStreamBytes();

                int bytesRead = inReader.Read(bytes, 0, Size);
                for (int i = bytesRead; i < Size; ++i)
                    bytes[i] = 0;

                long value = Pack(bytes);
                return new EightCC(value);
            }
        }

        #endregion // Serialization

        #region Bytes

        /// <summary>
        /// Unpacks an integer value into eight bytes in an array.
        /// </summary>
        static private void Unpack(long inValue, byte[] outBytes)
        {
            #if DEBUG
            if (outBytes == null)
                throw new ArgumentNullException("outBytes");
            else if (outBytes.Length != Size)
                throw new ArgumentException("Byte array must be length 8", "outBytes");
            #endif // DEBUG

            outBytes[0] = (byte) ((inValue >> 56) & 0xFF);
            outBytes[1] = (byte) ((inValue >> 48) & 0xFF);
            outBytes[2] = (byte) ((inValue >> 40) & 0xFF);
            outBytes[3] = (byte) ((inValue >> 32) & 0xFF);
            outBytes[4] = (byte) ((inValue >> 24) & 0xFF);
            outBytes[5] = (byte) ((inValue >> 16) & 0xFF);
            outBytes[6] = (byte) ((inValue >> 8) & 0xFF);
            outBytes[7] = (byte) ((inValue) & 0xFF);
        }

        /// <summary>
        /// Unpacks an integer value into eight bytes in an array.
        /// </summary>
        static private unsafe void Unpack(long inValue, byte* outBytes)
        {
            #if DEBUG
            if (outBytes == null)
                throw new ArgumentNullException("outBytes");
            #endif // DEBUG

            outBytes[0] = (byte) ((inValue >> 56) & 0xFF);
            outBytes[1] = (byte) ((inValue >> 48) & 0xFF);
            outBytes[2] = (byte) ((inValue >> 40) & 0xFF);
            outBytes[3] = (byte) ((inValue >> 32) & 0xFF);
            outBytes[4] = (byte) ((inValue >> 24) & 0xFF);
            outBytes[5] = (byte) ((inValue >> 16) & 0xFF);
            outBytes[6] = (byte) ((inValue >> 8) & 0xFF);
            outBytes[7] = (byte) ((inValue) & 0xFF);
        }

        /// <summary>
        /// Unpacks an integer value into eight bytes.
        /// </summary>
        static private void Unpack(long inValue, out byte outA, out byte outB, out byte outC, out byte outD, out byte outE, out byte outF, out byte outG, out byte outH)
        {
            outA = (byte) ((inValue >> 56) & 0xFF);
            outB = (byte) ((inValue >> 48) & 0xFF);
            outC = (byte) ((inValue >> 40) & 0xFF);
            outD = (byte) ((inValue >> 32) & 0xFF);
            outE = (byte) ((inValue >> 24) & 0xFF);
            outF = (byte) ((inValue >> 16) & 0xFF);
            outG = (byte) ((inValue >> 8) & 0xFF);
            outH = (byte) ((inValue) & 0xFF);
        }

        /// <summary>
        /// Packs the eight bytes into an integer value.
        /// </summary>
        static private long Pack(byte inA, byte inB, byte inC, byte inD, byte inE, byte inF, byte inG, byte inH)
        {
            return ((long) inA << 56) | ((long) inB << 48) | ((long) inC << 40) | ((long) inD << 32)
                | ((long) inE << 24) | ((long) inF << 16) | ((long) inG << 8) | ((long) inH);
        }

        /// <summary>
        /// Packs the eight bytes from the given array into an integer value.
        /// </summary>
        static private long Pack(byte[] inBytes)
        {
            #if DEBUG
            if (inBytes == null)
                throw new ArgumentNullException("inBytes");
            else if (inBytes.Length != Size)
                throw new ArgumentException("Byte array must be length 8", "outBytes");
            #endif // DEBUG

            return ((long) inBytes[0] << 56) | ((long) inBytes[1] << 48) | ((long) inBytes[2] << 40) | ((long) inBytes[3] << 32)
                | ((long) inBytes[4] << 24) | ((long) inBytes[5] << 16) | ((long) inBytes[6] << 8) | ((long) inBytes[7]);
        }

        /// <summary>
        /// Packs the eight bytes from the given array into an integer value.
        /// </summary>
        static private unsafe long Pack(byte* inBytes)
        {
            #if DEBUG
            if (inBytes == null)
                throw new ArgumentNullException("inBytes");
            #endif // DEBUG

            return ((long) inBytes[0] << 56) | ((long) inBytes[1] << 48) | ((long) inBytes[2] << 40) | ((long) inBytes[3] << 32)
                | ((long) inBytes[4] << 24) | ((long) inBytes[5] << 16) | ((long) inBytes[6] << 8) | ((long) inBytes[7]);
        }

        #endregion // Bytes

        #region Utils

        /// <summary>
        /// Stringifies the given value, optionally trimming off empty space.
        /// </summary>
        static private unsafe string Stringify(long inValue, bool inbTrim)
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
        static private long Pack(string inString)
        {
            if (inString == null)
                return 0;

            int strLength = inString.Length;
            if (strLength <= 0)
                return 0;

            #if DEBUG
            if (strLength > Size)
                throw new FormatException("Cannot pack from a string longer than 8 characters");
            #endif // DEBUG

            byte a = CharUtils.MapCC(inString[0]);
            byte b = strLength < 2 ? (byte) 0 : CharUtils.MapCC(inString[1]);
            byte c = strLength < 3 ? (byte) 0 : CharUtils.MapCC(inString[2]);
            byte d = strLength < 4 ? (byte) 0 : CharUtils.MapCC(inString[3]);
            byte e = strLength < 5 ? (byte) 0 : CharUtils.MapCC(inString[4]);
            byte f = strLength < 6 ? (byte) 0 : CharUtils.MapCC(inString[5]);
            byte g = strLength < 7 ? (byte) 0 : CharUtils.MapCC(inString[6]);
            byte h = strLength < 8 ? (byte) 0 : CharUtils.MapCC(inString[7]);

            return Pack(a, b, c, d, e, f, g, h);
        }

        /// <summary>
        /// Attempts to pack the string into an integer value.
        /// Returns false if the string is poorly formatted.
        /// </summary>
        static private unsafe bool TryPack(string inString, out long outValue)
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
        /// Writes an EightCC to the given byte array.
        /// </summary>
        static public void WriteTo(EightCC inFourCC, byte[] inBytes, int inOffset = 0)
        {
            EightCC.Serialization.WriteTo(inFourCC, inBytes, inOffset);
        }

        /// <summary>
        /// Writes an EightCC to the given stream.
        /// </summary>
        static public void WriteTo(EightCC inFourCC, Stream inStream)
        {
            EightCC.Serialization.WriteTo(inFourCC, inStream);
        }

        /// <summary>
        /// Reads an EightCC from the given byte array.
        /// </summary>
        static public EightCC ReadFrom(byte[] inBytes, int inOffset = 0)
        {
            return EightCC.Serialization.ReadFrom(inBytes, inOffset);
        }

        /// <summary>
        /// Reads an EightCC from the given stream.
        /// </summary>
        static public EightCC ReadFrom(Stream inStream)
        {
            return EightCC.Serialization.ReadFrom(inStream);
        }

        #endregion // Statics
    }

    /// <summary>
    /// Extension methods for dealing with EightCCs.
    /// </summary>
    static public class EightCCExtentions
    {
        #region Serialization

        /// <summary>
        /// Writes an EightCC value to this TextWriter.
        /// </summary>
        static public void Write(this TextWriter inWriter, EightCC inEightCC)
        {
            EightCC.Serialization.WriteTo(inEightCC, inWriter);
        }

        /// <summary>
        /// Writes an EightCC value to this BinaryWriter.
        /// </summary>
        static public void Write(this BinaryWriter inWriter, EightCC inEightCC)
        {
            EightCC.Serialization.WriteTo(inEightCC, inWriter);
        }

        /// <summary>
        /// Writes an EightCC value to this StringBuilder, optionally trimming spaces from the end.
        /// </summary>
        static public StringBuilder Append(this StringBuilder inBuilder, EightCC inEightCC, bool inbTrimSpaces = false)
        {
            EightCC.Serialization.WriteTo(inEightCC, inBuilder, inbTrimSpaces);
            return inBuilder;
        }

        /// <summary>
        /// Reads an EightCC value from this TextReader.
        /// </summary>
        static public EightCC ReadEightCC(this TextReader inReader)
        {
            return EightCC.Serialization.ReadFrom(inReader);
        }

        /// <summary>
        /// Reads an EightCC value from this BinaryReader.
        /// </summary>
        static public EightCC ReadEightCC(this BinaryReader inReader)
        {
            return EightCC.Serialization.ReadFrom(inReader);
        }

        #endregion // Serialization
    }

}