/*
 * Copyright (C) 2017 - 2020. Autumn Beauchesne. All rights reserved.
 * Author:  Autumn Beauchesne
 * Date:    8 April 2020
 * 
 * File:    EightCC.Registry.cs
 * Purpose: EightCC registration.
 */

#if UNITY_EDITOR
#define ALLOW_REGISTRY
#endif // UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BeauData
{
    public partial struct EightCC : IEquatable<EightCC>, IComparable<EightCC>
    {
        #if ALLOW_REGISTRY

        /// <summary>
        /// Registers a EightCC value under the given type.
        /// This will then be available for any EightCCSelector attributes with that type.
        /// </summary>
        static public EightCC Register(Type inType, string inCode, string inName = null, string inDescription = null)
        {
            EightCC code = EightCC.Parse(inCode);
            return Register(inType, code, inName, inDescription);
        }

        /// <summary>
        /// Registers a EightCC value under the given type.
        /// This will then be available for any EightCCSelector attributes with that type.
        /// </summary>
        static public EightCC Register(Type inType, EightCC inCode, string inName = null, string inDescription = null)
        {
            GetRegistry(inType, true, false).AddEntry(inCode, inName, inDescription);
            return inCode;
        }

        /// <summary>
        /// Deregisters an EightCC value under the given type.
        /// This will no longer be available for any EightCCSelector with that type.
        /// </summary>
        static public void Deregister(Type inType, string inCode)
        {
            EightCC code = EightCC.Parse(inCode);
            Deregister(inType, inCode);
        }

        /// <summary>
        /// Deregisters an EightCC value under the given type.
        /// This will no longer be available for any EightCCSelector with that type.
        /// </summary>
        static public void Deregister(Type inType, EightCC inCode)
        {
            Registry r = GetRegistry(inType, false, false);
            if (r != null)
            {
                r.RemoveEntry(inCode);
            }
        }

        static private Dictionary<long, Registry> s_Registry = new Dictionary<long, Registry>();

        static private Registry GetRegistry(Type inType, bool inbCreate, bool inbScan)
        {
            if (inbScan)
                ScanForRegistry();

            long handle = inType.TypeHandle.Value.ToInt64();

            Registry registry;
            if (!s_Registry.TryGetValue(handle, out registry) && inbCreate)
            {
                registry = new Registry();
                s_Registry.Add(handle, registry);
            }
            return registry;
        }

        #region Types

        private class Registry
        {
            private List<RegistryEntry> m_EntryList = new List<RegistryEntry>();
            private Dictionary<EightCC, RegistryEntry> m_EntryMap = new Dictionary<EightCC, RegistryEntry>();

            public void AddEntry(EightCC inCode, string inName, string inDescription)
            {
                RegistryEntry entry;
                if (m_EntryMap.TryGetValue(inCode, out entry))
                    throw new ArgumentException("EightCC with code " + inCode.ToString() + " has already been registered", "inCode");

                entry = new RegistryEntry(inCode, inName, inDescription);
                m_EntryList.Add(entry);
                m_EntryMap.Add(inCode, entry);
            }

            public bool RemoveEntry(EightCC inCode)
            {
                RegistryEntry entry;
                if (m_EntryMap.TryGetValue(inCode, out entry))
                {
                    m_EntryMap.Remove(inCode);
                    m_EntryList.Remove(entry);
                    return true;
                }
                
                return false;
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
            public EightCC Value;

            public string Display;
            public string Tooltip;

            public RegistryEntry(EightCC inValue, string inName, string inDescription)
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
                    if (member.FieldType == typeof(EightCC) && member.IsStatic && member.IsInitOnly)
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
        /// Registers a EightCC value under the given type.
        /// This will then be available for any EightCCSelector attributes with that type.
        /// </summary>
        static public EightCC Register(Type inType, string inCode, string inName = null, string inDescription = null)
        {
            return EightCC.Parse(inCode);
        }

        /// <summary>
        /// Registers a EightCC value under the given type.
        /// This will then be available for any EightCCSelector attributes with that type.
        /// </summary>
        static public EightCC Register(Type inType, EightCC inCode, string inName = null, string inDescription = null)
        {
            return inCode;
        }

        /// <summary>
        /// Deregisters an EightCC value under the given type.
        /// This will no longer be available for any EightCCSelector with that type.
        /// </summary>
        static public void Deregister(Type inType, string inCode)
        {
        }

        /// <summary>
        /// Deregisters an EightCC value under the given type.
        /// This will no longer be available for any EightCCSelector with that type.
        /// </summary>
        static public void Deregister(Type inType, EightCC inCode)
        {
        }

        #endif // ALLOW_REGISTRY
    }
}