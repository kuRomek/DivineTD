using System;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    public IDamageable Damageable { get; private set; }
    public IFactionRelated Faction { get; private set; }

    public event Action<IDamageable, IFactionRelated> EnteredTrigger;
    public event Action<IDamageable, IFactionRelated> ExitedTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out TriggerDetector triggerDetector))
            EnteredTrigger?.Invoke(triggerDetector.Damageable, triggerDetector.Faction);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out TriggerDetector triggerDetector))
            ExitedTrigger?.Invoke(triggerDetector.Damageable, triggerDetector.Faction);
    }

    public void AttachComponents(IDamageable damageable, IFactionRelated faction)
    {
        Damageable = damageable;
        Faction = faction;
    }
}
