using kuRomek.SimpleVG;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapEditingSystem : IUpdatable
{
    private readonly MainCamera _camera;
    private readonly GridSystem _gridSystem;
    private readonly TowerFactory _towerFactory;
    private readonly Transform _cursor;

    private Brush _brush;
    private TowerType _towerType;

    private GridObjectModel _draft;

    public MapEditingSystem(MainCamera mainCamera, GridSystem gridSystem, TowerFactory towerFactory, Transform cursor)
    {
        _camera = mainCamera;
        _gridSystem = gridSystem;
        _towerFactory = towerFactory;
        _cursor = cursor;
    }

    void IUpdatable.Update(float deltaTime)
    {
        if (EventSystem.current.IsPointerOverGameObject() == false)
            _gridSystem.Drag(_cursor.transform, _camera.TargetFaction);

        if (InputController.Current.Pressing == false || EventSystem.current.IsPointerOverGameObject())
            return;

        switch (_brush)
        {
            case Brush.Tile:
                OnGroundHit(DrawTile);
                break;

            case Brush.Eraser:
                OnGroundHit(Erase);
                break;

            case Brush.Tower:
                OnGroundHit(CreateTower);
                break;

            case Brush.Castle:
                OnGroundHit(CreateCastle);
                break;
        }
    }

    private void OnGroundHit(System.Action<Vector3> action)
    {
        Ray ray = _camera.Camera.ScreenPointToRay(InputController.Current.Position);

        if (Physics.Raycast(ray, out RaycastHit hit, 25f, LayerMask.GetMask(Layers.Ground.ToString())))
            action(hit.point);
    }

    private void DrawTile(Vector3 position)
    {
        var cell = _gridSystem.GetCell(_camera.TargetFaction, position);
        var snappedPosition = _gridSystem.GetWorldPosition(_camera.TargetFaction, cell);

        if (_gridSystem.CheckIfTileExist(cell, _camera.TargetFaction) == false)
        {
            _gridSystem.Map.PlaceTile(_camera.TargetFaction, cell,
                (cell) => _gridSystem.GetWorldPosition(_camera.TargetFaction, cell));
        }
    }

    private void Erase(Vector3 position)
    {
        var cell = _gridSystem.GetCell(_camera.TargetFaction, position);

        if (_gridSystem.CheckIfTileExist(cell, _camera.TargetFaction))
            _gridSystem.Map.RemoveTile(_camera.TargetFaction, cell);
    }

    private void CreateTower(Vector3 position)
    {
        var cell = _gridSystem.GetCell(_camera.TargetFaction, position);

        if (_gridSystem.CheckTileAvailability(cell, _camera.TargetFaction))
        {
            TowerModel tower = _towerFactory.CreateTower(_camera.TargetFaction, _towerType, false);
            _gridSystem.Map.PlaceObjectOnTile(_camera.TargetFaction, cell, tower);
        }
    }

    private void CreateCastle(Vector3 position)
    {
        var cell = _gridSystem.GetCell(_camera.TargetFaction, position);

        var targetCastle = _camera.TargetFaction == Faction.Heaven ? _gridSystem.Map.HeavenCastle : _gridSystem.Map.HellCastle;

        if (targetCastle == null && _gridSystem.CheckTileAvailability(cell, _camera.TargetFaction))
        {
            CastleModel castle = CreateCastle(false);
            _gridSystem.Map.PlaceObjectOnTile(_camera.TargetFaction, cell, castle);
        }
    }

    public void SetBrush(Brush brush)
    {
        _draft?.Destroy();
        _draft = null;

        _brush = brush;

        _camera.ToggleControlBlock(_brush != Brush.None);
        _cursor.gameObject.SetActive(_brush != Brush.None);

        if (brush == Brush.Tower)
        {
            _draft = _towerFactory.CreateTower(_camera.TargetFaction, _towerType, true);
            _draft.SetCursorFollowing(true);
        }
        else if (brush == Brush.Castle)
        {
            _draft = CreateCastle(true);
            _draft.SetCursorFollowing(true);
        }
    }

    public void SaveMap()
    {
        Configs.Levels.SaveMapData(ConvertData(_gridSystem.Map), GameState.CurrentPlayerFaction, GameState.CurrentLevel);
    }

    public void SetTowerType(TowerType type)
    {
        _towerType = type;
    }

    public MapData ConvertData(Map map)
    {
        MapData mapData = new(true);

        FillFaction(map, ref mapData, Faction.Heaven);
        FillFaction(map, ref mapData, Faction.Hell);

        return mapData;
    }

    private void FillFaction(Map map, ref MapData mapData, Faction faction)
    {
        foreach (var cell in map[faction])
        {
            MapGridObjectData @object = cell.Value.Object switch
            {
                TowerModel tower => new MapTowerData() { Type = tower.Type },
                CastleModel => new MapCastleData(),
                _ => null,
            };

            mapData.PlaceTile(faction, cell.Key, @object);
        }
    }

    private CastleModel CreateCastle(bool draft)
    {
        var castlesData = Configs.Levels.GetCastleData(GameState.CurrentPlayerFaction, GameState.CurrentLevel);
        int healthAmount = castlesData[_camera.TargetFaction];

        CastleView castleView = Object.Instantiate(Configs.Buildings.CastlePrefab);

        HealthModel health = new(castleView.HealthBar.transform, healthAmount, healthAmount);
        castleView.HealthBar.AttachPresenter(new Health(castleView.HealthBar, health));

        CastleModel castle = new(castleView.transform, health, _camera.TargetFaction, draft);
        castleView.AttachPresenter(new Castle(castleView, castle, _gridSystem));

        return castle;
    }

    public enum Brush
    {
        None,
        Eraser,
        ObjectEraser,
        Tile,
        Tower,
        Castle,
    }
}