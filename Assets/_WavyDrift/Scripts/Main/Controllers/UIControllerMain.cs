using Racer.LoadManager;
using Racer.SaveManager;
using Racer.SaveSystem;
using Racer.Utilities;
using TMPro;
using UnityEngine;

/// <summary>
/// Handles certain stuffs relating to the game's main-menu UI.
/// </summary>
internal class UIControllerMain : SingletonPattern.Singleton<UIControllerMain>
{
    public UITween UITween { get; private set; }

    public int CoinsCount { get; private set; }
    public int DiamondCount { get; private set; }
    public int BestCount { get; private set; }

    private bool _hasValidatedUser;

    [Header("TEXTS")]
    [SerializeField] private TextMeshProUGUI coinT;
    [SerializeField] private TextMeshProUGUI diamondT;
    [SerializeField] private TextMeshProUGUI bestT;
    [SerializeField] private TextMeshProUGUI infoTipT;


    protected override void Awake()
    {
        base.Awake();

        UITween = GetComponent<UITween>();

        _hasValidatedUser = SaveManager.GetBool("Validated");

        Init();
    }

    private void Start()
    {
        if (!_hasValidatedUser)
            ShowInfoTip("Hey welcome!\nVisit the help section for a brief overview :-)");
    }

    /// <summary>
    /// Retrieves all the saved-in values from save-file.
    /// </summary>
    private void Init()
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
    /// Called once and only during player's first time lunch of the game.
    /// Prompts player to navigate through a particular
    /// UI-panel before proceeding to play.
    /// </summary>
    public void ValidateUser()
    {
        if (_hasValidatedUser) return;

        _hasValidatedUser = true;

        SaveManager.SaveBool("Validated", _hasValidatedUser);
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
        UITween.DisplayInfoBar(autoHide);

        Haptics.Vibrate(500);
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

        // Restarts application when a save-file is deleted, so as to apply changes.
        LoadManager.Instance.LoadSceneAsync(0);
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
        if (_hasValidatedUser)
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