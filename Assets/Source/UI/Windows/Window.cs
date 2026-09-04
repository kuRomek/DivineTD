using System;
using UnityEngine;
using UnityEngine.UI;

public class Window : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _closeButton;

    public event Action Opened;
    public event Action Closed;

    private void Start()
    {
        SubscribeToButtons();
    }

    public void Open()
    {
        if (gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
            Opened?.Invoke();
        }
    }

    public void Close()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            Closed?.Invoke();
        }
    }

    protected virtual void SubscribeToButtons()
    {
        _closeButton.onClick.AddListener(Close);
    }
}
