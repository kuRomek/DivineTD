using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.U2D;

public class PathFindingSystem
{
    private const float MaxLittleOffset = 0.2f;
    private const float LineThickness = 0.3f;
    private const float AnimationDuration = 1.5f;

    private readonly GridSystem _gridSystem;
    private readonly IReadOnlyDictionary<Faction, SpriteShapeController> _pathSplines;
    private readonly Dictionary<Faction, FullPath> _fullPaths = new()
    {
        { Faction.Heaven, new() },
        { Faction.Hell, new() },
    };

    private readonly Dictionary<Faction, Tween> _pathAnimations = new Dictionary<Faction, Tween>()
    {
        { Faction.Heaven, null },
        { Faction.Hell, null },
    };

    public PathFindingSystem(GridSystem gridSystem, IReadOnlyDictionary<Faction, SpriteShapeController> pathSplines)
    {
        _gridSystem = gridSystem;
        _pathSplines = pathSplines;

        _gridSystem.GridInitialized += OnGridInitialized;
        _gridSystem.ObjectPlaced += ReCalculatePath;
    }

    public Path GetPath(int checkpointNumber, Faction faction)
    {
        return _fullPaths[faction].Get(checkpointNumber);
    }

    public void CalculatePaths(Map map)
    {
        _fullPaths[Faction.Heaven].SetData(
            map[Faction.Heaven],
            map.GetCheckpoints(Faction.Heaven),
            map.SpawnPoints[Faction.Heaven].Item1);

        _fullPaths[Faction.Hell].SetData(
            map[Faction.Hell],
            map.GetCheckpoints(Faction.Hell),
            map.SpawnPoints[Faction.Hell].Item1);

        _fullPaths[Faction.Heaven].Calculate();
        _fullPaths[Faction.Hell].Calculate();

        DisplaySpline(Faction.Heaven);
        DisplaySpline(Faction.Hell);
    }

    public void DisplaySpline(Faction faction)
    {
        _pathAnimations[faction]?.Kill();
        SpriteShapeController trajectory = _pathSplines[faction];
        int spritesCount = trajectory.spriteShape.angleRanges[0].sprites.Count;

        int pathIndex = 1;
        Path currentPath = _fullPaths[faction].Get(1);

        if (currentPath == null)
            return;

        Vector3 position = _gridSystem.GetWorldPosition(faction, currentPath.Start);
        position = new Vector3(position.x, position.z);

        trajectory.spline.SetPosition(0, position);
        trajectory.spline.SetHeight(0, LineThickness);
        trajectory.spline.SetSpriteIndex(0, 0);

        IEnumerator<Vector2Int> pathEnumerator = currentPath.GetEnumerator();

        int cellIndex;

        for (cellIndex = 1; cellIndex < trajectory.spline.GetPointCount(); cellIndex++)
        {
            if (cellIndex >= _fullPaths[faction].AllCellsCount)
                break;

            if (pathEnumerator.MoveNext() == false)
            {
                currentPath = _fullPaths[faction].Get(++pathIndex);
                pathEnumerator = currentPath.GetEnumerator();

                if (pathEnumerator.MoveNext() == false)
                    continue;

                trajectory.spline.SetSpriteIndex(cellIndex - 1, (pathIndex - 1) % spritesCount);
            }

            position = _gridSystem.GetWorldPosition(faction, pathEnumerator.Current);
            position = new Vector3(position.x, position.z);

            trajectory.spline.SetPosition(cellIndex, position + GetLittleOffset());
            trajectory.spline.SetHeight(cellIndex, LineThickness);
            trajectory.spline.SetSpriteIndex(cellIndex, (pathIndex - 1) % spritesCount);
        }

        while (trajectory.spline.GetPointCount() < _fullPaths[faction].AllCellsCount)
        {
            if (pathEnumerator.MoveNext() == false)
            {
                currentPath = _fullPaths[faction].Get(++pathIndex);
                pathEnumerator = currentPath.GetEnumerator();

                if (pathEnumerator.MoveNext() == false)
                    continue;

                trajectory.spline.SetSpriteIndex(cellIndex - 1, (pathIndex - 1) % spritesCount);
            }

            position = _gridSystem.GetWorldPosition(faction, pathEnumerator.Current);
            position = new Vector3(position.x, position.z);

            trajectory.spline.InsertPointAt(cellIndex, position + GetLittleOffset());
            trajectory.spline.SetHeight(cellIndex, LineThickness);
            trajectory.spline.SetSpriteIndex(cellIndex++, (pathIndex - 1) % spritesCount);
        }

        while (trajectory.spline.GetPointCount() > _fullPaths[faction].AllCellsCount)
            trajectory.spline.RemovePointAt(trajectory.spline.GetPointCount() - 1);

        //float length = (trajectory.spline.GetPointCount() - 1) / AnimationDuration;

        // _pathAnimations[faction] = DOVirtual.Float(0f, AnimationDuration, AnimationDuration, (elapsedTime) =>
        // {
        //     int currentIndex = Mathf.RoundToInt(elapsedTime * length);
        //     trajectory.spline.SetSpriteIndex(currentIndex, 0);

        //     if (currentIndex > 0)
        //         trajectory.spline.SetSpriteIndex(currentIndex - 1, 1);

        // }).SetEase(Ease.Linear).SetLoops(-1);
    }

    public void ReCalculatePath(Faction faction)
    {
        _fullPaths[faction].Calculate();
        DisplaySpline(faction);
    }

    private void OnGridInitialized()
    {
        CalculatePaths(_gridSystem.Map);
    }

    private Vector3 GetLittleOffset()
    {
        return new Vector3(Random.Range(-MaxLittleOffset, MaxLittleOffset), Random.Range(-MaxLittleOffset, MaxLittleOffset));
    }
}