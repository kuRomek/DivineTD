using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "Buildings", menuName = "Configs/Buildings")]
public class BuildingsConfig : ScriptableObject
{
    [SerializeField] private SerializedDictionary<TowerType, TowerData> _heavenData;
    [SerializeField] private SerializedDictionary<TowerType, TowerData> _hellData;

    public TowerData GetTowerData(Faction faction, TowerType towerType)
    {
        return faction == Faction.Heaven ? _heavenData[towerType] : _hellData[towerType];
    }

    public int GetCost(Faction faction, TowerType type)
    {
        return faction == Faction.Heaven ? _heavenData[type].Cost : _hellData[type].Cost;
    }
}
