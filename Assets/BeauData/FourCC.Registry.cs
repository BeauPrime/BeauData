/*
 * Copyright (C) 2017 - 2020. Filament Games, LLC. All rights reserved.
 * Author:  Autumn Beauchesne
 * Date:    25 Feb 2019
 * 
 * File:    FourCC.Registry.cs
 * Purpose: FourCC registration.
 */

#if UNITY_EDITOR
#define ALLOW_REGISTRY
#endif // UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BeauData
{
    public partial struct FourCC : IEquatable<FourCC>, IComparable<FourCC>
    {
        #if ALLOW_REGISTRY

        /// <summary>
        /// Registers a FourCC value under the given type.
        /// This will then be available for any FourCCSelector attributes with that type.
        /// </summary>
        static public FourCC Register(Type inType, string inCode, string inName = null, string inDescription = null)
        {
            FourCC code = FourCC.Parse(inCode);
            return Register(inType, code, inName, inDescription);
        }

        /// <summary>
        /// Registers a FourCC value under the given type.
        /// This will then be available for any FourCCSelector attributes with that type.
        /// </summary>
        static public FourCC Register(Type inType, FourCC inCode, string inName = null, string inDescription = null)
        {
            GetRegistry(inType, true, false).AddEntry(inCode, inName, inDescription);
            return inCode;
        }

        static private Dictionary<IntPtr, Registry> s_Registry = new Dictionary<IntPtr, Registry>();

        static private Registry GetRegistry(Type inType, bool inbCreate, bool inbScan)
        {
            if (inbScan)
                ScanForRegistry();

            Registry registry;
            if (!s_Registry.TryGetValue(inType.TypeHandle.Value, out registry) && inbCreate)
            {
                registry = new Registry();
                s_Registry.Add(inType.TypeHandle.Value, registry);
            }
            return registry;
        }

        #region Types

        private class Registry
        {
            private List<RegistryEntry> m_EntryList = new List<RegistryEntry>();
            private Dictionary<FourCC, RegistryEntry> m_EntryMap = new Dictionary<FourCC, RegistryEntry>();

            public void AddEntry(FourCC inCode, string inName, string inDescription)
            {
                RegistryEntry entry;
                if (m_EntryMap.TryGetValue(inCode, out entry))
                    throw new ArgumentException("FourCC with code " + inCode.ToString() + " has already been registered", "inCode");

                entry = new RegistryEntry(inCode, inName, inDescription);
                m_EntryList.Add(entry);
                m_EntryMap.Add(inCode, entry);
            }

            public RegistryEntry[] CopyEntries()
            {
                return m_EntryList.ToArray();
            }

            public void CopyEntries(ICollection<RegistryEntry> outCollection)
            {
                m_EntryList.Sort();
                foreach (var entry in m_EntryList)
                    outCollection.Add(entry);
            }
        }

        private struct RegistryEntry : IComparable<RegistryEntry>
        {
            public FourCC Value;

            public string Display;
            public string Tooltip;

            public RegistryEntry(FourCC inValue, string inName, string inDescription)
            {
                Value = inValue;

                if (string.IsNullOrEmpty(inName))
                {
                    Display = Value.ToString();

                    if (string.IsNullOrEmpty(inDescription))
                    {
                        Tooltip = Display;
                    }
                    else
                    {
                        Tooltip = Display + "\n- " + inDescription;
                    }
                }
                else
                {
                    Display = inName + " [" + Value.ToString() + "]";

                    if (string.IsNullOrEmpty(inDescription))
                    {
                        Tooltip = Display;
                    }
                    else
                    {
                        Tooltip = Display + "\n- " + inDescription;
                    }
                }
            }

            int IComparable<RegistryEntry>.CompareTo(RegistryEntry other)
            {
                #if UNITY_EDITOR
                return UnityEditor.EditorUtility.NaturalCompare(Display, other.Display);
                #else
                return Display.CompareTo(other.Display);
                #endif // UNITY_EDITOR
            }
        }

        #endregion // Types

        #region Assembly Scanning

        static private HashSet<Assembly> s_ScannedAssemblies = new HashSet<Assembly>();

        static private void ScanForRegistry()
        {
            if (s_ScannedAssemblies.Count > 0)
                return;

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                ScanAssembly(assembly);
        }

        static private void ScanAssembly(Assembly inAssembly)
        {
            if (!s_ScannedAssemblies.Add(inAssembly))
                return;

            foreach (var type in inAssembly.GetTypes())
            {
                foreach (var member in type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (member.FieldType == typeof(FourCC) && member.IsStatic && member.IsInitOnly)
                    {
                        // forces initialization
                        member.GetValue(null);
                        break;
                    }
                }
            }
        }

        #endregion // Assembly Scanning

        #else

        /// <summary>
        /// Registers a FourCC value under the given type.
        /// This will then be available for any FourCCSelector attributes with that type.
        /// </summary>
        static public FourCC Register(Type inType, string inCode, string inName = null, string inDescription = null)
        {
            return FourCC.Parse(inCode);
        }

        #endif // ALLOW_REGISTRY
    }
}