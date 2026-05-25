using kuRomek.SimpleVG;

public class Tower : GridObject
{
    public Tower(View view, Model model, GridSystem gridSystem) : base(view, model, gridSystem)
    {
        Model.Destroyed += OnDestroyed;
    }

    public new TowerModel Model => base.Model as TowerModel;
    public new TowerView View => base.View as TowerView;

    private void OnDestroyed()
    {
        UnityEngine.Object.Destroy(View.gameObject);
    }
}
