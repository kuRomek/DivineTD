using System;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    [SerializeField] private Collider _collider;

    public event Action<IDamageable, IFactionRelated> EnteredTrigger;
    public event Action<IDamageable, IFactionRelated> ExitedTrigger;

    public IDamageable Damageable { get; private set; }
    public IFactionRelated Faction { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out TriggerDetector triggerDetector) &&
            triggerDetector.Damageable != null && triggerDetector.Faction != null)
        {
            EnteredTrigger?.Invoke(triggerDetector.Damageable, triggerDetector.Faction);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out TriggerDetector triggerDetector) &&
            triggerDetector.Damageable != null && triggerDetector.Faction != null)
        {
            ExitedTrigger?.Invoke(triggerDetector.Damageable, triggerDetector.Faction);
        }
    }

    public void Toggle(bool isActive)
    {
        _collider.enabled = isActive;
    }

    public void SetTriggerRadius(float radius)
    {
        if (_collider is SphereCollider sphereCollider)
            sphereCollider.radius = radius;
    }

    public void AttachComponents(IDamageable damageable, IFactionRelated faction)
    {
        Damageable = damageable;
        Faction = faction;
    }
}
