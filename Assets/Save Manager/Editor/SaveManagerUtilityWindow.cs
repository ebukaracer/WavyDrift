# if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Racer.SaveManager
{
    internal class SaveManagerUtilityWindow : EditorWindow
    {
        private const string SaveKey = "SMUW_SearchCache";
        private static string _keyField;
        private static bool _shouldRetain;
        private const int Width = 800;

        [MenuItem("Save_Manager/Save Utility Window")]
        public static void DisplayWindow()
        {
            _keyField = EditorPrefs.GetString(SaveKey);

            var window = GetWindow<SaveManagerUtilityWindow>();

            window.titleContent = new GUIContent("Save Utility Window");

            // Limit size of the window, non re-sizable
            window.minSize = new Vector2(Width, Width / 2);
            window.maxSize = new Vector2(Width, Width / 2);
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(10);

            EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(Width));
            EditorGUILayout.HelpBox("Input multiple keys by separating with comma(,)", MessageType.Info);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(5);

            GUILayout.Label("Base Settings", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal(GUILayout.Width(Width - 5));

            _keyField = EditorGUILayout.TextField("Key", _keyField);

            if (GUILayout.Button(new GUIContent("Check value", "Checks if value associated with the provided key exists in save profile."),
                GUILayout.MaxWidth(150)))
            {
                SaveUtility.CheckValue(_keyField);
            }

            if (GUILayout.Button(new GUIContent("Erase value", "Deletes value associated with the provided key from save profile."),
                GUILayout.MaxWidth(150)))
            {
                SaveUtility.EraseValue(_keyField);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(15);

            GUILayout.Label("Other Settings", EditorStyles.boldLabel);

            EditorGUILayout.HelpBox("This would delete all data present in the save-profile.", MessageType.Info);
            if (GUILayout.Button(new GUIContent("Erase all values", "Deletes all values from save profile.")))
                SaveUtility.EraseAllValues();

            EditorGUILayout.Space(15);

            EditorGUILayout.HelpBox("Whether or not to preserve the Inputted Key-Value(s) every time this Window is opened.", MessageType.Info);
            _shouldRetain = EditorGUILayout.Toggle("Retain Key Input(s)", _shouldRetain, GUILayout.MaxWidth(Width / 4));
        }

        private void OnDestroy()
        {
            if (string.IsNullOrEmpty(_keyField))
                return;

            if (_shouldRetain)
                EditorPrefs.SetString(SaveKey, _keyField);
            else
                EditorPrefs.DeleteKey(SaveKey);
        }
    }
}
#endif
