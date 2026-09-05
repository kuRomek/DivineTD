using UnityEngine;
using UnityEngine.UI;

public abstract class MenuButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    private void Start()
    {
        _button.onClick.AddListener(OnButtonClicked);
    }

    protected abstract void OnButtonClicked();
}