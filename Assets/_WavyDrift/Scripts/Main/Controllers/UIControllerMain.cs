using Racer.LoadManager;
using Racer.SaveManager;
using Racer.SaveSystem;
using Racer.SoundManager;
using Racer.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles certain stuffs relating to the game's main-menu UI.
/// </summary>
public class UIControllerMain : SingletonPattern.Singleton<UIControllerMain>
{
    public UITweens UITweens { get; private set; }

    public int CoinsCount { get; private set; }

    public int DiamondCount { get; private set; }

    public int BestCount { get; private set; }

    bool hasValidatedUser;

    // Sound
    bool inverseSoundState;
    bool isSoundStateInit;

    // Vibration
    bool inverseVibrationState;
    bool isVibrationStateInit;

    [Header("Texts")]

    [SerializeField]
    TextMeshProUGUI coinT;

    [SerializeField]
    TextMeshProUGUI diamondT;

    [SerializeField]
    TextMeshProUGUI bestT;

    [SerializeField]
    TextMeshProUGUI infoTipT;

    [Header("Images")]

    [Space(10), SerializeField]
    Image musicBtnSprite;

    [SerializeField]
    Image vibrationBtnSprite;

    [Header("Sprites")]

    [Space(5), SerializeField]
    Sprite[] musicBtnSprites;

    [SerializeField]
    Sprite[] vibrationBtnSprites;

    protected override void Awake()
    {
        base.Awake();

        UITweens = GetComponent<UITweens>();

        hasValidatedUser = SaveManager.GetBool("Validated");

        Init();

        SoundAction();

        VibrationAction();
    }

    private void Start()
    {

        if (!hasValidatedUser)
            ShowInfoTip("Hey welcome!\nVisit the help section for a brief overview :-)");
    }

    /// <summary>
    /// Retrieves all the saved-in values from save-file.
    /// </summary>
    void Init()
    {
        CoinsCount = SaveSystem.GetData<int>("TotalCoins");
        coinT.SetText("{0}", CoinsCount);


        DiamondCount = SaveSystem.GetData<int>("Diamond");
        diamondT.SetText("{0}", DiamondCount);

        BestCount = SaveSystem.GetData<int>("BestScore");
        bestT.SetText("{0}", BestCount);
    }

    /// <summary>
    /// Update the current value of coins if modified.
    /// Saves the updated value. 
    /// </summary>
    /// <param name="amount">Updated value</param>
    public void UpdateCoins(int amount)
    {
        CoinsCount -= amount;

        coinT.SetText("{0}", CoinsCount);

        SaveSystem.SaveData("TotalCoins", CoinsCount);
    }

    /// <summary>
    /// Update the current value of diamonds if modified.
    /// Saves the updated value. </summary>
    /// <param name="amount">Updated value</param>
    public void UpdateDiamonds(int amount)
    {
        DiamondCount -= amount;

        diamondT.SetText("{0}", DiamondCount);

        SaveSystem.SaveData("Diamond", DiamondCount);
    }

    /// <summary>
    /// Initializes sound state(on/off) upon game start.
    /// Assigned to the sound button from the scene.
    /// Toggles mute/Un-mute sound states.
    /// </summary>
    public void SoundAction()
    {
        // Called once and only if sound hasn't been initialized with the saved in value.
        if (!isSoundStateInit)
        {
            inverseSoundState = SaveManager.GetBool("Sound", true);

            isSoundStateInit = true;
        }

        // Un-mute
        if (inverseSoundState)
        {
            // Logic
            SoundManager.Instance.EnableMusic(true);
            SoundManager.Instance.EnableSfx(true);

            // Save point could be called elsewhere
            SaveManager.SaveBool("Sound", inverseSoundState);

            musicBtnSprite.sprite = musicBtnSprites[0];

            // Reverse
            inverseSoundState = false;
        }

        // Mute
        else
        {
            // Logic
            SoundManager.Instance.EnableMusic(false);
            SoundManager.Instance.EnableSfx(false);

            SaveManager.SaveBool("Sound", inverseSoundState);

            musicBtnSprite.sprite = musicBtnSprites[1];

            // Reverse
            inverseSoundState = true;
        }
    }

    /// <summary>
    /// Similar to <see cref="SoundAction"/>, instead vibration is being taken account of.
    /// </summary>
    public void VibrationAction()
    {
        // Called once and only if vibration hasn't been initialized with the saved in value.
        if (!isVibrationStateInit)
        {
            inverseVibrationState = SaveManager.GetBool("Vibration", false);

            isVibrationStateInit = true;
        }

        // Vibration disabled
        if (inverseVibrationState)
        {
            // Logic
            Vibrator.Mute(false);

            // Save point could be called elsewhere
            SaveManager.SaveBool("Vibration", inverseVibrationState);

            vibrationBtnSprite.sprite = vibrationBtnSprites[0];

            // Reverse
            inverseVibrationState = false;
        }

        // Vibration enabled
        else
        {
            // Logic
            Vibrator.Mute(true);

            SaveManager.SaveBool("Vibration", inverseVibrationState);

            vibrationBtnSprite.sprite = vibrationBtnSprites[1];

            // Reverse
            inverseVibrationState = true;
        }
    }

    /// <summary>
    /// Called once and only during player's first time lunch of the game.
    /// Prompts player to navigate through a particular
    /// UI-panel before proceeding to play.
    /// </summary>
    public void ValidateUser()
    {
        if (!hasValidatedUser)
        {
            hasValidatedUser = true;

            SaveManager.SaveBool("Validated", hasValidatedUser);
        }
    }

    /// <summary>
    /// Displays a pop-out filled with text, for tips etc.
    /// Assigned to the [Exit] button in the [About] panel from the scene.
    /// </summary>
    /// <param name="textToDisplay">Actual text to display on the pop-out</param>
    /// <param name="autoHide">Whether to automatically hide the pop-out?</param>
    public void ShowInfoTip(string textToDisplay, bool autoHide = true)
    {
        infoTipT.text = textToDisplay;

        // Info tip Displayed
        UITweens.DisplayInfoBar(autoHide);

        Vibrator.Vibrate(500);
    }

    /// <summary>
    /// Clear all player's progress instantly.
    /// Restarts game as soon as it's done.
    /// Restart does not happen in the editor!
    /// </summary>
    public void ClearProgress()
    {
        SaveSystem.DeleteSaveFile();

        SaveManager.ClearAllPrefs();
    }

    /// <summary>
    /// Exits Game.
    /// </summary>
    public void LeaveGame()
    {
#if !UNITY_EDITOR
        Application.Quit();
#endif
    }

    /// <summary>
    /// Loads into the next scene if player has been validated.
    /// </summary>
    /// <param name="sceneIndex">Next scene's index.</param>
    public void LoadGame(int sceneIndex)
    {
        if (hasValidatedUser)
            LoadManager.Instance.LoadSceneAsync(sceneIndex);
        else
            FirstTimeInit("First visit the help section for a brief overview :-(");
    }

    /// <summary>
    /// Displays a pop-out during player's initial validation.
    /// </summary>
    /// <param name="t">Text to display on the pop-out</param>
    public void FirstTimeInit(string t)
    {
        ShowInfoTip(t);
    }

}