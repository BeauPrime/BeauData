/*
 * Copyright (C) 2017 - 2020. Autumn Beauchesne. All rights reserved.
 * Author:  Autumn Beauchesne
 * Date:    9 Dec 2019
 * 
 * File:    ISerializedProxy.cs
 * Purpose: Interface for proxy objects.
 */

namespace BeauData
{
    public interface ISerializedProxy<T>
    {
        T GetProxyValue(ISerializerContext inContext);
        void SetProxyValue(T inValue, ISerializerContext inContext);
    }
}