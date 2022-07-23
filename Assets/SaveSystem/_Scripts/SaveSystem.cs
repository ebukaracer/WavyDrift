using Newtonsoft.Json;
using Racer.Utilities;
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
        private static DataValues dataValues = new DataValues();

        // Checks to know if the contents has been loaded previously instead of reloading every time.
        static bool hasLoaded;


        /// <summary>
        /// Saves in a value (to the save-file) marked with the key provided.
        /// </summary>
        /// <typeparam name="T">Type of data to save(int, float, string, bool)</typeparam>
        /// <param name="key">Your Save-key</param>
        /// <param name="data">Your Save-value</param>
        public static void SaveData<T>(string key, T data)
        {
            dataValues.AddValue(key, data);

            Save();
        }

        /// <summary>
        /// Does the specified key contain such data in the save-file..?
        /// </summary>
        /// <param name="Key">Your Save-key</param>
        /// <returns>True if such data exists otherwise False</returns>
        public static bool Contains(string Key)
        {
            if (!hasLoaded)
                Load();

            return dataValues.ContainsKey(Key);
        }

        /// <summary>
        /// Retrieves the value(from the save-file) associated with the key provided.
        /// </summary>
        /// <typeparam name="T">Type of data to load(int, float, bool, string)</typeparam>
        /// <param name="key">Your Save-key</param>
        /// <returns>Value of the specified 'Save-key'.</returns>
        public static T GetData<T>(string key)
        {
            if (!hasLoaded)
                Load();

            if (!Contains(key))
            {
                Logging.LogWarning($"[{key}] does not exist initially. <b>Returned type's default value</b>.");

                return default;
            }

            return dataValues.GetValue<T>(key);
        }

        /// <summary>
        /// See: <see cref="GetData{T}(string)"/>.
        /// However, if the value does not exist in the save-file, a default value is returned.(if specified).
        /// </summary>
        /// <returns>Value of the specified 'Save-key' otherwise the default value specified.</returns>
        public static T GetData<T>(string key, T defaultValue)
        {
            if (Contains(key))
                return dataValues.GetValue<T>(key);

            return defaultValue;
        }

        /// <summary>
        /// Saves all data at once.
        /// This function is public since you might decide to save everything
        /// at a specific point in time throughout your application lifetime.
        /// For example you might decide to call this function when 
        /// loading into a new scene or when your app looses focus.
        /// See: <see cref="SavePoint"/>.
        /// </summary>
        public static void Save()
        {
            var contents = JsonConvert.SerializeObject(dataValues, Formatting.Indented);

            File.WriteAllText(SavePaths.SaveFilePath, contents);
        }


        /// <summary>
        /// Loads all key_values stored in the (save-file) as json string.
        /// Creates a new save-file if there is none to store the loaded contents.
        /// Loaded save-file casted to the target type upon which it was saved.
        /// </summary>
        public static void Load()
        {
            var saveFile = string.Empty;

            try
            {
                if (!InitSaveDirAndFile())
                {
                    Logging.LogWarning("Couldn't Load a Save File");

                    return;
                }

                saveFile = SavePaths.SaveFilePath;

                string contents = File.ReadAllText(saveFile);

                ///<summary>
                /// Basically, if the contents of the save-file is null/empty, the data-value container,
                /// gets initialized to null which would result to an exception,
                /// to prevent this we re-save the null/empty-contents which,
                /// initializes the content(s) of the data-value container to,
                /// empty strings.
                /// </summary>
                if (string.IsNullOrEmpty(contents))
                {
                    Save();

                    contents = File.ReadAllText(saveFile);
                }

                dataValues = JsonConvert.DeserializeObject<DataValues>(contents, new CustomConverter());

                hasLoaded = true;
            }

            catch (Exception ex)
            {
                Logging.Log(ex.Message);
            }

            // Creates a new save-file and directory.
            // Swift if save-file/directory  already exits.
            static bool InitSaveDirAndFile()
            {
                string saveDir = SavePaths.SaveDirectoryPath;

                string saveFile = SavePaths.SaveFilePath;

                try
                {
                    if (!Directory.Exists(saveDir))
                        Directory.CreateDirectory(saveDir);

                    if (!File.Exists(saveFile))
                    {
                        // Refactor from (unity version >= 2021)
                        using (var fs = File.Create(saveFile)) { }

                        Logging.Log($"<b>{SavePaths.SaveFileName}</b> was successfully created at <b>{saveFile}</b>.");
#if UNITY_EDITOR
                        AssetDatabase.Refresh();
#endif
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    Logging.LogWarning(ex.Message);

                    return false;
                }
            }
        }


#if UNITY_EDITOR
        /// <summary>
        /// Handles the actual creation of save-file and it's directory in the editor.
        /// </summary>
        public static void CreateSaveDirAndFile()
        {
            string saveDir = SavePaths.SaveDirectoryPath;

            string saveFile = SavePaths.SaveFilePath;

            try
            {
                // Creates a save-directory
                if (!Directory.Exists(saveDir))
                    Directory.CreateDirectory(saveDir);


                // Creates or overwrites an existing save-file
                if (File.Exists(saveFile))
                {
                    if (SaveSystemUtilityWindow.OverwriteSaveFile)
                        CreateFile($"<b>{SavePaths.SaveFileName}</b> was successfully overwritten <b>{saveFile}</b>.");
                    else
                        Logging.LogWarning($"A <b>{SavePaths.SaveFileName}</b> was previously created. " +
                        $"You may toggle <b>overwrite</b> if you want to overwrite the file instead.");
                }
                else
                    CreateFile($"<b>{SavePaths.SaveFileName}</b> was successfully created at <b>{saveFile}</b>.");
            }

            catch (Exception ex)
            {
                Logging.LogWarning(ex.Message);
            }

            void CreateFile(object msg)
            {
                // Refactor from (unity version >= 2021)
                using (var fs = File.Create(saveFile)) { }

                Logging.Log(msg);

                AssetDatabase.Refresh();
            }
        }
#endif

        /// <summary>
        /// Handles the deletion of a save-file.
        /// </summary>
        public static void DeleteSaveFile()
        {
            string saveFile = SavePaths.SaveFilePath;

            string saveFileMeta = SavePaths.SaveFileMetaPath;

            try
            {
                if (!File.Exists(saveFile))
                {
                    Logging.LogWarning($"No save-file found to delete!");

                    return;
                }

                File.Delete(saveFile);

                if (!string.IsNullOrEmpty(saveFileMeta))
                    File.Delete(saveFileMeta);

                Logging.Log($"<b>{SavePaths.SaveFileName}</b> was successfully deleted.");

                RestartAndroid.Restart();

#if UNITY_EDITOR
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
