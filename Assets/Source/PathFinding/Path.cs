using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : IEnumerable<Vector2Int>
{
    private readonly Stack<Vector2Int> _cells = new();

    public Path(IReadOnlyDictionary<Vector2Int, Tile> walkingField, Vector2Int start, Vector2Int checkpoint, int checkpointNumber)
    {
        Field = walkingField;

        Calculate(start, checkpoint, checkpointNumber);
    }

    public IReadOnlyDictionary<Vector2Int, Tile> Field { get; }
    public IEnumerable<Vector2Int> Cells => _cells;
    public int CellCount => _cells.Count;

    public Vector2Int Checkpoint { get; private set; }
    public Vector2Int Start { get; private set; }
    public int CheckpointNumber { get; private set; }

    public Path Calculate(Vector2Int start, Vector2Int checkpoint, int checkpointNumber)
    {
        _cells.Clear();
        Start = start;
        Checkpoint = checkpoint;
        CheckpointNumber = checkpointNumber;

        PerformAStar(start, checkpoint);

        return this;
    }

    private void PerformAStar(Vector2Int start, Vector2Int checkpoint)
    {
        var openSet = new MinHeap<Node>();
        var openLookup = new HashSet<Vector2Int>();
        var closedSet = new HashSet<Vector2Int>();

        Node finishNode = null;

        bool diagonalsAllowed = Configs.PathFinding.DiagonalsAllowed;

        float g = 0;
        float h = PathFinding.DistanceEvaluation(start, checkpoint, diagonalsAllowed);
        openSet.Push(new(start, null, g, h));
        openLookup.Add(start);

        int constraint = 1000;
        Node current = null;
        float minH = int.MaxValue;
        Node closestNode = null;
        bool success = false;

        while (openSet.Count > 0 && constraint-- > 0)
        {
            current = openSet.Pop();
            openLookup.Remove(current.Position);
            closedSet.Add(current.Position);

            if (current.Position == checkpoint)
            {
                success = true;
                break;
            }

            foreach (var offset in GetOffsets(diagonalsAllowed))
            {
                bool diagonal = offset.x != 0 && offset.y != 0;

                var neighborPosition = current.Position + offset;

                if (Field.TryGetValue(neighborPosition, out Tile tile) == false)
                    continue;

                if (diagonal)
                {
                    Vector2Int[] adjacentCells = new Vector2Int[2]
                    {
                        neighborPosition + new Vector2Int(-offset.x, 0),
                        neighborPosition + new Vector2Int(0, -offset.y)
                    };

                    if (Field.TryGetValue(adjacentCells[0], out Tile adjacentTile) == false || adjacentTile.Walkable == false)
                        continue;

                    if (Field.TryGetValue(adjacentCells[1], out adjacentTile) == false || adjacentTile.Walkable == false)
                        continue;
                }

                if (tile.Walkable == false || closedSet.Contains(neighborPosition) || openLookup.Contains(neighborPosition))
                    continue;

                g = current.G + 1 + Convert.ToInt32(diagonal) * 0.5f;
                h = PathFinding.DistanceEvaluation(neighborPosition, checkpoint, diagonalsAllowed);
                Node node = new(neighborPosition, current, g, h);

                if (h < minH)
                {
                    minH = h;
                    closestNode = node;
                }

                openSet.Push(node);
                openLookup.Add(neighborPosition);
            }
        }

        if (success)
            finishNode = current;
        else
            finishNode = closestNode;

        while (finishNode != null && finishNode.Parent != null)
        {
            _cells.Push(finishNode.Position);
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

    public IEnumerator<Vector2Int> GetEnumerator()
    {
        return ((IEnumerable<Vector2Int>)_cells).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_cells).GetEnumerator();
    }

    private class Node : IComparable<Node>
    {
        public Vector2Int Position;
        public float G;
        public float H;
        public float F;
        public Node Parent;

        public Node(Vector2Int position, Node parent, float g, float h)
        {
            Position = position;
            G = g;
            H = h;
            F = g + h;
            Parent = parent;
        }

        int IComparable<Node>.CompareTo(Node other)
        {
            return F.CompareTo(other.F);
        }
    }
}
