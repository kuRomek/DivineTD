using UnityEngine;

public class MainCameraEditor : MainCamera
{
    [Header("Camera Spots")]
    [SerializeField] private Transform _heavenCameraInitialSpot;
    [SerializeField] private Transform _hellCameraInitialSpot;

    private void Start()
    {
        CalculateConstraints();
        SwitchTargetFactionTo(GameState.CurrentPlayerFaction, true);
    }

    protected override Constraints CalculateConstraints(Faction faction)
    {
        var constraints = new Constraints();

        if (faction == Faction.Heaven)
            constraints.Offset = _heavenCameraInitialSpot.position;
        else
            constraints.Offset = _hellCameraInitialSpot.position;

        constraints.FieldWidth = new(-50f, 50f);
        constraints.FieldHeight = new(-50f, 50f);

        return constraints;
    }
}
