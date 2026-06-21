using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[Serializable]
public struct LevelData
{
    //[ReadOnly, AllowNesting]
    [SerializeField] public MapData Map;
    [SerializeField] private SerializedDictionary<Faction, int> _castleHealthAmounts;
    [SerializeField] private SerializedDictionary<Faction, (int Income, int Amount)> _funds;

    public LevelData(MapData map)
    {
        Map = map;
        _castleHealthAmounts = new()
        {
            { Faction.Heaven, 100 },
            { Faction.Hell, 100 }
        };
        _funds = new()
        {
            { Faction.Heaven, (100, 100) },
            { Faction.Hell, (100, 100) }
        };
    }

    public readonly int GetCastleHealth(Faction faction)
        => _castleHealthAmounts[faction];

    public readonly (int Income, int Amount) GetFunds(Faction faction)
        => _funds[faction];
}