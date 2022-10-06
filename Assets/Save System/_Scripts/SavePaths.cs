using UnityEngine;

namespace Racer.SaveSystem
{
    /// <summary>
    /// Encapsulates all paths required by the<see cref="SaveSystem"/>.
    /// </summary>
    internal static class SavePaths
    {
        // Modify your custom path here:
#if !UNITY_EDITOR && UNITY_ANDROID
        public static string SaveDirectoryPath { get; } = $"{Application.persistentDataPath}/Save System/Saves/";
#else
        public static string SaveDirectoryPath { get; } = $"{Application.dataPath}/Save System/Saves/";
#endif
        public static string SaveFileName => "Save.txt";

        public static string SaveFilePath { get; } = SaveDirectoryPath + SaveFileName;

        public static string SaveFileMetaPath { get; } = SaveFilePath + ".meta";
    }
}