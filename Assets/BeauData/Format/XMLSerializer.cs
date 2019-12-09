/*
 * Copyright (C) 2017 - 2019. Filament Games, LLC. All rights reserved.
 * Author:  Alex Beauchesne
 * Date:    3 May 2017
 * 
 * File:    BinarySerializer.cs
 * Purpose: Serializes to an XML format.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace BeauData.Format
{
    internal sealed class XMLSerializer : Serializer
    {
        public const string FILE_EXTENSION = "xml";
        public const string FILE_CONTENT_PREFIX = "<";

        private Stack<XmlNode> m_Stack = new Stack<XmlNode>();
        private XmlNode m_Current;
        private XmlElement m_Root;
        private XmlDocument m_Document;

        public XMLSerializer()
        {
            m_Document = new XmlDocument();
        }

        internal XMLSerializer(XmlDocument inDocument)
        {
            m_Document = inDocument;
            ClearComments(m_Document);
        }

        public override void Dispose()
        {
            if (m_Stack != null)
            {
                m_Stack.Clear();
                m_Stack = null;
            }

            m_Current = null;
            m_Root = null;
            m_Document = null;
        }

        static private string SanitizeElementName(string inName)
        {
            return inName.Replace("+", "_");
        }

        static private bool ClearComments(XmlNode inNode)
        {
            if (inNode.ParentNode != null)
            {
                if (inNode is XmlComment)
                    return true;
            }

            var childNodes = inNode.ChildNodes;
            for (int i = childNodes.Count - 1; i >= 0; --i)
            {
                if (ClearComments(childNodes[i]))
                    inNode.RemoveChild(childNodes[i]);
            }

            return false;
        }

        #region Group abstraction

        protected override void BeginReadRoot(string inRootName)
        {
            m_Stack.Clear();
            m_Current = m_Root = (XmlElement) m_Document.DocumentElement;

            XmlAttribute versionAttribute = m_Current.Attributes[SERIALIZER_VERSION_KEY];
            if (versionAttribute != null)
                SerializerVersion = ushort.Parse(versionAttribute.InnerText);
            else
                SerializerVersion = VERSION_INITIAL;
        }

        protected override void BeginWriteRoot(string inRootName)
        {
            m_Stack.Clear();
            m_Current = m_Root = m_Document.CreateElement(SanitizeElementName(inRootName));
            m_Document.AppendChild(m_Root);

            XmlAttribute serializerVersionNode = m_Document.CreateAttribute(SERIALIZER_VERSION_KEY);
            serializerVersionNode.InnerText = SerializerVersion.ToString();
        }

        protected override void EndRoot()
        {
            m_Stack.Clear();
            m_Current = m_Root;
        }

        protected override void BeginWriteArray(string inKey)
        {
            m_Stack.Push(m_Current);
            XmlNode newObj = m_Root.OwnerDocument.CreateElement(inKey);
            m_Current.AppendChild(newObj);
            m_Current = newObj;
        }

        protected override bool BeginReadArray(string inKey)
        {
            m_Stack.Push(m_Current);
            if (m_Current != null)
                m_Current = m_Current[inKey];
            return true;
        }

        protected override void EndArray()
        {
            // If we end an object and we don't have any children, we don't want this to be mistakenly read as null
            if (IsWriting && (string.IsNullOrEmpty(m_Current.InnerText) && m_Current.ChildNodes.Count == 0 && m_Current.Attributes.Count == 0))
                m_Current.Attributes.Append(m_Document.CreateAttribute("__empty"));
            m_Current = m_Stack.Pop();
        }

        protected override void BeginWriteObject()
        {
            m_Stack.Push(m_Current);
            XmlNode newObj = m_Root.OwnerDocument.CreateElement("object");
            m_Current.AppendChild(newObj);
            m_Current = newObj;
        }

        protected override void BeginWriteObject(string inKey)
        {
            m_Stack.Push(m_Current);
            XmlNode newObj = m_Root.OwnerDocument.CreateElement(inKey);
            m_Current.AppendChild(newObj);
            m_Current = newObj;
        }

        protected override bool BeginReadObject(int inIndex)
        {
            m_Stack.Push(m_Current);
            if (m_Current != null)
                m_Current = m_Current.ChildNodes[inIndex];
            return true;
        }

        protected override bool BeginReadObject(string inKey)
        {
            m_Stack.Push(m_Current);
            if (m_Current != null)
                m_Current = m_Current[inKey];
            return true;
        }

        protected override void EndObject()
        {
            // If we end an object and we don't have any children, we don't want this to be mistakenly read as null
            if (IsWriting && (string.IsNullOrEmpty(m_Current.InnerText) && m_Current.ChildNodes.Count == 0 && m_Current.Attributes.Count == 0))
                m_Current.Attributes.Append(m_Document.CreateAttribute("__empty"));
            m_Current = m_Stack.Pop();
        }

        protected override void BeginWriteValue()
        {
            m_Stack.Push(m_Current);
            XmlNode newObj = m_Root.OwnerDocument.CreateElement("value");
            m_Current.AppendChild(newObj);
            m_Current = newObj;
        }

        protected override void BeginWriteValue(string inKey, FieldOptions inOptions)
        {
            m_Stack.Push(m_Current);
            XmlNode newObj;
            if ((inOptions & FieldOptions.PreferAttribute) != 0)
            {
                newObj = m_Root.OwnerDocument.CreateAttribute(inKey);
                m_Current.Attributes.Append((XmlAttribute) newObj);
            }
            else
            {
                newObj = m_Root.OwnerDocument.CreateElement(inKey);
                m_Current.AppendChild(newObj);
            }
            m_Current = newObj;
        }

        protected override bool BeginReadValue(int inIndex)
        {
            m_Stack.Push(m_Current);
            if (m_Current != null)
                m_Current = m_Current.ChildNodes[inIndex];
            return true;
        }

        protected override bool BeginReadValue(string inKey)
        {
            m_Stack.Push(m_Current);
            if (m_Current != null)
            {
                XmlNode attr = m_Current.Attributes[inKey];
                if (attr != null)
                    m_Current = attr;
                else
                    m_Current = m_Current[inKey];
            }
            return true;
        }

        protected override void EndValue()
        {
            m_Current = m_Stack.Pop();
        }

        protected override void WriteNull()
        {
            m_Current.AppendChild(m_Root.OwnerDocument.CreateElement("null"));
        }

        protected override void WriteNull(string inKey)
        {
            m_Current.AppendChild(m_Root.OwnerDocument.CreateElement(inKey));
        }

        protected override bool IsMissing()
        {
            return m_Current == null;
        }

        protected override bool IsNull()
        {
            return m_Current.Name == "null" || m_Current.InnerText == "null" ||
                (string.IsNullOrEmpty(m_Current.InnerText) && m_Current.ChildNodes.Count == 0 && m_Current.Attributes.Count == 0);
        }

        protected override int GetChildCount()
        {
            return m_Current.ChildNodes.Count;
        }

        protected override void DeclareChildCount(int inCount) { }

        #endregion

        #region Read/Write

        protected override bool RequiresExplicitNull() { return false; }

        // Boolean
        protected override bool Read_Boolean(ref bool ioData)
        {
            return bool.TryParse(m_Current.InnerText, out ioData);
        }

        protected override void Write_Boolean(ref bool ioData)
        {
            m_Current.InnerText = ioData.ToString();
        }

        // Byte
        protected override bool Read_Byte(ref byte ioData)
        {
            return byte.TryParse(m_Current.InnerText, out ioData);
        }

        protected override void Write_Byte(ref byte ioData)
        {
            m_Current.InnerText = ioData.ToString();
        }

        // ByteArray
        protected override bool Read_ByteArray(ref byte[] ioData)
        {
            string base64 = m_Current.InnerText;
            try
            {
                ioData = System.Convert.FromBase64String(base64);
                return true;
            }
            catch (Exception e)
            {
                AddErrorMessage("Unable to convert string to base64: {0}", e.Message);
                return false;
            }
        }

        protected override void Write_ByteArray(ref byte[] ioData)
        {
            string base64 = Convert.ToBase64String(ioData);
            m_Current.InnerText = base64;
        }

        // Double
        protected override bool Read_Double(ref double ioData)
        {
            return double.TryParse(m_Current.InnerText, out ioData);
        }

        protected override void Write_Double(ref double ioData)
        {
            m_Current.InnerText = ioData.ToString();
        }

        // Guid
        protected override bool Read_Guid(ref Guid ioData)
        {
            ioData = new Guid(m_Current.InnerText);
            return true;
        }

        protected override void Write_Guid(ref Guid ioData)
        {
            m_Current.InnerText = ioData.ToString();
        }

        // Int16
        protected override bool Read_Int16(ref short ioData)
        {
            return short.TryParse(m_Current.InnerText, out ioData);
        }

        protected override void Write_Int16(ref short ioData)
        {
            m_Current.InnerText = ioData.ToString();
        }

        // Int32
        protected override bool Read_Int32(ref int ioData)
        {
            return int.TryParse(m_Current.InnerText, out ioData);
        }

        protected override void Write_Int32(ref int ioData)
        {
            m_Current.InnerText = ioData.ToString();
        }

        // Int64
        protected override bool Read_Int64(ref long ioData)
        {
            return long.TryParse(m_Current.InnerText, out ioData);
        }

        protected override void Write_Int64(ref long ioData)
        {
            m_Current.InnerText = ioData.ToString();
        }

        // Single
        protected override bool Read_Single(ref float ioData)
        {
            return float.TryParse(m_Current.InnerText, out ioData);
        }

        protected override void Write_Single(ref float ioData)
        {
            m_Current.InnerText = ioData.ToString();
        }

        // String
        protected override bool Read_String(ref string ioData)
        {
            ioData = m_Current.InnerText;
            return true;
        }

        protected override void Write_String(ref string ioData)
        {
            m_Current.InnerText = ioData;
        }

        // Uint16
        protected override bool Read_UInt16(ref ushort ioData)
        {
            return ushort.TryParse(m_Current.InnerText, out ioData);
        }

        protected override void Write_UInt16(ref ushort ioData)
        {
            m_Current.InnerText = ioData.ToString();
        }

        // Uint32
        protected override bool Read_UInt32(ref uint ioData)
        {
            return uint.TryParse(m_Current.InnerText, out ioData);
        }

        protected override void Write_UInt32(ref uint ioData)
        {
            m_Current.InnerText = ioData.ToString();
        }

        // Uint64
        protected override bool Read_UInt64(ref ulong ioData)
        {
            return ulong.TryParse(m_Current.InnerText, out ioData);
        }

        protected override void Write_UInt64(ref ulong ioData)
        {
            m_Current.InnerText = ioData.ToString();
        }

        // FourCC
        protected override bool Read_FourCC(ref FourCC ioData)
        {
            return FourCC.TryParse(m_Current.InnerText, out ioData);
        }

        protected override void Write_FourCC(ref FourCC ioData)
        {
            m_Current.InnerText = ioData.ToString(true);
        }

        #endregion

        #region Output

        internal override string AsString(OutputOptions inOptions = OutputOptions.None)
        {
            if ((inOptions & OutputOptions.PrettyPrint) == 0)
                return m_Document.OuterXml;

            using(MemoryStream stream = new MemoryStream())
            using(XmlTextWriter textWriter = new XmlTextWriter(stream, System.Text.Encoding.UTF8))
            {
                textWriter.Formatting = Formatting.Indented;
                m_Document.WriteContentTo(textWriter);
                textWriter.Flush();
                stream.Flush();

                stream.Position = 0;

                StreamReader reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }
        }

        internal override void AsStream(Stream inStream, OutputOptions inOptions = OutputOptions.None)
        {
            using(StreamWriter writer = new StreamWriter(inStream))
            {
                writer.Write(AsString(inOptions));
            }
        }

        #endregion
    }
}