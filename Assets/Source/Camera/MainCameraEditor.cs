using UnityEngine;

public class MainCameraEditor : MainCamera
{
    private GridSystem _gridSystem;

    private void Construct(GridSystem gridSystem)
    {
        _gridSystem = gridSystem;
    }

    private void Start()
    {
        CalculateConstraints();
        SwitchTargetFactionTo(GameState.CurrentPlayerFaction, true);
    }

    protected override Constraints CalculateConstraints(Faction faction)
    {
        return new Constraints
        {
            Offset = _gridSystem.Map.CameraSpots[faction].position,
            FieldWidth = new(-50f, 50f),
            FieldHeight = new(-50f, 50f)
        }; ;
    }
}
