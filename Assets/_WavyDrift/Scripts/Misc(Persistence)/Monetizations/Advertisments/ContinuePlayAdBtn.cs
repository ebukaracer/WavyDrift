using UnityEngine;
using UnityEngine.Advertisements;

class ContinuePlayAdBtn : RewardedAds
{
    public override void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        base.OnUnityAdsShowComplete(adUnitId, showCompletionState);

        UIControllerGame.Instance.SetRespawnStateWithAds();
    }

    public override void OnUnityAdsShowStart(string adUnitId)
    {
        base.OnUnityAdsShowStart(adUnitId);

        BrokenPlayerController.Instance.BrokenPlayer.StopCountdown(true);
    }
}
