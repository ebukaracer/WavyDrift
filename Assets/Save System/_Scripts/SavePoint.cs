using Racer.Utilities;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace Racer.SaveSystem
{
    /// <summary>
    /// Establishes a point to store all the saved data to a file.
    /// </summary>
    /// <remarks>
    /// Writing the contents of the saved data is a pretty much a heavy operation,
    /// This script implements some unity-callbacks where the actual saving(writing to file)
    /// would be done.
    /// This script must be added to at-most one gameobject in the scene, it can also be 
    /// persisted across scenes.
    /// </remarks>

    [DefaultExecutionOrder(-500), AddComponentMenu("SaveSystem/SavePoint")]
    public class SavePoint : SingletonPattern.SingletonPersistent<SavePoint>
    {
        /// <summary>
        /// Loads all saved-in data when game is loaded.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            if (Instance != this)
                return;

            SaveSystem.Load();

            // Comment out, if not already...
            // Logging.Log($"{nameof(SavePoint)} Initialized!");
        }


        /// <summary>
        /// Saves all values manually at the point of calling.
        /// Saving is performed anytime this function is called.
        /// </summary>
        public void SaveAll()
        {
            SaveSystem.Save();
        }

#if !UNITY_EDITOR
        /// <summary>
        /// Saves all values anytime player looses focus.
        /// </summary>
        /// <param name="focus">False if player looses focus otherwise true</param>
        private void OnApplicationFocus(bool focus)
        {
            if (!focus)
            {
                SaveSystem.Save();
            }
        }
#endif
        /// <summary>
        /// Saves all values as soon as game is terminated.
        /// </summary>
        private void OnApplicationQuit()
        {
            SaveSystem.Save();
        }
    }
}