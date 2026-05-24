using UnityEngine;

public static class InputInterpreter
{
    public static PlayerInput ReadInputValues(InputActions actions)
    {
        return new PlayerInput()
        {
            DeltaPosition = actions.Player.Drag.ReadValue<Vector2>(),
            Position = actions.Player.Position.ReadValue<Vector2>(),
            Pressing = actions.Player.Click.IsPressed(),
            PressedThisFrame = actions.Player.Click.WasPressedThisFrame(),
        };
    }
}
