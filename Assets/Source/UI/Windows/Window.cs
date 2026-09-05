using System;
using UnityEngine;
using UnityEngine.UI;

public class Window : MonoBehaviour
{
    [field: SerializeField] public Button CloseButton { get; private set; }

    public event Action Opened;
    public event Action Closed;

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

}
