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
        var constraints = new Constraints();

        if (faction == Faction.Heaven)
            constraints.Offset = _gridSystem.Map.HeavenCameraSpot.position;
        else
            constraints.Offset = _gridSystem.Map.HellCameraSpot.position;

        constraints.FieldWidth = new(-50f, 50f);
        constraints.FieldHeight = new(-50f, 50f);

        return constraints;
    }
}
