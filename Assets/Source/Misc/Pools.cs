using UnityEngine;

public class Pools : MonoBehaviour
{
    [Header("Projectiles")]
    [SerializeField] private Projectile _bullet;

    private Pool<Projectile> _bullets;

    public static Pool<Projectile> Bullets => _instance._bullets;

    private static Pools _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError($"Multiple instances of {nameof(TowerFactory)} detected. Leaving the last instantiated one.");
            Destroy(_instance.gameObject);
        }

        _instance = this;

        InitializePools();
    }

    private void InitializePools()
    {
        _bullets = new Pool<Projectile>(_bullet);
    }
}