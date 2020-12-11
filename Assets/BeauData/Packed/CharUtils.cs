/*
 * Copyright (C) 2017 - 2020. Autumn Beauchesne. All rights reserved.
 * Author:  Autumn Beauchesne
 * Date:    8 April 2020
 * 
 * File:    CharUtils.cs
 * Purpose: Utility functions for packed char structs (FourCC, EightCC)
 */

// Comment this out to disable lowercase letters
// If enabled, uppercase and lowercase letters will be treated as distinct
// If disabled, lowercase letters will be cast to uppercase
#define CASE_SENSITIVE

#if UNITY_EDITOR || UNITY_DEVELOPMENT || DEVELOPMENT
#define DEBUG
#endif // UNITY_EDITOR || UNITY_DEVELOPMENT || DEVELOPMENT

using System;

namespace BeauData.Packed
{
    internal class CharUtils
    {
        // Map of valid characters to byte values.
        // '\0' indicates the char is 0.
        // '.' indicates the char is an invalid character.
        // Most often, the char maps to itself.
        static private readonly string ByteValidationChars =
            #if CASE_SENSITIVE
            "\0...............................\0!.#$......+.-..0123456789.....?.ABCDEFGHIJKLMNOPQRSTUVWXYZ...._.abcdefghijklmnopqrstuvwxyz";
        #else
            "\0...............................\0!.#$......+.-..0123456789.....?.ABCDEFGHIJKLMNOPQRSTUVWXYZ...._.ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        #endif // CASE_SENSITIVE

        private const int ByteValidationCharsLength = 123;
        private const char InvalidChar = '.';
        
        internal const char PaddingChar = ' ';

        /// <summary>
        /// Maps a char to a byte.
        /// </summary>
        static internal byte MapCC(char inChar)
        {
            #if DEBUG
            if (inChar >= ByteValidationCharsLength)
                throw new ArgumentException("Invalid character: " + inChar, "inChar");
            char mapped = ByteValidationChars[inChar];
            if (mapped == InvalidChar)
                throw new ArgumentException("Invalid character: " + inChar, "inChar");
            return (byte) mapped;
            #else
            return (byte) (ByteValidationChars[inChar]);
            #endif // DEBUG
        }

        /// <summary>
        /// Tries to map a char to a byte.
        /// </summary>
        static internal bool TryMapCC(char inChar, out byte outByte)
        {
            if (inChar >= ByteValidationCharsLength)
            {
                outByte = 0;
                return false;
            }

            char mapped = ByteValidationChars[inChar];
            if (mapped == InvalidChar)
            {
                outByte = 0;
                return false;
            }

            outByte = (byte) mapped;
            return true;
        }
    }
}