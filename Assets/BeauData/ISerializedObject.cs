/*
 * Copyright (C) 2017 - 2020. Autumn Beauchesne. All rights reserved.
 * Author:  Autumn Beauchesne
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