using UnityEngine;

[CreateAssetMenu(fileName = "Grid", menuName = "Configs/Grid")]
public class GridConfig : ScriptableObject
{
    [field: SerializeField] public Tile TilePrefab { get; private set; }
}
