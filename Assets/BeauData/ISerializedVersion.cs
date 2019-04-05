/*
 * Copyright (C) 2017 - 2019. Filament Games, LLC. All rights reserved.
 * Author:  Alex Beauchesne
 * Date:    3 May 2017
 * 
 * File:    ISerializedVersion.cs
 * Purpose: Provides versioning info for an ISerializedObject.
*/

namespace BeauData
{
    /// <summary>
    /// Implement this on an ISerializedObject to provide versioning info.
    /// Version defaults to 1 if this isn't implemented.
    /// </summary>
    public interface ISerializedVersion
    {
        ushort Version { get; }
    }
}