using kuRomek.SimpleVG;
using UnityEngine;

public class MainCamera : Model, IUpdatable
{
    public MainCamera(Transform transform) : base(transform)
    {
    }

    public void Update(float deltaTime)
    {
        if (InputController.Current.Pressing)
            Move(deltaTime);
    }

    private void Move(float deltaTime)
    {
        Vector3 delta = new Vector3(InputController.Current.PointerPosition.x, 0f, InputController.Current.PointerPosition.y);
        Transform.position -= delta * deltaTime;
    }
}