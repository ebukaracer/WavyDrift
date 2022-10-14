#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Racer.SaveSystem
{
    internal class SaveSystemUtilityWindow : EditorWindow
    {
        private const int Width = 400;

        public static bool OverwriteSaveFile { get; private set; }

        [MenuItem("Save_System/Save Utility Window")]
        public static void DisplayWindow()
        {
            OverwriteSaveFile = EditorPrefs.GetBool("SSUW_owsf");

            var window = GetWindow<SaveSystemUtilityWindow>();

            window.titleContent = new GUIContent("Save Utility Window");

            // Limit size of the window, non re-sizable
            window.minSize = new Vector2(Width, Width);
            window.maxSize = new Vector2(Width, Width);
        }

        /// <value>
        /// Returns a path from <see cref="SavePaths"/> class.
        /// </value>
        private static string PathTemplate
        {
            get
            {
                var path = SavePaths.SaveDirectoryPath;

#if UNITY_2021_1_OR_NEWER
                var shortenedPath = path[path.IndexOf("Assets", StringComparison.Ordinal)..];
#else
                var shortenedPath = path.Substring(path.IndexOf("Assets", StringComparison.Ordinal));
#endif
                return string.IsNullOrEmpty(shortenedPath) ? path : shortenedPath;
            }
        }

        private void OnGUI()
        {
#if UNITY_2019_1_OR_NEWER
            EditorGUILayout.Space(10);
#else
            EditorGUILayout.Space();
#endif
            EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(Width));

            EditorGUILayout.HelpBox($"A default [{SavePaths.SaveFileName}] would be created at the following directory: [{PathTemplate}].\n" +
                $" The default settings such as the save-file path or save-file name can be modified via: [{nameof(SavePaths)}.cs]", MessageType.Info);

            EditorGUILayout.EndHorizontal();

#if UNITY_2019_1_OR_NEWER
            EditorGUILayout.Space(5);
#else
            EditorGUILayout.Space();
#endif
            GUILayout.Label("Base Settings", EditorStyles.boldLabel);

            if (GUILayout.Button(new GUIContent("Create New Save File", "Creates a new save file."), GUILayout.MaxWidth(Width)))
                SaveSystem.CreateSaveDirAndFile(OverwriteSaveFile);

            if (GUILayout.Button(new GUIContent("Delete Save File", "Deletes the created save file"), GUILayout.MaxWidth(Width)))
                SaveSystem.DeleteSaveFile();

#if UNITY_2019_1_OR_NEWER
            EditorGUILayout.Space(15);
#else
            EditorGUILayout.Space();
#endif
            GUILayout.Label("Other Settings", EditorStyles.boldLabel);

            EditorGUILayout.HelpBox($"Toggling [overwrite] would overwrite the previously created save-file during creation.", MessageType.Info);

            OverwriteSaveFile = EditorGUILayout.Toggle("Overwrite Save file", OverwriteSaveFile, GUILayout.MaxWidth(Width));
        }
        private void OnDestroy()
        {
            EditorPrefs.SetBool("SSUW_owsf", OverwriteSaveFile);
        }
    }
}
#endif
