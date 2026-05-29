using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _switchFactionButton;
    [Scene, SerializeField] private string _gameplayScene;

    private void Start()
    {
        _playButton.onClick.AddListener(LoadGameplayScene);
        _switchFactionButton.onClick.AddListener(SwitchFaction);
        SwitchFaction();
    }

    private void SwitchFaction()
    {
        GameState.SwitchFaction(GameState.IsCurrentFactionHeaven == false);
    }

    private void LoadGameplayScene()
    {
        SceneManager.LoadScene(_gameplayScene);
    }
}
