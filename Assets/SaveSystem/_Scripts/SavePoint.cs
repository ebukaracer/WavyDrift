using UnityEngine;
#if UNITY_EDITOR
#endif

namespace Racer.SaveSystem
{
    /// <summary>
    /// Establishes a point to store all the saved data to a file.
    /// Writing the contents of the saved data is a pretty much a heavy operation,
    /// This script implements some unity-callbacks where the actual saving(writing to file)
    /// would be done.
    /// This script must be added to at-most one gameobject in the scene, it can also be 
    /// persisted across scenes.
    /// </summary>

    [DefaultExecutionOrder(-100), AddComponentMenu("SaveSystem/SavePoint")]
    class SavePoint : MonoBehaviour
    {
        /// <summary>
        /// Loads all saved- in data when game is loaded.
        /// </summary>
        private void Awake()
        {
            SaveSystem.Load();

            // Comment out
            Logging.Log($"{nameof(SavePoint)} Initialized!");
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
        /// See also: <seealso cref="OnApplicationFocus(bool)"/>.
        /// </summary>
        private void OnApplicationQuit()
        {
            SaveSystem.Save();
        }
    }
}