using UnityEngine;

internal class PauseController : MonoBehaviour
{
    private bool _hasPaused;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            SetPause();
    }

    public void SetPause()
    {
        if (GameManager.Instance.CurrentState != GameStates.Playing && GameManager.Instance.CurrentState != GameStates.Pause)
            return;

        switch (_hasPaused)
        {
            case false:
                Pause();
                break;
            case true:
                Resume();
                break;
        }
    }

    private void Pause()
    {
        GameManager.Instance.SetGameState(GameStates.Pause);

        _hasPaused = true;

        Time.timeScale = 0;
    }

    private void Resume()
    {
        GameManager.Instance.SetGameState(GameStates.Playing);

        _hasPaused = false;

        Time.timeScale = 1;
    }
}
