using kuRomek.SimpleVG;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapEditingSystem : IUpdatable
{
    private readonly MainCamera _camera;
    private readonly GridSystem _gridSystem;
    private readonly GridObjectsFactory _gridObjectsFactory;
    private readonly Transform _cursor;

    private Brush _brush;
    private TowerType _towerType;

    private GridObjectModel _draft;

    public MapEditingSystem(MainCamera mainCamera, GridSystem gridSystem, GridObjectsFactory gridObjectsFactory, Transform cursor)
    {
        _camera = mainCamera;
        _gridSystem = gridSystem;
        _gridObjectsFactory = gridObjectsFactory;
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

            case Brush.ObjectEraser:
                OnGroundHit(EraseObject);
                break;

            case Brush.Tower:
                OnGroundHit(CreateTower);
                break;

            case Brush.Castle:
                OnGroundHit(CreateCastle);
                break;

            case Brush.Obstacle:
                OnGroundHit(CreateObstacle);
                break;

            case Brush.SpawnPoint:
                OnGroundHit(CreateSpawnPoint);
                break;

            case Brush.Checkpoint:
                OnGroundHit(CreateCheckpoint);
                break;
        }
    }

    private void OnGroundHit(System.Action<Vector2Int> onHit)
    {
        Ray ray = _camera.Camera.ScreenPointToRay(InputController.Current.Position);

        if (Physics.Raycast(ray, out RaycastHit hit, 25f, LayerMask.GetMask(Layers.Ground.ToString())))
            onHit(_gridSystem.GetCell(_camera.TargetFaction, hit.point));
    }

    private void DrawTile(Vector2Int cell)
    {
        var snappedPosition = _gridSystem.GetWorldPosition(_camera.TargetFaction, cell);

        if (_gridSystem.CheckIfTileExist(cell, _camera.TargetFaction) == false)
        {
            _gridSystem.Map.PlaceTile(_camera.TargetFaction, cell,
                (cell) => _gridSystem.GetWorldPosition(_camera.TargetFaction, cell));
        }
    }

    private void Erase(Vector2Int cell)
    {
        if (_gridSystem.CheckIfTileExist(cell, _camera.TargetFaction))
            _gridSystem.Map.RemoveTile(_camera.TargetFaction, cell);
    }

    private void EraseObject(Vector2Int cell)
    {
        if (_gridSystem.CheckIfTileExist(cell, _camera.TargetFaction))
            _gridSystem.Map.RemoveObjectFromTile(_camera.TargetFaction, cell, true);
    }

    private void CreateTower(Vector2Int cell)
    {
        if (_gridSystem.CheckTileAvailability(cell, _camera.TargetFaction))
        {
            TowerModel tower = _gridObjectsFactory.CreateTower(_camera.TargetFaction, _towerType, false);
            _gridSystem.Map.PlaceObjectOnTile(_camera.TargetFaction, cell, tower);
        }
    }

    private void CreateCastle(Vector2Int cell)
    {
        CastleModel targetCastle = _gridSystem.Map.Castles[_camera.TargetFaction].Item2;

        if (targetCastle == null && _gridSystem.CheckTileAvailability(cell, _camera.TargetFaction))
        {
            CastleModel castle = _gridObjectsFactory.CreateCastle(_camera.TargetFaction, false);
            _gridSystem.Map.PlaceObjectOnTile(_camera.TargetFaction, cell, castle);
        }
    }

    public void CreateObstacle(Vector2Int cell)
    {
        if (_gridSystem.CheckTileAvailability(cell, _camera.TargetFaction))
        {
            ObstacleModel obstacle = _gridObjectsFactory.CreateObstacle(_camera.TargetFaction, false);
            _gridSystem.Map.PlaceObjectOnTile(_camera.TargetFaction, cell, obstacle);
        }
    }

    public void CreateSpawnPoint(Vector2Int cell)
    {
        SpawnPointModel targetSpawnPoint = _gridSystem.Map.SpawnPoints[_camera.TargetFaction].Item2;

        if (targetSpawnPoint == null && _gridSystem.CheckTileAvailability(cell, _camera.TargetFaction))
        {
            SpawnPointModel spawnPoint = _gridObjectsFactory.CreateSpawnPoint(_camera.TargetFaction, false);
            _gridSystem.Map.PlaceObjectOnTile(_camera.TargetFaction, cell, spawnPoint);
        }
    }

    public void CreateCheckpoint(Vector2Int cell)
    {
        if (_gridSystem.CheckTileAvailability(cell, _camera.TargetFaction))
        {
            int number = _gridSystem.Map.CheckpointNumbers[_camera.TargetFaction];
            CheckpointModel checkpoint = _gridObjectsFactory.CreateCheckpoint(_camera.TargetFaction, false, number);
            _gridSystem.Map.PlaceObjectOnTile(_camera.TargetFaction, cell, checkpoint);
        }
    }

    public void SetBrush(Brush brush)
    {
        _draft?.Destroy();
        _draft = null;

        _brush = brush;

        _camera.ToggleControlBlock(_brush != Brush.None);
        _cursor.gameObject.SetActive(_brush != Brush.None);

        _draft = brush switch
        {
            Brush.Tower => _gridObjectsFactory.CreateTower(_camera.TargetFaction, _towerType, true),
            Brush.Castle => _gridObjectsFactory.CreateCastle(_camera.TargetFaction, true),
            Brush.Obstacle => _gridObjectsFactory.CreateObstacle(_camera.TargetFaction, true),
            Brush.SpawnPoint => _gridObjectsFactory.CreateSpawnPoint(_camera.TargetFaction, true),
            Brush.Checkpoint => _gridObjectsFactory.CreateCheckpoint(_camera.TargetFaction, true,
                                _gridSystem.Map.CheckpointNumbers[_camera.TargetFaction]),
            _ => null,
        };

        _draft?.SetCursorFollowing(true);
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
                CastleModel castle => new MapCastleData() { HealthPoints = castle.Health.MaxAmount },
                ObstacleModel _ => new MapObstacleData(),
                SpawnPointModel _ => new MapSpawnPointData(),
                CheckpointModel checkpoint => new MapCheckpointData() { Number = checkpoint.Number },
                _ => null,
            };

            mapData.PlaceTile(faction, cell.Key, @object);
        }
    }

    public enum Brush
    {
        None,
        Eraser,
        ObjectEraser,
        Tile,
        Tower,
        Castle,
        Obstacle,
        SpawnPoint,
        Checkpoint,
    }
}