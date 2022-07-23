using UnityEngine;

namespace Racer.SaveSystem
{
    class Logging
    {
        [System.Diagnostics.Conditional("ENABLE_LOG")]
        static public void Log(object message)
        {
            Debug.Log(message);
        }

        [System.Diagnostics.Conditional("ENABLE_LOG_WARNING")]
        static public void LogWarning(object message)
        {
            Debug.LogWarning(message);
        }
    }
}