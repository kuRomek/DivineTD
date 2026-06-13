using System.Collections.Generic;
using kuRomek.SimpleVG;
using UnityEngine;

public class Unit : Presenter, IUpdatable, IInteractable
{
    private const float DistanceTolerance = 0.01f;

    private readonly IReadOnlyDictionary<Vector2Int, Tile> _grid;
    private readonly GridSystem _gridSystem;

    private Vector3 _currentTarget;
    private bool _stalled;

    public Unit(View view, Model model, GridSystem gridSystem, TriggerDetector triggerDetector) : base(view, model)
    {
        _gridSystem = gridSystem;
        _grid = _gridSystem.Map[1 - Model.Faction];
        TriggerDetector = triggerDetector;

        _currentTarget = _gridSystem.GetWorldPosition(1 - Model.Faction, Model.CurrentTarget);

        TriggerDetector.EnteredTrigger += (this as IInteractable).OnTriggerEnter;
        Model.Health.Died += View.OnDestroyed;
    }

    protected new UnitModel Model => base.Model as UnitModel;
    protected new UnitView View => base.View as UnitView;

    public TriggerDetector TriggerDetector { get; }

    public void Update(float deltaTime)
    {
        MoveOnPath(deltaTime);
    }

    void IInteractable.OnTriggerEnter(IDamageable damageable, IFactionRelated faction)
    {
        if (faction.Faction != Model.Faction && damageable is CastleModel)
        {
            damageable.TakeDamage(5f);
            (Model as IDamageable).Die();
        }
    }

    void IInteractable.OnTriggerExited(IDamageable damageable, IFactionRelated faction)
    {

    }

    public void MoveOnPath(float deltaTime)
    {
        if (_stalled)
            return;

        Model.Transform.position = Vector3.MoveTowards(Model.Transform.position, _currentTarget, deltaTime * Model.Speed);

        if (Vector3.SqrMagnitude(Model.Transform.position - _currentTarget) < DistanceTolerance)
        {
            if (Model.TrySetNextTarget())
            {
                _stalled = true;
                return;
            }

            _currentTarget = _gridSystem.GetWorldPosition(1 - Model.Faction, Model.CurrentTarget);
        }
    }
}
