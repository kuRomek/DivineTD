using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(fileName = "Units", menuName = "Configs/Units")]
public class UnitsConfig : ScriptableObject
{
    [SerializeField] private SerializedDictionary<UnitType, UnitData> _heavenData;
    [SerializeField] private SerializedDictionary<UnitType, UnitData> _hellData;

    public float GetSpeed(bool isHeavenFaction, UnitType unitType)
    {
        return isHeavenFaction ? _heavenData[unitType].Speed : _hellData[unitType].Speed;
    }
}
