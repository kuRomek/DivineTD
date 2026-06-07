using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridInitializer
{
    private readonly GridSystem _gridSystem;
    private readonly TowerFactory _towerFactory;

    public GridInitializer(GridSystem gridSystem, TowerFactory towerFactory)
    {
        _gridSystem = gridSystem;
        _towerFactory = towerFactory;
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
                MapCastleData castle => InitializeCastle(castle, faction),
                MapTowerData tower => InitializeTower(tower, faction),
                _ => null,
            });
        }
    }

    private GridObjectModel InitializeCastle(MapCastleData data, Faction faction)
    {
        CastleView castleView = Object.Instantiate(Configs.Buildings.CastlePrefab);

        HealthModel castleHealthModel = new(castleView.HealthBar.transform, 100f, 100f);
        castleView.HealthBar.AttachPresenter(new Health(castleView.HealthBar, castleHealthModel));

        CastleModel castleModel = new(castleView.transform, castleHealthModel, faction, false);
        castleView.AttachPresenter(new Castle(castleView, castleModel, _gridSystem));

        castleView.TriggerDetector.AttachComponents(castleModel, castleModel);

        return castleModel;
    }

    private GridObjectModel InitializeTower(MapTowerData data, Faction faction)
    {
        return _towerFactory.CreateTower(faction, data.Type, false);
    }
}