/* * * * *
 * JSON parser/builder
 * Adapted with heavy modifications from SimpleJSON
 * 
 * ----------------------------------------
 * 
 * SimpleJSON License
 * 
 * The MIT License (MIT)
 * 
 * Copyright (c) 2012-2017 Markus Göbel
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 * 
 * * * * */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace BeauData
{
    /// <summary>
    /// JSON node.
    /// Supports lazy instantiation.
    /// </summary>
    public sealed class JSON
    {
        #region Types

        /// <summary>
        /// Marks the type of node.
        /// </summary>
        private enum NodeType
        {
            Unknown = 0,

            Null = 1,
            Array = 2,
            Object = 3,
            String = 4,
            Number = 5,
            Bool = 6,

            Lazy = 255
        }

        #endregion

        private NodeType m_Type = NodeType.Unknown;

        private List<JSON> m_List;
        private Dictionary<string, JSON> m_Dict;
        private double m_NumberValue;
        private string m_StringValue;
        private JSON m_LazyParent;

        private JSON(NodeType inType, int inStartingCollectionSize = 0)
        {
            SetType(inType);
        }

        private void SetType(NodeType inType, int inCollectionSize = 0)
        {
            if (inType == m_Type)
                return;

            if (m_Type == NodeType.Lazy)
            {
                switch(m_LazyParent.m_Type)
                {
                    case NodeType.Array:
                        m_LazyParent.m_List.Add(this);
                        break;
                    case NodeType.Object:
                        m_LazyParent.m_Dict.Add(m_StringValue, this);
                        break;

                    case NodeType.Lazy:
                    default:
                        if (m_StringValue == null)
                        {
                            m_LazyParent.SetType(NodeType.Array);
                            m_LazyParent.m_List.Add(this);
                        }
                        else
                        {
                            m_LazyParent.SetType(NodeType.Object);
                            m_LazyParent.m_Dict.Add(m_StringValue, this);
                        }
                        break;
                }
            }

            if (m_List != null)
            {
                m_List.Clear();
                m_List = null;
            }
            if (m_Dict != null)
            {
                m_Dict.Clear();
                m_Dict = null;
            }

            m_StringValue = null;
            m_NumberValue = 0.0;
            m_LazyParent = null;

            m_Type = inType;

            if (m_Type == NodeType.Array)
            {
                m_List = new List<JSON>(inCollectionSize);
            }
            else if (m_Type == NodeType.Object)
            {
                m_Dict = new Dictionary<string, JSON>(inCollectionSize);
            }
        }

        private bool IsNullOrLazy()
        {
            return m_Type == NodeType.Null || m_Type == NodeType.Lazy;
        }

        #region Collection Properties

        /// <summary>
        /// Gets/sets the node with the given index.
        /// </summary>
        public JSON this[int inIndex]
        {
            get
            {
                switch(m_Type)
                {
                    case NodeType.Array:
                        if (inIndex < 0 || inIndex >= m_List.Count)
                            return MakeLazy(this);
                        return m_List[inIndex];

                    case NodeType.Object:
                        return this[inIndex.ToString()];

                    default:
                        return MakeLazy(this);
                }
            }
            set
            {
                switch(m_Type)
                {
                    case NodeType.Object:
                        this[inIndex.ToString()] = value;
                        break;

                    case NodeType.Array:
                    default:
                        SetType(NodeType.Array);
                        if (ReferenceEquals(value, null) || value.m_Type == NodeType.Lazy)
                            value = new JSON(NodeType.Null);
                        if (inIndex >= 0 && inIndex < m_List.Count)
                            m_List[inIndex] = value;
                        else
                            m_List.Add(value);
                        break;
                }
            }
        }

        /// <summary>
        /// Gets/sets the node with the given key.
        /// </summary>
        public JSON this[string inKey]
        {
            get
            {
                switch(m_Type)
                {
                    case NodeType.Object:
                        JSON json;
                        if (!m_Dict.TryGetValue(inKey, out json))
                            return MakeLazy(this, inKey);
                        return json;

                    case NodeType.Array:
                        return MakeLazy(this);

                    default:
                        return MakeLazy(this, inKey);
                }
            }
            set
            {
                switch(m_Type)
                {
                    case NodeType.Array:
                        m_List.Add(value);
                        break;

                    case NodeType.Object:
                    default:
                        SetType(NodeType.Object);
                        if (ReferenceEquals(value, null) || value.m_Type == NodeType.Lazy)
                            value = new JSON(NodeType.Null);
                        m_Dict[inKey] = value;
                        break;
                }
            }
        }

        /// <summary>
        /// Returns the number of children in this node.
        /// </summary>
        public int Count
        {
            get
            {
                switch(m_Type)
                {
                    case NodeType.Array:
                        return m_List.Count;
                    case NodeType.Object:
                        return m_Dict.Count;
                    default:
                        return 0;
                }
            }
        }

        /// <summary>
        /// Returns all immediate children of this node.
        /// </summary>
        public IEnumerable<JSON> Children
        {
            get
            {
                switch(m_Type)
                {
                    case NodeType.Array:
                        for (int i = 0; i < m_List.Count; ++i)
                            yield return m_List[i];
                        break;
                    case NodeType.Object:
                        foreach (var node in m_Dict.Values)
                            yield return node;
                        break;
                    default:
                        yield break;
                }
            }
        }

        /// <summary>
        /// Returns all immediate children of this node, along with their keys.
        /// </summary>
        public IEnumerable<KeyValuePair<string, JSON>> KeyValues
        {
            get
            {
                switch(m_Type)
                {
                    case NodeType.Array:
                        for (int i = 0; i < m_List.Count; ++i)
                            yield return new KeyValuePair<string, JSON>(null, m_List[i]);
                        break;
                    case NodeType.Object:
                        foreach (var node in m_Dict)
                            yield return node;
                        break;
                    default:
                        yield break;
                }
            }
        }

        /// <summary>
        /// Removes all children from this node.
        /// </summary>
        public void Clear()
        {
            switch(m_Type)
            {
                case NodeType.Array:
                    m_List.Clear();
                    break;

                case NodeType.Object:
                    m_Dict.Clear();
                    break;
            }
        }

        #region Add/Remove

        /// <summary>
        /// Adds the given node as a child of this node.
        /// </summary>
        public JSON Add(JSON inJSON)
        {
            if (ReferenceEquals(inJSON, null))
                inJSON = new JSON(NodeType.Null);

            switch(m_Type)
            {
                case NodeType.Object:
                    m_Dict[Guid.NewGuid().ToString()] = inJSON;
                    break;

                case NodeType.Array:
                default:
                    SetType(NodeType.Array);
                    m_List.Add(inJSON);
                    break;
            }

            return this;
        }

        /// <summary>
        /// Adds the given node as a child with the given key.
        /// </summary>
        public JSON Add(string inKey, JSON inJSON)
        {
            if (ReferenceEquals(inJSON, null))
                inJSON = new JSON(NodeType.Null);

            switch(m_Type)
            {
                case NodeType.Array:
                    m_List.Add(inJSON);
                    break;

                case NodeType.Object:
                default:
                    SetType(NodeType.Object);
                    m_Dict[inKey] = inJSON;
                    break;
            }

            return this;
        }

        /// <summary>
        /// Detaches the given node from this node.
        /// </summary>
        public JSON Remove(JSON inJSON)
        {
            switch(m_Type)
            {
                case NodeType.Array:
                    m_List.Remove(inJSON);
                    break;

                case NodeType.Object:
                    foreach (var node in m_Dict)
                    {
                        if (node.Value == inJSON)
                        {
                            m_Dict.Remove(node.Key);
                            break;
                        }
                    }
                    break;
            }

            return this;
        }

        /// <summary>
        /// Detaches the node with the given key.
        /// </summary>
        public JSON Remove(string inKey)
        {
            switch(m_Type)
            {
                case NodeType.Object:
                    m_Dict.Remove(inKey);
                    break;
            }

            return this;
        }

        /// <summary>
        /// Detaches the node at the given index.
        /// </summary>
        public JSON Remove(int inIndex)
        {
            switch(m_Type)
            {
                case NodeType.Array:
                    if (inIndex >= 0 && inIndex < m_List.Count)
                        m_List.RemoveAt(inIndex);
                    break;

                case NodeType.Object:
                    m_Dict.Remove(inIndex.ToString());
                    break;
            }

            return this;
        }

        #endregion

        #endregion

        #region Type Properties

        /// <summary>
        /// Returns if this node is a number value.
        /// </summary>
        public bool IsNumber
        {
            get { return m_Type == NodeType.Number; }
        }

        /// <summary>
        /// Gets/sets the node's value as a double.
        /// </summary>
        public double AsDouble
        {
            get
            {
                double val = 0.0;
                if (m_Type == NodeType.Number)
                    val = m_NumberValue;
                if (m_Type == NodeType.String)
                    double.TryParse(m_StringValue, out val);
                return val;
            }
            set { SetType(NodeType.Number); m_NumberValue = value; }
        }

        /// <summary>
        /// Gets/sets the node's value as a float.
        /// </summary>
        public float AsFloat
        {
            get
            {
                float val = 0.0f;
                if (m_Type == NodeType.Number)
                    val = (float)m_NumberValue;
                if (m_Type == NodeType.String)
                    float.TryParse(m_StringValue, out val);
                return val;
            }
            set { SetType(NodeType.Number); m_NumberValue = (double)value; }
        }

        /// <summary>
        /// Gets/sets the node's value as an integer.
        /// </summary>
        public int AsInt
        {
            get
            {
                int val = 0;
                if (m_Type == NodeType.Number)
                    val = (int)m_NumberValue;
                if (m_Type == NodeType.String)
                    int.TryParse(m_StringValue, out val);
                return val;
            }
            set { SetType(NodeType.Number); m_NumberValue = (double)value; }
        }

        /// <summary>
        /// Gets/sets the node's value as an unsigned integer.
        /// </summary>
        public uint AsUInt
        {
            get
            {
                uint val = 0U;
                if (m_Type == NodeType.Number)
                    val = (uint)m_NumberValue;
                if (m_Type == NodeType.String)
                    uint.TryParse(m_StringValue, out val);
                return val;
            }
            set { SetType(NodeType.Number); m_NumberValue = (double)value; }
        }

        /// <summary>
        /// Gets/sets the node's value as a long integer.
        /// </summary>
        public long AsLong
        {
            get
            {
                long val = 0L;
                if (m_Type == NodeType.Number)
                    val = (long)m_NumberValue;
                if (m_Type == NodeType.String)
                    long.TryParse(m_StringValue, out val);
                return val;
            }
            set { SetType(NodeType.Number); m_NumberValue = (double)value; }
        }

        /// <summary>
        /// Gets/sets the node's value as an unsigned long integer.
        /// </summary>
        public ulong AsULong
        {
            get
            {
                ulong val = 0L;
                if (m_Type == NodeType.Number)
                    val = (ulong)m_NumberValue;
                if (m_Type == NodeType.String)
                    ulong.TryParse(m_StringValue, out val);
                return val;
            }
            set { SetType(NodeType.Number); m_NumberValue = (double)value; }
        }

        /// <summary>
        /// Gets/sets the node's value as a short integer.
        /// </summary>
        public short AsShort
        {
            get
            {
                short val = 0;
                if (m_Type == NodeType.Number)
                    val = (short)m_NumberValue;
                if (m_Type == NodeType.String)
                    short.TryParse(m_StringValue, out val);
                return val;
            }
            set { SetType(NodeType.Number); m_NumberValue = (double)value; }
        }

        /// <summary>
        /// Gets/sets the node's value as an unsigned short integer.
        /// </summary>
        public ushort AsUShort
        {
            get
            {
                ushort val = 0;
                if (m_Type == NodeType.Number)
                    val = (ushort)m_NumberValue;
                if (m_Type == NodeType.String)
                    ushort.TryParse(m_StringValue, out val);
                return val;
            }
            set { SetType(NodeType.Number); m_NumberValue = (double)value; }
        }

        /// <summary>
        /// Returns if this node is a string value.
        /// </summary>
        public bool IsString
        {
            get { return m_Type == NodeType.String; }
        }

        /// <summary>
        /// Gets/sets the node's value as a string.
        /// </summary>
        public string AsString
        {
            get
            {
                switch(m_Type)
                {
                    case NodeType.String:
                        return m_StringValue;
                    case NodeType.Number:
                        return m_NumberValue.ToString();
                    case NodeType.Bool:
                        return m_NumberValue > 0 ? "true" : "false";
                    case NodeType.Null:
                    case NodeType.Lazy:
                        return null;
                    default:
                        return string.Empty;
                }
            }
            set { SetType(NodeType.String); m_StringValue = value; }
        }

        /// <summary>
        /// Returns if this node is a boolean value.
        /// </summary>
        public bool IsBool
        {
            get { return m_Type == NodeType.Bool; }
        }

        /// <summary>
        /// Gets/sets the node's value as a boolean.
        /// </summary>
        public bool AsBool
        {
            get
            {
                switch(m_Type)
                {
                    case NodeType.Bool:
                    case NodeType.Number:
                        return m_NumberValue > 0;

                    case NodeType.Null:
                    case NodeType.Lazy:
                        return false;

                    default:
                        return true;
                }
            }
            set { SetType(NodeType.Bool); m_NumberValue = value ? 1 : 0; }
        }

        /// <summary>
        /// Returns if this node is a null value.
        /// </summary>
        public bool IsNull
        {
            get { return m_Type == NodeType.Null || m_Type == NodeType.Lazy; }
        }

        /// <summary>
        /// Returns if this node is an undefined value.
        /// </summary>
        public bool IsUndefined
        {
            get { return m_Type == NodeType.Lazy; }
        }

        /// <summary>
        /// Returns if this node is an array.
        /// </summary>
        public bool IsArray
        {
            get { return m_Type == NodeType.Array; }
        }

        /// <summary>
        /// Returns if this node is an object.
        /// </summary>
        public bool IsObject
        {
            get { return m_Type == NodeType.Object; }
        }

        /// <summary>
        /// Gets the node's value as an anonymous object.
        /// </summary>
        public object AsValue
        {
            get
            {
                switch(m_Type)
                {
                    case NodeType.Bool:
                        return AsBool;

                    case NodeType.Number:
                        return m_NumberValue;

                    case NodeType.String:
                        return m_StringValue;

                    case NodeType.Null:
                    case NodeType.Lazy:
                    default:
                        return null;
                }
            }
        }

        #endregion

        #region Parsing

        private class Parser
        {
            private readonly Stack<JSON> m_Stack;
            private readonly StringBuilder m_TokenBuilder;
            private readonly char[] m_UnicodeReadChars;
            
            private JSON m_Context;
            private bool m_HasToken;
            private string m_Key;

            private bool m_QuoteMode;
            private bool m_TokenQuoted;

            private TextReader m_Stream;
            private string m_Prefix;
            private int m_PrefixLength;
            private int m_PrefixPosition;

            internal Parser()
            {
                m_Stack = new Stack<JSON>();
                m_Context = null;
                m_UnicodeReadChars = new char[4];

                m_TokenBuilder = new StringBuilder(256);
                m_HasToken = false;
                m_Key = null;

                m_QuoteMode = m_TokenQuoted = false;
            }

            private void ResetState()
            {
                m_Stack.Clear();
                m_Context = null;

                m_TokenBuilder.Length = 0;
                m_HasToken = false;
                m_Key = null;

                m_QuoteMode = m_TokenQuoted = false;

                m_Stream = null;
                m_Prefix = null;
                m_PrefixLength = 0;
                m_PrefixPosition = 0;
            }

            public JSON Parse(TextReader inStream, string inPrefix)
            {
                ResetState();
                if (inPrefix != null)
                {
                    m_Prefix = inPrefix;
                    m_PrefixLength = inPrefix.Length;
                }

                using(m_Stream = inStream)
                {
                    while (ParseCharacter()) ;

                    if (m_QuoteMode)
                        throw new Exception("JSON parsing error: Unclosed quotation mark.");
                    if (m_Stack.Count > 0)
                        throw new Exception("JSON parsing error: Unclosed objects or arrays.");
                    if (m_HasToken)
                        throw new Exception("JSON parsing error: Unfinished token '" + m_TokenBuilder.ToString() + "'");
                    if (m_Key != null)
                        throw new Exception("JSON parsing error: Unfinished token '" + m_Key + "'");
                }

                JSON content = m_Context;
                ResetState();
                return content;
            }

            private bool ParseCharacter()
            {
                int read = ReadChar();
                if (read == -1)
                    return false;

                char c = (char) read;
                switch(c)
                {
                    case '{':
                        if (m_QuoteMode)
                            AddToToken(c);
                        else
                            StartObject();
                        break;

                    case '[':
                        if (m_QuoteMode)
                            AddToToken(c);
                        else
                            StartArray();
                        break;

                    case '}':
                        if (m_QuoteMode)
                            AddToToken(c);
                        else
                            EndObject();
                        break;

                    case ']':
                        if (m_QuoteMode)
                            AddToToken(c);
                        else
                            EndArray();
                        break;

                    case ':':
                        if (m_QuoteMode)
                            AddToToken(c);
                        else
                            SetTokenAsKey();
                        break;

                    case '"':
                        ToggleQuotes();
                        break;

                    case ',':
                        if (m_QuoteMode)
                            AddToToken(c);
                        else
                            SeparateElement();
                        break;

                    case '\r':
                    case '\n':
                    case '\0':
                        break;

                    case ' ':
                    case '\t':
                        if (m_QuoteMode)
                            AddToToken(c);
                        break;

                    case '\\':
                        if (m_QuoteMode)
                        {
                            read = ReadChar();
                            if (read == -1)
                                throw new ParseException("Escape with no corresponding character");

                            c = (char) read;
                            switch(c)
                            {
                                case 't':
                                    AddToToken('\t');
                                    break;
                                case 'r':
                                    AddToToken('\r');
                                    break;
                                case 'n':
                                    AddToToken('\n');
                                    break;
                                case 'b':
                                    AddToToken('\b');
                                    break;
                                case 'f':
                                    AddToToken('\f');
                                    break;
                                case 'u':
                                    {
                                        int readChars = ReadBlock(m_UnicodeReadChars, 0, 4);
                                        if (readChars < 4)
                                            throw new ParseException("Unicode escape with less than 4 subsequent digits");
                                        
                                        string unicodeStr = new string(m_UnicodeReadChars);
                                        AddToToken((char)int.Parse(unicodeStr, NumberStyles.AllowHexSpecifier, CultureUtility.InvariantNumberFormat));
                                        break;
                                    }
                                default:
                                    AddToToken(c);
                                    break;
                            }
                        }
                        break;

                    default:
                        AddToToken(c);
                        break;
                }

                return true;
            }

            #region Read

            private int ReadChar()
            {
                if (m_PrefixPosition >= m_PrefixLength)
                {
                    return m_Stream != null ? m_Stream.Read() : -1;
                }
                return m_Prefix[m_PrefixPosition++];
            }

            private int ReadBlock(char[] ioBuffer, int inIndex, int inCount)
            {
                int readCount = 0;
                while(m_PrefixPosition < m_PrefixLength && readCount < inCount)
                {
                    ioBuffer[inIndex + readCount++] = m_Prefix[m_PrefixPosition++];
                }

                if (readCount < inCount && m_Stream != null)
                {
                    readCount += m_Stream.ReadBlock(ioBuffer, readCount, inCount - readCount);
                }

                return readCount;
            }

            #endregion // Read

            #region Tokens

            private void AddToken(NodeType inExpectedType = NodeType.Unknown)
            {
                if (!m_HasToken)
                {
                    if (m_Key != null)
                        throw new ParseException("Key '" + m_Key + "' does not have a corresponding value.");
                    return;
                }

                if (inExpectedType != NodeType.Unknown)
                {
                    NodeType actualType = m_Context.m_Type;
                    if (inExpectedType != actualType)
                        throw new ParseException("Expected " + inExpectedType + ", got " + actualType);
                }

                if (m_TokenQuoted)
                {
                    m_Context.Add(m_Key, JSON.CreateValue(m_TokenBuilder.ToString()));
                    return;
                }

                if (TokenMatches("false"))
                    m_Context.Add(m_Key, JSON.CreateValue(false));
                else if (TokenMatches("true"))
                    m_Context.Add(m_Key, JSON.CreateValue(true));
                else if (TokenMatches("null"))
                    m_Context.Add(m_Key, JSON.CreateNull());
                else
                {
                    string token = m_TokenBuilder.ToString();
                    double number;
                    if (double.TryParse(token, NumberStyles.Float, CultureUtility.InvariantNumberFormat, out number))
                        m_Context.Add(m_Key, JSON.CreateValue(number));
                    else
                        m_Context.Add(m_Key, JSON.CreateValue(token));
                }
            }

            private void AddToToken(char inChar)
            {
                if (inChar == 0)
                    throw new ParseException("adding null character to token");
                m_TokenBuilder.Append(inChar);
                m_HasToken = true;
            }

            private bool TokenMatches(string inToken)
            {
                int length = m_TokenBuilder.Length;
                if (length != inToken.Length)
                    return false;

                while(--length >= 0)
                    if (char.ToLowerInvariant(m_TokenBuilder[length]) != inToken[length])
                        return false;

                return true;
            }

            private void SetTokenAsKey()
            {
                m_Key = m_TokenBuilder.ToString();
                m_TokenBuilder.Length = 0;
                m_TokenQuoted = false;
                m_HasToken = false;
            }

            private void ResetTokens()
            {
                m_TokenBuilder.Length = 0;
                m_Key = null;
                m_QuoteMode = m_TokenQuoted = false;
                m_HasToken = false;
            }

            #endregion // Tokens

            #region Elements

            private void StartArray()
            {
                JSON array = new JSON(NodeType.Array);
                m_Stack.Push(array);

                if (m_Context != null)
                    m_Context.Add(m_Key, array);

                ResetTokens();
                m_Context = array;
            }

            private void StartObject()
            {
                JSON obj = new JSON(NodeType.Object);
                m_Stack.Push(obj);

                if (m_Context != null)
                    m_Context.Add(m_Key, obj);

                ResetTokens();
                m_Context = obj;
            }

            private void ToggleQuotes()
            {
                if (!m_QuoteMode)
                {
                    if (m_HasToken)
                        throw new ParseException("Multiple quotation blocks for a single token is not allowed.");

                    m_QuoteMode = true;
                    m_HasToken = true;
                    m_TokenQuoted = true;
                }
                else
                {
                    m_QuoteMode = false;
                }
            }

            private void SeparateElement()
            {
                AddToken();
                ResetTokens();
            }

            private void EndObject()
            {
                if (m_Stack.Count == 0)
                    throw new ParseException("Too many closing brackets");

                m_Stack.Pop();

                AddToken(NodeType.Object);
                ResetTokens();

                if (m_Stack.Count > 0)
                    m_Context = m_Stack.Peek();
            }

            private void EndArray()
            {
                if (m_Stack.Count == 0)
                    throw new ParseException("Too many closing brackets");

                m_Stack.Pop();

                AddToken(NodeType.Array);
                ResetTokens();

                if (m_Stack.Count > 0)
                    m_Context = m_Stack.Peek();
            }

            #endregion // Elements
        }

        /// <summary>
        /// Parses the JSON string into an object.
        /// </summary>
        static public JSON Parse(string inJSON)
        {
            return GetParser().Parse(null, inJSON);
        }

        /// <summary>
        /// Parses the JSON stream into an object.
        /// </summary>
        static public JSON Parse(Stream inStream)
        {
            StreamReader reader = new StreamReader(inStream);
            return GetParser().Parse(reader, null);
        }

        /// <summary>
        /// Parses the given prefix string and JSON stream into an object.
        /// </summary>
        static public JSON Parse(Stream inStream, string inPrefix)
        {
            StreamReader reader = new StreamReader(inStream);
            return GetParser().Parse(reader, inPrefix);
        }

        #region Instance

        [ThreadStatic] static private Parser s_ThreadParser;

        static private Parser GetParser()
        {
            return s_ThreadParser ?? (s_ThreadParser = new Parser());
        }

        #endregion // Instance

        #endregion

        #region Stringify

        #region String Builder

        [ThreadStatic] static private StringBuilder s_ThreadStringBuilder;

        static private StringBuilder GetStringBuilder()
        {
            StringBuilder builder = s_ThreadStringBuilder ?? (s_ThreadStringBuilder = new StringBuilder(512));
            builder.Length = 0;
            return builder;
        }

        static private string PopString(StringBuilder ioBuilder)
        {
            string str = ioBuilder.ToString();
            ioBuilder.Length = 0;
            return str;
        }

        #endregion // String Builder

        public override string ToString()
        {
            StringBuilder builder = GetStringBuilder();
            Stringify(builder);
            return PopString(builder);
        }

        public string ToString(int inIndentLevel)
        {
            StringBuilder builder = GetStringBuilder();
            Stringify(builder, 0, inIndentLevel);
            return PopString(builder);
        }

        /// <summary>
        /// Writes the JSON to the given TextWriter.
        /// </summary>
        public void WriteTo(TextWriter inWriter)
        {
            Stringify(inWriter);
        }

        /// <summary>
        /// Writes the JSON to the given TextWriter.
        /// </summary>
        public void WriteTo(TextWriter inWriter, int inIndentLevel)
        {
            Stringify(inWriter, 0, inIndentLevel);
        }

        #region StringBuilder

        // Writes inline json to the StringBuilder
        private void Stringify(StringBuilder ioBuilder)
        {
            switch (m_Type)
            {
                case NodeType.Array:
                    {
                        ioBuilder.Append('[');
                        for (int i = 0; i < m_List.Count; ++i)
                        {
                            if (i > 0)
                                ioBuilder.Append(',');
                            m_List[i].Stringify(ioBuilder);
                        }
                        ioBuilder.Append(']');
                        break;
                    }

                case NodeType.Object:
                    {
                        ioBuilder.Append('{');
                        bool bComma = false;
                        foreach (var keyValue in m_Dict)
                        {
                            if (bComma)
                                ioBuilder.Append(',');
                            ioBuilder.Append('\"');
                            WriteEscaped(ioBuilder, keyValue.Key);
                            ioBuilder.Append('\"').Append(':');
                            keyValue.Value.Stringify(ioBuilder);
                            bComma = true;
                        }
                        ioBuilder.Append('}');
                        break;
                    }

                case NodeType.Bool:
                    {
                        ioBuilder.Append(m_NumberValue > 0 ? "true" : "false");
                        break;
                    }

                case NodeType.Number:
                    {
                        ioBuilder.Append(m_NumberValue.ToString(CultureUtility.InvariantNumberFormat));
                        break;
                    }

                case NodeType.String:
                    {
                        ioBuilder.Append('\"');
                        WriteEscaped(ioBuilder, m_StringValue);
                        ioBuilder.Append('\"');
                        break;
                    }

                case NodeType.Null:
                    {
                        ioBuilder.Append("null");
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }

        // Writes formatted json to the StringBuilder
        private void Stringify(StringBuilder ioBuilder, int inTabs, int inIndentLevel)
        {
            switch (m_Type)
            {
                case NodeType.Array:
                    {
                        ioBuilder.Append('[');
                        for (int i = 0; i < m_List.Count; ++i)
                        {
                            if (i > 0)
                                ioBuilder.Append(',');
                            ioBuilder.Append('\n').Append(' ', inTabs * inIndentLevel + inIndentLevel);
                            m_List[i].Stringify(ioBuilder, inTabs + 1, inIndentLevel);
                        }
                        ioBuilder.Append('\n').Append(' ', inTabs * inIndentLevel).Append(']');
                        break;
                    }

                case NodeType.Object:
                    {
                        ioBuilder.Append('{');
                        bool bComma = false;
                        foreach (var keyValue in m_Dict)
                        {
                            if (bComma)
                                ioBuilder.Append(',');
                            ioBuilder.Append('\n').Append(' ', inTabs * inIndentLevel + inIndentLevel);
                            ioBuilder.Append('\"');
                            WriteEscaped(ioBuilder, keyValue.Key);
                            ioBuilder.Append("\": ");
                            keyValue.Value.Stringify(ioBuilder, inTabs + 1, inIndentLevel);
                            bComma = true;
                        }
                        ioBuilder.Append('\n').Append(' ', inTabs * inIndentLevel).Append('}');
                        break;
                    }

                default:
                    Stringify(ioBuilder);
                    break;
            }
        }

        // Writes the given text with escaped characters
        private static void WriteEscaped(StringBuilder ioBuilder, string inText)
        {
            if (inText == null)
                return;
                
            for (int i = 0; i < inText.Length; ++i)
            {
                char c = inText[i];
                switch (c)
                {
                    case '\\':
                        ioBuilder.Append("\\\\");
                        break;
                    case '\"':
                        ioBuilder.Append("\\\"");
                        break;
                    case '\n':
                        ioBuilder.Append("\\n");
                        break;
                    case '\r':
                        ioBuilder.Append("\\r");
                        break;
                    case '\t':
                        ioBuilder.Append("\\t");
                        break;
                    case '\b':
                        ioBuilder.Append("\\b");
                        break;
                    case '\f':
                        ioBuilder.Append("\\f");
                        break;
                    default:
                        ioBuilder.Append(c);
                        break;
                }
            }
        }

        #endregion // StringBuilder

        #region Stream

        // Writes inline json to the TextWriter
        private void Stringify(TextWriter ioWriter)
        {
            switch (m_Type)
            {
                case NodeType.Array:
                    {
                        ioWriter.Write('[');
                        for (int i = 0; i < m_List.Count; ++i)
                        {
                            if (i > 0)
                                ioWriter.Write(',');
                            m_List[i].Stringify(ioWriter);
                        }
                        ioWriter.Write(']');
                        break;
                    }

                case NodeType.Object:
                    {
                        ioWriter.Write('{');
                        bool bComma = false;
                        foreach (var keyValue in m_Dict)
                        {
                            if (bComma)
                                ioWriter.Write(',');
                            ioWriter.Write('\"');
                            WriteEscaped(ioWriter, keyValue.Key);
                            ioWriter.Write('\"');
                            ioWriter.Write(':');
                            keyValue.Value.Stringify(ioWriter);
                            bComma = true;
                        }
                        ioWriter.Write('}');
                        break;
                    }

                case NodeType.Bool:
                    {
                        ioWriter.Write(m_NumberValue > 0 ? "true" : "false");
                        break;
                    }

                case NodeType.Number:
                    {
                        ioWriter.Write(m_NumberValue.ToString(CultureUtility.InvariantNumberFormat));
                        break;
                    }

                case NodeType.String:
                    {
                        ioWriter.Write('\"');
                        WriteEscaped(ioWriter, m_StringValue);
                        ioWriter.Write('\"');
                        break;
                    }

                case NodeType.Null:
                    {
                        ioWriter.Write("null");
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }

        // Writes formatted json to the TextWriter
        private void Stringify(TextWriter ioWriter, int inTabs, int inIndentLevel)
        {
            switch (m_Type)
            {
                case NodeType.Array:
                    {
                        ioWriter.Write('[');
                        for (int i = 0; i < m_List.Count; ++i)
                        {
                            if (i > 0)
                                ioWriter.Write(',');
                            ioWriter.Write('\n');
                            
                            WriteChars(ioWriter, ' ', inTabs * inIndentLevel + inIndentLevel);
                            m_List[i].Stringify(ioWriter, inTabs + 1, inIndentLevel);
                        }
                        ioWriter.Write('\n');
                        WriteChars(ioWriter, ' ', inTabs * inIndentLevel);
                        ioWriter.Write(']');
                        break;
                    }

                case NodeType.Object:
                    {
                        ioWriter.Write('{');
                        bool bComma = false;
                        foreach (var keyValue in m_Dict)
                        {
                            if (bComma)
                                ioWriter.Write(',');
                            ioWriter.Write('\n');
                            WriteChars(ioWriter, ' ', inTabs * inIndentLevel + inIndentLevel);
                            ioWriter.Write('\"');
                            WriteEscaped(ioWriter, keyValue.Key);
                            ioWriter.Write("\": ");
                            keyValue.Value.Stringify(ioWriter, inTabs + 1, inIndentLevel);
                            bComma = true;
                        }
                        ioWriter.Write('\n');
                        WriteChars(ioWriter, ' ', inTabs * inIndentLevel);
                        ioWriter.Write('}');
                        break;
                    }

                default:
                    Stringify(ioWriter);
                    break;
            }
        }

        // Writes the given text with escaped characters
        private static void WriteEscaped(TextWriter ioWriter, string inText)
        {
            if (inText == null)
                return;
                
            for (int i = 0; i < inText.Length; ++i)
            {
                char c = inText[i];
                switch (c)
                {
                    case '\\':
                        ioWriter.Write("\\\\");
                        break;
                    case '\"':
                        ioWriter.Write("\\\"");
                        break;
                    case '\n':
                        ioWriter.Write("\\n");
                        break;
                    case '\r':
                        ioWriter.Write("\\r");
                        break;
                    case '\t':
                        ioWriter.Write("\\t");
                        break;
                    case '\b':
                        ioWriter.Write("\\b");
                        break;
                    case '\f':
                        ioWriter.Write("\\f");
                        break;
                    default:
                        ioWriter.Write(c);
                        break;
                }
            }
        }

        private static void WriteChars(TextWriter ioWriter, char inChar, int inCount)
        {
            while(--inCount >= 0)
                ioWriter.Write(inChar);
        }

        #endregion // Stream

        #endregion

        #region Serialization

        /// <summary>
        /// Binary serialization/deserialization.
        /// </summary>
        static public class Binary
        {
            /// <summary>
            /// Returns the JSON as a Base64 string.
            /// </summary>
            static public string ToBase64(JSON inJSON)
            {
                using(var stream = new MemoryStream(512))
                {
                    Serialize(inJSON, stream);
                    return Convert.ToBase64String(stream.ToArray());
                }
            }

            /// <summary>
            /// Writes the JSON in binary form into the stream.
            /// </summary>
            static public void Serialize(JSON inJSON, Stream inStream)
            {
                using(var writer = new BinaryWriter(inStream))
                {
                    Serialize(inJSON, writer);
                }
            }

            /// <summary>
            /// Writes the JSON in binary form into the stream
            /// using the given BinaryWriter.
            /// </summary>
            static public void Serialize(JSON inJSON, BinaryWriter ioWriter)
            {
                ioWriter.Write((byte)inJSON.m_Type);

                switch (inJSON.m_Type)
                {
                    case NodeType.Object:
                        ioWriter.Write(inJSON.m_Dict.Count);
                        foreach (var keyValue in inJSON.m_Dict)
                        {
                            ioWriter.Write(keyValue.Key);
                            Serialize(keyValue.Value, ioWriter);
                        }
                        break;

                    case NodeType.Array:
                        ioWriter.Write(inJSON.m_List.Count);
                        for (int i = 0; i < inJSON.m_List.Count; ++i)
                            Serialize(inJSON.m_List[i], ioWriter);
                        break;

                    case NodeType.Bool:
                        ioWriter.Write(inJSON.m_NumberValue > 0);
                        break;

                    case NodeType.Number:
                        ioWriter.Write(inJSON.m_NumberValue);
                        break;

                    case NodeType.String:
                        ioWriter.Write(inJSON.m_StringValue);
                        break;
                }
            }

            /// <summary>
            /// Converts from a Base64 string into a JSON object.
            /// </summary>
            static public JSON FromBase64(JSON inJSON, string inBase64)
            {
                var bytes = Convert.FromBase64String(inBase64);
                using(var stream = new MemoryStream(bytes))
                {
                    stream.Position = 0;
                    return Deserialize(stream);
                }
            }

            /// <summary>
            /// Reads a JSON object from the Stream.
            /// </summary>
            static public JSON Deserialize(Stream inStream)
            {
                using(var reader = new BinaryReader(inStream))
                {
                    return Deserialize(reader);
                }
            }

            /// <summary>
            /// Reads a JSON object from the BinaryReader.
            /// </summary>
            static public JSON Deserialize(BinaryReader inReader)
            {
                NodeType type = (NodeType)inReader.ReadByte();
                switch(type)
                {
                    case NodeType.Object:
                        {
                            int count = inReader.ReadInt32();
                            JSON obj = CreateObject();
                            for (int i = 0; i < count; ++i)
                            {
                                string key = inReader.ReadString();
                                obj.m_Dict.Add(key, Deserialize(inReader));
                            }
                            return obj;
                        }

                    case NodeType.Array:
                        {
                            int count = inReader.ReadInt32();
                            JSON arr = CreateArray(count);
                            for (int i = 0; i < count; ++i)
                                arr.m_List.Add(Deserialize(inReader));
                            return arr;
                        }

                    case NodeType.Bool:
                        {
                            JSON boolNode = new JSON(NodeType.Bool);
                            boolNode.AsBool = inReader.ReadBoolean();
                            return boolNode;
                        }

                    case NodeType.Number:
                        {
                            JSON numberNode = new JSON(NodeType.Number);
                            numberNode.AsDouble = inReader.ReadDouble();
                            return numberNode;
                        }

                    case NodeType.String:
                        {
                            JSON stringNode = new JSON(NodeType.String);
                            stringNode.AsString = inReader.ReadString();
                            return stringNode;
                        }

                    default:
                        {
                            JSON nullNode = new JSON(NodeType.Null);
                            return nullNode;
                        }
                }
            }
        }

        #endregion

        #region Factory

        /// <summary>
        /// Returns a new JSON object.
        /// </summary>
        static public JSON CreateObject()
        {
            return new JSON(NodeType.Object);
        }

        /// <summary>
        /// Returns a new JSON object with the starting capacity.
        /// </summary>
        static public JSON CreateObject(int inCapacity)
        {
            return new JSON(NodeType.Object, inCapacity);
        }

        /// <summary>
        /// Returns a new JSON array.
        /// </summary>
        static public JSON CreateArray()
        {
            return new JSON(NodeType.Array);
        }

        /// <summary>
        /// Returns a new JSON array with the starting capacity.
        /// </summary>
        static public JSON CreateArray(int inCapacity)
        {
            return new JSON(NodeType.Array, inCapacity);
        }

        /// <summary>
        /// Creates a null JSON value.
        /// </summary>
        static public JSON CreateNull()
        {
            return new JSON(NodeType.Null);
        }

        /// <summary>
        /// Creates a boolean JSON value.
        /// </summary>
        static public JSON CreateValue(bool inbValue)
        {
            JSON json = new JSON(NodeType.Bool);
            json.AsBool = inbValue;
            return json;
        }

        /// <summary>
        /// Creates a string JSON value.
        /// </summary>
        static public JSON CreateValue(string inValue)
        {
            JSON json = new JSON(NodeType.String);
            json.AsString = inValue;
            return json;
        }

        /// <summary>
        /// Creates a Double JSON value.
        /// </summary>
        static public JSON CreateValue(double inValue)
        {
            JSON json = new JSON(NodeType.Number);
            json.AsDouble = inValue;
            return json;
        }

        /// <summary>
        /// Creates a Float JSON value.
        /// </summary>
        static public JSON CreateValue(float inValue)
        {
            JSON json = new JSON(NodeType.Number);
            json.AsFloat = inValue;
            return json;
        }

        /// <summary>
        /// Creates an Integer JSON value.
        /// </summary>
        static public JSON CreateValue(int inValue)
        {
            JSON json = new JSON(NodeType.Number);
            json.AsInt = inValue;
            return json;
        }

        /// <summary>
        /// Creates an Unsigned Integer JSON value.
        /// </summary>
        static public JSON CreateValue(uint inValue)
        {
            JSON json = new JSON(NodeType.Number);
            json.AsUInt = inValue;
            return json;
        }

        /// <summary>
        /// Creates a Long Integer JSON value.
        /// </summary>
        static public JSON CreateValue(long inValue)
        {
            JSON json = new JSON(NodeType.Number);
            json.AsLong = inValue;
            return json;
        }

        // Generates a Lazy node.
        static private JSON MakeLazy(JSON inParent, string inKey = null)
        {
            JSON lazy = new JSON(NodeType.Lazy);
            lazy.m_LazyParent = inParent;
            lazy.m_StringValue = inKey;
            return lazy;
        }

        #endregion

        #region Overrides

        static public bool operator==(JSON inA, object inB)
        {
            if (ReferenceEquals(inA, inB))
                return true;

            bool aIsNull = ReferenceEquals(inA, null) || inA.IsNullOrLazy();
            bool bIsNull = ReferenceEquals(inB, null) || (inB is JSON && ((JSON)inB).IsNullOrLazy());
            return aIsNull && bIsNull;
        }

        static public bool operator!=(JSON inA, object inB)
        {
            return !(inA == inB);
        }

        static public implicit operator bool(JSON inJSON)
        {
            return ReferenceEquals(inJSON, null) ? false : inJSON.AsBool;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        #endregion
    
        #region Errors

        public sealed class ParseException : Exception
        {
            public ParseException(string inMessage)
                : base(inMessage)
                {
                }
        }

        #endregion // Errors
    }
}