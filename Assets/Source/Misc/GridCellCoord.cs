using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class GridCellCoord : MonoBehaviour
{
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void Init(int x, int y, Color color, Vector3 localPosition)
    {
        _text.text = $"[{x}, {y}]";
        _text.color = color;

        transform.localPosition = localPosition;
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }
}
