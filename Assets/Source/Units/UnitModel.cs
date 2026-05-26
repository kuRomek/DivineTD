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
        bool isHeavenFaction,
        UnitType type,
        IReadOnlyList<Vector2Int> targets)
        : base(transform)
    {
        Health = health;
        IsHeavenFaction = isHeavenFaction;
        Type = type;
        _targets = targets;
        _currentTargetIndex = 0;
    }

    public HealthModel Health { get; }

    public bool IsHeavenFaction { get; }

    public UnitType Type { get; }

    public Vector2Int CurrentTarget => _targets[_currentTargetIndex];

    public float Speed => Configs.Units.GetSpeed(IsHeavenFaction, Type);

    void IDamageable.TakeDamage(float amount)
    {
        Health.ChangeAmount(-amount);
    }

    public bool TrySetNextTarget()
    {
        return ++_currentTargetIndex < _targets.Count;
    }
}