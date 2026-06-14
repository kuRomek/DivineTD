using System;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
    private readonly IReadOnlyDictionary<Vector2Int, Tile> _field;
    private readonly Stack<Vector2Int> _path = new();

    private IEnumerator<Vector2Int> _pathEnumerator;

    public Path(IReadOnlyDictionary<Vector2Int, Tile> walkingField)
    {
        _field = walkingField;
    }

    public Vector2Int CurrentTarget { get; private set; }

    public void Calculate(Vector2Int start, Vector2Int checkpoint)
    {
        _path.Clear();
        PerformAStar(start, checkpoint);
        _pathEnumerator = _path.GetEnumerator();
    }

    public bool TrySetNextTarget()
    {
        if (_pathEnumerator == null)
            return false;

        bool success = _pathEnumerator.MoveNext();

        if (success)
            CurrentTarget = _pathEnumerator.Current;

        return success;
    }

    private void PerformAStar(Vector2Int start, Vector2Int checkpoint)
    {
        var openSet = new MinHeap<Node>();
        var openLookup = new HashSet<Vector2Int>();
        var closedSet = new HashSet<Vector2Int>();

        Node finishNode = null;

        bool diagonalsAllowed = Configs.Pathfinding.DiagonalsAllowed;

        int g = 0;
        int h = Pathfinding.DistanceEvaluation(start, checkpoint, diagonalsAllowed);
        int f = h;
        openSet.Push(new(start, null, g, h, f));
        openLookup.Add(start);

        int constraint = 1000;

        while (openSet.Count > 0 && constraint-- > 0)
        {
            var current = openSet.Pop();
            Debug.Log($"{current.Position} F={current.F}");
            openLookup.Remove(current.Position);
            closedSet.Add(current.Position);

            if (current.Position == checkpoint)
            {
                finishNode = current;
                break;
            }

            foreach (var offset in GetOffsets(diagonalsAllowed))
            {
                var neighborPosition = current.Position + offset;

                if (_field.TryGetValue(neighborPosition, out Tile tile) == false)
                    continue;

                if (tile.Walkable == false || closedSet.Contains(neighborPosition) || openLookup.Contains(neighborPosition))
                    continue;

                g = current.G + 1;
                h = Pathfinding.DistanceEvaluation(neighborPosition, checkpoint, diagonalsAllowed);
                f = g + h;

                openSet.Push(new Node(neighborPosition, current, g, h, f));
                openLookup.Add(neighborPosition);
            }
        }

        while (finishNode != null && finishNode.Parent != null)
        {
            _path.Push(finishNode.Position);
            finishNode = finishNode.Parent;
        }
    }

    private IEnumerable<Vector2Int> GetOffsets(bool withDiagonals)
    {
        yield return new Vector2Int(0, 1);
        yield return new Vector2Int(1, 0);
        yield return new Vector2Int(-1, 0);
        yield return new Vector2Int(0, -1);

        if (withDiagonals)
        {
            yield return new Vector2Int(1, 1);
            yield return new Vector2Int(-1, -1);
            yield return new Vector2Int(1, -1);
            yield return new Vector2Int(-1, 1);
        }
    }

    private class Node : IComparable<Node>
    {
        public Vector2Int Position;
        public int G;
        public int H;
        public int F;
        public Node Parent;

        public Node(Vector2Int position, Node parent, int g, int h, int f)
        {
            Position = position;
            G = g;
            H = h;
            F = f;
            Parent = parent;
        }

        int IComparable<Node>.CompareTo(Node other)
        {
            return F.CompareTo(other.F);
        }
    }
}