using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using UnityEngine;

public class UnitFactory : MonoBehaviour
{
    [Header("Heaven Unit Prefabs")]
    [SerializeField] private SerializedDictionary<UnitType, UnitView> _heavenUnits;

    [Header("Hell Unit Prefabs")]
    [SerializeField] private SerializedDictionary<UnitType, UnitView> _hellUnits;

    private static UnitFactory _instance;

    private GridSystem _gridSystem;

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError($"Multiple instances of {nameof(UnitFactory)} detected. Leaving the last instantiated one.");
            Destroy(_instance.gameObject);
        }

        _instance = this;
    }

    private void Construct(GridSystem gridSystem)
    {
        _gridSystem = gridSystem;
    }

    [Button]
    public void LaunchTestUnitToHeaven()
    {
        var unit = CreateUnit(Faction.Hell, UnitType.Type1, _gridSystem.Map.GetCheckpoints(Faction.Heaven));

        Faction enemyFaction = 1 - unit.Faction;
        unit.Transform.position = _gridSystem.GetWorldPosition(enemyFaction, _gridSystem.Map.SpawnPoints[enemyFaction].Item1);
    }

    [Button]
    public void LaunchTestUnitToHell()
    {
        var unit = CreateUnit(Faction.Heaven, UnitType.Type1, _gridSystem.Map.GetCheckpoints(Faction.Hell));

        Faction enemyFaction = 1 - unit.Faction;
        unit.Transform.position = _gridSystem.GetWorldPosition(enemyFaction, _gridSystem.Map.SpawnPoints[enemyFaction].Item1);
    }

    public UnitModel CreateUnit(Faction faction, UnitType type, IEnumerable<Vector2Int> checkpoints)
    {
        UnitModel unitModel = null;
        UnitView unit;
        unit = faction == Faction.Heaven ? Instantiate(_heavenUnits[type]) : Instantiate(_hellUnits[type]);

        HealthView healthView = unit.GetComponentInChildren<HealthView>(true);
        TriggerDetector triggerDetector = unit.GetComponentInChildren<TriggerDetector>(true);

        if (healthView != null)
        {
            float healthPoints = Configs.Units.GetHealthPoints(faction, type);

            HealthModel unitHealthModel = new(healthView.transform, healthPoints, healthPoints);
            healthView.AttachPresenter(new Health(healthView, unitHealthModel));
            unitModel = new(unit.transform, unitHealthModel, faction, type, checkpoints);

            unit.AttachPresenter(new Unit(unit, unitModel, _gridSystem, triggerDetector));

            if (triggerDetector != null)
                triggerDetector.AttachComponents(unitModel, unitModel);
        }
        else
        {
            Debug.Log($"Health view has not been assigned to {unit.name}.");
        }

        return unitModel;
    }
}
