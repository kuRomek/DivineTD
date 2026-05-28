using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "Towers", menuName = "Configs/Towers")]
public class TowersConfig : ScriptableObject
{
    [SerializeField] private SerializedDictionary<TowerType, TowerData> _heavenData;
    [SerializeField] private SerializedDictionary<TowerType, TowerData> _hellData;

    public TowerData GetParams(bool isHeavenFaction, TowerType towerType)
    {
        return isHeavenFaction ? _heavenData[towerType] : _hellData[towerType];
    }
}
