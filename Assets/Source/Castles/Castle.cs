using kuRomek.SimpleVG;

public class Castle : Presenter, IInteractable
{
    public Castle(View view, Model model) : base(view, model)
    {
        Model.Health.Died += View.OnDestroyed;
        View.TookDamage += Model.TakeDamage;
    }

    protected new CastleModel Model => base.Model as CastleModel;
    protected new CastleView View => base.View as CastleView;

    TriggerDetector IInteractable.TriggerDetector => null;

    void IInteractable.OnTriggerEnter(IDamageable damageable, IFactionRelated faction)
    {
    }

    void IInteractable.OnTriggerExited(IDamageable damageable, IFactionRelated faction)
    {
    }
}
