using UnityEngine;

[CreateAssetMenu(fileName = "PathFinding", menuName = "Configs/PathFinding")]
public class PathFindingConfig : ScriptableObject
{
    [field: SerializeField] public bool DiagonalsAllowed { get; private set; }
}
