using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ButtonsMenu : MonoBehaviour
{
    [SerializeField] private RectTransform _towersButtonContainer;
    [SerializeField] private RectTransform _unitsButtonContainer;
    [SerializeField] private TowerButton _towerButtonPrefab;
    [SerializeField] private UnitButton _unitButtonPrefab;

    private readonly List<TowerButton> _towerButtons = new();
    private readonly List<UnitButton> _unitButtons = new();

    private const float SlidingDuration = 0.4f;

    private RectTransform _rect;
    private Tween _slidingAnimation;

    private void Awake()
    {
        _rect = transform as RectTransform;
    }

    public Tween Open()
    {
        gameObject.SetActive(true);
        _slidingAnimation?.Kill();
        _slidingAnimation = _rect.DOAnchorPosX(-_rect.rect.width, SlidingDuration);
        return _slidingAnimation;
    }

    public Tween Close()
    {
        _slidingAnimation?.Kill();
        _slidingAnimation = _rect.DOAnchorPosX(0f, SlidingDuration).OnComplete(() => gameObject.SetActive(false));
        return _slidingAnimation;
    }

    public void FillButtons()
    {
        var availableTowers = Configs.Levels.GetAvailableTowers(
            GameState.CurrentPlayerFaction,
            GameState.CurrentPlayerFaction,
            GameState.CurrentLevel);

        while (_towerButtons.Count < availableTowers.Count)
            _towerButtons.Add(Instantiate(_towerButtonPrefab, _towersButtonContainer));

        while (_towerButtons.Count > availableTowers.Count)
        {
            var lastIndex = _towerButtons.Count - 1;
            Destroy(_towerButtons[lastIndex].gameObject);
            _towerButtons.RemoveAt(lastIndex);
        }

        for (int i = 0; i < _towerButtons.Count; i++)
        {
            Root.Container.InjectConstructors(_towerButtons[i]);
            _towerButtons[i].Initialize(availableTowers[i]);
        }

        var availableUnits = Configs.Levels.GetAvailableUnits(
        GameState.CurrentPlayerFaction,
        GameState.CurrentPlayerFaction,
        GameState.CurrentLevel);

        while (_unitButtons.Count < availableUnits.Count)
            _unitButtons.Add(Instantiate(_unitButtonPrefab, _unitsButtonContainer));

        while (_unitButtons.Count > availableUnits.Count)
        {
            var lastIndex = _unitButtons.Count - 1;
            Destroy(_unitButtons[lastIndex].gameObject);
            _unitButtons.RemoveAt(lastIndex);
        }

        for (int i = 0; i < _unitButtons.Count; i++)
        {
            Root.Container.InjectConstructors(_unitButtons[i]);
            _unitButtons[i].Initialize(availableUnits[i]);
        }
    }
}
