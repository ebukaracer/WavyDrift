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
internal class UIControllerGame : SingletonPattern.Singleton<UIControllerGame>
{
    // Class Fields
    private Animator _animator;

    private ItemManager _itemManager;

    private int _scoreCount;

    private int _diamondCount;

    private int _diamondRespawnCount = 1;

    private int _coinCount;

    private int _best;

    private int _totalCoins;

    public bool HasGhostPortion { get; private set; }

    public bool HasCoinMagnet { get; private set; }


    // Inspector Fields
    [Header("Texts"), SerializeField] private TextMeshProUGUI scoreT;

    [SerializeField] private TextMeshProUGUI coinT;

    [SerializeField] private TextMeshProUGUI diamondT;

    [SerializeField] private TextMeshProUGUI diamondRespawnT;

    [SerializeField] private TextMeshProUGUI bestT;

    [Header("Images"), Space(10),]

    [SerializeField] private Image pauseResumeI;

    [Space(5), SerializeField] private Image[] collectibleTopImages;

    [Space(5), SerializeField] private Sprite[] pauseResumeSprites;

    [Space(5), SerializeField] private AudioClip triggerClip;

    [Space(5), SerializeField] private CanvasGroup scoreCg;


    protected override void Awake()
    {
        base.Awake();

        _itemManager = ItemManager.Instance;

        RetrieveSaves();
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();

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
        _diamondCount = SaveSystem.GetData<int>("Diamond");

        SetDiamondT(0);

        _totalCoins = SaveSystem.GetData<int>("TotalCoins");

        _best = SaveSystem.GetData<int>("BestScore");

        HasCoinMagnet = _itemManager.CollectibleItem.GetItemByName(CollectibleName.CoinMagnet).IsUnlocked;

        HasGhostPortion = _itemManager.CollectibleItem.GetItemByName(CollectibleName.GhostPortion).IsUnlocked;
    }

    /// <summary>
    /// Saves all data on game over.
    /// </summary>
    private void SaveOnGameOver()
    {
        SaveSystem.SaveData("TotalCoins", _totalCoins + _coinCount);

        SaveSystem.SaveData("BestScore", SetBest);

        SaveSystem.SaveData("Diamond", _diamondCount);
    }

    /// <summary>
    /// Calculates player's best score from the their current score.
    /// </summary>
    private int SetBest => _scoreCount > _best ? _scoreCount : _best;

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
                diamondRespawnT.SetText("{0}", _diamondRespawnCount);
                break;
        }
    }

    /// <summary>
    /// Modifies the pause-image sprite if game is paused/resumed.
    /// </summary>
    /// <param name="isPaused">is game paused or not?</param>
    public void SetPauseImage(bool isPaused)
    {
        pauseResumeI.sprite = isPaused ? pauseResumeSprites[1] : pauseResumeSprites[0];
    }

    /// <summary>
    /// Adds up player's score.
    /// Assigns it to a UI text.
    /// Plays a sound effect if player reaches a score-count divisible by 50.
    /// </summary>
    /// <param name="score">Value to add up</param>
    public void SetScoreT(int score)
    {
        _scoreCount += score;

        if (_scoreCount % 50 == 0)
            SoundManager.Instance.PlaySfx(triggerClip);

        scoreT.SetText("{0}", _scoreCount);

        ScoreAnim();
    }

    /// <summary>
    ///  Adds up player's coin.
    ///  Assigns it to a UI text</summary>
    /// <param name="amt">Value to add up</param>
    public void SetCoinT(int amt)
    {
        _coinCount += amt;

        coinT.SetText("{0}", _coinCount);
    }

    // Just as (SetCoinsT); in this case player's diamond.
    public void SetDiamondT(int amt)
    {
        _diamondCount += amt;

        diamondT.SetText("{0}", _diamondCount);
    }

    /// <summary>
    /// Enables a specific game over UI based on player's state.
    /// See <see cref="GameManager"/> for more info.
    /// </summary>
    /// <param name="paramName">Animator parameter name</param>
    public void EnableGameover(string paramName)
    {
        _animator.SetTrigger(Utility.AnimatorId(paramName));
    }

    /// <summary>
    /// Animates player's score as it changes.
    /// </summary>
    private void ScoreAnim()
    {
        scoreCg.alpha = 1f;

        scoreCg.DOFade(0f, .5f);
    }



    /// <summary>
    /// Re-spawns player with diamond if they have enough.
    /// </summary>
    public void SetRespawnStateWithDiamond()
    {
        if (GameManager.Instance.CurrentState == GameStates.DestroyWait && RespawnWithDiamond())
        {
            GameManager.Instance.SetGameState(GameStates.Respawn);

            _diamondRespawnCount++;
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
    private bool RespawnWithDiamond()
    {
        if (_diamondRespawnCount > _diamondCount)
            return false;

        // Decrement player's total diamonds
        _diamondCount -= _diamondRespawnCount;

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
    private void SetTopImagesOpacity(int index)
    {
        var originalColor0 = collectibleTopImages[index].color;
        var originalColor1 = collectibleTopImages[index].color;

        originalColor0.a = 1;
        originalColor1.a = 1;

        collectibleTopImages[index].color = originalColor0;
        collectibleTopImages[index].color = originalColor1;
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
