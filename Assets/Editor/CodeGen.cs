using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace BeauData.Editor
{
    static public class CodeGen
    {
        private const string BASIC_TEMPLATE_PATH = "Assets/Editor/Serializer.Generic.Template.txt";
        private const string STRUCT_TEMPLATE_PATH = "Assets/Editor/Serializer.Struct.Template.txt";
        private const string GENERATED_PATH = "Assets/BeauData/Generated/Serializer.%TypeName%.cs";

        static private readonly Type[] BASIC_TYPES = new Type[]
        {
            typeof(Byte),
            typeof(Boolean),
            typeof(Double),
            typeof(Guid),
            typeof(Int16),
            typeof(Int32),
            typeof(Int64),
            typeof(Single),
            typeof(String),
            typeof(UInt16),
            typeof(UInt32),
            typeof(UInt64)
        };

        static private readonly Type[] STRUCT_TYPES = new Type[]
        {
            typeof(Vector2),
            typeof(Vector2Int),
            typeof(Vector3),
            typeof(Vector3Int),
            typeof(Vector4),
            typeof(Quaternion),
            typeof(Rect),
            typeof(RectInt),
            typeof(Bounds),
            typeof(BoundsInt)
        };

        static private string s_GenericTemplate = null;
        static private string s_StructTemplate = null;

        [MenuItem("Edit/BeauData/Generate Generic")]
        static private void Generate()
        {
            foreach (var type in BASIC_TYPES)
                GenerateBasicSerializer(type);

            foreach (var type in STRUCT_TYPES)
                GenerateStructSerializer(type);
        }

        static private void GenerateBasicSerializer(Type inType)
        {
            string path = GENERATED_PATH.Replace("%TypeName%", inType.Name);

            if (s_GenericTemplate == null)
                s_GenericTemplate = File.ReadAllText(BASIC_TEMPLATE_PATH);

            string modifiedText = s_GenericTemplate.Replace("%TypeName%", inType.Name).Replace("%TypeName-Full%", inType.FullName);
            File.WriteAllText(path, modifiedText);
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }

        static private void GenerateStructSerializer(Type inType)
        {
            string path = GENERATED_PATH.Replace("%TypeName%", inType.Name);

            if (s_StructTemplate == null)
                s_StructTemplate = File.ReadAllText(STRUCT_TEMPLATE_PATH);

            string modifiedText = s_StructTemplate.Replace("%TypeName%", inType.Name).Replace("%TypeName-Full%", inType.FullName);
            File.WriteAllText(path, modifiedText);
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }
    }
}