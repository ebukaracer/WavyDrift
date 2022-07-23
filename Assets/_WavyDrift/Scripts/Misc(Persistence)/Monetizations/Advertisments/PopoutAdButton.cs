using UnityEngine.Advertisements;

class PopoutAdButton : RewardedAds
{
    public override void OnUnityAdsAdLoaded(string adUnitId)
    {
        base.OnUnityAdsAdLoaded(adUnitId);

        UIControllerMain.Instance.UITweens.DisplayAdBtnInterval();
    }

    public override void OnUnityAdsShowStart(string adUnitId)
    {
        base.OnUnityAdsShowStart(adUnitId);

        UIControllerMain.Instance.UITweens.HideAdBtnInterval();
    }

    public override void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        base.OnUnityAdsShowComplete(adUnitId, showCompletionState);

        UIControllerMain.Instance.UpdateDiamonds(1);

        UIControllerMain.Instance.UITweens.HideAdBtnInterval();
    }


    public override void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        base.OnUnityAdsShowFailure(adUnitId, error, message);

        UIControllerMain.Instance.ShowInfoTip($"{error}");
    }
}
