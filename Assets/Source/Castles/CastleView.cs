using System;
using NaughtyAttributes;
using UnityEngine;

public class CastleView : GridObjectView
{
    [field: SerializeField] public HealthView HealthBar { get; private set; }
    [field: SerializeField] public TriggerDetector TriggerDetector { get; private set; }

    public event Action<float> TookDamage;

    public void OnDestroyed()
    {
        Destroy(gameObject, 1f);
    }

    [Button]
    public void TakeDamage()
    {
        TookDamage?.Invoke(UnityEngine.Random.Range(10f, 20f));
    }
}