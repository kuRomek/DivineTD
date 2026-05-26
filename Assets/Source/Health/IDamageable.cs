public interface IDamageable
{
    public HealthModel Health { get; }

    public void TakeDamage(float amount);

    public void Die()
        => TakeDamage(Health.CurrentAmount);
}
