using System;
using kuRomek.SimpleVG;
using UnityEngine;

public class HealthModel : Model
{
    public HealthModel(Transform transform, float maxAmount, float currentAmount) : base(transform)
    {
        MaxAmount = maxAmount;
        CurrentAmount = currentAmount;
    }

    public event Action Died;
    public event Action AmountChanged;

    public float MaxAmount { get; private set; }
    public float CurrentAmount { get; private set; }
    public bool IsDead => CurrentAmount == 0f;

    public void ChangeAmount(float value)
    {
        if (IsDead)
            return;

        CurrentAmount = Mathf.Max(0f, CurrentAmount + value);
        AmountChanged?.Invoke();

        if (IsDead)
            Died?.Invoke();
    }
}