using UnityEditor;
using UnityEngine;

namespace Racer.SaveManager
{
    public class SaveManagerUtilityWindow : EditorWindow
    {
        static string keyField;
        static string[] multipleKeyValues;
        const int WIDTH = 800;

        [MenuItem("Save_Manager/Save Utility Window")]
        public static void DisplayWindow()
        {
            keyField = EditorPrefs.GetString("keyfield");

            var window = GetWindow<SaveManagerUtilityWindow>();

            // Add tool-tip argument as well
            window.titleContent = new GUIContent("Save Utility Window");

            // Limit size of the window, non re-sizable
            window.minSize = new Vector2(WIDTH, WIDTH / 2);
            window.maxSize = new Vector2(WIDTH, WIDTH / 2);
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(10);

            EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(WIDTH));
            EditorGUILayout.HelpBox("Input multiple keys by separating with comma(,) or space(' ').", MessageType.Info);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(5);

            GUILayout.Label("Base Settings", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal(GUILayout.Width(WIDTH - 5));
            keyField = EditorGUILayout.TextField("Key", keyField);
            if (GUILayout.Button(new GUIContent("Check value", "Checks if value associated with the provided key exists in save profile."),
                GUILayout.MaxWidth(150)))
            {
                multipleKeyValues = keyField.Split(',', ' ');

                foreach (var value in multipleKeyValues)
                {
                    if (string.IsNullOrEmpty(value))
                        continue;

                    Logging.Log($"{value}: { SaveManager.Contains(value)}");
                }
            }
            if (GUILayout.Button(new GUIContent("Erase value", "Deletes value associated with the provided key from save profile."),
                GUILayout.MaxWidth(150)))
            {
                multipleKeyValues = keyField.Split(',', ' ');

                foreach (var value in multipleKeyValues)
                {
                    if (!SaveManager.Contains(value) || string.IsNullOrEmpty(value))
                        continue;

                    SaveManager.ClearPref(value);
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(15);

            GUILayout.Label("Other Settings", EditorStyles.boldLabel);

            if (GUILayout.Button(new GUIContent("Erase all values", "Deletes all values from save profile.")))
                SaveManager.ClearAllPrefs();


            EditorGUILayout.Space(5);

            EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(WIDTH));
            EditorGUILayout.HelpBox("This would delete all data present in the save-profile.", MessageType.Info);
            EditorGUILayout.EndHorizontal();
        }

        private void OnDestroy()
        {
            EditorPrefs.SetString("keyfield", keyField);
        }
    }
}
