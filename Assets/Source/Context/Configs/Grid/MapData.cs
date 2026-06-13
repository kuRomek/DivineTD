using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using UnityEngine;

[Serializable]
public struct MapData
{
    [SerializeField, ReadOnly] private SerializedDictionary<Vector2Int, TileData> _cellsHeaven;
    [SerializeField, ReadOnly] private SerializedDictionary<Vector2Int, TileData> _cellsHell;

    public MapData(bool initializeCells)
    {
        if (initializeCells)
        {
            _cellsHeaven = new SerializedDictionary<Vector2Int, TileData>();
            _cellsHell = new SerializedDictionary<Vector2Int, TileData>();
        }
        else
        {
            _cellsHeaven = null;
            _cellsHell = null;
        }
    }

    public readonly IReadOnlyDictionary<Vector2Int, TileData> CellsHeaven => _cellsHeaven;
    public readonly IReadOnlyDictionary<Vector2Int, TileData> CellsHell => _cellsHell;

    public readonly void PlaceTile(Faction faction, Vector2Int cell)
    {
        var cells = faction == Faction.Heaven ? _cellsHeaven : _cellsHell;
        cells[cell] = new TileData();
    }

    public void PlaceTile(Faction faction, Vector2Int cell, MapGridObjectData @object)
    {
        var cells = faction == Faction.Heaven ? _cellsHeaven : _cellsHell;

        TileData tile = new TileData { Object = @object };

        cells[cell] = tile;
    }

    public readonly void RemoveTile(Faction faction, Vector2Int cell)
    {
        (faction == Faction.Heaven ? _cellsHeaven : _cellsHell).Remove(cell);
    }

    public void AttachObjectToTile(Faction faction, Vector2Int cell, MapGridObjectData @object)
    {
        if (this[faction].TryGetValue(cell, out TileData tile))
            tile.Object = @object;
    }

    public readonly void RemoveObjectFromTile(Faction faction, Vector2Int cell)
    {
        if (this[faction].TryGetValue(cell, out TileData tile))
            tile.Object = null;
    }

    public readonly IReadOnlyDictionary<Vector2Int, TileData> this[Faction faction]
        => faction == Faction.Heaven ? CellsHeaven : CellsHell;

}
