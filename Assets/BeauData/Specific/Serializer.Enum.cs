#if NET_4_6
#define USE_ENUM_CONSTRAINT
#endif // NET_4_6
using System;
using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        private TypeCode GetEnumTypeCode(Type inEnumType)
        {
            if (SerializerVersion < VERSION_ENUM_COMPRESSION)
                return TypeCode.Int32;
            
            Type underlyingType = System.Enum.GetUnderlyingType(inEnumType);
            return System.Type.GetTypeCode(underlyingType);
        }

        private bool Read_Enum<T>(ref T inEnum)
        #if USE_ENUM_CONSTRAINT
        where T : Enum
        #else
        where T : struct, IConvertible
        #endif // USE_ENUM_CONSTRAINT
        {
            Type enumType = typeof(T);

            if (!IsBinary())
            {
                string str = null;
                bool bSuccess = Read_String(ref str);
                if (bSuccess)
                {
                    try { inEnum = (T) System.Enum.Parse(enumType, str, true); }
                    catch (Exception) { AddErrorMessage("Value {0} unable to be translated to Enum {1}", str, enumType.FullName); bSuccess = false; }
                }
                return bSuccess;
            }
            else
            {
                TypeCode typeCode = GetEnumTypeCode(enumType);
                switch(typeCode)
                {
                    case TypeCode.Byte:
                        {
                            byte num = default(byte);
                            bool bSuccess = Read_Byte(ref num);
                            if (bSuccess)
                                inEnum = (T) System.Enum.ToObject(enumType, num);
                            return bSuccess;
                        }

                    case TypeCode.SByte:
                    case TypeCode.Int16:
                        {
                            short num = default(short);
                            bool bSuccess = Read_Int16(ref num);
                            if (bSuccess)
                                inEnum = (T) System.Enum.ToObject(enumType, num);
                            return bSuccess;
                        }

                        case TypeCode.UInt16:
                        {
                            ushort num = default(ushort);
                            bool bSuccess = Read_UInt16(ref num);
                            if (bSuccess)
                                inEnum = (T) System.Enum.ToObject(enumType, num);
                            return bSuccess;
                        }

                    case TypeCode.Int32:
                    default:
                        {
                            int num = default(int);
                            bool bSuccess = Read_Int32(ref num);
                            if (bSuccess)
                                inEnum = (T) System.Enum.ToObject(enumType, num);
                            return bSuccess;
                        }

                    case TypeCode.UInt32:
                        {
                            uint num = default(uint);
                            bool bSuccess = Read_UInt32(ref num);
                            if (bSuccess)
                                inEnum = (T) System.Enum.ToObject(enumType, num);
                            return bSuccess;
                        }

                    case TypeCode.Int64:
                        {
                            long num = default(long);
                            bool bSuccess = Read_Int64(ref num);
                            if (bSuccess)
                                inEnum = (T) System.Enum.ToObject(enumType, num);
                            return bSuccess;
                        }

                    case TypeCode.UInt64:
                        {
                            ulong num = default(ulong);
                            bool bSuccess = Read_UInt64(ref num);
                            if (bSuccess)
                                inEnum = (T) System.Enum.ToObject(enumType, num);
                            return bSuccess;
                        }
                }
            }
        }

        private void Write_Enum<T>(ref T inEnum)
        #if USE_ENUM_CONSTRAINT
        where T : Enum
        #else
        where T : struct, IConvertible
        #endif // USE_ENUM_CONSTRAINT
        {
            Type enumType = typeof(T);

            if (!IsBinary())
            {
                string str = inEnum.ToString();
                Write_String(ref str);
            }
            else
            {
                TypeCode typeCode = GetEnumTypeCode(enumType);
                switch(typeCode)
                {
                    case TypeCode.Byte:
                        {
                            byte num = Convert.ToByte(inEnum);
                            Write_Byte(ref num);
                            break;
                        }

                    case TypeCode.SByte:
                    case TypeCode.Int16:
                        {
                            short num = Convert.ToInt16(inEnum);
                            Write_Int16(ref num);
                            break;
                        }

                    case TypeCode.UInt16:
                        {
                            ushort num = Convert.ToUInt16(inEnum);
                            Write_UInt16(ref num);
                            break;
                        }

                    case TypeCode.Int32:
                    default:
                        {
                            int num = Convert.ToInt32(inEnum);
                            Write_Int32(ref num);
                            break;
                        }

                    case TypeCode.UInt32:
                        {
                            uint num = Convert.ToUInt32(inEnum);
                            Write_UInt32(ref num);
                            break;
                        }

                    case TypeCode.Int64:
                        {
                            long num = Convert.ToInt64(inEnum);
                            Write_Int64(ref num);
                            break;
                        }

                    case TypeCode.UInt64:
                        {
                            ulong num = Convert.ToUInt64(inEnum);
                            Write_UInt64(ref num);
                            break;
                        }
                }
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