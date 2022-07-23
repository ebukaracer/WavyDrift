using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Encapsulates various UI Fill-bars and their fill amount.
/// </summary>
public class FillBar : MonoBehaviour
{
    Image fill;

    IEnumerator startDecreaseCache;


    [field: SerializeField]
    public float DecreaseTime { get; set; }

    public event Action OnDecreaseStarted;
    public event Action OnDecreaseFinished;


    private void Awake()
    {
        fill = GetComponent<Image>();

        fill.fillAmount = 0;
    }

    /// <summary>
    /// Start decreasing a fill-bar over a specified delay.
    /// </summary>
    public void DecreaseFill()
    {
        // Overwrites the existing coroutine instead of waiting for it to finish.
        if (startDecreaseCache != null)
            StopCoroutine(startDecreaseCache);

        startDecreaseCache = StartDecrease();

        StartCoroutine(startDecreaseCache);
    }

    /// <summary>
    /// See: <see cref="DecreaseFill"/>.
    /// This function would return immediately if <see cref="IsStopRoutine"/> is true.
    /// </summary>
    IEnumerator StartDecrease()
    {
        // Before decreasing, notify listeners
        OnDecreaseStarted?.Invoke();

        fill.fillAmount = 1f;

        var end = Time.time + DecreaseTime;

        var changeRate = fill.fillAmount / DecreaseTime;

        // While decreasing
        while (Time.time < end)
        {
            if (IsStopRoutine)
                yield break;

            fill.fillAmount -= changeRate * Time.smoothDeltaTime;

            yield return null;
        }

        // After decreasing, notify listeners
        OnDecreaseFinished?.Invoke();
    }


    public bool IsStopRoutine { get; set; }
}

