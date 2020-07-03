/*
 * Copyright (C) 2017 - 2020. Autumn Beauchesne. All rights reserved.
 * Author:  Autumn Beauchesne
 * Date:    5 Dec 2019
 * 
 * File:    ISerializedCallbacks.cs
 * Purpose: Base interface for serialization callbacks.
 */

namespace BeauData
{
    public interface ISerializedCallbacks
    {
        void PostSerialize(Serializer.Mode inMode, ISerializerContext inContext);
    }
}