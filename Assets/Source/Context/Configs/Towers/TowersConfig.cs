using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "Buildings", menuName = "Configs/Buildings")]
public class BuildingsConfig : ScriptableObject
{
    [SerializeField] private SerializedDictionary<TowerType, TowerData> _heavenData;
    [SerializeField] private SerializedDictionary<TowerType, TowerData> _hellData;

    [field: SerializeField] public CastleView CastlePrefab { get; private set; }

    public TowerData GetTowerData(Faction faction, TowerType towerType)
    {
        return faction == Faction.Heaven ? _heavenData[towerType] : _hellData[towerType];
    }
}
