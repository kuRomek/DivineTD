using UnityEngine;

public class BuildingSystem
{
    private readonly GridSystem _gridSystem;
    private readonly TowerFactory _towerFactory;
    private readonly MainCamera _mainCamera;

    private GridObjectModel _draft = null;

    public BuildingSystem(GridSystem gridSystem, TowerFactory towerFactory, MainCamera mainCamera)
    {
        _gridSystem = gridSystem;
        _towerFactory = towerFactory;
        _mainCamera = mainCamera;
    }

    public void CreateTowerDraft(Faction faction, TowerType type)
    {
        _mainCamera.ToggleControlBlock(true);
        _draft = _towerFactory.CreateTower(faction, type, true);
        _draft.MoveAt(_gridSystem.GetSnappedPosition(faction));
    }

    public void CreateCastleDraft(Faction faction)
    {
        _mainCamera.ToggleControlBlock(true);

        var castlesData = Configs.Levels.GetCastleData(GameState.CurrentPlayerFaction, GameState.CurrentLevel);
        int healthAmount = castlesData[_mainCamera.TargetFaction];

        CastleView castleView = Object.Instantiate(Configs.Buildings.CastlePrefab);

        HealthModel health = new(castleView.HealthBar.transform, healthAmount, healthAmount);
        castleView.HealthBar.AttachPresenter(new Health(castleView.HealthBar, health));

        CastleModel castle = new(castleView.transform, health, _mainCamera.TargetFaction, true);
        castleView.AttachPresenter(new Castle(castleView, castle, _gridSystem));

        _draft = castle;
        _draft.MoveAt(_gridSystem.GetSnappedPosition(faction));
    }

    public bool TryBuild(Faction faction)
    {
        if (_gridSystem.TryPlace(_draft, faction))
        {
            _draft.ToggleDrafting(false);
            _draft = null;
            _mainCamera.ToggleControlBlock(false);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CancelBuilding()
    {
        _mainCamera.ToggleControlBlock(false);

        if (_draft != null)
        {
            _draft.Destroy();
            _draft = null;
        }
        else
        {
            Debug.LogWarning("Trying to cancel building, although there is no draft");
        }
    }
}
