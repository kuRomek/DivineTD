using System.Collections.Generic;
using kuRomek.SimpleVG;
using UnityEngine;

public class Unit : Presenter, IUpdatable, IInteractable
{
    private const float DistanceTolerance = 0.01f;

    private readonly IReadOnlyDictionary<Vector2Int, Tile> _grid;
    private readonly GridSystem _gridSystem;

    private Vector3 _currentTargetWorld;

    public Unit(View view,
        Model model,
        IReadOnlyDictionary<Vector2Int, Tile> grid,
        GridSystem gridSystem,
        TriggerDetector triggerDetector)
        : base(view, model)
    {
        _grid = grid;
        _gridSystem = gridSystem;
        TriggerDetector = triggerDetector;

        _currentTargetWorld = _gridSystem.GetWorldPosition(Model.IsHeavenFaction == false, Model.CurrentTarget);

        TriggerDetector.EnteredTrigger += (this as IInteractable).OnTriggerEnter;
        Model.Health.Died += View.OnDestroyed;
    }

    protected new UnitModel Model => base.Model as UnitModel;
    protected new UnitView View => base.View as UnitView;

    public TriggerDetector TriggerDetector { get; }

    public void Update(float deltaTime)
    {
        Model.Transform.position = Vector3.MoveTowards(Model.Transform.position, _currentTargetWorld, deltaTime * Model.Speed);

        if (Vector3.SqrMagnitude(Model.Transform.position - _currentTargetWorld) < DistanceTolerance)
            if (Model.TrySetNextTarget())
                _currentTargetWorld = _gridSystem.GetWorldPosition(Model.IsHeavenFaction, Model.CurrentTarget);
    }

    void IInteractable.OnTriggerEnter(IDamageable damageable, IFactionRelated faction)
    {
        if (faction.IsHeavenFaction != Model.IsHeavenFaction && damageable is CastleModel)
        {
            damageable.TakeDamage(5f);
            (Model as IDamageable).Die();
        }
    }

    void IInteractable.OnTriggerExited(IDamageable damageable, IFactionRelated faction)
    {

    }
}
