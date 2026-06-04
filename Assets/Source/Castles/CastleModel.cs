using System;
using UnityEngine;

[Serializable]
public class CastleModel : GridObjectModel, IDamageable, IFactionRelated
{
    public CastleModel(Transform transform, HealthModel healthModel, bool heavenFaction, bool isDraft)
        : base(transform, heavenFaction, isDraft)
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
