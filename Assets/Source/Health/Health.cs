using kuRomek.SimpleVG;

public class Health : Presenter
{
    public Health(View view, Model model) : base(view, model)
    {
        Model.AmountChanged += OnAmountChanged;
        Model.Died += OnDied;

        OnAmountChanged();
    }

    protected new HealthModel Model => base.Model as HealthModel;
    protected new HealthView View => base.View as HealthView;

    private void OnAmountChanged()
    {
        View.Display(Model.CurrentAmount, Model.MaxAmount);
    }

    private void OnDied()
    {
        View.Hide();
    }
}
