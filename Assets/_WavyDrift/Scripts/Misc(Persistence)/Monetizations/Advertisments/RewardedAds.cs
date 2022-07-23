using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

abstract class RewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    bool isAlreadyInitialized;

    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";

    [SerializeField, Space(10)] Button _showAdButton;

    // This will remain null for unsupported platforms
    string _adUnitId = null;

    void Awake()
    {
        // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif

        // Disable the button until the ad is ready to show:
        _showAdButton.interactable = false;


        if (Advertisement.isInitialized)
        {
            LoadAd();

            isAlreadyInitialized = true;
        }
    }

    // Load content to the Ad Unit:
    public void LoadAd()
    {
        if (isAlreadyInitialized)
            return;

        // IMPORTANT! Only load content AFTER initialization from 'AdsInitializer.cs'.
        LogConsole.Log("Loading Ad: " + _adUnitId);

        Advertisement.Load(_adUnitId, this);
    }

    // If the ad successfully loads, add a listener to the button and enable it:
    public virtual void OnUnityAdsAdLoaded(string adUnitId)
    {
        LogConsole.Log("Ad Loaded: " + adUnitId);

        if (adUnitId.Equals(_adUnitId))

            // Enable the button for users to click:
            _showAdButton.interactable = true;
        {
            // Configure the button to call the ShowAd() method when clicked:
            _showAdButton.onClick.AddListener(ShowAd);
        }
    }

    // Implement a method to execute when the user clicks the button:
    public void ShowAd()
    {
        // Disable the button:
        _showAdButton.interactable = false;

        // Then show the ad:
        Advertisement.Show(_adUnitId, this);
    }

    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public virtual void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            LogConsole.Log("Unity Ads Rewarded Ad Completed");
            // Grant a reward.

            // Load another ad:
            Advertisement.Load(_adUnitId, this);
        }
    }

    // Implement Load and Show Listener error callbacks:
    public virtual void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        LogConsole.Log($"Error loading Ad Unit {adUnitId}: {error} - {message}");

        // Tries to load another ad after failure.
        //Advertisement.Load(_adUnitId, this);
    }

    public virtual void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        LogConsole.Log($"Error showing Ad Unit {adUnitId}: {error} - {message}");

        // Tries to load another ad after failure.
        //Advertisement.Load(_adUnitId, this);
    }

    // Callback to determine whether ad has started showing. 
    public virtual void OnUnityAdsShowStart(string adUnitId) { LogConsole.Log("Ad is showing currently!"); }

    public void OnUnityAdsShowClick(string adUnitId) { }

    void OnDestroy()
    {
        // Clean up the button listeners:
        _showAdButton.onClick.RemoveAllListeners();
    }
}