using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[Serializable]
public struct LevelData
{
    //[ReadOnly, AllowNesting]
    [SerializeField] public MapData Map;
    [SerializeField] private SerializedDictionary<Faction, LevelMiscData> _miscData;

    public readonly IReadOnlyDictionary<Faction, LevelMiscData> MiscData => _miscData;

    public LevelData(MapData map)
    {
        Map = map;

        _miscData = new()
        {
            { Faction.Heaven, default },
            { Faction.Hell, default }
        };
    }

    public readonly int GetCastleHealth(Faction faction)
        => _miscData[faction].Buildings.CastleHealthOnStart;

    public readonly (int Income, int Amount) GetFunds(Faction faction)
        => (_miscData[faction].Economy.IncomeOnStart, _miscData[faction].Economy.FundsAmountOnStart);
}

[Serializable]
public struct LevelMiscData
{
    [SerializeField] private List<TowerType> _availableTowerTypes;
    [SerializeField] private List<UnitType> _availableUnitTypes;

    public LevelBuildingsData Buildings;
    public LevelEconomyData Economy;

    public readonly IReadOnlyList<TowerType> AvailableTowerTypes => _availableTowerTypes;
    public readonly IReadOnlyList<UnitType> AvailableUnitTypes => _availableUnitTypes;
}

[Serializable]
public struct LevelBuildingsData
{
    public int CastleHealthOnStart;
}

[Serializable]
public struct LevelEconomyData
{
    public int IncomeOnStart;
    public int FundsAmountOnStart;
}