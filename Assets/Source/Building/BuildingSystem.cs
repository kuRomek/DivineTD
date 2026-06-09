using UnityEngine;

public class BuildingSystem
{
    private readonly GridSystem _gridSystem;
    private readonly GridObjectsFactory _gridObjectsFactory;
    private readonly MainCamera _mainCamera;

    private GridObjectModel _draft = null;

    public BuildingSystem(GridSystem gridSystem, GridObjectsFactory gridObjectsFactory, MainCamera mainCamera)
    {
        _gridSystem = gridSystem;
        _gridObjectsFactory = gridObjectsFactory;
        _mainCamera = mainCamera;
    }

    public void CreateTowerDraft(Faction faction, TowerType type)
    {
        _mainCamera.ToggleControlBlock(true);
        _draft = _gridObjectsFactory.CreateTower(faction, type, true);
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