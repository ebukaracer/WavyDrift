using UnityEngine;
using DG.Tweening;
using System;
using Racer.SoundManager;

[Serializable]
class SettingTween : TweenProperties
{
    public float offscreenPosY;

    [Space(10)]

    public RectTransform[] settingsBtns;
}

[Serializable]
class AdButtonTween : TweenProperties
{
    public RectTransform adBtn;

    [Space(10)]

    public float offscreenPosY;
}

[Serializable]
class InforBarTween : TweenProperties
{
    public RectTransform infoBar;

    [Space(10)]

    public float offscreenPosY;
}
/// <summary>
/// Handles various animations coming from the UI's elements.
/// </summary>
public class UITweens : MonoBehaviour
{
    [SerializeField]
    AudioClip uiSfx;

    #region Settings Buttons

    Sequence settingBtnsSequence;

    bool hasReturned = true;

    [Space(10), SerializeField]
    SettingTween settingTween;

    #endregion

    #region Ad Button

    [SerializeField]
    AdButtonTween adButtonTween;

    #endregion

    #region Info Bar

    [SerializeField]
    InforBarTween inforBarTween;

    #endregion


    /// <summary>
    /// Smoothly displays SettingUI elements upon first click, hides them upon second click.
    /// </summary>
    public void Settings_TweenPos()
    {
        settingBtnsSequence = DOTween.Sequence();

        if (!hasReturned)
            foreach (var btn in settingTween.settingsBtns)
            {
                settingBtnsSequence.Append(btn.DOAnchorPosY(settingTween.offscreenPosY, settingTween.duration)
                    .SetEase(settingTween.easeType)).OnComplete(() => hasReturned = true);
            }
        else
            foreach (var btn in settingTween.settingsBtns)
            {
                settingBtnsSequence.Append(btn.DOAnchorPosY(settingTween.endValue, settingTween.duration)
                    .SetEase(settingTween.easeType)).OnComplete(() => hasReturned = false);
            }
    }
    /// <summary>
    /// Smoothly displays Ad button.
    /// </summary>
    public void DisplayAdBtnInterval()
    {
        SoundManager.Instance.PlaySfx(uiSfx);

        adButtonTween.adBtn.DOAnchorPosY(adButtonTween.endValue, adButtonTween.duration).SetEase(adButtonTween.easeType);
    }
    /// <summary>
    /// Smoothly hides Ad button.
    /// </summary>
    public void HideAdBtnInterval()
    {
        adButtonTween.adBtn.DOAnchorPosY(adButtonTween.offscreenPosY, adButtonTween.duration).SetEase(adButtonTween.easeType);
    }

    /// <summary>
    /// Smoothly displays InfoUI.
    /// </summary>
    /// <param name="autoHide">Should hide automatically after displaying?</param>
    public void DisplayInfoBar(bool autoHide = true)
    {
        SoundManager.Instance.PlaySfx(uiSfx, .8f);

        inforBarTween.infoBar.DOAnchorPosY(inforBarTween.endValue, inforBarTween.duration).SetEase(inforBarTween.easeType);

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
        inforBarTween.infoBar.DOAnchorPosY(inforBarTween.offscreenPosY, inforBarTween.duration).SetEase(inforBarTween.easeType);

        CancelInvoke();
    }

    private void OnDisable()
    {
        DOTween.Kill(settingBtnsSequence);
    }
}
