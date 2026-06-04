using kuRomek.SimpleVG;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridObject : Presenter, IUpdatable
{
    private readonly GridSystem _gridSystem;

    public GridObject(View view, Model model, GridSystem gridSystem) : base(view, model)
    {
        _gridSystem = gridSystem;
        Model.ToggledDrafting += View.ToggleBuildingIndicator;
        Model.Moved += OnMoved;
        Model.Destroyed += OnDestroyed;
    }

    protected new GridObjectModel Model => base.Model as GridObjectModel;
    protected new GridObjectView View => base.View as GridObjectView;

    public void Update(float deltaTime)
    {
        if (Model.IsDraft && InputController.Current.Pressing && EventSystem.current.IsPointerOverGameObject() == false)
            _gridSystem.Drag(Model);

        OnUpdate(deltaTime);
    }

    protected virtual void OnUpdate(float deltaTime)
    {
    }

    private void OnMoved(bool differentPosition)
    {
        if (differentPosition)
            View.SetBuildingIndicatorColor(_gridSystem.CheckAvailability(Model.Transform.position, Model.IsHeavenFaction));
    }

    private void OnDestroyed()
    {
        Object.Destroy(View.gameObject);
    }
}