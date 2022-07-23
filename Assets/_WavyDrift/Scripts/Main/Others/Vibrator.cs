using UnityEngine;
using UnityEngine.Android;

/// <summary>
/// Controls vibration for android.
/// </summary>
public static class Vibrator
{

#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
    public static AndroidJavaClass unityPlayer;
    public static AndroidJavaObject currentActivity;
    public static AndroidJavaObject vibrator;
#endif

    /// <summary>
    /// Are we running on android platform?
    /// </summary>
    /// <returns>true if so.</returns>
    private static bool IsAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return true;
#else
        return false;
#endif
    }

    /// <summary>
    /// Encapsulated version of <see cref="IsAndroid"/>.
    /// Checks if vibration is enabled by the player.
    /// </summary>
    static bool HasVibrator => !IsMuteVibration && IsAndroid();

    /// <summary>
    /// Triggers a vibration via the <see cref="AndroidJavaObject"/> class.
    /// </summary>
    public static void Vibrate()
    {
        if (HasVibrator)
            vibrator.Call("vibrate");
    }

    /// <summary>
    /// See <see cref="Vibrate"/>
    /// </summary>
    /// <param name="milliseconds">Time to spend while vibrating.</param>
    public static void Vibrate(long milliseconds)
    {
        if (HasVibrator)
            vibrator.Call("vibrate", milliseconds);
    }

    public static void Vibrate(long[] pattern, int repeat)
    {
        if (HasVibrator)
            vibrator.Call("vibrate", pattern, repeat);
        //else
        //    Handheld.Vibrate();
    }


    public static void Cancel()
    {
        if (HasVibrator)
            vibrator.Call("cancel");
    }

    static bool IsMuteVibration { get; set; }

    public static void Mute(bool state)
    {
        if (state)
            IsMuteVibration = true;
        else
            IsMuteVibration = false;

    }
}