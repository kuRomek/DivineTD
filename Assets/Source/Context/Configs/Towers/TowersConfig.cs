using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "Buildings", menuName = "Configs/Buildings")]
public class BuildingsConfig : ScriptableObject
{
    [SerializeField] private SerializedDictionary<TowerType, TowerData> _heavenData;
    [SerializeField] private SerializedDictionary<TowerType, TowerData> _hellData;

    [field: SerializeField] public CastleView CastlePrefab { get; private set; }

    public TowerData GetTowerParams(bool heavenFaction, TowerType towerType)
    {
        return heavenFaction ? _heavenData[towerType] : _hellData[towerType];
    }
}
