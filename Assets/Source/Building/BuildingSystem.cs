using UnityEngine;

public class BuildingSystem
{
    private readonly GridSystem _gridSystem;
    private readonly TowerFactory _towerFactory;
    private readonly MainCamera _mainCamera;

    private TowerModel _towerDraft = null;

    public BuildingSystem(GridSystem gridSystem, TowerFactory towerFactory, MainCamera mainCamera)
    {
        _gridSystem = gridSystem;
        _towerFactory = towerFactory;
        _mainCamera = mainCamera;
    }

    public void CreateTowerDraft(Faction faction, TowerType type)
    {
        _mainCamera.ToggleControlBlock(true);
        _towerDraft = _towerFactory.CreateTower(faction, type, true);
        _towerDraft.MoveAt(_gridSystem.GetSnappedPosition(faction));
    }

    public bool TryBuildTower(Faction faction)
    {
        if (_gridSystem.TryPlace(_towerDraft, faction))
        {
            _towerDraft.ToggleDrafting(false);
            _towerDraft = null;
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

        if (_towerDraft != null)
        {
            _towerDraft.Destroy();
            _towerDraft = null;
        }
        else
        {
            Debug.LogWarning("Trying to cancel building, although there is no draft");
        }
    }
}
