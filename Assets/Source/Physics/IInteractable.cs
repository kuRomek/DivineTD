public interface IInteractable
{
    public TriggerDetector TriggerDetector { get; }

    public void OnTriggerEnter(IDamageable damageable, IFactionRelated faction);

    public void OnTriggerExited(IDamageable damageable, IFactionRelated faction);
}