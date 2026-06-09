using kuRomek.SimpleVG;
using UnityEngine;

public class GridSystem
{
    private readonly LevelsSystem _levelsSystem;
    private readonly Grid _heavenGrid;
    private readonly Grid _hellGrid;
    private readonly Camera _camera;
    private readonly GridInitializer _gridInitializer;

    public GridSystem(LevelsSystem levelsSystem, Map map, GridObjectsFactory gridObjectsFactory, Camera camera)
    {
        _levelsSystem = levelsSystem;
        _heavenGrid = map.HeavenGrid;
        _hellGrid = map.HellGrid;
        _camera = camera;
        _gridInitializer = new(this, gridObjectsFactory);

        Map = map;

        if (_levelsSystem != null)
            _levelsSystem.LevelStarted += OnLevelStarted;
        else
            OnLevelStarted();
    }

    public Map Map { get; }

    public bool TryPlace(GridObjectModel gridObject, Faction faction)
    {
        Grid grid = faction == Faction.Heaven ? _heavenGrid : _hellGrid;

        Vector2Int cell = (Vector2Int)grid.WorldToCell(gridObject.Transform.position);

        if (CheckTileAvailability(cell, faction))
        {
            Map.PlaceObjectOnTile(faction, cell, gridObject);
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
            gridObject.MoveAt(GetSnappedPosition(gridObject.Faction, hit.point));
    }

    public void Drag(Transform @object, Faction faction)
    {
        Ray ray = _camera.ScreenPointToRay(InputController.Current.Position);

        if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, LayerMask.GetMask("Ground")))
            @object.position = GetSnappedPosition(faction, hit.point);
    }

    public Vector3 GetSnappedPosition(Faction faction, Vector3 worldPosition)
    {
        Vector3Int cell = (faction == Faction.Heaven ? _heavenGrid : _hellGrid).WorldToCell(worldPosition);

        return GetWorldPosition(faction, (Vector2Int)cell);
    }

    public Vector3 GetSnappedPosition(Faction faction)
    {
        Ray ray = _camera.ViewportPointToRay(Vector2.one * 0.5f);

        if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, LayerMask.GetMask("Ground")))
            return GetSnappedPosition(faction, hit.point);

        Debug.LogWarning("Beyond ground edges.");
        return -Vector3.one;
    }

    public Vector2Int GetCell(Faction faction, Vector3 worldPosition)
    {
        return (Vector2Int)(faction == Faction.Heaven ? _heavenGrid : _hellGrid).WorldToCell(worldPosition);
    }

    public Vector3 GetWorldPosition(Faction faction, Vector2Int cell)
    {
        Grid grid = faction == Faction.Heaven ? _heavenGrid : _hellGrid;

        Vector3 position = grid.CellToWorld((Vector3Int)cell) +
            grid.transform.rotation * new Vector3(grid.cellSize.x, 0f, grid.cellSize.y) / 2f;

        return position;
    }

    public bool CheckIfTileExist(Vector2Int cell, Faction faction)
    {
        return Map[faction].ContainsKey(cell);
    }

    public bool CheckTileAvailability(Vector2Int cell, Faction faction)
    {
        return Map[faction].ContainsKey(cell) && Map[faction][cell].Object == null;
    }

    public bool CheckTileAvailability(Vector3 worldPosition, Faction faction)
    {
        return CheckTileAvailability(GetCell(faction, worldPosition), faction);
    }

    private void OnLevelStarted()
    {
        _gridInitializer.InitializeMap();
    }
}
