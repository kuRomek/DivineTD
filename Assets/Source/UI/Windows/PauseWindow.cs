using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseWindow : Window
{
    [Space(5f)]
    [SerializeField] private Button _mainMenuButton;
    [Scene, SerializeField] private string _mainMenuScene;

    protected override void SubscribeToButtons()
    {
        base.SubscribeToButtons();

        _mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
    }

    private void OnMainMenuButtonClicked()
    {
        SceneManager.LoadScene(_mainMenuScene);
    }
}
