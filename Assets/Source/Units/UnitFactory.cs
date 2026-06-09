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
        var unit = CreateUnit(Faction.Hell, UnitType.Type1,
            new() { _gridSystem.GetCell(Faction.Heaven, _gridSystem.Map.HeavenCastle.Transform.position) });

        unit.Transform.position = _gridSystem.GetWorldPosition(Faction.Heaven, default);
    }

    [Button]
    public void LaunchTestUnitToHell()
    {
        var unit = CreateUnit(Faction.Heaven, UnitType.Type1,
            new() { _gridSystem.GetCell(Faction.Hell, _gridSystem.Map.HellCastle.Transform.position) });

        unit.Transform.position = _gridSystem.GetWorldPosition(Faction.Hell, default);
    }

    public UnitModel CreateUnit(Faction faction, UnitType type, List<Vector2Int> targets)
    {
        UnitModel unitModel = null;
        UnitView unit;
        IReadOnlyDictionary<Vector2Int, Tile> walkingGrid = _gridSystem.Map[faction];
        unit = faction == Faction.Heaven ? Instantiate(_heavenUnits[type]) : Instantiate(_hellUnits[type]);

        HealthView healthView = unit.GetComponentInChildren<HealthView>(true);
        TriggerDetector triggerDetector = unit.GetComponentInChildren<TriggerDetector>(true);

        if (healthView != null)
        {
            float healthPoints = Configs.Units.GetHealthPoints(faction, type);

            HealthModel unitHealthModel = new(healthView.transform, healthPoints, healthPoints);
            healthView.AttachPresenter(new Health(healthView, unitHealthModel));
            unitModel = new(unit.transform, unitHealthModel, faction, type, targets);

            unit.AttachPresenter(new Unit(unit, unitModel, walkingGrid, _gridSystem, triggerDetector));

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
