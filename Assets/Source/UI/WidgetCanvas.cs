using UnityEngine;
using UnityEngine.UI;

public class WidgetCanvas : MonoBehaviour
{
    [SerializeField] private RectTransform _towerButtons;
    [SerializeField] private RectTransform _buildingButtons;

    [Space(3f)]
    [SerializeField] private Button _acceptBuildingButton;
    [SerializeField] private Button _cancelBuildingButton;

    private BuildingSystem _buildingSystem;

    public void Construct(BuildingSystem buildingSystem)
    {
        _buildingSystem = buildingSystem;
    }

    private void Start()
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
        if (_buildingSystem.TryBuildTower(true))
            ToggleBuildingMode(false);
    }

    private void OnCancelBuildingButtonClicked()
    {
        _buildingSystem.CancelBuilding();
        ToggleBuildingMode(false);
    }
}
