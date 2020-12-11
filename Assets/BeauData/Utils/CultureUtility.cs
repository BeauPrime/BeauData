/*
 * Copyright (C) 2017 - 2020. Autumn Beauchesne. All rights reserved.
 * Author:  Autumn Beauchesne
 * Date:    24 May 2020
 * 
 * File:    CultureUtility.cs
 * Purpose: Culture utility methods.
 */

using System;
using System.Globalization;
using System.Threading;

namespace BeauData
{
    /// <summary>
    /// Culture utility methods and references.
    /// </summary>
    static public class CultureUtility
    {
        #region Invariant

        /// <summary>
        /// Invariant number format.
        /// Use this to ensure consistent number serialization/deserialization across cultures.
        /// </summary>
        static public readonly NumberFormatInfo InvariantNumberFormat = CultureInfo.InvariantCulture.NumberFormat;

        #endregion // Invariant

        #region Swap

        public struct CultureSwapScope : IDisposable
        {
            private CultureInfo m_Restore;

            internal CultureSwapScope(CultureInfo inTarget)
            {
                CultureInfo current = Thread.CurrentThread.CurrentCulture;
                if (inTarget != current)
                {
                    m_Restore = current;
                    Thread.CurrentThread.CurrentCulture = inTarget;
                }
                else
                {
                    m_Restore = null;
                }
            }

            public void Dispose()
            {
                if (m_Restore != null)
                {
                    Thread.CurrentThread.CurrentCulture = m_Restore;
                    m_Restore = null;
                }
            }
        }

        /// <summary>
        /// Temporary swaps the current thread over to the given CultureInfo.
        /// Dispose of the returned object to restore the thread to its previous CultureInfo.
        /// </summary>
        static public CultureSwapScope UseCulture(CultureInfo inInfo)
        {
            return new CultureSwapScope(inInfo);
        }

        /// <summary>
        /// Temporary swaps the current thread over to the invariant CultureInfo.
        /// Dispose of the returned object to restore the thread to its previous CultureInfo.
        /// </summary>
        static public CultureSwapScope UseInvariantCulture()
        {
            return new CultureSwapScope(CultureInfo.InvariantCulture);
        }
    
        #endregion // Swap
    }
}