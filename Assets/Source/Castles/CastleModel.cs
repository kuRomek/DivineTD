using System;
using UnityEngine;

[Serializable]
public class CastleModel : GridObjectModel, IDamageable, IFactionRelated
{
    public CastleModel(Transform transform, HealthModel healthModel, Faction faction, bool isDraft)
        : base(transform, faction, isDraft)
    {
        Health = healthModel;

        Health.Died += () => Died?.Invoke(this);
    }

    public event Action<IDamageable> Died;

    public HealthModel Health { get; }

    public void TakeDamage(float amount)
    {
        Health.ChangeAmount(-amount);
    }
}
