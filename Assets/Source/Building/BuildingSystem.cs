using UnityEngine;

public class BuildingSystem
{
    private readonly GridSystem _gridSystem;
    private readonly GridObjectsFactory _gridObjectsFactory;
    private readonly EconomySystem _economySystem;
    private readonly MainCamera _mainCamera;

    private GridObjectModel _draft = null;

    public BuildingSystem(GridSystem gridSystem, GridObjectsFactory gridObjectsFactory, EconomySystem economySystem, MainCamera mainCamera)
    {
        _gridSystem = gridSystem;
        _gridObjectsFactory = gridObjectsFactory;
        _economySystem = economySystem;
        _mainCamera = mainCamera;
    }

    public bool TryCreateTowerDraft(Faction faction, TowerType type)
    {
        int cost = Configs.Buildings.GetCost(faction, type);

        if (_economySystem.Funds[faction].Amount.Value < cost)
            return false;

        _economySystem.ChangeFundsAmount(faction, -cost);

        _mainCamera.ToggleControlBlock(true);
        _draft = _gridObjectsFactory.CreateTower(faction, type, true);
        _draft.MoveAt(_gridSystem.GetSnappedPosition(faction));

        return true;
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
            if (_draft is TowerModel tower)
                _economySystem.ChangeFundsAmount(tower.Faction, Configs.Buildings.GetCost(tower.Faction, tower.Type));

            _draft.Destroy();
            _draft = null;
        }
        else
        {
            Debug.LogWarning("Trying to cancel building, although there is no draft");
        }
    }
}