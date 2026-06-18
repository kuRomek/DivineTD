using UnityEngine;

public static class PathFinding
{
    public static float DistanceEvaluation(Vector2Int a, Vector2Int b, bool withDiagonals)
    {
        if (withDiagonals)
            return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y));
        //return Mathf.Sqrt(Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2));
        else
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
}