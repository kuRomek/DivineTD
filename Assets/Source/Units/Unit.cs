using kuRomek.SimpleVG;
using UnityEngine;

public class Unit : Presenter, IUpdatable, IInteractable
{
    private const float DistanceTolerance = 0.01f;

    private readonly GridSystem _gridSystem;
    private readonly LevelsSystem _levelsSystem;
    private readonly PathFindingSystem _pathFindingSystem;
    private readonly Faction _enemyFaction;

    private Vector3 _currentTarget;
    private bool _stalled = true;

    public Unit(
        View view,
        Model model,
        GridSystem gridSystem,
        LevelsSystem levelsSystem,
        PathFindingSystem pathFindingSystem,
        TriggerDetector triggerDetector)
        : base(view, model)
    {
        _enemyFaction = 1 - Model.Faction;

        _gridSystem = gridSystem;
        _levelsSystem = levelsSystem;
        _pathFindingSystem = pathFindingSystem;
        TriggerDetector = triggerDetector;

        _currentTarget = _gridSystem.GetWorldPosition(_enemyFaction, Model.CurrentTarget);

        gridSystem.ObjectPlaced += OnMapChanged;
        levelsSystem.LevelStarted += Model.Die;
        TriggerDetector.EnteredTrigger += (this as IInteractable).OnTriggerEnter;
        Model.Health.Died += OnDied;
        Model.Launched += StartWalking;
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
            if (Model.TrySetNextTarget() == false)
            {
                Path path = _pathFindingSystem.GetPath(Model.CurrentCheckpointNumber + 1, _enemyFaction);

                if (path != null)
                {
                    Model.SetPath(path);
                    Model.TrySetNextTarget();
                }
                else
                {
                    _stalled = true;
                    return;
                }
            }

            _currentTarget = _gridSystem.GetWorldPosition(_enemyFaction, Model.CurrentTarget);
        }
    }

    private void OnMapChanged(Faction faction)
    {
        if (faction == _enemyFaction)
        {
            Model.ReCalculatePathToCheckpoint();
            Model.TrySetNextTarget();
            _currentTarget = _gridSystem.GetWorldPosition(_enemyFaction, Model.CurrentTarget);
        }
    }

    private void StartWalking()
    {
        _stalled = false;
        _currentTarget = _gridSystem.GetWorldPosition(_enemyFaction, Model.CurrentTarget);
    }

    private void OnDied()
    {
        _gridSystem.ObjectPlaced -= OnMapChanged;
        TriggerDetector.EnteredTrigger -= (this as IInteractable).OnTriggerEnter;
        Model.Health.Died -= OnDied;
        Model.Launched -= StartWalking;

        View.OnDestroyed();
    }
}
