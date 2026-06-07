using System;
using System.Linq;
using kuRomek.SimpleVG;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainCamera : MonoBehaviour
{
    [Range(0f, 1f), SerializeField] private float _smoothness;

    [Header("Camera Spots")]
    [SerializeField] private Transform _heavenCameraInitialSpot;
    [SerializeField] private Transform _hellCameraInitialSpot;

    [field: SerializeField] public Camera Camera { get; private set; }

    private Vector3 _accumDelta;
    private Vector3 _positionOnClicked;
    private Vector3 _targetPosition;
    private GridSystem _gridSystem;
    private LevelsSystem _levelsSystem;
    private Constraints _constraints;
    private Constraints _heavenConstraints;
    private Constraints _hellConstraints;

    public bool IsDragging { get; private set; }
    public bool IsControlBlocked { get; private set; }

    private void Construct(GridSystem gridSystem, LevelsSystem levelsSystem)
    {
        _gridSystem = gridSystem;
        _levelsSystem = levelsSystem;

        _levelsSystem.LevelStarted += OnLevelStarted;
    }

    private void Update()
    {
        if (IsControlBlocked)
            return;

        if (InputController.Current.PressedThisFrame)
        {
            IsDragging = EventSystem.current.IsPointerOverGameObject() == false;

            if (IsDragging)
            {
                _accumDelta = default;
                _positionOnClicked = transform.position;
            }
        }

        if (IsDragging)
            IsDragging = InputController.Current.Pressing;

        if (IsDragging)
            CalculateTargetPosition();

        transform.position = Vector3.Lerp(transform.position, _targetPosition, _smoothness);
    }

    public void SwitchTargetFactionTo(Faction faction)
    {
        _constraints = faction == Faction.Heaven ? _heavenConstraints : _hellConstraints;
        CalculateTargetPosition();
    }

    public void ToggleControlBlock(bool isBlocked)
    {
        IsDragging = false;
        _accumDelta = default;
        _targetPosition = transform.position;
        IsControlBlocked = isBlocked;
    }

    public void CalculateTargetPosition()
    {
        Vector3 delta = Configs.MainCamera.Sensitivity * Time.deltaTime *
            new Vector3(InputController.Current.DeltaPosition.x, 0f, InputController.Current.DeltaPosition.y);

        _accumDelta += delta;

        _targetPosition = new Vector3(
            Mathf.Clamp(
                _positionOnClicked.x - _accumDelta.x,
                _constraints.FieldWidth.x,
                _constraints.FieldWidth.y),
            _constraints.Offset.y,
            Mathf.Clamp(
                _positionOnClicked.z - _accumDelta.z,
                _constraints.FieldHeight.x + _constraints.Offset.z,
                _constraints.FieldHeight.y + _constraints.Offset.z));
    }

    private void OnLevelStarted()
    {
        _heavenConstraints = CalculateConstraints(Faction.Heaven);
        _hellConstraints = CalculateConstraints(Faction.Hell);

        SwitchTargetFactionTo(GameState.CurrentPlayerFaction);
    }

    private Constraints CalculateConstraints(Faction faction)
    {
        var constraints = new Constraints();

        if (faction == Faction.Heaven)
            constraints.Offset = _heavenCameraInitialSpot.position;
        else
            constraints.Offset = _hellCameraInitialSpot.position;

        var width = new Vector2Int(
            _gridSystem.Map[faction].Min(cell => cell.Key.x),
            _gridSystem.Map[faction].Max(cell => cell.Key.x));

        var height = new Vector2Int(
            _gridSystem.Map[faction].Min(cell => cell.Key.y),
            _gridSystem.Map[faction].Max(cell => cell.Key.y));

        // ! This assumes that the grid is a square. (overall some bullshit solution, gotta refactor)
        Vector3 convertedWidth = _gridSystem.GetWorldPosition(faction, width);
        Vector3 convertedHeight = _gridSystem.GetWorldPosition(faction, height);
        constraints.FieldWidth = new(-convertedWidth.x, -convertedWidth.z);
        constraints.FieldHeight = new(convertedHeight.z, convertedHeight.x);

        return constraints;
    }

    private struct Constraints
    {
        public Vector2 FieldWidth;
        public Vector2 FieldHeight;
        public Vector3 Offset;
    }
}
