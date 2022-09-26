using DG.Tweening;
using Racer.SoundManager;
using Racer.Utilities;
using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Game states available for transitioning.
/// </summary>
internal enum GameStates
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
internal class GameManager : SingletonPattern.Singleton<GameManager>
{
    // Private fields dependencies
    private BrokenPlayerController _brokenPlayerController;

    private PlayerController _playerController;

    private SoundManager _soundManager;

    private AudioSource _musicSource;

    private bool _isMusicSrcEnabled;


    public static event Action<GameStates> OnCurrentState;


    // For Visualization purpose only
    [field: SerializeField]
    public GameStates CurrentState { get; private set; }

    [field: SerializeField]
    public float StartDelay { get; private set; }


    // Other fields
    [Space(10), SerializeField] private AudioClip uiSfx;



    private void Start()
    {
        // Player
        _playerController = PlayerController.Instance;

        _brokenPlayerController = BrokenPlayerController.Instance;

        // Sound
        _soundManager = SoundManager.Instance;

        _musicSource = _soundManager.GetMusic();

        _isMusicSrcEnabled = _musicSource.enabled;

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
    private void RespawnState()
    {
        PlayState();

        // Disable game-over UI
        UIControllerGame.Instance.EnableGameover("Gameover_A-out");

        // Re-Initialize player
        _playerController.PlayerMovement.Init(3f);

        // Re-Initialize broken-player
        _brokenPlayerController.BrokenPlayer.gameObject.ToggleActive(false);

        // Switch to playing-state over a period of delay.
        StartCoroutine(CountDown(.1f));
    }

    /// <summary>
    /// Wait for the player to fulfill some conditions before switching to game-over state.
    /// If conditions are satisfied player gets re-spawned.
    /// </summary>
    private void DestroyWaitState()
    {
        // Lower music
        _soundManager.GetSnapShot(2).TransitionTo(0.1f);

        // Enable UI
        UIControllerGame.Instance.EnableGameover("Gameover_A");

        // Play a sound effect
        _soundManager.PlaySfx(uiSfx);

        // Initialize a broken-player counterpart
        _brokenPlayerController.BrokenPlayer.InitializeOnStartup(_playerController.PlayerMovement.gameObject.transform.position,
                                                       _playerController.PlayerMovement.gameObject.transform.rotation);
    }

    /// <summary>
    /// Triggers when game is over.
    /// </summary>
    private void GameOverState()
    {
        FadeOutMusic();

        // Enable a UI for game over.
        UIControllerGame.Instance.EnableGameover("Gameover_B");
    }

    /// <summary>
    /// Smoothly fades-out Un-muted music(background).
    /// </summary>
    public void FadeOutMusic()
    {
        if (_isMusicSrcEnabled)
            _musicSource.DOFade(0, .2f).OnComplete(() => _musicSource.Stop());
    }

    /// <summary>
    /// Smoothly fades-in Un-muted music(background).
    /// </summary>
    public void FadeInMusic()
    {
        if (!_isMusicSrcEnabled)
            return;

        _musicSource.Play();

        _musicSource.DOFade(1f, .2f);
    }

    /// <summary>
    /// Whilst on Play-state...
    /// Plays an Un-muted music(background).
    /// </summary>
    private void PlayState()
    {
        if (_isMusicSrcEnabled)
            _soundManager.GetSnapShot(1).TransitionTo(.1f);

        // Other logic
    }

    /// <summary>
    /// Whilst on Pause-state...
    /// Reduces Un-muted music(background).
    /// </summary>
    private void PauseState()
    {
        if (_isMusicSrcEnabled)
            _soundManager.GetSnapShot(2).TransitionTo(.1f);

        // Other logic
    }

    /// <summary>
    /// A little delay before the game's state is set to 'Playing'
    /// </summary>
    private IEnumerator CountDown(float delay)
    {
        yield return Utility.GetWaitForSeconds(delay);

        // Updates the game's state when the delay is over
        SetGameState(GameStates.Playing);
    }
}
