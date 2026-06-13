using kuRomek.SimpleVG;
using UnityEngine;

public class GridObjectView : View
{
    [SerializeField] private MeshRenderer _buildingIndicator;
    [SerializeField] private MeshRenderer _mesh;

    public void ToggleBuildingIndicator(bool isActive)
    {
        if (_mesh != null)
        {
            Color color = _mesh.material.color;

            if (isActive == false)
                color = Color.white;
            else
                color.a = 0.5f;

            _mesh.material.color = color;
        }

        if (_buildingIndicator != null)
            _buildingIndicator.gameObject.SetActive(isActive);
    }

    public void SetBuildingIndicatorColor(bool isValidPosition)
    {
        if (_mesh != null)
            _mesh.material.color = isValidPosition ? new Color(0f, 1f, 0f, 0.5f) : new Color(1f, 0f, 0f, 0.5f);

        if (_buildingIndicator != null)
            _buildingIndicator.material.color = isValidPosition ? new Color(0f, 1f, 0f, 0.5f) : new Color(1f, 0f, 0f, 0.5f);
    }
}
