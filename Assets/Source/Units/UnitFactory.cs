using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using UnityEngine;

public class UnitFactory : MonoBehaviour
{
    [SerializeField] private float _cd = 1f;

    [Header("Heaven Unit Prefabs")]
    [SerializeField] private SerializedDictionary<UnitType, UnitView> _heavenUnits;

    [Header("Hell Unit Prefabs")]
    [SerializeField] private SerializedDictionary<UnitType, UnitView> _hellUnits;

    private static UnitFactory _instance;

    private float _accumTime = 0f;
    private GridSystem _gridSystem;
    private PathFindingSystem _pathFindingSystem;

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError($"Multiple instances of {nameof(UnitFactory)} detected. Leaving the last instantiated one.");
            Destroy(_instance.gameObject);
        }

        _instance = this;
    }

    private void Start()
    {
        //LaunchTestUnitToHeaven();
    }

    private void Update()
    {
        _accumTime += Time.deltaTime;

        if (_accumTime > _cd)
        {
            LaunchTestUnitToHeaven();
            _accumTime = 0f;
        }
    }

    private void Construct(GridSystem gridSystem, PathFindingSystem pathFindingSystem)
    {
        _gridSystem = gridSystem;
        _pathFindingSystem = pathFindingSystem;
    }

    [Button]
    public void LaunchTestUnitToHeaven()
    {
        var unit = CreateUnit(Faction.Hell, UnitType.Type1);

        Faction enemyFaction = 1 - unit.Faction;
        unit.Transform.position = _gridSystem.GetWorldPosition(enemyFaction, _gridSystem.Map.SpawnPoints[enemyFaction].Item1);
        unit.Go();
    }

    [Button]
    public void LaunchTestUnitToHell()
    {
        var unit = CreateUnit(Faction.Heaven, UnitType.Type1);

        Faction enemyFaction = 1 - unit.Faction;
        unit.Transform.position = _gridSystem.GetWorldPosition(enemyFaction, _gridSystem.Map.SpawnPoints[enemyFaction].Item1);
        unit.Go();
    }

    public UnitModel CreateUnit(Faction faction, UnitType type)
    {
        UnitModel unitModel = null;
        UnitView unit;
        unit = faction == Faction.Heaven ? Instantiate(_heavenUnits[type]) : Instantiate(_hellUnits[type]);

        HealthView healthView = unit.GetComponentInChildren<HealthView>(true);
        TriggerDetector triggerDetector = unit.GetComponentInChildren<TriggerDetector>(true);

        Faction enemyFaction = 1 - faction;

        if (healthView != null)
        {
            float healthPoints = Configs.Units.GetHealthPoints(faction, type);

            HealthModel unitHealthModel = new(healthView.transform, healthPoints, healthPoints);
            healthView.AttachPresenter(new Health(healthView, unitHealthModel));

            Path path = _pathFindingSystem.GetPath(1, enemyFaction);

            unitModel = new(unit.transform, unitHealthModel, faction, type,
                (position) => _gridSystem.GetCell(enemyFaction, position), path);

            unit.AttachPresenter(new Unit(unit, unitModel, _gridSystem, _pathFindingSystem, triggerDetector));

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
