using System.Collections.Generic;
using kuRomek.SimpleVG;
using UnityEngine;

public class GridSystem
{
    private readonly Grid _heavenGrid;
    private readonly Grid _hellGrid;
    private readonly Camera _camera;

    private readonly Dictionary<Vector2Int, GridObjectModel> _cellsHeaven = new();
    private readonly Dictionary<Vector2Int, GridObjectModel> _cellsHell = new();

    public GridSystem(Grid heavenGrid, Grid hellGrid, Camera camera)
    {
        _heavenGrid = heavenGrid;
        _hellGrid = hellGrid;
        _camera = camera;
    }

    public IReadOnlyDictionary<Vector2Int, GridObjectModel> CellsHeaven => _cellsHeaven;
    public IReadOnlyDictionary<Vector2Int, GridObjectModel> CellsHell => _cellsHell;

    public bool TryPlace(GridObjectModel gridObject, bool isHeavenSide)
    {
        Grid grid = isHeavenSide ? _heavenGrid : _hellGrid;

        Vector2Int cell = (Vector2Int)grid.WorldToCell(gridObject.Transform.position);

        if (_cellsHeaven.TryAdd(cell, gridObject) == false)
        {
            Debug.Log("Invalid position");
            return false;
        }

        Debug.Log($"World position: {gridObject.Transform.position}");
        Debug.Log($"Actual cell world position: {grid.CellToWorld((Vector3Int)cell)}");
        Debug.Log($"Object placed onto {cell}");

        return true;
    }

    public void Drag(GridObjectModel gridObject)
    {
        if (gridObject.IsDraft == false)
        {
            Debug.LogWarning("The object is not draft, ignoring the method call");
            return;
        }

        Ray ray = _camera.ScreenPointToRay(InputController.Current.Position);

        if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, LayerMask.GetMask("Ground")))
            gridObject.Transform.position = GetSnappedPosition(gridObject.IsHeavenFaction, hit.point);
    }

    public Vector3 GetSnappedPosition(bool isHeavenSide, Vector3 worldPosition)
    {
        Grid grid = isHeavenSide ? _heavenGrid : _hellGrid;

        Vector3Int cell = grid.WorldToCell(worldPosition);
        return grid.CellToWorld(cell) + grid.transform.rotation * new Vector3(grid.cellSize.x, -0.001f, grid.cellSize.y) / 2f;
    }

    public Vector3 GetSnappedPosition(bool isHeavenSide)
    {
        Ray ray = _camera.ViewportPointToRay(Vector2.one * 0.5f);

        if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, LayerMask.GetMask("Ground")))
            return GetSnappedPosition(isHeavenSide, hit.point);

        Debug.LogWarning("Beyond ground edges.");
        return -Vector3.one;
    }
}
