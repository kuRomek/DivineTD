using kuRomek.SimpleVG;
using UnityEngine;

public class GridObjectView : View
{
    [SerializeField] private MeshRenderer _buildingIndicator;
    [SerializeField] private MeshRenderer _mesh;

    public void ToggleBuildingIndicator(bool isActive)
    {
        Color color = _mesh.material.color;
        color.a = isActive ? 0.5f : 1f;
        _mesh.material.color = color;

        if (_buildingIndicator != null)
            _buildingIndicator.gameObject.SetActive(isActive);
    }

    public void SetBuildingIndicatorColor(bool isValidPosition)
    {
        _mesh.material.color = isValidPosition ? new Color(0f, 0f, 1f, 0.5f) : new Color(1f, 0f, 0f, 0.5f);

        if (_buildingIndicator != null)
            _buildingIndicator.material.color = isValidPosition ? new Color(0f, 1f, 0f, 0.5f) : new Color(1f, 0f, 0f, 0.5f);
    }
}
