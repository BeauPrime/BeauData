using System;
using System.Runtime.InteropServices;
using BeauData.Format;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace BeauData.Editor
{
    [CreateAssetMenu(menuName = "BeauData/TestContext")]
    public class TestContext : ScriptableObject, ISerializerContext
    {
        public Sprite[] sprites;

        bool ISerializerContext.TryGetAssetId<T>(T inObject, out string outId)
        {
            if (typeof(T) == typeof(Sprite))
            {
                Sprite spr = inObject as Sprite;
                if (spr != null && Array.IndexOf(sprites, spr) >= 0)
                {
                    outId = spr.name;
                    return true;
                }
            }

            outId = null;
            return false;
        }

        bool ISerializerContext.TryResolveAsset<T>(string inId, out T outObject)
        {
            if (typeof(T) == typeof(Sprite))
            {
                foreach (var spr in sprites)
                {
                    if (spr.name == inId)
                    {
                        outObject = spr as T;
                        return true;
                    }
                }
            }

            outObject = null;
            return false;
        }
    }
}