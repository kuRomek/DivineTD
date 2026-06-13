using System.Collections.Generic;
using UnityEngine;

public class Path
{
    private readonly List<Vector2Int> _path = new();

    private IEnumerator<Vector2Int> _pathEnumerator;

    public Vector2Int CurrentTarget { get; private set; }

    public void Calculate(Vector2Int checkpoint)
    {
        _path.Clear();
        _path.Add(checkpoint);

        _pathEnumerator = _path.GetEnumerator();
        TrySetNextTarget();
    }

    public bool TrySetNextTarget()
    {
        bool success = _pathEnumerator.MoveNext();

        if (success)
            CurrentTarget = _pathEnumerator.Current;

        return success;
    }
}