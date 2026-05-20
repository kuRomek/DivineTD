using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    [SerializeField] private Button _buildButton;

    private void Start()
    {
        _buildButton.onClick.AddListener(CreateTowerGhost);
    }

    private void CreateTowerGhost()
    {

    }
}
