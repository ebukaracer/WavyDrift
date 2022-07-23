using Racer.LoadManager;
using Racer.SaveSystem;
using Racer.SoundManager;
using Racer.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// Handles certain stuffs relating the game's menu UI.
/// </summary>
public class UIControllerGame : SingletonPattern.Singleton<UIControllerGame>
{
    // Class Fields
    int scoreCount;

    int diamondCount;

    int diamondRespawnCount = 1;

    int coinCount;

    int best;

    int totalCoins;

    public bool HasGhostPortion { get; private set; }

    public bool HasCoinMagnet { get; private set; }

    Animator animator;

    ItemManager itemManager;

    // Inspector Fields
    [Header("Texts")]

    [SerializeField]
    TextMeshProUGUI scoreT;

    [SerializeField]
    TextMeshProUGUI coinT;

    [SerializeField]
    TextMeshProUGUI diamondT;

    [SerializeField]
    TextMeshProUGUI diamondRespawnT;

    [SerializeField]
    TextMeshProUGUI bestT;


    [Header("Images")]
    [Space(10)]

    [SerializeField]
    Image pauseResumeI;

    [Space(5)]

    [SerializeField]
    Image[] collectibleTopImages;

    [Space(5)]

    [SerializeField]
    Sprite[] pauseResumeSprites;

    [Space(5)]

    [SerializeField]
    AudioClip triggerClip;

    [Space(5)]

    [SerializeField]
    CanvasGroup scoreCG;

    protected override void Awake()
    {
        base.Awake();

        itemManager = ItemManager.Instance;

        RetrieveSaves();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (HasCoinMagnet)
            SetTopImagesOpacity(0);
        if (HasGhostPortion)
            SetTopImagesOpacity(1);

        // Subscription
        GameManager.OnCurrentState += GameManager_OnCurrentState;
    }

    /// <summary>
    /// Retrieves all saved-in values from save-file.
    /// </summary>
    public void RetrieveSaves()
    {
        diamondCount = SaveSystem.GetData<int>("Diamond");

        SetDiamondT(0);

        totalCoins = SaveSystem.GetData<int>("TotalCoins");

        best = SaveSystem.GetData<int>("BestScore");

        HasCoinMagnet = itemManager.CollectibleItem.GetItemByName(CollectibleName.Coin_Magnet).IsUnlocked;

        HasGhostPortion = itemManager.CollectibleItem.GetItemByName(CollectibleName.Ghost_Portion).IsUnlocked;
    }

    /// <summary>
    /// Saves all data on game over.
    /// </summary>
    void SaveOnGameOver()
    {
        SaveSystem.SaveData("TotalCoins", totalCoins + coinCount);

        SaveSystem.SaveData("BestScore", SetBest);

        SaveSystem.SaveData("Diamond", diamondCount);
    }

    /// <summary>
    /// Calculates player's best score from the their current score.
    /// </summary>
    int SetBest => scoreCount > best ? scoreCount : best;

    /// <summary>
    /// Callback(listener) to game's current state from game manager(publisher).
    /// Performs various action based on the game's current state
    /// </summary>
    /// <param name="currentState"></param>
    private void GameManager_OnCurrentState(GameStates currentState)
    {
        SetPauseImage(currentState == GameStates.Pause);

        switch (currentState)
        {
            case GameStates.GameOver:
                SaveOnGameOver();
                bestT.SetText("{0}", SetBest);
                break;

            case GameStates.DestroyWait:
                diamondRespawnT.SetText("{0}", diamondRespawnCount);
                break;
        }
    }

    /// <summary>
    /// Modifies the pause-image sprite if game is paused/resumed.
    /// </summary>
    /// <param name="isPaused">is game paused or not?</param>
    public void SetPauseImage(bool isPaused)
    {
        if (isPaused)
            pauseResumeI.sprite = pauseResumeSprites[1];
        else
            pauseResumeI.sprite = pauseResumeSprites[0];
    }

    /// <summary>
    /// Adds up player's score.
    /// Assigns it to a UI text.
    /// Plays a sound effect if player reaches a score-count divisible by 50.
    /// </summary>
    /// <param name="score">Value to add up</param>
    public void SetScoreT(int score)
    {
        scoreCount += score;

        if (scoreCount % 50 == 0)
            SoundManager.Instance.PlaySfx(triggerClip);

        scoreT.SetText("{0}", scoreCount);

        ScoreAnim();
    }

    /// <summary>
    ///  Adds up player's coin.
    ///  Assigns it to a UI text</summary>
    /// <param name="amt">Value to add up</param>
    public void SetCoinT(int amt)
    {
        coinCount += amt;

        coinT.SetText("{0}", coinCount);
    }

    // Just as (SetCoinsT); in this case player's diamond.
    public void SetDiamondT(int amt)
    {
        diamondCount += amt;

        diamondT.SetText("{0}", diamondCount);
    }

    /// <summary>
    /// Enables a specific game over UI based on player's state.
    /// See <see cref="GameManager"/> for more info.
    /// </summary>
    /// <param name="paramName">Animator parameter name</param>
    public void EnableGameover(string paramName)
    {
        animator.SetTrigger(Utility.AnimatorId(paramName));
    }

    /// <summary>
    /// Animates player's score as it changes.
    /// </summary>
    void ScoreAnim()
    {
        scoreCG.alpha = 1f;

        scoreCG.DOFade(0f, .5f);

        //animator.SetTrigger(Utility.AnimatorId("Score"));
    }



    /// <summary>
    /// Re-spawns player with diamond if they have enough.
    /// </summary>
    public void SetRespawnStateWithDiamond()
    {
        if (GameManager.Instance.CurrentState == GameStates.DestroyWait && RespawnWithDiamond())
        {
            GameManager.Instance.SetGameState(GameStates.Respawn);

            diamondRespawnCount++;
        }
    }

    /// <summary>
    /// Re-spawns player with ads(rewardedVideo) if available.
    /// </summary>
    public void SetRespawnStateWithAds()
    {
        if (GameManager.Instance.CurrentState == GameStates.DestroyWait)
            GameManager.Instance.SetGameState(GameStates.Respawn);

    }

    /// <summary>
    /// Does player have enough diamond to begin with?
    /// </summary>
    /// <returns>true otherwise false</returns>
    bool RespawnWithDiamond()
    {
        if (diamondRespawnCount > diamondCount)
            return false;

        // Decrement player's total diamonds
        diamondCount -= diamondRespawnCount;

        // Update diamond UI text
        SetDiamondT(0);

        return true;
    }

    /// <summary>
    /// Ghost/Diamond images located at the top of the UI from the scene.
    /// This controls their current opacity if player has unlocked their collectible
    /// alias.
    /// </summary>
    /// <param name="index">Image index to choose from</param>
    void SetTopImagesOpacity(int index)
    {
        var originalColor0 = collectibleTopImages[index].color;
        var originalColor1 = collectibleTopImages[index].color;

        collectibleTopImages[index].color = new Color(originalColor0.r, originalColor0.g, originalColor0.b, 255f);
        collectibleTopImages[index].color = new Color(originalColor1.r, originalColor1.g, originalColor1.b, 255f);
    }

    /// <summary>
    /// Loads into the next scene.
    /// </summary>
    /// <param name="index">Next scene's index</param>
    public void LoadScene(int index)
    {
        LoadManager.Instance.LoadSceneAsync(index);

        GameManager.Instance.FadeInMusic();
    }

    private void OnDisable()
    {
        // Unregister callbacks
        GameManager.OnCurrentState -= GameManager_OnCurrentState;
    }
}
