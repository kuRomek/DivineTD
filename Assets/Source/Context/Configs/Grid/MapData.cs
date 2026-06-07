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

    [field: ValidateInput(nameof(CheckIfHeavenCastleNull), "The Heaven castle cannot be null")]
    [field: SerializeField, ReadOnly] public MapCastleData HeavenCastle { get; private set; }

    [field: ValidateInput(nameof(CheckIfHellCastleNull), "The Hell castle cannot be null")]
    [field: SerializeField, ReadOnly] public MapCastleData HellCastle { get; private set; }

    public readonly IReadOnlyDictionary<Vector2Int, TileData> CellsHeaven => _cellsHeaven;
    public readonly IReadOnlyDictionary<Vector2Int, TileData> CellsHell => _cellsHell;

    public readonly void PlaceTile(Faction faction, Vector2Int cell)
    {
        (faction == Faction.Heaven ? _cellsHeaven : _cellsHell)[cell] = new TileData();
    }

    public readonly void RemoveTile(Faction faction, Vector2Int cell)
    {
        (faction == Faction.Heaven ? _cellsHeaven : _cellsHell).Remove(cell);
    }

    public void AttachObjectToTile(Faction faction, Vector2Int cell, MapGridObjectData @object)
    {
        if (this[faction].TryGetValue(cell, out TileData tile))
        {
            tile.Object = @object;

            if (@object is MapCastleData castle)
                FillCastleData(castle, faction);
        }
    }

    public readonly void RemoveObjectFromTile(Faction faction, Vector2Int cell)
    {
        if (this[faction].TryGetValue(cell, out TileData tile))
            tile.Object = null;
    }

    public readonly IReadOnlyDictionary<Vector2Int, TileData> this[Faction faction]
        => faction == Faction.Heaven ? CellsHeaven : CellsHell;

    private readonly bool CheckIfHeavenCastleNull()
        => HeavenCastle != null;

    private readonly bool CheckIfHellCastleNull()
        => HellCastle != null;

    private void FillCastleData(MapCastleData castle, Faction faction)
    {
        if (faction == Faction.Heaven)
        {
            if (HeavenCastle != null)
                Debug.LogError("There is already a Heaven castle set up.");
            else
                HeavenCastle = castle;
        }
        else
        {
            if (HellCastle != null)
                Debug.LogError("There is already a Hell castle set up.");
            else
                HellCastle = castle;
        }
    }
}
