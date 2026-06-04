using System;

public class LevelsSystem
{
    public LevelsSystem()
    {

    }

    public event Action LevelStarted;
    public event Action LevelEnded;

    public void StartLevel()
    {
        LevelStarted?.Invoke();
    }

    public void EndLevel()
    {
        LevelEnded?.Invoke();
    }
}
