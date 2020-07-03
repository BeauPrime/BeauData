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

// Comment this out to disable lowercase letters
// If enabled, uppercase and lowercase letters will be treated as distinct
// If disabled, lowercase letters will be cast to uppercase
#define CASE_SENSITIVE

// -- END CONFIGURATION OPTIONS -- //

#if UNITY_EDITOR || UNITY_DEVELOPMENT || DEVELOPMENT
#define DEBUG
#endif // UNITY_EDITOR || UNITY_DEVELOPMENT || DEVELOPMENT

using System;
using System.IO;
using System.Runtime.InteropServices;

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

        private const int LENGTH = 4;
        private const string EMPTY_STRING = "    ";
        private const char PADDING_CHAR = ' ';

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

        /// <summary>
        /// Creates a FourCC with the given string representation.
        /// </summary>
        public FourCC(string inString)
        {
            m_Value = Pack(inString);
        }

        #region Factory

        /// <summary>
        /// Attempts to sanitize the given string.
        /// Returns if it was able to be sanitized.
        /// </summary>
        static public bool Sanitize(ref string ioString)
        {
            return TrySanitize(ref ioString);
        }

        /// <summary>
        /// Attempts to sanitize the given string.
        /// Returns the sanitized string, or null if unable to be sanitized.
        /// </summary>
        static public string Sanitize(string inString)
        {
            string str = inString;
            if (!TrySanitize(ref str))
                return null;
            return str;
        }

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
            string sanitizedString = inString;
            if (TrySanitize(ref sanitizedString))
            {
                int value = Pack(sanitizedString);
                outResult = new FourCC(value);
                return true;
            }

            outResult = default(FourCC);
            return false;
        }

        #endregion // Factory

        #region Overrides

        /// <summary>
        /// Returns the string representation of the FourCC.
        /// </summary>
        public override string ToString()
        {
            return Stringify(m_Value, false);
        }

        /// <summary>
        /// Returns the string representation of the FourCC,
        /// optionally trimming any trailing spaces.
        /// </summary>
        public string ToString(bool inbTrimSpaces)
        {
            return Stringify(m_Value, inbTrimSpaces);
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

            static private void RequireStreamBytes()
            {
                if (s_StreamBytes == null)
                    s_StreamBytes = new byte[LENGTH];
            }

            static private void RequireStreamChars()
            {
                if (s_StreamChars == null)
                    s_StreamChars = new char[LENGTH];
            }

            /// <summary>
            /// Writes the given FourCC to the given Stream.
            /// </summary>
            static public void WriteTo(FourCC inFourCC, Stream inStream)
            {
                RequireStreamBytes();

                Unpack(inFourCC.m_Value, s_StreamBytes);
                inStream.Write(s_StreamBytes, 0, LENGTH);
            }

            /// <summary>
            /// Writes the given FourCC to the given byte array.
            /// </summary>
            static public void WriteTo(FourCC inFourCC, byte[] inBytes, int inOffset = 0)
            {
                RequireStreamBytes();

                Unpack(inFourCC.m_Value, s_StreamBytes);
                Array.Copy(s_StreamBytes, 0, inBytes, inOffset, LENGTH);
            }

            /// <summary>
            /// Writes the given FourCC to the given TextWriter.
            /// Note that this will write spaces in place of empty bytes.
            /// </summary>
            static public void WriteTo(FourCC inFourCC, TextWriter inWriter)
            {
                RequireStreamBytes();
                RequireStreamChars();

                Unpack(inFourCC.m_Value, s_StreamBytes);
                for (int i = 0; i < LENGTH; ++i)
                {
                    byte val = s_StreamBytes[i];
                    s_StreamChars[i] = (val == 0) ? ' ' : (char) val;
                }

                inWriter.Write(s_StreamChars);
            }

            /// <summary>
            /// Writes the given FourCC to the given BinaryWriter.
            /// </summary>
            static public void WriteTo(FourCC inFourCC, BinaryWriter inWriter)
            {
                RequireStreamBytes();

                Unpack(inFourCC.m_Value, s_StreamBytes);
                inWriter.Write(s_StreamBytes);
            }

            /// <summary>
            /// Reads a FourCC from the given Stream.
            /// </summary>
            static public FourCC ReadFrom(Stream inStream)
            {
                RequireStreamBytes();

                int bytesRead = inStream.Read(s_StreamBytes, 0, LENGTH);
                for (int i = bytesRead; i < LENGTH; ++i)
                    s_StreamBytes[i] = 0;

                int value = Pack(s_StreamBytes);
                return new FourCC(value);
            }

            /// <summary>
            /// Reads a FourCC from the given byte array.
            /// </summary>
            static public FourCC ReadFrom(byte[] inBytes, int inOffset = 0)
            {
                RequireStreamBytes();

                int numBytesRead = Math.Min(LENGTH, inBytes.Length - inOffset);
                for (int i = 0; i < numBytesRead; ++i)
                    s_StreamBytes[i] = inBytes[inOffset + i];
                for (int i = numBytesRead; i < LENGTH; ++i)
                    s_StreamBytes[i] = 0;

                int value = Pack(s_StreamBytes);
                return new FourCC(value);
            }

            /// <summary>
            /// Reads a FourCC from the given TextReader.
            /// </summary>
            static public FourCC ReadFrom(TextReader inReader)
            {
                RequireStreamChars();
                RequireStreamBytes();

                int charsRead = inReader.Read(s_StreamChars, 0, LENGTH);
                for (int i = 0; i < charsRead; ++i)
                {
                    char read = s_StreamChars[i];
                    if (read == ' ')
                        s_StreamBytes[i] = 0;
                    else
                        s_StreamBytes[i] = (byte) read;
                }
                for (int i = charsRead; i < LENGTH; ++i)
                    s_StreamBytes[i] = 0;

                int value = Pack(s_StreamBytes);
                return new FourCC(value);
            }

            /// <summary>
            /// Reads a FourCC from the given BinaryReader.
            /// </summary>
            static public FourCC ReadFrom(BinaryReader inReader)
            {
                RequireStreamBytes();

                int bytesRead = inReader.Read(s_StreamBytes, 0, LENGTH);
                for (int i = bytesRead; i < LENGTH; ++i)
                    s_StreamBytes[i] = 0;

                int value = Pack(s_StreamBytes);
                return new FourCC(value);
            }
        }

        #endregion // Serialization

        #region Utils

        // Map of valid characters to byte values.
        // '\0' indicates the char is 0.
        // '.' indicates the char is an invalid character.
        // Most often, the char maps to itself.
        private const string MAP_TO_BYTE =
            #if CASE_SENSITIVE
            "\0...............................\0!.#$......+.-..0123456789.....?.ABCDEFGHIJKLMNOPQRSTUVWXYZ...._.abcdefghijklmnopqrstuvwxyz";
        #else
        "\0...............................\0!.#$......+.-..0123456789.....?.ABCDEFGHIJKLMNOPQRSTUVWXYZ...._.ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        #endif // CASE_SENSITIVE
        private const char INVALID_CHAR = '.';

        // Cached char array for stringifying values
        [ThreadStatic]
        static private char[] s_StringAssembler;

        static private void RequireStringAssembler()
        {
            if (s_StringAssembler == null)
                s_StringAssembler = new char[LENGTH];
        }

        /// <summary>
        /// Sanitizes the given string.
        /// Returns if it was able to be sanitized.
        /// </summary>
        static private bool TrySanitize(ref string ioString)
        {
            if (ioString == null || ioString.Length == 0)
            {
                ioString = EMPTY_STRING;
                return true;
            }

            // If the string is too long, it cannot be sanitized without truncation
            if (ioString.Length > LENGTH)
            {
                return false;
            }

            RequireStringAssembler();

            bool bCorrected = ioString.Length != LENGTH;
            bool bHasSpace = false;
            for (int i = 0; i < LENGTH; ++i)
            {
                char c = (i < ioString.Length) ? ioString[i] : ' ';

                if (c == ' ')
                {
                    bHasSpace = true;
                }
                else
                {
                    if (bHasSpace)
                    {
                        // if we encounter a non-space character after a space, this is not a valid string.
                        return false;
                    }
                }

                if (c > MAP_TO_BYTE.Length)
                {
                    return false;
                }
                char mapped = MAP_TO_BYTE[c];
                if (mapped == INVALID_CHAR)
                {
                    return false;
                }

                s_StringAssembler[i] = mapped;
                bCorrected |= (c != mapped);
            }

            if (bCorrected)
                ioString = new string(s_StringAssembler, 0, LENGTH);
            return true;
        }

        /// <summary>
        /// Stringifies the given value, optionally trimming off empty space.
        /// </summary>
        static private string Stringify(int inValue, bool inbTrim)
        {
            if (inValue == 0)
                return inbTrim ? string.Empty : EMPTY_STRING;

            RequireStringAssembler();

            char a = (char) ((inValue >> 24) & 0xFF);
            char b = (char) ((inValue >> 16) & 0xFF);
            char c = (char) ((inValue >> 8) & 0xFF);
            char d = (char) ((inValue) & 0xFF);

            if (a == '\0')
            {
                if (inbTrim)
                    return string.Empty;

                a = ' ';
            }
            if (b == '\0')
            {
                if (inbTrim)
                    return char.ToString(a);

                b = ' ';
            }
            if (c == '\0')
            {
                if (inbTrim)
                {
                    s_StringAssembler[0] = a;
                    s_StringAssembler[1] = b;
                    return new string(s_StringAssembler, 0, 2);
                }

                c = ' ';
            }
            if (d == '\0')
            {
                if (inbTrim)
                {
                    s_StringAssembler[0] = a;
                    s_StringAssembler[1] = b;
                    s_StringAssembler[2] = c;
                    return new string(s_StringAssembler, 0, 3);
                }

                d = ' ';
            }

            s_StringAssembler[0] = a;
            s_StringAssembler[1] = b;
            s_StringAssembler[2] = c;
            s_StringAssembler[3] = d;

            return new string(s_StringAssembler, 0, 4);
        }

        /// <summary>
        /// Maps a char to a byte.
        /// </summary>
        static private byte Map(char inChar)
        {
            #if DEBUG
            if (inChar > MAP_TO_BYTE.Length)
                throw new ArgumentException("Invalid character: " + inChar, "inChar");
            char mapped = MAP_TO_BYTE[inChar];
            if (mapped == INVALID_CHAR)
                throw new ArgumentException("Invalid character: " + inChar, "inChar");
            return (byte) mapped;
            #else
            return (byte) (MAP_TO_BYTE[inChar]);
            #endif // DEBUG
        }

        /// <summary>
        /// Unpacks an integer value into four bytes in an array.
        /// </summary>
        static private void Unpack(int inValue, byte[] outBytes)
        {
            #if DEBUG
            if (outBytes == null)
                throw new ArgumentNullException("outBytes");
            else if (outBytes.Length != LENGTH)
                throw new ArgumentException("Byte array must be length 4", "outBytes");
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
                throw new ArgumentNullException("outBytes");
            else if (inBytes.Length != LENGTH)
                throw new ArgumentException("Byte array must be length 4", "outBytes");
            #endif // DEBUG

            return (inBytes[0] << 24) | (inBytes[1] << 16) | (inBytes[2] << 8) | (inBytes[3]);
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
            if (strLength > LENGTH)
                throw new ArgumentException("Cannot pack from a string longer than 4 characters", "inString");
            #endif // DEBUG

            byte a = Map(inString[0]);
            byte b = strLength < 2 ? (byte) 0 : Map(inString[1]);
            byte c = strLength < 3 ? (byte) 0 : Map(inString[2]);
            byte d = strLength < 4 ? (byte) 0 : Map(inString[3]);

            return Pack(a, b, c, d);
        }

        #endregion // Utils
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