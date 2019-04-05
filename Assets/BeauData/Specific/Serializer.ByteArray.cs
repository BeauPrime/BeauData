using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        protected abstract bool Read_ByteArray(ref System.Byte[] ioData);
        protected abstract void Write_ByteArray(ref System.Byte[] ioData);

        public void Serialize(string inKey, ref System.Byte[] ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoSerialize<System.Byte[]>(inKey, ref ioData, inOptions, Read_ByteArray, Write_ByteArray);
        }

        public void Array(string inKey, ref List<System.Byte[]> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.Byte[]>(inKey, ref ioArray, inOptions, Read_ByteArray, Write_ByteArray);
        }

        public void Array(string inKey, ref System.Byte[][] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoArray<System.Byte[]>(inKey, ref ioArray, inOptions, Read_ByteArray, Write_ByteArray);
        }

        public void Set(string inKey, ref HashSet<System.Byte[]> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoSet<System.Byte[]>(inKey, ref ioSet, inOptions, Read_ByteArray, Write_ByteArray);
        }

        public void Map(string inKey, ref Dictionary<string, System.Byte[]> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.Byte[]>(inKey, ref ioMap, inOptions, Read_ByteArray, Write_ByteArray);
        }

        public void Map(string inKey, ref Dictionary<int, System.Byte[]> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoMap<System.Byte[]>(inKey, ref ioMap, inOptions, Read_ByteArray, Write_ByteArray);
        }
    }
}
