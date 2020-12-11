/*
 * Copyright (C) 2017 - 2020. Autumn Beauchesne. All rights reserved.
 * Author:  Autumn Beauchesne
 * Date:    3 May 2017
 * 
 * File:    PathUtility.cs
 * Purpose: Path shortcut functions.
*/

using System.IO;
using UnityEngine;

namespace BeauData
{
    /// <summary>
    /// Serialization format and shortcuts.
    /// </summary>
    static public class PathUtility
    {
        /// <summary>
        /// Returns the path with the extension for the given format, if an extension was not provided already.
        /// </summary>
        static public string CorrectPath(string inFilePath, Serializer.Format inFormat = Serializer.Format.AutoDetect)
        {
            // Appends the correct file extension if we haven't provided one
            if (!Path.HasExtension(inFilePath))
                return Path.ChangeExtension(inFilePath, inFormat.Extension());

            return inFilePath;
        }

        /// <summary>
        /// Returns the path for a file in StreamingAssets.
        /// </summary>
        static public string StreamingAssetsPath(string inRelativePath)
        {
            return Path.Combine(Application.streamingAssetsPath, inRelativePath);
        }

        /// <summary>
        /// Returns the path for a file in PersistentData.
        /// </summary>
        static public string PersistentDataPath(string inRelativePath)
        {
            return Path.Combine(Application.persistentDataPath, inRelativePath);
        }

        /// <summary>
        /// Returns the path for a file in Data.
        /// </summary>
        static public string DataPath(string inRelativePath)
        {
            return Path.Combine(Application.dataPath, inRelativePath);
        }

        /// <summary>
        /// Returns the path for a file in the temporary cache.
        /// </summary>
        static public string TempCachePath(string inRelativePath)
        {
            return Path.Combine(Application.temporaryCachePath, inRelativePath);
        }

        /// <summary>
        /// Transforms a file path into a URL.
        /// For use with UnityWebRequest or WWW.
        /// </summary>
        static public string PathToURL(string inFilePath)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                case RuntimePlatform.WebGLPlayer:
                    return inFilePath;

                case RuntimePlatform.WSAPlayerARM:
                case RuntimePlatform.WSAPlayerX64:
                case RuntimePlatform.WSAPlayerX86:
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    return "file:///" + inFilePath;

                default:
                    return "file://" + inFilePath;
            }
        }
    }
}
