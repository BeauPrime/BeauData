using System.Collections.Generic;
using UnityEngine;

namespace BeauData
{
    public abstract partial class Serializer
    {
        private bool Read_Texture2D(ref UnityEngine.Texture2D ioData)
        {
            if (ioData == null)
                ioData = new UnityEngine.Texture2D(2, 2);

            byte[] bytes = null;
            bool bSuccess = Read_ByteArray(ref bytes);
            if (bSuccess)
                bSuccess &= ioData.LoadImage(bytes);

            return bSuccess;
        }

        private void Write_Texture2DPNG(ref UnityEngine.Texture2D ioData)
        {
            byte[] png = ioData.EncodeToPNG();
            Write_ByteArray(ref png);
        }

        private void Write_Texture2DJPG(ref UnityEngine.Texture2D ioData)
        {
            byte[] png = ioData.EncodeToJPG();
            Write_ByteArray(ref png);
        }

        private ReadFunc<UnityEngine.Texture2D> Read_Texture2D_Cached;
        private WriteFunc<UnityEngine.Texture2D> Write_Texture2DPNG_Cached;
        private WriteFunc<UnityEngine.Texture2D> Write_Texture2DJPG_Cached;

        private WriteFunc<UnityEngine.Texture2D> GetTextureWriter(TextureOptions inTextureOptions)
        {
            if ((inTextureOptions & TextureOptions.JPG) != 0)
                return Write_Texture2DJPG_Cached ?? (Write_Texture2DJPG_Cached = Write_Texture2DJPG);
            return Write_Texture2DPNG_Cached ?? (Write_Texture2DPNG_Cached = Write_Texture2DPNG);
        }

        public void Serialize(string inKey, ref UnityEngine.Texture2D ioData, TextureOptions inTextureOptions = TextureOptions.Default, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerializeUnity<UnityEngine.Texture2D>(inKey, ref ioData, inOptions,
                Read_Texture2D_Cached ?? (Read_Texture2D_Cached = Read_Texture2D), 
                GetTextureWriter(inTextureOptions));
        }

        public void Array(string inKey, ref List<UnityEngine.Texture2D> ioArray, TextureOptions inTextureOptions = TextureOptions.Default, FieldOptions inOptions = FieldOptions.None)
        {
            DoArrayUnity<UnityEngine.Texture2D>(inKey, ref ioArray, inOptions,
                Read_Texture2D_Cached ?? (Read_Texture2D_Cached = Read_Texture2D), 
                GetTextureWriter(inTextureOptions));
        }

        public void Array(string inKey, ref UnityEngine.Texture2D[] ioArray, TextureOptions inTextureOptions = TextureOptions.Default, FieldOptions inOptions = FieldOptions.None)
        {
            DoArrayUnity<UnityEngine.Texture2D>(inKey, ref ioArray, inOptions,
                Read_Texture2D_Cached ?? (Read_Texture2D_Cached = Read_Texture2D), 
                GetTextureWriter(inTextureOptions));
        }

        public void Set(string inKey, ref HashSet<UnityEngine.Texture2D> ioSet, TextureOptions inTextureOptions = TextureOptions.Default, FieldOptions inOptions = FieldOptions.None)
        {
            DoSetUnity<UnityEngine.Texture2D>(inKey, ref ioSet, inOptions,
                Read_Texture2D_Cached ?? (Read_Texture2D_Cached = Read_Texture2D), 
                GetTextureWriter(inTextureOptions));
        }

        public void Map(string inKey, ref Dictionary<string, UnityEngine.Texture2D> ioMap, TextureOptions inTextureOptions = TextureOptions.Default, FieldOptions inOptions = FieldOptions.None)
        {
            DoMapUnity<UnityEngine.Texture2D>(inKey, ref ioMap, inOptions,
                Read_Texture2D_Cached ?? (Read_Texture2D_Cached = Read_Texture2D), 
                GetTextureWriter(inTextureOptions));
        }

        public void Map(string inKey, ref Dictionary<int, UnityEngine.Texture2D> ioMap, TextureOptions inTextureOptions = TextureOptions.Default, FieldOptions inOptions = FieldOptions.None)
        {
            DoMapUnity<UnityEngine.Texture2D>(inKey, ref ioMap, inOptions,
                Read_Texture2D_Cached ?? (Read_Texture2D_Cached = Read_Texture2D), 
                GetTextureWriter(inTextureOptions));
        }
    }
}