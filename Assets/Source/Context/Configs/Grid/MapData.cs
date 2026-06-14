using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using UnityEngine;

[Serializable]
public struct MapData
{
    [SerializeField, ReadOnly] private SerializedDictionary<Faction, SerializedDictionary<Vector2Int, TileData>> _cells;

    public MapData(bool initializeCells)
    {
        if (initializeCells)
        {
            _cells = new()
            {
                { Faction.Heaven, new() { { default, new() } } },
                { Faction.Hell, new() { { default, new() } } },
            };
        }
        else
        {
            _cells = null;
        }
    }

    public readonly IReadOnlyDictionary<Vector2Int, TileData> this[Faction faction]
        => _cells[faction];

    public readonly void PlaceTile(Faction faction, Vector2Int cell)
    {
        _cells[faction][cell] = new TileData();
    }

    public readonly void RemoveTile(Faction faction, Vector2Int cell)
    {
        _cells[faction].Remove(cell);
    }

    public readonly void AttachObjectToTile(Faction faction, Vector2Int cell, MapGridObjectData @object)
    {
        if (_cells[faction].TryGetValue(cell, out TileData tile))
            tile.Object = @object;
    }

    public readonly void RemoveObjectFromTile(Faction faction, Vector2Int cell)
    {
        if (_cells[faction].TryGetValue(cell, out TileData tile))
            tile.Object = null;
    }

    public void PlaceTile(Faction faction, Vector2Int cell, MapGridObjectData @object)
    {
        _cells[faction][cell] = new() { Object = @object };
    }
}
