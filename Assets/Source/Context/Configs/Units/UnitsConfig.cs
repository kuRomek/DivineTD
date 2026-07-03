using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(fileName = "Units", menuName = "Configs/Units")]
public class UnitsConfig : ScriptableObject
{
    [SerializeField] private SerializedDictionary<UnitType, UnitData> _heavenData;
    [SerializeField] private SerializedDictionary<UnitType, UnitData> _hellData;

    public int GetCost(Faction faction, UnitType unitType)
    {
        return faction == Faction.Heaven ? _heavenData[unitType].Cost : _hellData[unitType].Cost;
    }

    public int GetIncomeAdding(Faction faction, UnitType unitType)
    {
        return faction == Faction.Heaven ? _heavenData[unitType].IncomeAdding : _hellData[unitType].IncomeAdding;
    }

    public float GetSpeed(Faction faction, UnitType unitType)
    {
        return faction == Faction.Heaven ? _heavenData[unitType].Speed : _hellData[unitType].Speed;
    }

    public float GetHealthPoints(Faction faction, UnitType unitType)
    {
        return faction == Faction.Heaven ? _heavenData[unitType].HealthAmount : _hellData[unitType].HealthAmount;
    }
}
