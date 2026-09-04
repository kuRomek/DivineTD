using UnityEngine;
using UnityEngine.UI;

public class RuntimePauseWindow : PauseWindow
{
    [Space(5f)]
    [SerializeField] private Button _restartButton;

    private LevelsSystem _levelsSystem;
    private PauseSystem _pauseSystem;

    private void Construct(LevelsSystem levelsSystem, PauseSystem pauseSystem)
    {
        _levelsSystem = levelsSystem;
        _pauseSystem = pauseSystem;
    }

    protected override void SubscribeToButtons()
    {
        base.SubscribeToButtons();

        _restartButton.onClick.AddListener(OnRestartButtonClicked);
    }

    private void OnRestartButtonClicked()
    {
        _pauseSystem.Unpause();
        _levelsSystem.StartLevel();
    }
}