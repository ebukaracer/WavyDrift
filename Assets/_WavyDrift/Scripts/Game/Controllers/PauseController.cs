using UnityEngine;

class PauseController : MonoBehaviour
{
    bool hasPaused;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            SetPause();
    }

    public void SetPause()
    {
        if (GameManager.Instance.CurrentState != GameStates.Playing && GameManager.Instance.CurrentState != GameStates.Pause)
            return;

        if (!hasPaused)
            Pause();
        else if (hasPaused)
            Resume();
    }

    void Pause()
    {
        GameManager.Instance.SetGameState(GameStates.Pause);

        hasPaused = true;

        // TODO: Inputs get registered one frame.
        Time.timeScale = 0;
    }

    void Resume()
    {
        GameManager.Instance.SetGameState(GameStates.Playing);

        hasPaused = false;

        Time.timeScale = 1;
    }
}
