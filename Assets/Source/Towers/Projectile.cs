using UnityEngine;

public class Projectile : MonoBehaviour, IPooledObject, IInteractable
{
    [SerializeField] private Rigidbody _rigidbody;

    [field: SerializeField] public TriggerDetector TriggerDetector { get; private set; }

    private float _damage;
    private float _secondsLeft;
    private float _lifeTime = 1f;

    public bool IsTargetHeavenFaction { get; private set; }

    private void OnEnable()
    {
        TriggerDetector.EnteredTrigger += (this as IInteractable).OnTriggerEnter;
        TriggerDetector.ExitedTrigger += (this as IInteractable).OnTriggerExited;
    }

    private void OnDisable()
    {
        TriggerDetector.EnteredTrigger -= (this as IInteractable).OnTriggerEnter;
        TriggerDetector.ExitedTrigger -= (this as IInteractable).OnTriggerExited;
    }

    private void Update()
    {
        _secondsLeft -= Time.deltaTime;

        if (_secondsLeft <= 0f)
        {
            Pools.Bullets.Release(this);
            _secondsLeft = _lifeTime;
        }
    }

    void IPooledObject.OnGet()
    {
        gameObject.SetActive(true);
    }

    void IPooledObject.OnRelease()
    {
        _rigidbody.ResetInertiaTensor();
        gameObject.SetActive(false);
        _secondsLeft = _lifeTime;
    }

    void IInteractable.OnTriggerEnter(IDamageable damageable, IFactionRelated faction)
    {
        if (damageable is UnitModel && faction.IsHeavenFaction == IsTargetHeavenFaction)
        {
            damageable.TakeDamage(_damage);
            Pools.Bullets.Release(this);
        }
    }

    void IInteractable.OnTriggerExited(IDamageable damageable, IFactionRelated faction)
    {
    }

    public Projectile Launch(Vector3 position, Vector3 target, float damage, bool isTargetHeavenFaction)
    {
        Vector3 direction = target - position;

        transform.position = position;
        transform.LookAt(target, Vector3.up);
        _rigidbody.linearVelocity = direction.normalized * 10f;
        _damage = damage;
        IsTargetHeavenFaction = isTargetHeavenFaction;

        return this;
    }
}