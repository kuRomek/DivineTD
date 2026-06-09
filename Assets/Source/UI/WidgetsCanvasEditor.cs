using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WidgetsCanvasEditor : WidgetCanvas
{
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _eraserButton;
    [SerializeField] private Button _noneButton;
    [SerializeField] private Button _tileButton;
    [SerializeField] private Button _castleButton;
    [SerializeField] private List<Button> _towerButtons;

    private MapEditingSystem _mapEditingSystem;

    private void Construct(MapEditingSystem mapEditingSystem)
    {
        _mapEditingSystem = mapEditingSystem;
    }

    protected override void SubscribeToButtons()
    {
        _noneButton.onClick.AddListener(() => _mapEditingSystem.SetBrush(MapEditingSystem.Brush.None));
        _eraserButton.onClick.AddListener(() => _mapEditingSystem.SetBrush(MapEditingSystem.Brush.Eraser));
        _tileButton.onClick.AddListener(() => _mapEditingSystem.SetBrush(MapEditingSystem.Brush.Tile));
        _castleButton.onClick.AddListener(() => _mapEditingSystem.SetBrush(MapEditingSystem.Brush.Castle));

        for (int i = 0; i < _towerButtons.Count; i++)
        {
            TowerType type = (TowerType)i;

            _towerButtons[i].onClick.AddListener(() =>
            {
                _mapEditingSystem.SetTowerType(type);
                _mapEditingSystem.SetBrush(MapEditingSystem.Brush.Tower);
            });
        }

        _saveButton.onClick.AddListener(_mapEditingSystem.SaveMap);
    }
}