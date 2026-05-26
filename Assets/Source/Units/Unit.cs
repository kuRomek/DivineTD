using System;
using System.Collections.Generic;
using kuRomek.SimpleVG;
using UnityEngine;

public class Unit : Presenter, IUpdatable
{
    private const float DistanceTolerance = 0.01f;

    private readonly IReadOnlyDictionary<Vector2Int, GridObjectModel> _grid;
    private readonly GridSystem _gridSystem;
    private readonly TriggerDetector _triggerDetector;

    private Vector3 _currentTargetWorld;

    public Unit(View view,
        Model model,
        IReadOnlyDictionary<Vector2Int, GridObjectModel> grid,
        GridSystem gridSystem,
        TriggerDetector triggerDetector)
        : base(view, model)
    {
        _grid = grid;
        _gridSystem = gridSystem;
        _triggerDetector = triggerDetector;

        _currentTargetWorld = _gridSystem.GetWorldPosition(Model.IsHeavenFaction == false, Model.CurrentTarget);

        _triggerDetector.EnteredTrigger += OnTriggerEnter;
        Model.Health.Died += View.OnDestroyed;
    }

    protected new UnitModel Model => base.Model as UnitModel;
    protected new UnitView View => base.View as UnitView;

    public void Update(float deltaTime)
    {
        Model.Transform.position = Vector3.MoveTowards(Model.Transform.position, _currentTargetWorld, deltaTime * Model.Speed);

        if (Vector3.SqrMagnitude(Model.Transform.position - _currentTargetWorld) < DistanceTolerance)
            if (Model.TrySetNextTarget())
                _currentTargetWorld = _gridSystem.GetWorldPosition(Model.IsHeavenFaction, Model.CurrentTarget);
    }

    private void OnTriggerEnter(IDamageable damageable, IFactionRelated faction)
    {
        if (faction.IsHeavenFaction != Model.IsHeavenFaction)
        {
            damageable.TakeDamage(5f);
            (Model as IDamageable).Die();
        }
    }
}
