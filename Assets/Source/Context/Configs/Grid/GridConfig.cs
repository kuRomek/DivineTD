using UnityEngine;

[CreateAssetMenu(fileName = "Grid", menuName = "Configs/Grid")]
public class GridConfig : ScriptableObject
{
    [field: SerializeField] public Tile TilePrefab { get; private set; }
    [field: SerializeField] public UnityEngine.Tilemaps.Tile HeavenTile { get; private set; }
    [field: SerializeField] public UnityEngine.Tilemaps.Tile HellTile { get; private set; }
}
