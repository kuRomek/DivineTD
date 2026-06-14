using UnityEngine;

[CreateAssetMenu(fileName = "Pathfinding", menuName = "Configs/Pathfinding")]
public class PathfindingConfig : ScriptableObject
{
    [field: SerializeField] public bool DiagonalsAllowed { get; private set; }
}
