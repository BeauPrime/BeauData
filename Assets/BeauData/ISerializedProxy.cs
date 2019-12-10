/*
 * Copyright (C) 2017 - 2019. Filament Games, LLC. All rights reserved.
 * Author:  Alex Beauchesne
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