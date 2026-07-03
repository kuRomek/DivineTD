using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour
{
    [SerializeField] private Button _sendButton;
    [SerializeField] private UnitType _type;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private TextMeshProUGUI _incomeAdding;

    private RecruitingSystem _recruitingSystem;
    private WidgetCanvasRuntime _widgetCanvas;

    public void Construct(RecruitingSystem recruitingSystem, WidgetCanvas widgetCanvas)
    {
        _recruitingSystem = recruitingSystem;
        _widgetCanvas = widgetCanvas as WidgetCanvasRuntime;
    }

    public void Initialize(UnitType type)
    {
        _type = type;

        int cost = Configs.Units.GetCost(GameState.CurrentPlayerFaction, _type);
        _costText.text = cost.ToString();

        int incomeAdding = Configs.Units.GetIncomeAdding(GameState.CurrentPlayerFaction, _type);
        _incomeAdding.text = incomeAdding.ToString();
    }

    private void Start()
    {
        _sendButton.onClick.AddListener(OnBuildButtonClicked);
    }

    private void OnBuildButtonClicked()
    {
        _recruitingSystem.TryRecruit(GameState.CurrentPlayerFaction, _type);
    }
}
