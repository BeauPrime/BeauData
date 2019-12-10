using System;
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
            typeof(UInt64),
            typeof(FourCC)
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

        [MenuItem("Assets/BeauData/Generate Generic")]
        static private void Generate()
        {
            GenerateBasic();
            GenerateStructs();
        }

        [MenuItem("Assets/BeauData/Generate Generic (Basic)")]
        static private void GenerateBasic()
        {
            foreach (var type in BASIC_TYPES)
                GenerateBasicSerializer(type);
        }

        [MenuItem("Assets/BeauData/Generate Generic (Struct)")]
        static private void GenerateStructs()
        {
            foreach (var type in STRUCT_TYPES)
                GenerateStructSerializer(type);
        }

        static private void GenerateBasicSerializer(Type inType)
        {
            string typeName, typeNameFull;
            GetTypeNames(inType, out typeName, out typeNameFull);

            string path = GENERATED_PATH.Replace("%TypeName%", typeName);

            if (s_GenericTemplate == null)
                s_GenericTemplate = File.ReadAllText(BASIC_TEMPLATE_PATH);

            string modifiedText = s_GenericTemplate.Replace("%TypeName%", typeName).Replace("%TypeName-Full%", typeNameFull);
            File.WriteAllText(path, modifiedText);
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }

        static private void GenerateStructSerializer(Type inType)
        {
            string typeName, typeNameFull;
            GetTypeNames(inType, out typeName, out typeNameFull);

            string path = GENERATED_PATH.Replace("%TypeName%", typeName);

            if (s_StructTemplate == null)
                s_StructTemplate = File.ReadAllText(STRUCT_TEMPLATE_PATH);

            string modifiedText = s_StructTemplate.Replace("%TypeName%", typeName).Replace("%TypeName-Full%", typeNameFull);
            File.WriteAllText(path, modifiedText);
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }

        static private void GetTypeNames(Type inType, out string outName, out string outFullName)
        {
            string name = inType.Name;
            string fullname = inType.FullName;

            if (inType.IsArray)
            {
                name = inType.GetElementType().Name + "Array";
                fullname = inType.GetElementType().FullName + "[]";
            }

            outName = name;
            outFullName = fullname;
        }
    }
}