using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    [Header("Grids")]
    [SerializeField] private SerializedDictionary<Faction, Grid> _grids;

    [Header("Camera Spots")]
    [SerializeField] private SerializedDictionary<Faction, Transform> _cameraSpots;

    [Header("Tilemaps")]
    [SerializeField] private SerializedDictionary<Faction, Tilemap> _tilemaps;

    private readonly Dictionary<Faction, (Vector2Int, SpawnPointModel)> _spawnPoints = new()
    {
        { Faction.Heaven, default },
        { Faction.Hell, default }
    };
    private readonly Dictionary<Faction, (Vector2Int, CastleModel)> _castles = new()
    {
        { Faction.Heaven, default },
        { Faction.Hell, default }
    };
    private readonly Dictionary<Faction, List<Vector2Int>> _checkpoints = new()
    {
        { Faction.Heaven, new() },
        { Faction.Hell, new() }
    };
    private readonly Dictionary<Faction, Dictionary<Vector2Int, Tile>> _cells = new()
    {
        { Faction.Heaven, new() },
        { Faction.Hell, new() }
    };
    private readonly Dictionary<Faction, int> _checkpointNumbers = new()
    {
        { Faction.Heaven, default },
        { Faction.Hell, default }
    };

    public IReadOnlyDictionary<Faction, Grid> Grids => _grids;
    public IReadOnlyDictionary<Faction, Transform> CameraSpots => _cameraSpots;
    public IReadOnlyDictionary<Faction, Tilemap> Tilemaps => _tilemaps;
    public IReadOnlyDictionary<Faction, (Vector2Int, SpawnPointModel)> SpawnPoints => _spawnPoints;
    public IReadOnlyDictionary<Faction, (Vector2Int, CastleModel)> Castles => _castles;
    public IReadOnlyDictionary<Faction, int> CheckpointNumbers => _checkpointNumbers;

    public IReadOnlyDictionary<Vector2Int, Tile> this[Faction faction]
        => _cells[faction];

    public void PlaceTile(Faction faction, Vector2Int cell, Func<Vector2Int, Vector3> convertPosition)
    {
        RemoveTile(faction, cell);

        Tile tile = Instantiate(Configs.Grid.TilePrefab, convertPosition(cell), default);
        tile.transform.SetParent(_grids[faction].transform, true);
        _tilemaps[faction].SetTile((Vector3Int)cell, faction == Faction.Heaven ? Configs.Grid.HeavenTile : Configs.Grid.HellTile);

        _cells[faction][cell] = tile;
    }

    public void RemoveTile(Faction faction, Vector2Int cell)
    {
        if (_cells[faction].TryGetValue(cell, out Tile tile) == false)
            return;

        if (tile != null)
        {
            if (tile.Object != null)
                RemoveObjectFromTile(faction, cell, true);

            tile.Destroy();
            _tilemaps[faction].SetTile((Vector3Int)cell, null);
        }

        _cells[faction].Remove(cell);
    }

    public void PlaceObjectOnTile(Faction faction, Vector2Int cell, GridObjectModel @object)
    {
        if (_cells[faction].TryGetValue(cell, out Tile tile) && tile.Object == null)
        {
            tile.SetObject(@object);

            switch (@object)
            {
                case CastleModel castle:
                    _castles[faction] = (cell, castle);
                    break;

                case CheckpointModel:
                    _checkpoints[faction].Add(cell);
                    UpdateCheckpointsNumber();
                    break;

                case SpawnPointModel spawnPoint:
                    _spawnPoints[faction] = (cell, spawnPoint);
                    break;
            }
        }
    }

    public void RemoveObjectFromTile(Faction faction, Vector2Int cell, bool destroy = true)
    {
        if (this[faction].TryGetValue(cell, out Tile tile) && tile.Object != null)
        {
            GridObjectModel @object = tile.RemoveObject();

            switch (@object)
            {
                case CastleModel:
                    _castles[faction] = default;
                    break;

                case CheckpointModel:
                    _checkpoints[faction].Remove(cell);
                    UpdateCheckpointsNumber();
                    break;

                case SpawnPointModel:
                    _spawnPoints[faction] = default;
                    break;
            }

            if (destroy && @object != null)
                @object.Destroy();
        }
    }

    public IEnumerable<Vector2Int> GetCheckpoints(Faction faction)
    {
        return _castles[faction] == default ? _checkpoints[faction] : _checkpoints[faction].Append(_castles[faction].Item1);
    }

    private void UpdateCheckpointsNumber()
    {
        foreach (Faction faction in new Faction[2] { Faction.Heaven, Faction.Hell })
        {
            int i = 1;

            foreach (var cell in _checkpoints[faction])
                if (_cells[faction][cell].Object is CheckpointModel checkpoint)
                    checkpoint.SetNumber(i++);

            _checkpointNumbers[faction] = _castles[faction] == default ? i - 1 : i;
        }
    }
}
