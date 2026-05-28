using System;
using kuRomek.SimpleVG;
using UnityEngine;

public class CastleModel : Model, IDamageable, IFactionRelated
{
    public CastleModel(Transform transform, HealthModel healthModel, bool isHeavenFaction) : base(transform)
    {
        Health = healthModel;
        IsHeavenFaction = isHeavenFaction;

        Health.Died += () => Died?.Invoke(this);
    }

    public event Action<IDamageable> Died;

    public bool IsHeavenFaction { get; }
    public HealthModel Health { get; }

    public void TakeDamage(float amount)
    {
        Health.ChangeAmount(-amount);
    }
}
