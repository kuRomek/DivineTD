using System;
using System.Collections.Generic;
using kuRomek.SimpleVG;
using UnityEngine;

public class UnitModel : Model, IDamageable, IFactionRelated
{
    private readonly IEnumerator<Vector2Int> _checkpoints;
    private readonly Func<Vector3, Vector2Int> _getCellPosition;

    public UnitModel(Transform transform, HealthModel health, Faction faction, UnitType type,
        IEnumerable<Vector2Int> checkpoints, Func<Vector3, Vector2Int> getWorldPosition, Path path)
        : base(transform)
    {
        Health = health;
        Faction = faction;
        Type = type;
        _getCellPosition = getWorldPosition;
        _checkpoints = checkpoints.GetEnumerator();
        Path = path;

        Health.Died += () => Died?.Invoke(this);
    }

    public event Action<IDamageable> Died;
    public event Action Launched;

    public HealthModel Health { get; }
    public Faction Faction { get; }
    public UnitType Type { get; }
    public Path Path { get; }

    public Vector2Int CurrentTarget => Path.CurrentTarget;
    public float Speed => Configs.Units.GetSpeed(Faction, Type);

    void IDamageable.TakeDamage(float amount)
    {
        Health.ChangeAmount(-amount);
    }

    public bool TrySetNextTarget()
    {
        bool success = Path.TrySetNextTarget();

        if (success == false && _checkpoints.MoveNext())
        {
            Path.Calculate(_getCellPosition(Transform.position), _checkpoints.Current);
            success = TrySetNextTarget();
        }

        return success;
    }

    public void Go()
    {
        TrySetNextTarget();
        Launched?.Invoke();
    }
}