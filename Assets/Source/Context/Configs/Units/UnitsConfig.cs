using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(fileName = "Units", menuName = "Configs/Units")]
public class UnitsConfig : ScriptableObject
{
    [SerializeField] private SerializedDictionary<UnitType, UnitData> _heavenData;
    [SerializeField] private SerializedDictionary<UnitType, UnitData> _hellData;

    public float GetSpeed(bool heavenFaction, UnitType unitType)
    {
        return heavenFaction ? _heavenData[unitType].Speed : _hellData[unitType].Speed;
    }

    public float GetHealthPoints(bool heavenFaction, UnitType unitType)
    {
        return heavenFaction ? _heavenData[unitType].HealthAmount : _hellData[unitType].HealthAmount;
    }
}
