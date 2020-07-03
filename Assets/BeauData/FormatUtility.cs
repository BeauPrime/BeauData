/*
 * Copyright (C) 2017 - 2020. Autumn Beauchesne. All rights reserved.
 * Author:  Autumn Beauchesne
 * Date:    2 Feb 2018
 * 
 * File:    FormatUtility.cs
 * Purpose: Format extension methods.
 */

using System;
using BeauData.Format;

namespace BeauData
{
    /// <summary>
    /// Serialization format shortcuts.
    /// </summary>
    static public class FormatUtility
    {
        /// <summary>
        /// Returns the default file extension for the given format.
        /// </summary>
        static public string Extension(this Serializer.Format inFormat)
        {
            if (inFormat == Serializer.Format.AutoDetect)
                inFormat = Serializer.DefaultWriteFormat;

            switch (inFormat)
            {
                case Serializer.Format.Binary:
                    return BinarySerializer.FILE_EXTENSION;
                case Serializer.Format.GZIP:
                    return GZIPSerializer.FILE_EXTENSION;
                case Serializer.Format.JSON:
                    return JSONSerializer.FILE_EXTENSION;
                case Serializer.Format.XML:
                    return XMLSerializer.FILE_EXTENSION;
                default:
                    throw new Exception("Unknown format!");
            }
        }

        /// <summary>
        /// Returns if the given format is a binary format.
        /// </summary>
        static public bool IsBinary(this Serializer.Format inFormat)
        {
            switch (inFormat)
            {
                case Serializer.Format.Binary:
                case Serializer.Format.GZIP:
                    return true;

                default:
                    return false;
            }
        }
    }
}