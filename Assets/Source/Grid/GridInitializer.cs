using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridInitializer
{
    private readonly GridSystem _gridSystem;

    public GridInitializer(GridSystem gridSystem)
    {
        _gridSystem = gridSystem;
    }

    public void InitializeMap()
    {
        Map mapPrefab = Configs.Levels.GetMapPrefab(GameState.IsCurrentFactionHeaven, GameState.CurrentLevel);

        InitializeFactionTiles(mapPrefab[true], true, _gridSystem.Map);
        InitializeFactionTiles(mapPrefab[false], false, _gridSystem.Map);
    }

    private void InitializeFactionTiles(IEnumerable<KeyValuePair<Vector2Int, Tile>> cells, bool heaven, Map mapToInit)
    {
        while (mapToInit[heaven].Count != 0)
            mapToInit.RemoveTile(heaven, mapToInit[heaven].First().Key);

        foreach (var (cell, tile) in cells)
            mapToInit.PlaceTile(heaven, cell, (cell) => _gridSystem.GetWorldPosition(heaven, cell));
    }


    private void InitializeCastle(Vector2Int cell, CastleModel castleModel)
    {
        CastleView castleView = Object.Instantiate(Configs.Buildings.CastlePrefab);

        HealthModel castleHealthModel = new(castleView.HealthBar.transform, 100f, 100f);
        castleView.HealthBar.AttachPresenter(new Health(castleView.HealthBar, castleHealthModel));
        castleView.AttachPresenter(new Castle(castleView, castleModel, _gridSystem));

        castleView.TriggerDetector.AttachComponents(castleModel, castleModel);
    }
}