using TMPro;
using UnityEngine;

public class CheckpointView : GridObjectView
{
    [SerializeField] private TextMeshProUGUI _number;

    public void DisplayNumber(int number)
    {
        _number.text = number.ToString();
    }
}