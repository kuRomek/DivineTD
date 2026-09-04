using System;
using System.Collections.Generic;
using kuRomek.SimpleVG;
using UnityEngine;

public class UnitModel : Model, IDamageable, IFactionRelated
{
    private readonly Func<Vector3, Vector2Int> _getCellPosition;

    private Path _path;
    private IEnumerator<Vector2Int> _pathEnumerator;
    private bool _recalculatedPathBefore;

    public UnitModel(Transform transform, HealthModel health, Faction faction, UnitType type,
        Func<Vector3, Vector2Int> getWorldPosition, Path path)
        : base(transform)
    {
        Health = health;
        Faction = faction;
        Type = type;
        _getCellPosition = getWorldPosition;
        SetPath(path);

        Health.Died += () => Died?.Invoke(this);
    }

    public event Action<IDamageable> Died;
    public event Action Launched;

    public HealthModel Health { get; }
    public Faction Faction { get; }
    public UnitType Type { get; }

    public Vector2Int CurrentTarget { get; private set; }
    public int CurrentCheckpointNumber => _path.CheckpointNumber;
    public float Speed => Configs.Units.GetSpeed(Faction, Type);

    void IDamageable.TakeDamage(float amount)
    {
        Health.ChangeAmount(-amount);
    }

    public void SetPath(Path path)
    {
        _path = path;
        _pathEnumerator = _path.GetEnumerator();
    }

    public void ReCalculatePathToCheckpoint()
    {
        if (_recalculatedPathBefore)
            _path.Calculate(_getCellPosition(Transform.position), _path.Checkpoint, _path.CheckpointNumber);
        else
            _path = new(_path.Field, _getCellPosition(Transform.position), _path.Checkpoint, _path.CheckpointNumber);

        _pathEnumerator = _path.GetEnumerator();
        _pathEnumerator.Reset();

        _recalculatedPathBefore = true;
    }

    public bool TrySetNextTarget()
    {
        if (_pathEnumerator == null)
            return false;

        bool success = _pathEnumerator.MoveNext();

        if (success)
            CurrentTarget = _pathEnumerator.Current;

        return success;
    }

    public void Go()
    {
        TrySetNextTarget();
        Launched?.Invoke();
    }

    public void Die()
    {
        Health.ChangeAmount(-Health.CurrentAmount);
    }
}