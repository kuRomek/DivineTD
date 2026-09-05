using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LevelsScreen : MonoBehaviour
{
    private const float FadeDuration = 0.2f;

    [SerializeField] private Button _closeButton;
    [SerializeField] private Image _blurredHeavenBackground;
    [SerializeField] private Image _blurredHellBackground;
    [SerializeField] private RectTransform _levelButtonsContainer;
    [SerializeField] private RectTransform _widgetsParent;

    private readonly List<LevelButton> _levelButtons = new();

    private LevelButton _newLevelButton;
    private Image _currentBackground;

    private Tween _fading;

    private void Awake()
    {
        _newLevelButton = Instantiate(Configs.Levels.LevelButtonPrefab, _levelButtonsContainer);
    }

    private void OnEnable()
    {
        _closeButton.onClick.AddListener(Close);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(Close);
        //_fading?.Kill();
    }

    public void Open(bool editorMode)
    {
        gameObject.SetActive(true);
        _fading?.Kill();
        _widgetsParent.gameObject.SetActive(false);

        var faction = GameState.CurrentPlayerFaction;
        IReadOnlyList<LevelData> levelsData;

        if (faction == Faction.Heaven)
        {
            levelsData = Configs.Levels.HeavenSection;
            _currentBackground = _blurredHeavenBackground;
        }
        else
        {
            levelsData = Configs.Levels.HellSection;
            _currentBackground = _blurredHellBackground;
        }

        while (_levelButtons.Count < levelsData.Count)
            _levelButtons.Add(Instantiate(Configs.Levels.LevelButtonPrefab, _levelButtonsContainer));

        while (_levelButtons.Count > levelsData.Count)
        {
            var lastIndex = _levelButtons.Count - 1;
            Destroy(_levelButtons[lastIndex].gameObject);
            _levelButtons.RemoveAt(lastIndex);
        }

        for (int i = 0; i < levelsData.Count; i++)
            _levelButtons[i].Initialize(editorMode, faction, i + 1);

        if (editorMode)
        {
            _newLevelButton.Initialize(editorMode, faction, levelsData.Count + 1, true);
            _newLevelButton.transform.SetAsLastSibling();
        }

        _newLevelButton.gameObject.SetActive(editorMode);

        _fading = _currentBackground.DOFade(1f, FadeDuration);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        _fading?.Kill();
        _fading = _currentBackground.DOFade(0f, FadeDuration);
        _widgetsParent.gameObject.SetActive(true);
    }
}