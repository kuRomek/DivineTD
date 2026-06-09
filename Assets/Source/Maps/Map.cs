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

    public CastleModel HeavenCastle { get; private set; }
    public CastleModel HellCastle { get; private set; }

    public IReadOnlyDictionary<Vector2Int, Tile> CellsHeaven => _cellsHeaven;
    public IReadOnlyDictionary<Vector2Int, Tile> CellsHell => _cellsHell;

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
        cells ??= faction == Faction.Heaven ? _cellsHeaven : _cellsHell;

        if (tile == null && cells.TryGetValue(cell, out tile) == false)
            return;

        if (tile != null)
            tile.Destroy();

        cells.Remove(cell);
    }

    public void PlaceObjectOnTile(Faction faction, Vector2Int cell, GridObjectModel @object)
    {
        if (this[faction].TryGetValue(cell, out Tile tile))
        {
            tile.SetObject(@object);

            if (@object is CastleModel castle)
            {
                if (faction == Faction.Heaven)
                    HeavenCastle = castle;
                else
                    HellCastle = castle;
            }
        }
    }

    public void RemoveObjectFromTile(Faction faction, Vector2Int cell, bool destroy = true)
    {
        if (this[faction].TryGetValue(cell, out Tile tile) && tile.Object != null)
            tile.RemoveObject().Destroy();
    }

    public IReadOnlyDictionary<Vector2Int, Tile> this[Faction faction]
        => faction == Faction.Heaven ? CellsHeaven : CellsHell;

    public bool CheckIfHeavenCastleNull()
        => HeavenCastle != null;

    public bool CheckIfHellCastleNull()
        => HellCastle != null;
}
