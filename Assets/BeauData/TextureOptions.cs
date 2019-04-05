/*
 * Copyright (C) 2017 - 2019. Filament Games, LLC. All rights reserved.
 * Author:  Alex Beauchesne
 * Date:    16 May 2017
 * 
 * File:    TextureOptions.cs
 * Purpose: Serialization behavior for textures.
*/

using System;

namespace BeauData
{
    [Flags]
    public enum TextureOptions
    {
        /// <summary>
        /// Default. Serializes to PNG.
        /// </summary>
        Default            = PNG,

        /// <summary>
        /// Serializes the texture in PNG format.
        /// </summary>
        PNG             = 0x001,

        /// <summary>
        /// Serializes the texture in JPEG format.
        /// </summary>
        JPG             = 0x002,
    }
}
