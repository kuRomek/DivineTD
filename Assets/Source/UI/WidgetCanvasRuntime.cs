using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WidgetCanvasRuntime : WidgetCanvas
{
    [Header("Building")]
    [SerializeField] private RectTransform _buildingButtons;
    [Space(3f)]
    [SerializeField] private Button _acceptBuildingButton;
    [SerializeField] private Button _cancelBuildingButton;

    [Header("Economy")]
    [SerializeField] private TextMeshProUGUI _currentFundsText;
    [SerializeField] private TextMeshProUGUI _incomeText;
    [SerializeField] private TextMeshProUGUI _cooldownText;

    [Header("Other")]
    [SerializeField] private ButtonsMenu _buttonsMenu;
    [SerializeField] private Button _openButtonsMenuButton;
    [SerializeField] private Button _closeButtonsMenuButton;

    private BuildingSystem _buildingSystem;
    private LevelsSystem _levelsSystem;
    private EconomySystem _economySystem;

    private void Construct(BuildingSystem buildingSystem, LevelsSystem levelsSystem, EconomySystem economySystem)
    {
        _buildingSystem = buildingSystem;
        _levelsSystem = levelsSystem;
        _economySystem = economySystem;

        _levelsSystem.LevelStarted += OnLevelStarted;
    }

    private void OnEnable()
    {
        _economySystem.Funds[GameState.CurrentPlayerFaction].Amount.Changed += OnFundsAmountChanged;
        _economySystem.Funds[GameState.CurrentPlayerFaction].Income.Changed += OnIncomeAmountChanged;
        _economySystem.Cooldowns[GameState.CurrentPlayerFaction].RemainingTime.Changed += OnIncomeRemainingTimeChanged;
    }

    protected override void SubscribeToButtons()
    {
        base.SubscribeToButtons();

        _acceptBuildingButton.onClick.AddListener(OnAcceptBuildingButtonClicked);
        _cancelBuildingButton.onClick.AddListener(OnCancelBuildingButtonClicked);
        _openButtonsMenuButton.onClick.AddListener(OnOpenButtonsMenuButtonClicked);
        _closeButtonsMenuButton.onClick.AddListener(OnCloseButtonsMenuButtonClicked);
    }

    public void ToggleBuildingMode(bool isActive)
    {
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

    private void OnFundsAmountChanged(int amount)
    {
        _currentFundsText.text = amount.ToString();
    }

    private void OnIncomeAmountChanged(int amount)
    {
        _incomeText.text = amount.ToString();
    }

    private void OnIncomeRemainingTimeChanged(float remainingTime)
    {
        _cooldownText.text = remainingTime.ToString("0.0");
    }

    private void OnOpenButtonsMenuButtonClicked()
    {
        _openButtonsMenuButton.gameObject.SetActive(false);
        _closeButtonsMenuButton.gameObject.SetActive(true);

        _buttonsMenu.Open();
    }

    private void OnCloseButtonsMenuButtonClicked()
    {
        _buttonsMenu.Close().OnComplete(() =>
        {
            _closeButtonsMenuButton.gameObject.SetActive(false);
            _openButtonsMenuButton.gameObject.SetActive(true);
        });
    }

    private void OnLevelStarted()
    {
        _buttonsMenu.InitializeButtons();
        SwitchCameraTarget(GameState.CurrentPlayerFaction);
    }
}
