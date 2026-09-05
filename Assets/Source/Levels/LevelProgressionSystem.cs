public class LevelProgressionSystem
{
    private readonly LevelsSystem _levelsSystem;
    private readonly GridSystem _gridSystem;
    private readonly Map _map;

    public LevelProgressionSystem(LevelsSystem levelsSystem, GridSystem gridSystem)
    {
        _levelsSystem = levelsSystem;
        _gridSystem = gridSystem;
        _map = gridSystem.Map;

        _gridSystem.GridInitialized += OnGridInitialized;
    }

    private void OnCastleDestroyed(IDamageable damageable)
    {
        if (damageable is IFactionRelated castle)
        {
            _map.Castles[Faction.Heaven].Model.Died -= OnCastleDestroyed;
            _map.Castles[Faction.Hell].Model.Died -= OnCastleDestroyed;

            _levelsSystem.EndLevel(GameState.CurrentPlayerFaction != castle.Faction);
        }
    }

    private void OnGridInitialized()
    {
        _map.Castles[Faction.Heaven].Model.Died += OnCastleDestroyed;
        _map.Castles[Faction.Hell].Model.Died += OnCastleDestroyed;
    }
}
