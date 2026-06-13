using AYellowpaper.SerializedCollections;
using UnityEngine;

public class GridObjectsFactory : MonoBehaviour
{
    [Header("Heaven Tower Prefabs")]
    [SerializeField] private SerializedDictionary<TowerType, TowerView> _heavenTowers;

    [Header("Hell Tower Prefabs")]
    [SerializeField] private SerializedDictionary<TowerType, TowerView> _hellTowers;

    [Header("Other Prefabs")]
    [SerializeField] private CastleView _castlePrefab;
    [SerializeField] private ObstacleView _obstaclePrefab;
    [SerializeField] private SpawnPointView _spawnPointPrefab;
    [SerializeField] private CheckpointView _checkpointPrefab;

    private static GridObjectsFactory _instance;

    private GridSystem _gridSystem;

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError($"Multiple instances of {nameof(GridObjectsFactory)} detected. Leaving the last instantiated one.");
            Destroy(_instance.gameObject);
        }

        _instance = this;
    }

    public void Construct(GridSystem gridSystem)
    {
        _gridSystem = gridSystem;
    }

    public TowerModel CreateTower(Faction faction, TowerType type, bool isDraft)
    {
        TowerView tower = Instantiate(faction == Faction.Heaven ? _heavenTowers[type] : _hellTowers[type]);
        TowerModel towerModel = new(tower.transform, isDraft, faction, type);
        TriggerDetector triggerDetector = tower.GetComponentInChildren<TriggerDetector>();

        if (triggerDetector != null)
            triggerDetector.AttachComponents(null, towerModel);

        tower.AttachPresenter(new Tower(tower, towerModel, _gridSystem, triggerDetector, tower.GunTip));

        tower.ToggleBuildingIndicator(isDraft);

        return towerModel;
    }

    public ObstacleModel CreateObstacle(Faction faction, bool draft)
    {
        ObstacleView obstacle = Instantiate(_obstaclePrefab);
        ObstacleModel towerModel = new(obstacle.transform, faction, draft);

        obstacle.AttachPresenter(new Obstacle(obstacle, towerModel, _gridSystem));
        obstacle.ToggleBuildingIndicator(draft);

        return towerModel;
    }

    public CastleModel CreateCastle(Faction faction, bool draft)
    {
        var castlesData = Configs.Levels.GetCastleData(GameState.CurrentPlayerFaction, GameState.CurrentLevel);
        int healthAmount = castlesData[faction];

        CastleView castleView = Instantiate(_castlePrefab);

        HealthModel health = new(castleView.HealthBar.transform, healthAmount, healthAmount);
        castleView.HealthBar.AttachPresenter(new Health(castleView.HealthBar, health));

        CastleModel castle = new(castleView.transform, health, faction, draft);
        castleView.AttachPresenter(new Castle(castleView, castle, _gridSystem));
        castleView.TriggerDetector.AttachComponents(castle, castle);

        return castle;
    }

    public SpawnPointModel CreateSpawnPoint(Faction faction, bool draft)
    {
        SpawnPointView spawnPoint = Instantiate(_spawnPointPrefab);
        SpawnPointModel spawnPointModel = new(spawnPoint.transform, faction, draft);

        spawnPoint.AttachPresenter(new SpawnPoint(spawnPoint, spawnPointModel, _gridSystem));
        spawnPoint.ToggleBuildingIndicator(draft);

        return spawnPointModel;
    }

    public CheckpointModel CreateCheckpoint(Faction faction, bool draft)
    {
        CheckpointView checkpoint = Instantiate(_checkpointPrefab);
        CheckpointModel checkpointModel = new(checkpoint.transform, faction, draft);

        checkpoint.AttachPresenter(new Checkpoint(checkpoint, checkpointModel, _gridSystem));
        checkpoint.ToggleBuildingIndicator(draft);

        return checkpointModel;
    }
}