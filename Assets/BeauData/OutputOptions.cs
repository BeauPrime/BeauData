/*
 * Copyright (C) 2017 - 2019. Filament Games, LLC. All rights reserved.
 * Author:  Alex Beauchesne
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
