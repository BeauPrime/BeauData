using System;
using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        public void Custom<T>(string inKey, ref T ioData, FieldOptions inOptions = FieldOptions.None)
        {
            var serializer = TypeUtility.CustomSerializer<T>();
            if (serializer == null)
            {
                AddErrorMessage("No serialization function registered for type '{0}'.", typeof(T).Name);
                return;
            }

            DoCustom(inKey, ref ioData, inOptions, serializer);
        }

        public void Custom<T>(string inKey, ref T ioData, T inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            var serializer = TypeUtility.CustomSerializer<T>();
            if (serializer == null)
            {
                AddErrorMessage("No serialization function registered for type '{0}'.", typeof(T).Name);
                return;
            }

            DoCustom(inKey, ref ioData, inDefault, inOptions, serializer);
        }

        public void CustomArray<T>(string inKey, ref List<T> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            var serializer = TypeUtility.CustomSerializer<T>();
            if (serializer == null)
            {
                AddErrorMessage("No serialization function registered for type '{0}'.", typeof(T).Name);
                return;
            }

            DoCustomArray(inKey, ref ioArray, inOptions, serializer);
        }

        public void CustomArray<T>(string inKey, ref T[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            var serializer = TypeUtility.CustomSerializer<T>();
            if (serializer == null)
            {
                AddErrorMessage("No serialization function registered for type '{0}'.", typeof(T).Name);
                return;
            }

            DoCustomArray(inKey, ref ioArray, inOptions, serializer);
        }

        public void CustomSet<T>(string inKey, ref HashSet<T> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            var serializer = TypeUtility.CustomSerializer<T>();
            if (serializer == null)
            {
                AddErrorMessage("No serialization function registered for type '{0}'.", typeof(T).Name);
                return;
            }

            DoCustomSet(inKey, ref ioSet, inOptions, serializer);
        }

        public void CustomMap<T>(string inKey, ref Dictionary<string, T> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            var serializer = TypeUtility.CustomSerializer<T>();
            if (serializer == null)
            {
                AddErrorMessage("No serialization function registered for type '{0}'.", typeof(T).Name);
                return;
            }

            DoCustomMap(inKey, ref ioMap, inOptions, serializer);
        }

        public void CustomMap<T>(string inKey, ref Dictionary<int, T> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            var serializer = TypeUtility.CustomSerializer<T>();
            if (serializer == null)
            {
                AddErrorMessage("No serialization function registered for type '{0}'.", typeof(T).Name);
                return;
            }

            DoCustomMap(inKey, ref ioMap, inOptions, serializer);
        }
    }
}