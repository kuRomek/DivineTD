using kuRomek.SimpleVG;
using UnityEngine;
using UnityEngine.UI;

public class HealthView : View
{
    [SerializeField] private Slider _sliderBar;

    private MainCamera _camera;

    private void Construct(MainCamera camera)
    {
        _camera = camera;
        transform.rotation = _camera.transform.rotation;
    }

    public void Display(float currentAmount, float maxAmount)
    {
        _sliderBar.maxValue = maxAmount;
        _sliderBar.value = currentAmount;
    }

    public void Show()
    {
        _sliderBar.gameObject.SetActive(false);
    }

    public void Hide()
    {
        _sliderBar.gameObject.SetActive(false);
    }
}