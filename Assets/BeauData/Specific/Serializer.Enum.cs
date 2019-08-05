#if NET_4_6
#define USE_ENUM_CONSTRAINT
#endif // NET_4_6

using System;
using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        private bool Read_Enum<T>(ref T inEnum)
        #if USE_ENUM_CONSTRAINT
        where T : Enum
        #else
        where T : struct, IConvertible
        #endif // USE_ENUM_CONSTRAINT
        {
            if (!IsBinary())
            {
                string str = null;
                bool bSuccess = Read_String(ref str);
                if (bSuccess)
                {
                    try { inEnum = (T) System.Enum.Parse(typeof(T), str, true); }
                    catch (Exception) { AddErrorMessage("Value {0} unable to be translated to Enum {1}", str, typeof(T).FullName); bSuccess = false; }
                }
                return bSuccess;
            }
            else
            {
                int num = default(int);
                bool bSuccess = Read_Int32(ref num);
                if (bSuccess)
                    // Not optimal
                    inEnum = (T) (object) num;
                return bSuccess;
            }
        }

        private void Write_Enum<T>(ref T inEnum)
        #if USE_ENUM_CONSTRAINT
        where T : Enum
        #else
        where T : struct, IConvertible
        #endif // USE_ENUM_CONSTRAINT
        {
            if (!IsBinary())
            {
                string str = inEnum.ToString();
                Write_String(ref str);
            }
            else
            {
                // Also not optimal
                int num = (int) (object) inEnum;
                Write_Int32(ref num);
            }
        }

        public void Enum<T>(string inKey, ref T ioData, FieldOptions inOptions = FieldOptions.None)
        #if USE_ENUM_CONSTRAINT
        where T : Enum
        #else
        where T : struct, IConvertible
        #endif // USE_ENUM_CONSTRAINT
        {
            DoSerialize<T>(inKey, ref ioData, inOptions, Read_Enum<T>, Write_Enum<T>);
        }

        public void Enum<T>(string inKey, ref T ioData, T inDefault, FieldOptions inOptions = FieldOptions.None)
        #if USE_ENUM_CONSTRAINT
        where T : Enum
        #else
        where T : struct, IConvertible
        #endif // USE_ENUM_CONSTRAINT
        {
            DoSerialize<T>(inKey, ref ioData, inDefault, inOptions, Read_Enum<T>, Write_Enum<T>);
        }

        public void EnumArray<T>(string inKey, ref List<T> ioArray, FieldOptions inOptions = FieldOptions.None)
        #if USE_ENUM_CONSTRAINT
        where T : Enum
        #else
        where T : struct, IConvertible
        #endif // USE_ENUM_CONSTRAINT
        {
            DoArray<T>(inKey, ref ioArray, inOptions, Read_Enum<T>, Write_Enum<T>);
        }

        public void EnumArray<T>(string inKey, ref T[] ioArray, FieldOptions inOptions = FieldOptions.None)
        #if USE_ENUM_CONSTRAINT
        where T : Enum
        #else
        where T : struct, IConvertible
        #endif // USE_ENUM_CONSTRAINT
        {
            DoArray<T>(inKey, ref ioArray, inOptions, Read_Enum<T>, Write_Enum<T>);
        }

        public void EnumSet<T>(string inKey, ref HashSet<T> ioSet, FieldOptions inOptions = FieldOptions.None)
        #if USE_ENUM_CONSTRAINT
        where T : Enum
        #else
        where T : struct, IConvertible
        #endif // USE_ENUM_CONSTRAINT
        {
            DoSet<T>(inKey, ref ioSet, inOptions, Read_Enum<T>, Write_Enum<T>);
        }

        public void EnumMap<T>(string inKey, ref Dictionary<string, T> ioMap, FieldOptions inOptions = FieldOptions.None)
        #if USE_ENUM_CONSTRAINT
        where T : Enum
        #else
        where T : struct, IConvertible
        #endif // USE_ENUM_CONSTRAINT
        {
            DoMap<T>(inKey, ref ioMap, inOptions, Read_Enum<T>, Write_Enum<T>);
        }

        public void EnumMap<T>(string inKey, ref Dictionary<int, T> ioMap, FieldOptions inOptions = FieldOptions.None)
        #if USE_ENUM_CONSTRAINT
        where T : Enum
        #else
        where T : struct, IConvertible
        #endif // USE_ENUM_CONSTRAINT
        {
            DoMap<T>(inKey, ref ioMap, inOptions, Read_Enum<T>, Write_Enum<T>);
        }
    }
}