using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class WidgetCanvas : MonoBehaviour
{
    private const float ShiftAnimationDuration = 0.2f;

    [SerializeField] private Button _switchFactionButton;
    [SerializeField] private RectTransform _switchButtonSprite;

    [field: SerializeField] public MainCamera MainCamera { get; private set; }

    private Tween _switchButtonRotation;

    protected abstract void SubscribeToButtons();

    private void Start()
    {
        _switchFactionButton.onClick.AddListener(() => SwitchCameraTarget(1 - MainCamera.TargetFaction));

        SubscribeToButtons();
    }

    protected void SwitchCameraTarget(Faction faction)
    {
        if (MainCamera.IsControlBlocked)
            return;

        _switchButtonRotation?.Kill();

        MainCamera.SwitchTargetFactionTo(faction, false);

        float rotation = faction == Faction.Heaven ? 0f : 180f;

        _switchButtonRotation = _switchButtonSprite.DORotate(Vector3.forward * rotation, ShiftAnimationDuration).
            SetEase(Ease.OutBack);
    }
}