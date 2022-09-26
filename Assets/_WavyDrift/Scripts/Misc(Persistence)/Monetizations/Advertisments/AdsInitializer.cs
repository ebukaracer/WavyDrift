using UnityEngine;
using UnityEngine.Advertisements;


public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    private string _gameId;

    [SerializeField] private bool testMode = true;

    [Space(10), SerializeField] private string androidGameId;

    [SerializeField] private string iOSGameId;

    [Space(10), SerializeField] private RewardedAds rewardedAds;

    private void Awake() => InitializeAds();

    public void InitializeAds()
    {
        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? iOSGameId
            : androidGameId;

        Advertisement.Initialize(_gameId, testMode, true, this);
    }

    public void OnInitializationComplete()
    {
        LogConsole.Log("Unity Ads initialization complete.");

        // Loads ads if successfully initialized.
        rewardedAds.LoadAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        LogConsole.Log($"Unity Ads Initialization Failed: {error} - {message}");
    }
}
