public class WindowsSystem
{
    private readonly PauseSystem _pauseSystem;
    private readonly LevelsSystem _levelsSystem;
    private readonly Window _pauseWindow;
    private readonly Window _loseWindow;
    private readonly Window _victoryWindow;

    private Window _currentWindow = null;

    public WindowsSystem(
        PauseSystem pauseSystem,
        LevelsSystem levelsSystem,
        Window pauseWindow,
        Window loseWindow,
        Window victoryWindow)
    {
        _pauseSystem = pauseSystem;
        _levelsSystem = levelsSystem;
        _pauseWindow = pauseWindow;
        _loseWindow = loseWindow;
        _victoryWindow = victoryWindow;

        if (_levelsSystem != null)
        {
            _levelsSystem.LevelStarted += CloseCurrentWindow;
            _levelsSystem.LevelEnded += OnLevelEnded;
        }
    }

    public void CloseCurrentWindow()
    {
        if (_currentWindow != null)
        {
            _currentWindow.Close();
            _currentWindow.CloseButton.onClick.RemoveListener(CloseCurrentWindow);
            _currentWindow = null;
        }

        _pauseSystem.Unpause();
    }

    public void ShowPauseWindow()
    {
        Show(_pauseWindow);
    }

    private void ShowVictoryWindow()
    {
        Show(_victoryWindow);
    }

    private void ShowLoseWindow()
    {
        Show(_loseWindow);
    }

    private void Show(Window window)
    {
        CloseCurrentWindow();
        _pauseSystem.Pause();
        _currentWindow = window;
        window.Open();
        window.CloseButton.onClick.AddListener(CloseCurrentWindow);
    }

    private void OnLevelEnded(bool win)
    {
        if (win)
            ShowVictoryWindow();
        else
            ShowLoseWindow();
    }
}
