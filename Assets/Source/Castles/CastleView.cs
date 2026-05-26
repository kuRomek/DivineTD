using System;
using kuRomek.SimpleVG;
using NaughtyAttributes;

public class CastleView : View
{
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