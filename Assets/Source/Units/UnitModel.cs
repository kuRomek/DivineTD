using System;
using System.Collections.Generic;
using kuRomek.SimpleVG;
using UnityEngine;

public class UnitModel : Model, IDamageable, IFactionRelated
{
    private readonly IEnumerator<Vector2Int> _checkpoints;

    public UnitModel(Transform transform, HealthModel health, Faction faction, UnitType type,
        IEnumerable<Vector2Int> checkpoints)
        : base(transform)
    {
        Health = health;
        Faction = faction;
        Type = type;
        _checkpoints = checkpoints.GetEnumerator();
        Path = new();
        TrySetNextCheckpoint();

        Health.Died += () => Died?.Invoke(this);
    }

    public event Action<IDamageable> Died;

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

        if (success == false)
            success = TrySetNextCheckpoint();

        return success;
    }

    private bool TrySetNextCheckpoint()
    {
        bool success = _checkpoints.MoveNext();

        if (success)
            Path.Calculate(_checkpoints.Current);

        return success;
    }
}