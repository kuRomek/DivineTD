using kuRomek.SimpleVG;
using UnityEngine;

public class GridObjectView : View
{
    [SerializeField] private MeshRenderer _buildingIndicator;

    public void ToggleBuildingIndicator(bool isActive)
    {
        _buildingIndicator.gameObject.SetActive(isActive);
    }

    public void SetBuildingIndicatorColor(bool isValidPosition)
    {

        _buildingIndicator.material.color = isValidPosition ? new Color(0f, 0f, 1f, 0.5f) : new Color(1f, 0f, 0f, 0.5f);
    }
}
