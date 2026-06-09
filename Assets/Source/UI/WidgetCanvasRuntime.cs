using UnityEngine;
using UnityEngine.UI;

public class WidgetCanvasRuntime : WidgetCanvas
{
    [SerializeField] private RectTransform _towerButtons;
    [SerializeField] private RectTransform _buildingButtons;

    [Space(3f)]
    [SerializeField] private Button _acceptBuildingButton;
    [SerializeField] private Button _cancelBuildingButton;

    private BuildingSystem _buildingSystem;
    private LevelsSystem _levelsSystem;

    private void Construct(BuildingSystem buildingSystem, LevelsSystem levelsSystem)
    {
        _buildingSystem = buildingSystem;
        _levelsSystem = levelsSystem;

        _levelsSystem.LevelStarted += OnLevelStarted;
    }

    protected override void SubscribeToButtons()
    {
        _acceptBuildingButton.onClick.AddListener(OnAcceptBuildingButtonClicked);
        _cancelBuildingButton.onClick.AddListener(OnCancelBuildingButtonClicked);
    }

    public void ToggleBuildingMode(bool isActive)
    {
        _towerButtons.gameObject.SetActive(isActive == false);
        _buildingButtons.gameObject.SetActive(isActive);
    }

    private void OnAcceptBuildingButtonClicked()
    {
        if (_buildingSystem.TryBuild(GameState.CurrentPlayerFaction))
            ToggleBuildingMode(false);
    }

    private void OnCancelBuildingButtonClicked()
    {
        _buildingSystem.CancelBuilding();
        ToggleBuildingMode(false);
    }

    private void OnLevelStarted()
    {
        SwitchCameraTarget(GameState.CurrentPlayerFaction);
    }
}
