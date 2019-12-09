/*
 * Copyright (C) 2017 - 2019. Filament Games, LLC. All rights reserved.
 * Author:  Alex Beauchesne
 * Date:    3 May 2017
 * 
 * File:    ISerializedObject.cs
 * Purpose: Base interface for serialized objects.
 */

namespace BeauData
{
    public interface ISerializedObject
    {
        void Serialize(Serializer ioSerializer);
    }
}