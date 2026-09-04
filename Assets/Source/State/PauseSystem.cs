using UnityEngine;

public class PauseSystem
{
    private PauseWindow _pauseWindow;

    public PauseSystem(PauseWindow pauseWindow)
    {
        _pauseWindow = pauseWindow;
    }

    public void Pause()
    {
        _pauseWindow.Open();
        Time.timeScale = 0f;

        _pauseWindow.Closed += Unpause;
    }

    public void Unpause()
    {
        _pauseWindow.Closed -= Unpause;

        _pauseWindow.Close();
        Time.timeScale = 1f;
    }
}
