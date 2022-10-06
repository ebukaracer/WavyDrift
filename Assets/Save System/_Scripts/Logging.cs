using UnityEngine;

namespace Racer.SaveSystem
{
    /// <summary>
    /// Handles all log messages. 
    /// </summary>
    /// <remarks>
    /// Add these conditionStrings: ENABLE_LOG, ENABLE_LOG_WARNING, ENABLE_LOG_ERROR, preprocessors
    /// in Player Settings under the Scripting Define Symbols Fields.
    /// Doing this would output any log messages from this project to the console.
    /// Also remember to remove the preprocessors when building your Game.
    /// </remarks>
    internal class Logging
    {
        [System.Diagnostics.Conditional("ENABLE_LOG")]
        public static void Log(object message)
        {
            Debug.Log(message);
        }

        [System.Diagnostics.Conditional("ENABLE_LOG_WARNING")]
        public static void LogWarning(object message)
        {
            Debug.LogWarning(message);
        }

        [System.Diagnostics.Conditional("ENABLE_LOG_ERROR")]
        public static void LogError(object message)
        {
            Debug.LogError(message);
        }
    }
}