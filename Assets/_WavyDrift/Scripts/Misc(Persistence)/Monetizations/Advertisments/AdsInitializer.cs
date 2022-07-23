using UnityEngine;
using UnityEngine.Advertisements;


public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    private string _gameId;

    [SerializeField] bool _testMode = true;

    [Space(10)]

    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;

    [Space(10), SerializeField]
    RewardedAds rewardedAds;

    void Awake() => InitializeAds();

    public void InitializeAds()
    {
        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOSGameId
            : _androidGameId;

        Advertisement.Initialize(_gameId, _testMode, true, this);
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
