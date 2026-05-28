using System;

public interface IDamageable
{
    event Action<IDamageable> Died;

    HealthModel Health { get; }
    bool IsDead => Health.IsDead;

    void TakeDamage(float amount);

    void Die() => TakeDamage(Health.CurrentAmount);
}
