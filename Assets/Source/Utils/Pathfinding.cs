using UnityEngine;

public static class Pathfinding
{
    public static int DistanceEvaluation(Vector2Int a, Vector2Int b, bool withDiagonals)
    {
        if (withDiagonals)
            return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y));
        else
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
}