using System;

public class LevelsSystem
{
    public LevelsSystem()
    {

    }

    public event Action LevelStarted;
    public event Action<bool> LevelEnded;

    public void StartLevel()
    {
        LevelStarted?.Invoke();
    }

    public void EndLevel(bool win)
    {
        LevelEnded?.Invoke(win);
    }
}
