using System.Collections.Generic;
using SerializedType = UnityEngine.Texture2D;
using UnityEngine;

namespace BeauData
{
    public abstract partial class Serializer
    {
        private bool Read_Texture2D(ref SerializedType ioData)
        {
            if (ioData == null)
                ioData = new SerializedType(2, 2);

            byte[] bytes = null;
            bool bSuccess = Read_ByteArray(ref bytes);
            if (bSuccess)
                bSuccess &= ioData.LoadImage(bytes);

            return bSuccess;
        }

        private void Write_Texture2DPNG(ref SerializedType ioData)
        {
            byte[] png = ioData.EncodeToPNG();
            Write_ByteArray(ref png);
        }

        private void Write_Texture2DJPG(ref SerializedType ioData)
        {
            byte[] png = ioData.EncodeToJPG();
            Write_ByteArray(ref png);
        }

        private WriteFunc<SerializedType> GetTextureWriter(TextureOptions inTextureOptions)
        {
            if ((inTextureOptions & TextureOptions.JPG) != 0)
                return Write_Texture2DJPG;
            return Write_Texture2DPNG;
        }

        public void Serialize(string inKey, ref SerializedType ioData, TextureOptions inTextureOptions = TextureOptions.Default, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerializeUnity<SerializedType>(inKey, ref ioData, inOptions, Read_Texture2D, GetTextureWriter(inTextureOptions));
        }

        public void Array(string inKey, ref List<SerializedType> ioArray, TextureOptions inTextureOptions = TextureOptions.Default, FieldOptions inOptions = FieldOptions.None)
        {
            DoArrayUnity<SerializedType>(inKey, ref ioArray, inOptions, Read_Texture2D, GetTextureWriter(inTextureOptions));
        }

        public void Array(string inKey, ref SerializedType[] ioArray, TextureOptions inTextureOptions = TextureOptions.Default, FieldOptions inOptions = FieldOptions.None)
        {
            DoArrayUnity<SerializedType>(inKey, ref ioArray, inOptions, Read_Texture2D, GetTextureWriter(inTextureOptions));
        }

        public void Set(string inKey, ref HashSet<SerializedType> ioSet, TextureOptions inTextureOptions = TextureOptions.Default, FieldOptions inOptions = FieldOptions.None)
        {
            DoSetUnity<SerializedType>(inKey, ref ioSet, inOptions, Read_Texture2D, GetTextureWriter(inTextureOptions));
        }

        public void Map(string inKey, ref Dictionary<string, SerializedType> ioMap, TextureOptions inTextureOptions = TextureOptions.Default, FieldOptions inOptions = FieldOptions.None)
        {
            DoMapUnity<SerializedType>(inKey, ref ioMap, inOptions, Read_Texture2D, GetTextureWriter(inTextureOptions));
        }
    }
}
