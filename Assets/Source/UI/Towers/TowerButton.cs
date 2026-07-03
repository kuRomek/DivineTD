using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    [SerializeField] private Button _buildButton;
    [SerializeField] private TowerType _type;
    [SerializeField] private TextMeshProUGUI _costText;

    private BuildingSystem _buildingSystem;
    private WidgetCanvasRuntime _widgetCanvas;

    public void Construct(BuildingSystem buildingSystem, WidgetCanvas widgetCanvas)
    {
        _buildingSystem = buildingSystem;
        _widgetCanvas = widgetCanvas as WidgetCanvasRuntime;
    }

    public void Initialize(TowerType type)
    {
        _type = type;

        int cost = Configs.Buildings.GetCost(GameState.CurrentPlayerFaction, _type);
        _costText.text = cost.ToString();
    }

    private void Start()
    {
        _buildButton.onClick.AddListener(OnBuildButtonClicked);
    }

    private void OnBuildButtonClicked()
    {
        if (_buildingSystem.TryCreateTowerDraft(GameState.CurrentPlayerFaction, _type))
            _widgetCanvas.ToggleBuildingMode(true);
    }
}
