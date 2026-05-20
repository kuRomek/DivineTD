using UnityEngine;

public static class InputInterpreter
{
    public static PlayerInput ReadInputValues(InputActions actions)
    {
        return new PlayerInput()
        {
            PointerPosition = actions.Player.Drag.ReadValue<Vector2>(),
            Pressing = actions.Player.Click.IsPressed(),
        };
    }
}
