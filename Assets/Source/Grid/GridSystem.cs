using kuRomek.SimpleVG;
using UnityEngine;

public class GridSystem
{
    private readonly LevelsSystem _levelsSystem;
    private readonly Grid _heavenGrid;
    private readonly Grid _hellGrid;
    private readonly Camera _camera;
    private readonly GridInitializer _gridInitializer;

    public GridSystem(LevelsSystem levelsSystem, Grid heavenGrid, Grid hellGrid, Map map, Camera camera)
    {
        _levelsSystem = levelsSystem;
        _heavenGrid = heavenGrid;
        _hellGrid = hellGrid;
        _camera = camera;
        _gridInitializer = new(this);

        Map = map;

        _levelsSystem.LevelStarted += OnLevelStarted;
    }

    public Map Map { get; }

    public bool TryPlace(GridObjectModel gridObject, bool heavenSide)
    {
        Grid grid = heavenSide ? _heavenGrid : _hellGrid;

        Vector2Int cell = (Vector2Int)grid.WorldToCell(gridObject.Transform.position);

        if (CheckAvailability(cell, heavenSide))
        {
            Map.PlaceObjectOnTile(heavenSide, cell, gridObject);
        }
        else
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
            Debug.LogWarning("The object is not draft, ignoring the method call");

        Ray ray = _camera.ScreenPointToRay(InputController.Current.Position);

        if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, LayerMask.GetMask("Ground")))
            gridObject.MoveAt(GetSnappedPosition(gridObject.IsHeavenFaction, hit.point));
    }

    public Vector3 GetSnappedPosition(bool heavenSide, Vector3 worldPosition)
    {
        Vector3Int cell = (heavenSide ? _heavenGrid : _hellGrid).WorldToCell(worldPosition);

        return GetWorldPosition(heavenSide, (Vector2Int)cell);
    }

    public Vector3 GetSnappedPosition(bool heavenSide)
    {
        Ray ray = _camera.ViewportPointToRay(Vector2.one * 0.5f);

        if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, LayerMask.GetMask("Ground")))
            return GetSnappedPosition(heavenSide, hit.point);

        Debug.LogWarning("Beyond ground edges.");
        return -Vector3.one;
    }

    public Vector2Int GetCell(bool heavenFaction, Vector3 worldPosition)
    {
        return (Vector2Int)(heavenFaction ? _heavenGrid : _hellGrid).WorldToCell(worldPosition);
    }

    public Vector3 GetWorldPosition(bool heavenFaction, Vector2Int cell)
    {
        Grid grid = heavenFaction ? _heavenGrid : _hellGrid;

        Vector3 position = grid.CellToWorld((Vector3Int)cell) +
            grid.transform.rotation * new Vector3(grid.cellSize.x, 0f, grid.cellSize.y) / 2f;

        return position;
    }

    public bool CheckAvailability(Vector2Int cell, bool heavenSide)
    {
        return Map[heavenSide].ContainsKey(cell) && Map[heavenSide][cell] == null;
    }

    public bool CheckAvailability(Vector3 worldPosition, bool heavenSide)
    {
        Vector2Int cell = (Vector2Int)(heavenSide ? _heavenGrid : _hellGrid).WorldToCell(worldPosition);
        return CheckAvailability(cell, heavenSide);
    }

    private void OnLevelStarted()
    {
        _gridInitializer.InitializeMap();
    }
}
