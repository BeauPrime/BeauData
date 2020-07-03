/*
 * Copyright (C) 2017 - 2020. Autumn Beauchesne. All rights reserved.
 * Author:  Autumn Beauchesne
 * Date:    5 Dec 2019
 * 
 * File:    ISerializerContext.cs
 * Purpose: Base interface for additional serializer context.
 */

namespace BeauData
{
    public interface ISerializerContext
    {
        bool TryResolveAsset<T>(string inId, out T outObject) where T : class;
        bool TryGetAssetId<T>(T inObject, out string outId) where T : class;
    }
}