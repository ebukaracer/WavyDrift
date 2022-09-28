using UnityEngine;

internal static class Logging
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
}