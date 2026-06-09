using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _label;

    private int _levelNumber;
    private bool _editorMode;
    private Faction _faction;

    private void Start()
    {
        _button.onClick.AddListener(OnButtonClicked);
    }

    public void Initialize(bool editorMode, Faction faction, int levelNumber = -1)
    {
        _levelNumber = levelNumber;
        _editorMode = editorMode;
        _faction = faction;
        _label.text = levelNumber == -1 ? "new" : _levelNumber.ToString();
    }

    private void OnButtonClicked()
    {
        GameState.CurrentLevel = _levelNumber;
        GameState.CurrentPlayerFaction = _faction;

        SceneManager.LoadScene((int)(_editorMode ? Scenes.MapEditor : Scenes.Main));
    }
}