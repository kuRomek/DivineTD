using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    [SerializeField] private Button _buildButton;
    [SerializeField] private TowerType _type;

    private BuildingSystem _buildingSystem;
    private WidgetCanvas _widgetCanvas;

    public void Construct(BuildingSystem buildingSystem, WidgetCanvas widgetCanvas)
    {
        _buildingSystem = buildingSystem;
        _widgetCanvas = widgetCanvas;
    }

    private void Start()
    {
        _buildButton.onClick.AddListener(OnBuildButtonClicked);
    }

    private void OnBuildButtonClicked()
    {
        _buildingSystem.CreateTowerDraft(GameState.CurrentPlayerFaction, _type);
        _widgetCanvas.ToggleBuildingMode(true);
    }
}
