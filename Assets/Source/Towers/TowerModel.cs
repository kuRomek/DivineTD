using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerModel : GridObjectModel
{
    private readonly List<IDamageable> _attackTargets = new();

    public TowerModel(Transform transform, bool isDraft, bool heavenFaction, TowerType type)
        : base(transform, heavenFaction, isDraft)
    {
        Type = type;
    }

    public TowerType Type { get; }
    public TargetPriorityType TargetPriorityType { get; private set; }
    public IDamageable CurrentAttackTarget { get; private set; }
    public IReadOnlyList<IDamageable> AttackTargets => _attackTargets;
    public TowerData Params => Configs.Buildings.GetTowerParams(IsHeavenFaction, Type);

    public void SetTargetPriority(TargetPriorityType type)
    {
        TargetPriorityType = type;
    }

    public void EnqueueTarget(IDamageable damageable)
    {
        _attackTargets.Add(damageable);
        ChangeCurrentTarget();
    }

    public void DequeueTarget(IDamageable damageable)
    {
        _attackTargets.Remove(damageable);
        ChangeCurrentTarget();
    }

    private void ChangeCurrentTarget()
    {
        if (_attackTargets.Count == 0)
        {
            CurrentAttackTarget = null;
            return;
        }

        switch (TargetPriorityType)
        {
            case TargetPriorityType.Queue:
                CurrentAttackTarget = _attackTargets[0];
                return;

            case TargetPriorityType.Stack:
                CurrentAttackTarget = _attackTargets[_attackTargets.Count - 1];
                return;
        }
    }
}
