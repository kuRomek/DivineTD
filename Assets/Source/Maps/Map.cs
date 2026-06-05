using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField, ReadOnly] private SerializedDictionary<Vector2Int, Tile> _cellsHeaven;
    [SerializeField, ReadOnly] private SerializedDictionary<Vector2Int, Tile> _cellsHell;

    [field: ValidateInput(nameof(CheckIfHeavenCastleNull), "The Heaven castle cannot be null")]
    [field: SerializeField, ReadOnly] public CastleModel HeavenCastle { get; private set; }

    [field: ValidateInput(nameof(CheckIfHellCastleNull), "The Hell castle cannot be null")]
    [field: SerializeField, ReadOnly] public CastleModel HellCastle { get; private set; }

    public IReadOnlyDictionary<Vector2Int, Tile> CellsHeaven => _cellsHeaven;
    public IReadOnlyDictionary<Vector2Int, Tile> CellsHell => _cellsHell;

    public void PlaceTile(Faction faction, Vector2Int cell, Func<Vector2Int, Vector3> convertPosition)
    {
        var cells = faction == Faction.Heaven ? _cellsHeaven : _cellsHell;

        if (cells.TryGetValue(cell, out Tile tile))
            RemoveTile(faction, cell, tile, cells);

        tile = Instantiate(Configs.Grid.TilePrefab, convertPosition(cell), default);

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
        if (this[faction].TryGetValue(cell, out Tile tile))
            tile.RemoveObject().Destroy();
    }

    public IReadOnlyDictionary<Vector2Int, Tile> this[Faction faction]
        => faction == Faction.Heaven ? CellsHeaven : CellsHell;

    public bool CheckIfHeavenCastleNull()
        => HeavenCastle != null;

    public bool CheckIfHellCastleNull()
        => HellCastle != null;
}
