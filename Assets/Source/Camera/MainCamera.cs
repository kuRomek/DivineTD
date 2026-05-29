using kuRomek.SimpleVG;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private float _fieldWidthMax = 10;
    [SerializeField] private float _fieldWidthMin = 10;
    [SerializeField] private float _fieldHeightMax = 20;
    [SerializeField] private float _fieldHeightMin = 20;
    [Range(0f, 1f), SerializeField] private float _smoothness;

    [field: SerializeField] public Camera Camera { get; private set; }

    private Vector3 _offset;
    private Vector3 _accumDelta;
    private Vector3 _positionOnClicked;
    private Vector3 _targetPosition;

    public bool IsDragging { get; private set; }
    public bool IsControlBlocked { get; private set; }

    private void Awake()
    {
        _offset = transform.position;
        _targetPosition = _offset;

        _fieldWidthMin += _offset.x;
        _fieldWidthMax += _offset.x;
        _fieldHeightMin += _offset.z;
        _fieldHeightMax += _offset.z;
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

        if (GameState.IsCurrentFactionHeaven)
            _accumDelta += delta;
        else
            _accumDelta -= delta;

        _targetPosition = new Vector3(
            Mathf.Clamp(_positionOnClicked.x - _accumDelta.x, _fieldWidthMin, _fieldWidthMax),
            transform.position.y,
            Mathf.Clamp(_positionOnClicked.z - _accumDelta.z, _fieldHeightMin, _fieldHeightMax));
    }
}
