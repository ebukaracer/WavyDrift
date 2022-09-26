using UnityEngine;

namespace Racer.ObjectPooler
{
    internal class Logging : MonoBehaviour
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