/*
 * Copyright (C) 2017 - 2019. Filament Games, LLC. All rights reserved.
 * Author:  Alex Beauchesne
 * Date:    3 May 2017
 * 
 * File:    FieldOptions.cs
 * Purpose: Serialization behavior for fields.
*/

using System;

namespace BeauData
{
    [Flags]
    public enum FieldOptions
    {
        /// <summary>
        /// No special properties.
        /// </summary>
        None                = 0,

        /// <summary>
        /// This field is optional.
        /// </summary>
        Optional            = 0x01,

        /// <summary>
        /// This field would prefer to be written to an attribute
        /// instead of an element, if available.
        /// </summary>
        PreferAttribute     = 0x02
    }
}
