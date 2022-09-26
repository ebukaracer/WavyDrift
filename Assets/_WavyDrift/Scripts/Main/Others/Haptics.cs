using UnityEngine;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

/// <summary>
/// Controls vibration for android.
/// </summary>
public static class Haptics
{

#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject CurrentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject Vibrator = CurrentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
    public static AndroidJavaObject Vibrator;
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
    /// Won't work in the unity editor.
    /// </summary>
    private static bool HasVibrator => !IsMuteVibration && IsAndroid();

    /// <summary>
    /// Triggers a vibration via the <see cref="AndroidJavaObject"/> class.
    /// </summary>
    public static void Vibrate()
    {
        if (HasVibrator)
            Vibrator.Call("vibrate");
    }

    /// <summary>
    /// See <see cref="Vibrate"/>
    /// </summary>
    /// <param name="milliseconds">Time to spend while vibrating.</param>
    public static void Vibrate(long milliseconds)
    {
        if (HasVibrator)
            Vibrator.Call("vibrate", milliseconds);
    }

    public static void Vibrate(long[] pattern, int repeat)
    {
        if (HasVibrator)
            Vibrator.Call("vibrate", pattern, repeat);
    }

    public static void VibrateOld() => Handheld.Vibrate();

    public static void Cancel()
    {
        if (HasVibrator)
            Vibrator.Call("cancel");
    }

    private static bool IsMuteVibration { get; set; }

    public static void Mute(bool state)
    {
        IsMuteVibration = state;
    }
}