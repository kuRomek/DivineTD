using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    private const float ShiftAnimationDuration = 0.2f;

    [SerializeField] private Button _playButton;
    [SerializeField] private Button _editorButton;
    [SerializeField] private Button _switchFactionButton;
    [SerializeField] private RectTransform _switchButtonSprite;
    [SerializeField] private RectTransform _heavenScreen;
    [SerializeField] private RectTransform _hellScreen;
    [SerializeField] private LevelsScreen _levelsScreen;

    private Tween _screenShifting;
    private Tween _switchButtonRotation;

    private void Start()
    {
        _heavenScreen.anchoredPosition = default;
        _hellScreen.anchoredPosition = new Vector2(0, -((RectTransform)transform).rect.height);

        _playButton.onClick.AddListener(OnPlayButtonClicked);
        _editorButton.onClick.AddListener(OnEditorButtonClicked);
        _switchFactionButton.onClick.AddListener(SwitchFaction);
    }

    private void SwitchFaction()
    {
        GameState.CurrentPlayerFaction = 1 - GameState.CurrentPlayerFaction;
        ShiftFactionScreen(true);
    }

    private void OnPlayButtonClicked()
    {
        _levelsScreen.Open(false);
    }

    private void OnEditorButtonClicked()
    {
        _levelsScreen.Open(true);
    }

    private void ShiftFactionScreen(bool withAnimation)
    {
        float height = ((RectTransform)transform).rect.height;
        bool toHeaven = GameState.CurrentPlayerFaction == Faction.Heaven;

        _screenShifting?.Kill();
        _switchButtonRotation?.Complete();

        if (withAnimation)
        {
            _screenShifting = DOTween.Sequence().
                Join(_heavenScreen.DOAnchorPos(new Vector2(0f, toHeaven ? 0f : height), ShiftAnimationDuration)).
                Join(_hellScreen.DOAnchorPos(new Vector2(0f, toHeaven ? -height : 0f), ShiftAnimationDuration));

            _switchButtonRotation = _switchButtonSprite.DORotate(Vector3.forward * 180f, ShiftAnimationDuration).
                SetEase(Ease.OutBack).SetRelative();
        }
        else
        {
            _heavenScreen.anchoredPosition = new Vector2(0f, toHeaven ? 0f : height);
            _hellScreen.anchoredPosition = new Vector2(0f, toHeaven ? -height : 0f);
            _switchButtonSprite.Rotate(Vector3.forward * 180f);
        }
    }
}
