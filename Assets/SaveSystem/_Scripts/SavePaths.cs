using UnityEngine;

namespace Racer.SaveSystem
{
    /// <summary>
    /// Encapsulates all paths required by the<see cref="SaveSystem"/>.
    /// </summary>
    static class SavePaths
    {
#if UNITY_EDITOR
        public static string SaveDirectoryPath { get; } = $"{Application.dataPath}/SaveSystem/Saves/";
#endif
#if !UNITY_EDITOR && UNITY_ANDROID
        public static string SaveDirectory { get; } = $"{Application.persistentDataPath}/SaveSystem/Saves/";
#endif
        public static string SaveFileName { get; } = "Save.txt";

        public static string SaveFilePath { get; } = SaveDirectoryPath + SaveFileName;

        public static string SaveFileMetaPath { get; } = SaveFilePath + ".meta";
    }
}