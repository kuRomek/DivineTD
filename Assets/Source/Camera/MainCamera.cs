using kuRomek.SimpleVG;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainCamera : Model, IUpdatable
{
    private bool _isDragging = false;

    public MainCamera(Transform transform) : base(transform)
    {
    }

    public void Update(float deltaTime)
    {
        if (InputController.Current.PressedThisFrame)
            _isDragging = EventSystem.current.IsPointerOverGameObject() == false;
        else if (_isDragging)
            _isDragging = InputController.Current.Pressing;

        if (_isDragging)
            Move(deltaTime);
    }

    private void Move(float deltaTime)
    {
        Vector3 delta = new Vector3(InputController.Current.PointerPosition.x, 0f, InputController.Current.PointerPosition.y);
        Transform.position -= delta * deltaTime;
    }
}