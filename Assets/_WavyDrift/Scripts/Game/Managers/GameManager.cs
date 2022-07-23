using DG.Tweening;
using Racer.SoundManager;
using Racer.Utilities;
using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Game states available for transitioning.
/// </summary>
public enum GameStates
{
    Loading,
    Playing,
    Pause,
    DestroyWait,
    Respawn,
    GameOver
}

/// <summary>
/// This manages the various states of the game.
/// </summary>
class GameManager : SingletonPattern.Singleton<GameManager>
{
    // Private fields dependencies
    BrokenPlayerController brokenPlayerController;

    PlayerController playerController;

    SoundManager soundManager;

    AudioSource musicSource;

    bool isMusicSrcEnabled;


    public static event Action<GameStates> OnCurrentState;


    // For Visualization purpose only
    [field: SerializeField]
    public GameStates CurrentState { get; private set; }

    [field: SerializeField]
    public float StartDelay { get; private set; }


    // Other fields
    [Space(10), SerializeField]
    AudioClip uiSfx;



    private void Start()
    {
        // Player
        playerController = PlayerController.Instance;

        brokenPlayerController = BrokenPlayerController.Instance;

        // Sound
        soundManager = SoundManager.Instance;

        musicSource = soundManager.GetMusic();

        isMusicSrcEnabled = musicSource.enabled;

        // State
        SetGameState(GameStates.Loading);

        StartCoroutine(CountDown(StartDelay));
    }

    /// <summary>
    /// Sets the current state of the game.
    /// </summary>
    /// <param name="state">Actual state to transition to</param>
    public void SetGameState(GameStates state)
    {
        switch (state)
        {
            case GameStates.Loading:
                break;
            case GameStates.Playing:
                PlayState();
                break;
            case GameStates.Pause:
                PauseState();
                break;
            case GameStates.Respawn:
                RespawnState();
                break;
            case GameStates.DestroyWait:
                DestroyWaitState();
                break;
            case GameStates.GameOver:
                GameOverState();
                break;
        }

        CurrentState = state;

        // Updates other scripts listening to the game's current state
        OnCurrentState?.Invoke(state);
    }

    /// <summary>
    /// Re-spawns the player if conditions were satisfied.
    /// </summary>
    void RespawnState()
    {
        PlayState();

        // Disable game-over UI
        UIControllerGame.Instance.EnableGameover("Gameover_A-out");

        // Re-Initialize player
        playerController.PlayerMovement.Init(3f);

        // Re-Initialize broken-player
        brokenPlayerController.BrokenPlayer.gameObject.ToggleActive(false);

        // Switch to playing-state over a period of delay.
        StartCoroutine(CountDown(.1f));
    }

    /// <summary>
    /// Wait for the player to fulfill some conditions before switching to game-over state.
    /// If conditions are satisfied player gets re-spawned.
    /// </summary>
    void DestroyWaitState()
    {
        // Lower music
        soundManager.GetSnapShot(2).TransitionTo(.1f);

        // Enable UI
        UIControllerGame.Instance.EnableGameover("Gameover_A");

        // Play a sound effect
        soundManager.PlaySfx(uiSfx);

        // Initialize a broken-player counterpart
        brokenPlayerController.BrokenPlayer.InitializeOnStartup(playerController.PlayerMovement.gameObject.transform.position,
                                                       playerController.PlayerMovement.gameObject.transform.rotation);
    }

    /// <summary>
    /// Triggers when game is over.
    /// </summary>
    void GameOverState()
    {
        Vibrator.Vibrate(500);

        FadeOutMusic();

        // Enable a UI for game over.
        UIControllerGame.Instance.EnableGameover("Gameover_B");
    }

    /// <summary>
    /// Smoothly fades-out Un-muted music(background).
    /// </summary>
    public void FadeOutMusic()
    {
        if (isMusicSrcEnabled)
            musicSource.DOFade(0, .2f).OnComplete(() => musicSource.Stop());
    }

    /// <summary>
    /// Smoothly fades-in Un-muted music(background).
    /// </summary>
    public void FadeInMusic()
    {
        if (!isMusicSrcEnabled)
            return;

        musicSource.Play();

        musicSource.DOFade(1f, .2f);
    }

    /// <summary>
    /// Whilst on Play-state...
    /// Plays an Un-muted music(background).
    /// </summary>
    void PlayState()
    {
        if (isMusicSrcEnabled)
            soundManager.GetSnapShot(1).TransitionTo(.1f);

        // Other logics
    }

    /// <summary>
    /// Whilst on Pause-state...
    /// Reduces Un-muted music(background).
    /// </summary>
    void PauseState()
    {
        if (isMusicSrcEnabled)
            soundManager.GetSnapShot(2).TransitionTo(.1f);

        // Other logics
    }

    /// <summary>
    /// A little delay before the game's state is set to 'Playing'
    /// </summary>
    IEnumerator CountDown(float delay)
    {
        yield return Utility.GetWaitForSeconds(delay);

        // Updates the game's state when the delay is over
        SetGameState(GameStates.Playing);
    }
}
