using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Encapsulates various UI Fill-bars and their fill amount.
/// </summary>
internal class FillBar : MonoBehaviour
{
    private Image _fill;

    private IEnumerator _startDecreaseCache;

    [field: SerializeField]
    public float DecreaseTime { get; set; }

    public event Action OnDecreaseStarted;
    public event Action OnDecreaseFinished;


    private void Awake()
    {
        _fill = GetComponent<Image>();

        _fill.fillAmount = 0;
    }

    /// <summary>
    /// Start decreasing a fill-bar over a specified delay.
    /// </summary>
    public void DecreaseFill()
    {
        // Overwrites the existing coroutine instead of waiting for it to finish.
        if (_startDecreaseCache != null)
            StopCoroutine(_startDecreaseCache);

        _startDecreaseCache = StartDecrease();

        StartCoroutine(_startDecreaseCache);
    }

    /// <summary>
    /// See: <see cref="DecreaseFill"/>.
    /// This function would return immediately if <see cref="IsStopRoutine"/> is true.
    /// </summary>
    private IEnumerator StartDecrease()
    {
        // Before decreasing, notify listeners
        OnDecreaseStarted?.Invoke();

        IsRoutineBusy = true;

        _fill.fillAmount = 1f;

        var end = Time.time + DecreaseTime;

        var changeRate = _fill.fillAmount / DecreaseTime;

        // While decreasing
        while (Time.time < end)
        {
            if (IsStopRoutine)
                yield break;

            _fill.fillAmount -= changeRate * Time.smoothDeltaTime;

            yield return null;
        }

        // After decreasing, notify listeners
        OnDecreaseFinished?.Invoke();

        IsRoutineBusy = false;
    }

    public bool IsStopRoutine { get; set; }

    public bool IsRoutineBusy { get; set; }
}

