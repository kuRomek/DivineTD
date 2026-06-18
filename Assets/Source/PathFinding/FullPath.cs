using System.Collections.Generic;
using UnityEngine;

public class FullPath
{
    private readonly List<Path> _paths = new();

    private IReadOnlyDictionary<Vector2Int, Tile> _field;
    private IEnumerable<Vector2Int> _checkpoints;
    private Vector2Int _start;
    private int _checkpointsNumber;

    public IReadOnlyList<Path> Paths => _paths;

    public int AllCellsCount { get; private set; }

    public void Calculate()
    {
        _paths.Clear();
        AllCellsCount = 0;

        Vector2Int start = _start;

        _checkpointsNumber = 0;

        foreach (var checkpoint in _checkpoints)
        {
            Path path = new(_field, start, checkpoint, ++_checkpointsNumber);
            _paths.Add(path);
            AllCellsCount += path.CellCount;
            start = checkpoint;
        }
    }

    public Path Get(int checkpointNumber)
    {
        return checkpointNumber <= _checkpointsNumber ? _paths[checkpointNumber - 1] : null;
    }

    public void SetData(IReadOnlyDictionary<Vector2Int, Tile> walkingField, IEnumerable<Vector2Int> checkpoints, Vector2Int start)
    {
        _field = walkingField;
        _checkpoints = checkpoints;
        _start = start;
    }
}