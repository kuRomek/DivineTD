public class RestartLevelButton : MenuButton
{
    private LevelsSystem _levelsSystem;

    private void Construct(LevelsSystem levelsSystem)
    {
        _levelsSystem = levelsSystem;
    }

    protected override void OnButtonClicked()
    {
        _levelsSystem.StartLevel();
    }
}
