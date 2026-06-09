using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridInitializer
{
    private readonly GridSystem _gridSystem;
    private readonly GridObjectsFactory _gridObjectFactory;

    public GridInitializer(GridSystem gridSystem, GridObjectsFactory gridObjectFactory)
    {
        _gridSystem = gridSystem;
        _gridObjectFactory = gridObjectFactory;
    }

    public void InitializeMap()
    {
        MapData mapData = Configs.Levels.GetMapData(GameState.CurrentPlayerFaction, GameState.CurrentLevel);

        InitializeFactionTiles(mapData[Faction.Heaven], Faction.Heaven, _gridSystem.Map);
        InitializeFactionTiles(mapData[Faction.Hell], Faction.Hell, _gridSystem.Map);
    }

    private void InitializeFactionTiles(IEnumerable<KeyValuePair<Vector2Int, TileData>> cells, Faction faction, Map mapToInit)
    {
        while (mapToInit[faction].Count != 0)
            mapToInit.RemoveTile(faction, mapToInit[faction].First().Key);

        foreach (var (cell, tile) in cells)
        {
            mapToInit.PlaceTile(faction, cell, (cell) => _gridSystem.GetWorldPosition(faction, cell));
            mapToInit.PlaceObjectOnTile(faction, cell, tile.Object switch
            {
                MapCastleData => _gridObjectFactory.CreateCastle(faction, false),
                MapTowerData tower => _gridObjectFactory.CreateTower(faction, tower.Type, false),
                MapObstacleData => _gridObjectFactory.CreateObstacle(faction, false),
                _ => null,
            });
        }
    }
}