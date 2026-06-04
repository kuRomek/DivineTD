using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private MainCamera _mainCamera;

    [Header("Grids")]
    [SerializeField] private Grid _heavenGrid;
    [SerializeField] private Grid _hellGrid;
    [SerializeField] private Map _map;

    [Header("Castles")]
    [SerializeField] private Transform _heavenCameraInitialSpot;
    [SerializeField] private Transform _hellCameraInitialSpot;

    [Header("UI")]
    [SerializeField] private WidgetCanvas _widgetCanvas;

    [Header("Factories")]
    [SerializeField] private TowerFactory _towerFactory;
    [SerializeField] private UnitFactory _unitFactory;

    public static SimpleDI Container;

    private LevelsSystem _levelsSystem;

    private void Awake()
    {
        Container = new SimpleDI();

        GameState.LoadSave();

        _levelsSystem = new();

        GridSystem grid = new(_levelsSystem, _heavenGrid, _hellGrid, _map, _mainCamera.Camera);
        BuildingSystem building = new(grid, _towerFactory, _mainCamera);

        Container.Register(grid);
        Container.Register(building);
        Container.Register(_levelsSystem);
        Container.Register(_widgetCanvas);
        Container.Register(_mainCamera);
        Container.Register(_towerFactory);
        Container.Register(_unitFactory);

        InjectScene(Container);
    }

    private void OnEnable()
    {
        _levelsSystem.LevelStarted += OnLevelStarted;
        _levelsSystem.LevelEnded += OnLevelEnded;
    }

    private void OnDisable()
    {
        _levelsSystem.LevelStarted -= OnLevelStarted;
        _levelsSystem.LevelEnded -= OnLevelEnded;
    }

    private void Start()
    {
        _levelsSystem.StartLevel();
    }

    private void InjectScene(SimpleDI di)
    {
        var objects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        foreach (var @object in objects)
            di.InjectConstructors(@object);
    }

    private void OnLevelStarted()
    {
        if (GameState.IsCurrentFactionHeaven)
            _mainCamera.transform.position = _heavenCameraInitialSpot.position;
        else
            _mainCamera.transform.position = _hellCameraInitialSpot.position;
    }

    private void OnLevelEnded()
    {

    }
}
