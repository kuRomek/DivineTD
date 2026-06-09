using kuRomek.SimpleVG;

public class Castle : GridObject, IInteractable
{
    public Castle(View view, Model model, GridSystem gridSystem)
        : base(view, model, gridSystem)
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
