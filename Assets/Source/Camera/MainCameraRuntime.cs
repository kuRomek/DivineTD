using System.Linq;
using UnityEngine;

public class MainCameraRuntime : MainCamera
{
    [Header("Camera Spots")]
    [SerializeField] private Transform _heavenCameraInitialSpot;
    [SerializeField] private Transform _hellCameraInitialSpot;

    private GridSystem _gridSystem;
    private LevelsSystem _levelsSystem;

    private void Construct(GridSystem gridSystem, LevelsSystem levelsSystem)
    {
        _gridSystem = gridSystem;
        _levelsSystem = levelsSystem;

        _levelsSystem.LevelStarted += OnLevelStarted;
    }

    private void OnLevelStarted()
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

        if (_gridSystem.Map[faction].Count == 0)
        {
            constraints.FieldWidth = default;
            constraints.FieldHeight = default;
            return constraints;
        }

        var width = new Vector2Int(
            _gridSystem.Map[faction].Min(cell => cell.Key.x),
            _gridSystem.Map[faction].Max(cell => cell.Key.x));

        var height = new Vector2Int(
            _gridSystem.Map[faction].Min(cell => cell.Key.y),
            _gridSystem.Map[faction].Max(cell => cell.Key.y));

        var lowestTile = new Vector2Int(width.x, height.x);
        var highestTile = new Vector2Int(width.y, height.y);

        Vector3 convertedLowestTile = _gridSystem.GetWorldPosition(faction, lowestTile);
        Vector3 convertedHighestTile = _gridSystem.GetWorldPosition(faction, highestTile);

        constraints.FieldWidth = new(convertedLowestTile.x, convertedHighestTile.x);
        constraints.FieldHeight = new(convertedHighestTile.z, convertedLowestTile.z);

        return constraints;
    }
}
