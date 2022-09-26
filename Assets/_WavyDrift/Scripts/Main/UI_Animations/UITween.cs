using UnityEngine;
using DG.Tweening;
using System;
using Racer.SoundManager;

[Serializable]
internal class SettingTween : TweenProperties
{
    public float offscreenPosY;

    public RectTransform[] settingBtns;
}

[Serializable]
internal class AdButtonTween : TweenProperties
{
    public RectTransform adBtn;

    [Space(10)]

    public float offscreenPosY;
}

[Serializable]
internal class InfoBarTween : TweenProperties
{
    public RectTransform infoBar;

    [Space(10)]

    public float offscreenPosY;
}
/// <summary>
/// Handles various animations coming from the UI's elements.
/// </summary>
public class UITween : MonoBehaviour
{
    [SerializeField] private AudioClip uiSfx;

    #region Setting Buttons

    private Sequence _settingBtnSequences;

    private bool _hasReturned = true;

    [Space(10), SerializeField] private SettingTween settingTween;

    #endregion

    #region Ad Button

    [SerializeField] private AdButtonTween adButtonTween;

    #endregion

    #region Info Bar

    [SerializeField] private InfoBarTween infoBarTween;

    #endregion


    /// <summary>
    /// Smoothly displays SettingUI elements upon first click, hides them upon second click.
    /// </summary>
    public void Settings_TweenPos()
    {
        _settingBtnSequences = DOTween.Sequence();

        if (!_hasReturned)
            for (var i = settingTween.settingBtns.Length - 1; i >= 0; i--)
            {
                var btn = settingTween.settingBtns[i];
                _settingBtnSequences.Append(btn.DOAnchorPosY(settingTween.offscreenPosY, settingTween.Duration)
                    .SetEase(settingTween.EaseType)).OnComplete(() => _hasReturned = true);
            }
        else
            for (var i = 0; i < settingTween.settingBtns.Length; i++)
            {
                var btn = settingTween.settingBtns[i];
                _settingBtnSequences.Append(btn.DOAnchorPosY(settingTween.EndValue, settingTween.Duration)
                    .SetEase(settingTween.EaseType)).OnComplete(() => _hasReturned = false);
            }
    }
    /// <summary>
    /// Smoothly displays Ad button.
    /// </summary>
    public void DisplayAdBtnInterval()
    {
        SoundManager.Instance.PlaySfx(uiSfx);

        adButtonTween.adBtn.DOAnchorPosY(adButtonTween.EndValue, adButtonTween.Duration).SetEase(adButtonTween.EaseType);
    }
    /// <summary>
    /// Smoothly hides Ad button.
    /// </summary>
    public void HideAdBtnInterval()
    {
        adButtonTween.adBtn.DOAnchorPosY(adButtonTween.offscreenPosY, adButtonTween.Duration).SetEase(adButtonTween.EaseType);
    }

    /// <summary>
    /// Smoothly displays InfoUI.
    /// </summary>
    /// <param name="autoHide">Should hide automatically after displaying?</param>
    public void DisplayInfoBar(bool autoHide = true)
    {
        SoundManager.Instance.PlaySfx(uiSfx, .8f);

        infoBarTween.infoBar.DOAnchorPosY(infoBarTween.EndValue, infoBarTween.Duration).SetEase(infoBarTween.EaseType);

        if (!autoHide)
            return;

        CancelInvoke(nameof(HideInfoBar));

        Invoke(nameof(HideInfoBar), 3f);
    }

    /// <summary>
    /// Smoothly hides InfoUI.
    /// </summary>
    public void HideInfoBar()
    {
        infoBarTween.infoBar.DOAnchorPosY(infoBarTween.offscreenPosY, infoBarTween.Duration).SetEase(infoBarTween.EaseType);

        CancelInvoke();
    }

    private void OnDisable()
    {
        DOTween.Kill(_settingBtnSequences);
    }
}
