using System.Collections.Generic;

namespace BeauData
{
    public abstract partial class Serializer
    {
        static private void Serialize_Bounds(ref UnityEngine.Bounds ioData, Serializer ioSerializer)
        {
            UnityEngine.Vector3 min = ioData.min, max = ioData.max;

            ioSerializer.Serialize("min", ref min, FieldOptions.PreferAttribute);
            ioSerializer.Serialize("max", ref max, FieldOptions.PreferAttribute);

            if (ioSerializer.IsReading)
            {
                ioData.SetMinMax(min, max);
            }
        }

        static private void Serialize_BoundsInt(ref UnityEngine.BoundsInt ioData, Serializer ioSerializer)
        {
            UnityEngine.Vector3Int min = ioData.min, max = ioData.max;

            ioSerializer.Serialize("min", ref min, FieldOptions.PreferAttribute);
            ioSerializer.Serialize("max", ref max, FieldOptions.PreferAttribute);

            if (ioSerializer.IsReading)
            {
                ioData.SetMinMax(min, max);
            }
        }

        static private void Serialize_Quaternion(ref UnityEngine.Quaternion ioData, Serializer ioSerializer)
        {
            ioSerializer.Serialize("x", ref ioData.x, FieldOptions.PreferAttribute);
            ioSerializer.Serialize("y", ref ioData.y, FieldOptions.PreferAttribute);
            ioSerializer.Serialize("z", ref ioData.z, FieldOptions.PreferAttribute);
            ioSerializer.Serialize("w", ref ioData.w, FieldOptions.PreferAttribute);
        }

        static private void Serialize_Rect(ref UnityEngine.Rect ioData, Serializer ioSerializer)
        {
            float x = ioData.x, y = ioData.y, width = ioData.width, height = ioData.height;

            ioSerializer.Serialize("x", ref x, FieldOptions.PreferAttribute);
            ioSerializer.Serialize("y", ref y, FieldOptions.PreferAttribute);
            ioSerializer.Serialize("w", ref width, FieldOptions.PreferAttribute);
            ioSerializer.Serialize("h", ref height, FieldOptions.PreferAttribute);

            if (ioSerializer.IsReading)
            {
                ioData.Set(x, y, width, height);
            }
        }

        static private void Serialize_RectInt(ref UnityEngine.RectInt ioData, Serializer ioSerializer)
        {
            int x = ioData.x, y = ioData.y, width = ioData.width, height = ioData.height;

            ioSerializer.Serialize("x", ref x, FieldOptions.PreferAttribute);
            ioSerializer.Serialize("y", ref y, FieldOptions.PreferAttribute);
            ioSerializer.Serialize("w", ref width, FieldOptions.PreferAttribute);
            ioSerializer.Serialize("h", ref height, FieldOptions.PreferAttribute);

            if (ioSerializer.IsReading)
            {
                ioData.x = x;
                ioData.y = y;
                ioData.width = width;
                ioData.height = height;
            }
        }

        static private void Serialize_Vector2(ref UnityEngine.Vector2 ioData, Serializer ioSerializer)
        {
            ioSerializer.Serialize("x", ref ioData.x, FieldOptions.PreferAttribute);
            ioSerializer.Serialize("y", ref ioData.y, FieldOptions.PreferAttribute);
        }

        static private void Serialize_Vector2Int(ref UnityEngine.Vector2Int ioData, Serializer ioSerializer)
        {
            int x = ioData.x, y = ioData.y;

            ioSerializer.Serialize("x", ref x, FieldOptions.PreferAttribute);
            ioSerializer.Serialize("y", ref y, FieldOptions.PreferAttribute);

            if (ioSerializer.IsReading)
            {
                ioData.x = x;
                ioData.y = y;
            }
        }

        static private void Serialize_Vector3(ref UnityEngine.Vector3 ioData, Serializer ioSerializer)
        {
            ioSerializer.Serialize("x", ref ioData.x, FieldOptions.PreferAttribute);
            ioSerializer.Serialize("y", ref ioData.y, FieldOptions.PreferAttribute);
            ioSerializer.Serialize("z", ref ioData.z, FieldOptions.PreferAttribute);
        }

        static private void Serialize_Vector3Int(ref UnityEngine.Vector3Int ioData, Serializer ioSerializer)
        {
            int x = ioData.x, y = ioData.y, z = ioData.z;

            ioSerializer.Serialize("x", ref x, FieldOptions.PreferAttribute);
            ioSerializer.Serialize("y", ref y, FieldOptions.PreferAttribute);
            ioSerializer.Serialize("z", ref z, FieldOptions.PreferAttribute);

            if (ioSerializer.IsReading)
            {
                ioData.x = x;
                ioData.y = y;
                ioData.z = z;
            }
        }

        static private void Serialize_Vector4(ref UnityEngine.Vector4 ioData, Serializer ioSerializer)
        {
            ioSerializer.Serialize("x", ref ioData.x, FieldOptions.PreferAttribute);
            ioSerializer.Serialize("y", ref ioData.y, FieldOptions.PreferAttribute);
            ioSerializer.Serialize("z", ref ioData.z, FieldOptions.PreferAttribute);
            ioSerializer.Serialize("w", ref ioData.w, FieldOptions.PreferAttribute);
        }
    }
}
