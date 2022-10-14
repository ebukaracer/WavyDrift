using Newtonsoft.Json;
using System;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Racer.SaveSystem
{
    /// <summary>
    /// Saves and stores types of (int, float, bool, string) in a JSON format, to a file.
    /// The types available for saving are contained in the <see cref="CustomConverter"/>.
    /// </summary>
    public class SaveSystem
    {
#if UNITY_2021_1_OR_NEWER
        private static DataValues _dataValues = new();
#else
        private static DataValues _dataValues = new DataValues();
#endif
        // Checks if the contents has been loaded previously instead of reloading every time.
        private static bool _hasLoaded;

        // Short-hand way of accessing the static methods in (SavePaths.cs)
        private static string _saveDir = SavePaths.SaveDirectoryPath;
        private static string _saveFile = SavePaths.SaveFilePath;
        private static string _saveFileMeta = SavePaths.SaveFileMetaPath;



        /// <summary>
        /// Saves in a value (to the save-file) marked with the key provided.
        /// </summary>
        /// <typeparam name="T">Type of data to save(int, float, string, bool)</typeparam>
        /// <param name="key">Your Save-key</param>
        /// <param name="data">Your Save-value</param>
        public static void SaveData<T>(string key, T data)
        {
            _dataValues.AddValue(key, data);
        }

        /// <summary>
        /// Does the specified key contain such data in the save-file..?
        /// </summary>
        /// <param name="key">Your Save-key</param>
        /// <returns>True if such data exists otherwise False</returns>
        public static bool Contains(string key)
        {
            if (!_hasLoaded)
                Load();

            return _dataValues.ContainsKey(key);
        }

        /// <summary>
        /// Retrieves the value(from the save-file) associated with the key provided.
        /// </summary>
        /// <typeparam name="T">Type of data to load(int, float, bool, string)</typeparam>
        /// <param name="key">Your Save-key</param>
        /// <returns>Value of the specified 'Save-key'.</returns>
        public static T GetData<T>(string key)
        {
            if (!_hasLoaded)
                Load();

            if (Contains(key)) return _dataValues.GetValue<T>(key);

            Logging.LogWarning($"[{key}] does not exist initially. <b>Returned type's default value</b>.");

            return default;
        }

        /// <summary>
        /// See: <see cref="GetData{T}(string)"/>.
        /// However, if the value does not exist in the save-file, a default value is returned.(if specified).
        /// </summary>
        /// <returns>Value of the specified 'Save-key' otherwise the default value specified.</returns>
        public static T GetData<T>(string key, T defaultValue)
        {
            return Contains(key) ? _dataValues.GetValue<T>(key) : defaultValue;
        }

        /// <summary>
        /// Saves all data at once.
        /// </summary>
        /// <remarks>
        /// This function is public since you might decide to save everything
        /// at a specific point in time throughout your application lifetime.
        /// For example you might decide to call this function when 
        /// loading into a new scene or when your app looses focus.
        /// See: <see cref="SavePoint"/>.
        /// </remarks>
        internal static void Save()
        {
            var contents = JsonConvert.SerializeObject(_dataValues, Formatting.Indented);

            File.WriteAllText(SavePaths.SaveFilePath, contents);
        }


        /// <summary>
        /// Loads all key_values stored in the (save-file) as json string.
        /// Creates a new save-file if there is none to store the loaded contents.
        /// Loaded save-file type-cast to the target type upon which it was saved.
        /// </summary>
        /// <remarks>
        /// Basically, if the contents of the save-file is null/empty, the data-value container,
        /// gets initialized to null which would result to an exception,
        /// to prevent this we re-save the null/empty-contents which,
        /// initializes the content(s) of the data-value container to,
        /// empty strings.
        /// </remarks>
        internal static void Load()
        {
            try
            {
                if (!InitSaveDirAndFile())
                {
                    Logging.LogWarning("Couldn't Load a Save File");

                    return;
                }

                var contents = File.ReadAllText(_saveFile);

                if (string.IsNullOrEmpty(contents))
                {
                    Save();

                    contents = File.ReadAllText(_saveFile);
                }

                _dataValues = JsonConvert.DeserializeObject<DataValues>(contents, new CustomConverter());

                _hasLoaded = true;
            }

            catch (Exception ex)
            {
                Logging.Log(ex.Message);
            }

            // Creates a new save-file and directory.
            // Swift if save-file/directory  already exits.
            bool InitSaveDirAndFile()
            {
                try
                {
                    if (!Directory.Exists(_saveDir))
                        Directory.CreateDirectory(_saveDir);

                    if (File.Exists(_saveFile)) return true;

                    // Refactor from (unity version >= 2021)
                    using (File.Create(_saveFile)) { }

                    Logging.Log($"<b>{SavePaths.SaveFileName}</b> was successfully created at <b>{_saveFile}</b>.");
#if UNITY_EDITOR
                    if (!UnityEngine.Application.isPlaying)
                        AssetDatabase.Refresh();
#endif
                    return true;
                }
                catch (Exception ex)
                {
                    Logging.LogWarning(ex.Message);

                    return false;
                }
            }
        }

        /// <summary>
        /// Handles the actual creation of save-file and it's directory in the editor.
        /// </summary>
#if UNITY_EDITOR
        internal static void CreateSaveDirAndFile(bool overwriteSaveFile)
        {
            try
            {
                // Creates a save-directory
                if (!Directory.Exists(_saveDir))
                    Directory.CreateDirectory(_saveDir);


                // Creates or overwrites an existing save-file
                if (File.Exists(_saveFile))
                {
                    if (overwriteSaveFile)
                        CreateFile($"<b>{SavePaths.SaveFileName}</b> was successfully overwritten <b>{_saveFile}</b>.");
                    else
                        Logging.LogWarning($"A <b>{SavePaths.SaveFileName}</b> was previously created. " +
                        $"You may toggle <b>overwrite</b> if you want to overwrite the file instead.");
                }
                else
                    CreateFile($"<b>{SavePaths.SaveFileName}</b> was successfully created at <b>{_saveFile}</b>.");
            }

            catch (Exception ex)
            {
                Logging.LogWarning(ex.Message);
            }

            void CreateFile(object msg)
            {
#if UNITY_2021_1_OR_NEWER
                using var fs = File.Create(_saveFile);
#else
                using (var fs = File.Create(_saveFile)) { }
#endif
                Logging.Log(msg);

                if (!UnityEngine.Application.isPlaying)
                    AssetDatabase.Refresh();
            }
        }
#endif

        /// <summary>
        /// Handles the deletion of a save-file.
        /// </summary>
        public static void DeleteSaveFile()
        {
            try
            {
                if (!File.Exists(_saveFile))
                {
                    Logging.LogWarning($"No save-file found to delete!");

                    return;
                }

                File.Delete(_saveFile);

                if (!string.IsNullOrEmpty(_saveFileMeta))
                    File.Delete(_saveFileMeta);

                _dataValues.KeyValues.Clear();

                Logging.Log($"<b>{SavePaths.SaveFileName}</b> was successfully deleted.");

#if UNITY_EDITOR
                if (!UnityEngine.Application.isPlaying)
                    AssetDatabase.Refresh();
#endif
            }

            catch (Exception ex)
            {
                Logging.LogWarning(ex.Message);
            }
        }
    }
}
