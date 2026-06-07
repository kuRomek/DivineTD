using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WidgetCanvas : MonoBehaviour
{
    private const float ShiftAnimationDuration = 0.2f;

    [SerializeField] private RectTransform _towerButtons;
    [SerializeField] private RectTransform _buildingButtons;
    [SerializeField] private Button _switchFactionButton;
    [SerializeField] private RectTransform _switchButtonSprite;

    [Space(3f)]
    [SerializeField] private Button _acceptBuildingButton;
    [SerializeField] private Button _cancelBuildingButton;

    private BuildingSystem _buildingSystem;
    private LevelsSystem _levelsSystem;
    private MainCamera _mainCamera;

    private Faction _currentTargetFaction;

    private Tween _switchButtonRotation;

    public void Construct(BuildingSystem buildingSystem, LevelsSystem levelsSystem, MainCamera mainCamera)
    {
        _buildingSystem = buildingSystem;
        _levelsSystem = levelsSystem;
        _mainCamera = mainCamera;

        _levelsSystem.LevelStarted += OnLevelStarted;
    }

    private void Start()
    {
        _acceptBuildingButton.onClick.AddListener(OnAcceptBuildingButtonClicked);
        _cancelBuildingButton.onClick.AddListener(OnCancelBuildingButtonClicked);
        _switchFactionButton.onClick.AddListener(SwitchCameraTarget);
    }

    public void ToggleBuildingMode(bool isActive)
    {
        _towerButtons.gameObject.SetActive(isActive == false);
        _buildingButtons.gameObject.SetActive(isActive);
    }

    private void OnAcceptBuildingButtonClicked()
    {
        if (_buildingSystem.TryBuildTower(GameState.CurrentPlayerFaction))
            ToggleBuildingMode(false);
    }

    private void OnCancelBuildingButtonClicked()
    {
        _buildingSystem.CancelBuilding();
        ToggleBuildingMode(false);
    }

    private void SwitchCameraTarget()
    {
        _switchButtonRotation?.Complete();

        _switchButtonRotation = _switchButtonSprite.DORotate(Vector3.forward * 180f, ShiftAnimationDuration).
            SetEase(Ease.OutBack).SetRelative();

        _mainCamera.SwitchTargetFactionTo(1 - _currentTargetFaction);
        _currentTargetFaction = 1 - _currentTargetFaction;
    }

    private void OnLevelStarted()
    {
        _currentTargetFaction = GameState.CurrentPlayerFaction;

        _switchButtonSprite.rotation = Quaternion.Euler(0f, 0f, _currentTargetFaction == Faction.Heaven ? 0f : 180f);
    }
}
