using kuRomek.SimpleVG;
using UnityEngine.EventSystems;

public class GridObject : Presenter, IUpdatable
{
    private readonly GridSystem _gridSystem;

    public GridObject(View view, Model model, GridSystem gridSystem) : base(view, model)
    {
        _gridSystem = gridSystem;
    }

    protected new GridObjectModel Model => base.Model as GridObjectModel;

    public void Update(float deltaTime)
    {
        if (Model.IsDraft && InputController.Current.Pressing && EventSystem.current.IsPointerOverGameObject() == false)
            _gridSystem.Drag(Model);
    }
}