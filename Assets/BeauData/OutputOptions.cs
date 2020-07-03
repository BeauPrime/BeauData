/*
 * Copyright (C) 2017 - 2020. Autumn Beauchesne. All rights reserved.
 * Author:  Autumn Beauchesne
 * Date:    3 May 2017
 * 
 * File:    OutputOptions.cs
 * Purpose: Options for writing out a serialized object.
*/

using System;

namespace BeauData
{
    [Flags]
    public enum OutputOptions
    {
        /// <summary>
        /// No special properties.
        /// </summary>
        None            = 0,

        /// <summary>
        /// JSON and XML will be formatted.
        /// </summary>
        PrettyPrint     = 1,

        /// <summary>
        /// JSON will be output as a Base64 string.
        /// </summary>
        Base64          = 2,
    }
}
