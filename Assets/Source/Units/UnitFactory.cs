using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using UnityEngine;

public class UnitFactory : MonoBehaviour
{
    [Header("Essentials")]
    [SerializeField] private Transform _heavenCastle;
    [SerializeField] private Transform _hellCastle;

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
            Debug.LogError($"Multiple instances of {nameof(TowerFactory)} detected. Leaving the last instantiated one.");
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
        var unit = CreateUnit(false, UnitType.Type1, new() { _gridSystem.GetCell(true, _heavenCastle.transform.position) });
        unit.Transform.position = _gridSystem.GetWorldPosition(true, new(5, 0));
    }

    [Button]
    public void LaunchTestUnitToHell()
    {
        var unit = CreateUnit(true, UnitType.Type1, new() { _gridSystem.GetCell(false, _hellCastle.transform.position) });
        unit.Transform.position = _gridSystem.GetWorldPosition(false, new(5, 0));
    }

    public UnitModel CreateUnit(bool heavenFaction, UnitType type, List<Vector2Int> targets)
    {
        UnitModel unitModel = null;
        UnitView unit;
        IReadOnlyDictionary<Vector2Int, Tile> walkingGrid = _gridSystem.Map[heavenFaction];
        unit = heavenFaction ? Instantiate(_heavenUnits[type]) : Instantiate(_hellUnits[type]);

        HealthView healthView = unit.GetComponentInChildren<HealthView>(true);
        TriggerDetector triggerDetector = unit.GetComponentInChildren<TriggerDetector>(true);

        if (healthView != null)
        {
            float healthPoints = Configs.Units.GetHealthPoints(heavenFaction, type);

            HealthModel unitHealthModel = new(healthView.transform, healthPoints, healthPoints);
            healthView.AttachPresenter(new Health(healthView, unitHealthModel));
            unitModel = new(unit.transform, unitHealthModel, heavenFaction, type, targets);

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
