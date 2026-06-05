using System;
using System.Collections.Generic;
using kuRomek.SimpleVG;
using UnityEngine;

public class UnitModel : Model, IDamageable, IFactionRelated
{
    private readonly IReadOnlyList<Vector2Int> _targets;
    private int _currentTargetIndex;

    public UnitModel(
        Transform transform,
        HealthModel health,
        Faction faction,
        UnitType type,
        IReadOnlyList<Vector2Int> targets)
        : base(transform)
    {
        Health = health;
        Faction = faction;
        Type = type;
        _targets = targets;
        _currentTargetIndex = 0;

        Health.Died += () => Died?.Invoke(this);
    }

    public event Action<IDamageable> Died;

    public HealthModel Health { get; }
    public Faction Faction { get; }
    public UnitType Type { get; }
    public Vector2Int CurrentTarget => _targets[_currentTargetIndex];
    public float Speed => Configs.Units.GetSpeed(Faction, Type);

    void IDamageable.TakeDamage(float amount)
    {
        Health.ChangeAmount(-amount);
    }

    public bool TrySetNextTarget()
    {
        return ++_currentTargetIndex < _targets.Count;
    }
}