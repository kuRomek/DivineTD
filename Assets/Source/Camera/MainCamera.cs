using System;
using DG.Tweening;
using kuRomek.SimpleVG;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class MainCamera : MonoBehaviour
{
    private const float CameraLiftingDuration = 0.3f;

    [Range(0f, 1f), SerializeField] private float _smoothness;

    [field: SerializeField] public Camera Camera { get; private set; }

    private Vector3 _accumDelta;
    private Vector3 _positionOnClicked;
    private Vector3 _targetPosition;
    private float _currentCameraHeight;
    private Tween _cameraLifting;
    private bool _instantMove;

    public bool IsDragging { get; private set; }
    public Faction TargetFaction { get; private set; }
    public bool IsControlBlocked { get; private set; }

    protected Constraints CurrentConstraints { get; private set; }
    protected Constraints HeavenConstraints { get; private set; }
    protected Constraints HellConstraints { get; private set; }
    protected float HeavenHellDistance { get; private set; }

    private void Awake()
    {
        _positionOnClicked = transform.position;
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

        Vector3 targetPosition = Vector3.Lerp(transform.position, _targetPosition, _smoothness);

        if (_instantMove)
            transform.position = _targetPosition;
        else
            transform.position = new Vector3(targetPosition.x, _currentCameraHeight, targetPosition.z);

        _instantMove = false;

        float deepnessRatio = ((_currentCameraHeight - HellConstraints.Offset.y) / HeavenHellDistance);
        RenderSettings.skybox.SetFloat("_Exposure", deepnessRatio);
    }

    public void SwitchTargetFactionTo(Faction faction, bool instant)
    {
        if (IsControlBlocked)
            return;

        TargetFaction = faction;
        CurrentConstraints = faction == Faction.Heaven ? HeavenConstraints : HellConstraints;

        _cameraLifting?.Kill();

        _instantMove = instant;

        if (_instantMove)
        {
            _currentCameraHeight = CurrentConstraints.Offset.y;
        }
        else
        {
            _cameraLifting = DOVirtual.Float(_currentCameraHeight, CurrentConstraints.Offset.y, CameraLiftingDuration,
                (value) => _currentCameraHeight = value);
        }

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
                CurrentConstraints.FieldWidth.x,
                CurrentConstraints.FieldWidth.y),
            CurrentConstraints.Offset.y,
            Mathf.Clamp(
                _positionOnClicked.z - _accumDelta.z,
                CurrentConstraints.FieldHeight.x + CurrentConstraints.Offset.z,
                CurrentConstraints.FieldHeight.y + CurrentConstraints.Offset.z));
    }

    public void CalculateConstraints()
    {
        HeavenConstraints = CalculateConstraints(Faction.Heaven);
        HellConstraints = CalculateConstraints(Faction.Hell);
        HeavenHellDistance = HeavenConstraints.Offset.y - HellConstraints.Offset.y;
    }

    protected abstract Constraints CalculateConstraints(Faction faction);

    protected struct Constraints
    {
        public Vector2 FieldWidth;
        public Vector2 FieldHeight;
        public Vector3 Offset;
    }
}