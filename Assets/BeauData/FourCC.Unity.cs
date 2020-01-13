/*
 * Copyright (C) 2017 - 2020. Filament Games, LLC. All rights reserved.
 * Author:  Autumn Beauchesne
 * Date:    28 Nov 2018
 * 
 * File:    FourCC.Unity.cs
 * Purpose: Unity-specific.
 */

#if UNITY_EDITOR
#define ALLOW_REGISTRY
#endif // UNITY_EDITOR

using System;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;
#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
#endif // UNITY_EDITOR

namespace BeauData
{
    public partial struct FourCC : IEquatable<FourCC>, IComparable<FourCC>
    {
        #if UNITY_EDITOR && ALLOW_REGISTRY

        [CustomPropertyDrawer(typeof(FourCC), true)]
        [CustomPropertyDrawer(typeof(FourCCSelectorAttribute), true)]
        private class Editor : PropertyDrawer
        {
            private const int VALUE_WIDTH = 80;

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                CachedSelectionList list = null;

                FourCCSelectorAttribute selector = this.attribute as FourCCSelectorAttribute;
                bool bShowDebug = true;
                if (selector != null)
                {
                    list = GetList(selector);
                    bShowDebug = selector.ShowDebug;
                }

                SerializedProperty valueProp = property.FindPropertyRelative("m_Value");

                Rect labelRect = position;
                Rect indentedRect = EditorGUI.IndentedRect(labelRect);

                label = EditorGUI.BeginProperty(labelRect, label, property);

                if (!string.IsNullOrEmpty(label.text))
                {
                    labelRect.width = EditorGUIUtility.labelWidth;
                    indentedRect.x = labelRect.xMax;
                    indentedRect.width = position.xMax - indentedRect.xMin;
                    EditorGUI.LabelField(labelRect, label);
                }

                int prevIndent = EditorGUI.indentLevel;
                {
                    EditorGUI.indentLevel = 0;

                    Rect fieldRect = indentedRect;

                    if (bShowDebug)
                    {
                        fieldRect.width -= VALUE_WIDTH;
                    }

                    if (list != null)
                    {
                        DropdownInput(fieldRect, list, valueProp);
                    }
                    else
                    {
                        StringInput(fieldRect, valueProp);
                    }

                    if (bShowDebug)
                    {
                        Rect valueRect = indentedRect;
                        valueRect.x += valueRect.width - VALUE_WIDTH;
                        valueRect.width = VALUE_WIDTH;

                        ValueDisplay(valueRect, valueProp);
                    }
                }
                EditorGUI.indentLevel = prevIndent;

                EditorGUI.EndProperty();
            }

            #region Displays

            private void DropdownInput(Rect inRect, CachedSelectionList inList, SerializedProperty inProperty)
            {
                int currentValueIndex;
                if (inProperty.hasMultipleDifferentValues)
                    currentValueIndex = -1;
                else
                    currentValueIndex = inList.GetContentIndex(new FourCC(inProperty.intValue));

                EditorGUI.BeginChangeCheck();
                int nextValueIndex = EditorGUI.Popup(inRect, currentValueIndex, inList.Contents());
                if (EditorGUI.EndChangeCheck() && currentValueIndex != nextValueIndex)
                    inProperty.intValue = (int) inList.GetValue(nextValueIndex);
            }

            private void StringInput(Rect inRect, SerializedProperty inProperty)
            {
                string currentValue;
                if (inProperty.hasMultipleDifferentValues)
                {
                    currentValue = "[multiple]";
                }
                else
                {
                    try
                    {
                        currentValue = FourCC.Stringify(inProperty.intValue, true);
                    }
                    catch
                    {
                        currentValue = "[invalid]";
                    }
                }

                EditorGUI.BeginChangeCheck();
                string nextValue = EditorGUI.DelayedTextField(inRect, currentValue);
                if (EditorGUI.EndChangeCheck() && currentValue != nextValue)
                {
                    FourCC parsedValue;
                    bool bSuccess = FourCC.TryParse(nextValue, out parsedValue);
                    if (!bSuccess)
                        UnityEngine.Debug.LogErrorFormat("[FourCC] Unable to parse value '{0}' as a FourCC", nextValue);
                    else
                        inProperty.intValue = (int) parsedValue;
                }
            }

            private void ValueDisplay(Rect inRect, SerializedProperty inProperty)
            {
                string display;
                if (inProperty.hasMultipleDifferentValues)
                    display = "[multiple]";
                else
                    display = "0x" + inProperty.intValue.ToString("X8");

                EditorGUI.LabelField(inRect, display, EditorStyles.centeredGreyMiniLabel);
            }

            #endregion // Displays

            #region Cached Lists

            static private Dictionary<Type, CachedSelectionList> s_CachedLists = new Dictionary<Type, CachedSelectionList>();

            static private CachedSelectionList GetList(FourCCSelectorAttribute inSelector)
            {
                CachedSelectionList list;
                Type cachingType = inSelector.GetCachingType();
                if (cachingType != null)
                {
                    if (!s_CachedLists.TryGetValue(cachingType, out list))
                    {
                        list = new CachedSelectionList(inSelector);
                        s_CachedLists.Add(cachingType, list);
                    }
                    return list;
                }
                else
                {
                    return new CachedSelectionList(inSelector);
                }
            }

            #endregion // Cached Lists
        }

        private class CachedSelectionList
        {
            private GUIContent[] m_Content;
            private FourCC[] m_Options;

            public GUIContent[] Contents() { return m_Content; }

            public CachedSelectionList() { }

            public CachedSelectionList(FourCCSelectorAttribute inSelector)
            {
                List<Registry> registries = new List<Registry>();
                foreach (var type in inSelector.Types)
                {
                    Registry r = GetRegistry(type, false, true);
                    if (r != null)
                        registries.Add(r);
                }

                if (registries.Count == 0)
                {
                    m_Options = new FourCC[] { FourCC.Zero };
                    m_Content = new GUIContent[] { new GUIContent("[empty]") };
                }
                else
                {
                    List<RegistryEntry> entries = new List<RegistryEntry>();
                    foreach (var registry in registries)
                    {
                        registry.CopyEntries(entries);
                    }
                    entries.Sort();

                    m_Options = new FourCC[entries.Count + 1];
                    m_Content = new GUIContent[entries.Count + 1];

                    m_Options[0] = FourCC.Zero;
                    m_Content[0] = new GUIContent("[empty]");

                    for (int i = 0; i < entries.Count; ++i)
                    {
                        m_Options[i + 1] = entries[i].Value;
                        m_Content[i + 1] = new GUIContent(entries[i].Display, entries[i].Tooltip);
                    }
                }
            }

            public int GetContentIndex(FourCC inValue)
            {
                for (int i = m_Options.Length - 1; i >= 0; --i)
                {
                    if (m_Options[i] == inValue)
                        return i;
                }

                return -1;
            }

            public FourCC GetValue(int inIndex)
            {
                if (inIndex < 0 || inIndex >= m_Options.Length)
                    return FourCC.Zero;

                return m_Options[inIndex];
            }
        }

        #endif // UNITY_EDITOR && ALLOW_REGISTRY
    }

    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.ReturnValue | AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
    public class FourCCSelectorAttribute : PropertyAttribute
    {
        #if UNITY_EDITOR

        internal readonly Type[] Types;
        internal bool ShowDebug { get; set; }

        internal Type GetCachingType()
        {
            return ShouldCacheList() ? GetType() : null;
        }

        #endif // UNITY_EDITOR

        public FourCCSelectorAttribute(Type inType)
        {
            #if UNITY_EDITOR
            Types = new Type[] { inType };
            ShowDebug = false;
            #endif // UNITY_EDITOR
        }

        public FourCCSelectorAttribute(bool inbShowDebug, Type inType)
        {
            #if UNITY_EDITOR
            Types = new Type[] { inType };
            ShowDebug = inbShowDebug;
            #endif // UNITY_EDITOR
        }

        public FourCCSelectorAttribute(Type inFirstType, params Type[] inTypes)
        {
            #if UNITY_EDITOR
            Types = new Type[1 + inTypes.Length];
            Types[0] = inFirstType;
            Array.Copy(inTypes, 0, Types, 1, inTypes.Length);
            ShowDebug = false;
            #endif // UNITY_EDITOR
        }

        public FourCCSelectorAttribute(bool inbShowDebug, Type inFirstType, params Type[] inTypes)
        {
            #if UNITY_EDITOR
            Types = new Type[1 + inTypes.Length];
            Types[0] = inFirstType;
            Array.Copy(inTypes, 0, Types, 1, inTypes.Length);
            ShowDebug = inbShowDebug;
            #endif // UNITY_EDITOR
        }

        protected virtual bool ShouldCacheList()
        {
            return false;
        }
    }
}