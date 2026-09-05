public class NextLevelButton : MenuButton
{
    private LevelsSystem _levelsSystem;

    private void Construct(LevelsSystem levelsSystem)
    {
        _levelsSystem = levelsSystem;
    }

    private void OnEnable()
    {
        if (GameState.CurrentPlayerFaction == Faction.Heaven && GameState.CurrentLevel == Configs.Levels.HeavenSection.Count)
            gameObject.SetActive(false);
        else if (GameState.CurrentPlayerFaction == Faction.Hell && GameState.CurrentLevel == Configs.Levels.HellSection.Count)
            gameObject.SetActive(false);
    }

    protected override void OnButtonClicked()
    {
        GameState.CurrentLevel++;
        _levelsSystem.StartLevel();
    }
}
