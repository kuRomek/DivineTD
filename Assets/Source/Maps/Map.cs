using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    [field: Header("Grids")]
    [field: SerializeField] public Grid HeavenGrid { get; private set; }
    [field: SerializeField] public Grid HellGrid { get; private set; }

    [field: Header("Camera Spots")]
    [field: SerializeField] public Transform HeavenCameraSpot { get; private set; }
    [field: SerializeField] public Transform HellCameraSpot { get; private set; }

    [Header("Tilemaps")]
    [SerializeField] private Tilemap _heavenTilemap;
    [SerializeField] private Tilemap _hellTilemap;

    [SerializeField, ReadOnly] private SerializedDictionary<Vector2Int, Tile> _cellsHeaven;
    [SerializeField, ReadOnly] private SerializedDictionary<Vector2Int, Tile> _cellsHell;

    private readonly List<Vector2Int> _heavenCheckpoints = new();
    private readonly List<Vector2Int> _hellCheckpoints = new();

    private Vector2Int _heavenSpawnPoint;
    private Vector2Int _hellSpawnPoint;

    public SpawnPointModel HeavenSpawnPoint { get; private set; }
    public SpawnPointModel HellSpawnPoint { get; private set; }

    public CastleModel HeavenCastle { get; private set; }
    public CastleModel HellCastle { get; private set; }

    public IReadOnlyDictionary<Vector2Int, Tile> CellsHeaven => _cellsHeaven;
    public IReadOnlyDictionary<Vector2Int, Tile> CellsHell => _cellsHell;

    public IReadOnlyDictionary<Vector2Int, Tile> this[Faction faction]
        => faction == Faction.Heaven ? CellsHeaven : CellsHell;

    public void PlaceTile(Faction faction, Vector2Int cell, Func<Vector2Int, Vector3> convertPosition)
    {
        Dictionary<Vector2Int, Tile> cells;
        Tilemap tilemap;

        if (faction == Faction.Heaven)
        {
            cells = _cellsHeaven;
            tilemap = _heavenTilemap;
        }
        else
        {
            cells = _cellsHell;
            tilemap = _hellTilemap;
        }

        if (cells.TryGetValue(cell, out Tile tile))
            RemoveTile(faction, cell, tile, cells);

        tile = Instantiate(Configs.Grid.TilePrefab, convertPosition(cell), default);
        tile.transform.SetParent(faction == Faction.Heaven ? HeavenGrid.transform : HellGrid.transform, true);
        tilemap.SetTile((Vector3Int)cell, faction == Faction.Heaven ? Configs.Grid.HeavenTile : Configs.Grid.HellTile);

        cells[cell] = tile;
    }

    public void RemoveTile(Faction faction, Vector2Int cell, Tile tile = null, Dictionary<Vector2Int, Tile> cells = null)
    {
        Tilemap tilemap;
        List<Vector2Int> checkpoints;

        if (faction == Faction.Heaven)
        {
            cells ??= _cellsHeaven;
            tilemap = _heavenTilemap;
            checkpoints = _heavenCheckpoints;
        }
        else
        {
            cells ??= _cellsHell;
            tilemap = _hellTilemap;
            checkpoints = _hellCheckpoints;
        }

        if (tile == null && cells.TryGetValue(cell, out tile) == false)
            return;

        if (tile != null)
        {
            if (tile.Object is CheckpointModel)
                checkpoints.Remove(cell);

            tile.Destroy();
            tilemap.SetTile((Vector3Int)cell, null);
        }

        cells.Remove(cell);
    }

    public void PlaceObjectOnTile(Faction faction, Vector2Int cell, GridObjectModel @object)
    {
        if (this[faction].TryGetValue(cell, out Tile tile) && tile.Object == null)
        {
            tile.SetObject(@object);
            bool heaven = faction == Faction.Heaven;

            switch (@object)
            {
                case CastleModel castle:
                    if (heaven) HeavenCastle = castle; else HellCastle = castle;
                    break;

                case CheckpointModel:
                    (heaven ? _heavenCheckpoints : _hellCheckpoints).Add(cell);
                    break;

                case SpawnPointModel spawnPoint:
                    if (heaven)
                    {
                        _heavenSpawnPoint = cell;
                        HeavenSpawnPoint = spawnPoint;
                    }
                    else
                    {
                        _hellSpawnPoint = cell;
                        HellSpawnPoint = spawnPoint;
                    }
                    break;
            }
        }

        UpdateCheckpointsNumber();
    }

    public void RemoveObjectFromTile(Faction faction, Vector2Int cell, bool destroy = true)
    {
        if (this[faction].TryGetValue(cell, out Tile tile) && tile.Object != null)
        {
            GridObjectModel @object = tile.RemoveObject();

            bool heaven = faction == Faction.Heaven;

            switch (@object)
            {
                case CastleModel:
                    if (heaven) HeavenCastle = null; else HellCastle = null;
                    break;

                case CheckpointModel:
                    (heaven ? _heavenCheckpoints : _hellCheckpoints).Remove(cell);
                    break;

                case SpawnPointModel:
                    if (heaven)
                    {
                        _heavenSpawnPoint = cell;
                        HeavenSpawnPoint = null;
                    }
                    else
                    {
                        _hellSpawnPoint = cell;
                        HellSpawnPoint = null;
                    }
                    break;
            }

            if (destroy && @object != null)
                @object.Destroy();
        }

        UpdateCheckpointsNumber();
    }

    public Vector2Int GetSpawnPosition(Faction faction)
    {
        return faction == Faction.Heaven ? _heavenSpawnPoint : _hellSpawnPoint;
    }

    public IReadOnlyList<Vector2Int> GetCheckpoints(Faction faction)
    {
        return faction == Faction.Heaven ? _heavenCheckpoints : _hellCheckpoints;
    }

    private void UpdateCheckpointsNumber()
    {
        foreach (Faction faction in new Faction[2] { Faction.Heaven, Faction.Hell })
        {
            IEnumerable<Vector2Int> checkpoints;
            IReadOnlyDictionary<Vector2Int, Tile> cells;

            if (faction == Faction.Heaven)
            {
                checkpoints = _heavenCheckpoints;
                cells = _cellsHeaven;
            }
            else
            {
                checkpoints = _hellCheckpoints;
                cells = _cellsHell;
            }

            int i = 1;

            foreach (Vector2Int cell in checkpoints)
                if (cells[cell].Object is CheckpointModel checkpoint)
                    checkpoint.SetNumber(i++);
        }
    }
}
