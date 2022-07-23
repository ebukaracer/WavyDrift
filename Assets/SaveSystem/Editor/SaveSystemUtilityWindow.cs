#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Racer.SaveSystem
{
    public class SaveSystemUtilityWindow : EditorWindow
    {
        const int WIDTH = 400;

        public static bool OverwriteSaveFile { get; private set; }

        [MenuItem("Save_System/Save Utility Window")]
        public static void DisplayWindow()
        {
            OverwriteSaveFile = EditorPrefs.GetBool("owsf");

            var window = GetWindow<SaveSystemUtilityWindow>();

            // Add tool-tip argument as well
            window.titleContent = new GUIContent("Save Utility Window");

            // Limit size of the window, non re-sizable
            window.minSize = new Vector2(WIDTH, WIDTH);
            window.maxSize = new Vector2(WIDTH, WIDTH);
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(10);

            EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(WIDTH));
            EditorGUILayout.HelpBox("A default [Save.txt] would be created at the following directory: [Application.datapath/Assets/Save System/Saves].\n" +
                " The default settings such as the save-file path or save-file name can be modified via script.", MessageType.Info);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(5);

            GUILayout.Label("Base Settings", EditorStyles.boldLabel);

            if (GUILayout.Button(new GUIContent("Create New Save File", "Creates a new save file."), GUILayout.MaxWidth(WIDTH)))
                SaveSystem.CreateSaveDirAndFile();

            if (GUILayout.Button(new GUIContent("Delete Save File", "Deletes the created save file"), GUILayout.MaxWidth(WIDTH)))
                SaveSystem.DeleteSaveFile();

            EditorGUILayout.Space(15);

            GUILayout.Label("Other Settings", EditorStyles.boldLabel);

            OverwriteSaveFile = EditorGUILayout.Toggle("Overwrite Save file", OverwriteSaveFile, GUILayout.MaxWidth(WIDTH));

            EditorGUILayout.Space(10);

            EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(WIDTH));
            EditorGUILayout.HelpBox($"Toggling [overwrite] would overwrite the previously created save-file.", MessageType.Info);
            EditorGUILayout.EndHorizontal();
        }
        private void OnDestroy()
        {
            EditorPrefs.SetBool("owsf", OverwriteSaveFile);
        }
    }
}
#endif
