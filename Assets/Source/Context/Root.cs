using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.U2D;

public class Root : MonoBehaviour
{
    [SerializeField] private MainCamera _mainCamera;

    [Header("Map")]
    [SerializeField] private Map _map;

    [Header("UI")]
    [SerializeField] private WidgetCanvas _widgetCanvas;

    [Header("Path Splines")]
    [SerializeField] private SerializedDictionary<Faction, SpriteShapeController> _pathSplines;

    [Header("Factories")]
    [SerializeField] private GridObjectsFactory _gridObjectsFactory;
    [SerializeField] private UnitFactory _unitFactory;

    public static SimpleDI Container;

    private LevelsSystem _levelsSystem;

    private void Awake()
    {
        Container = new SimpleDI();

        GameState.LoadSave();

        _levelsSystem = new();

        GridSystem grid = new(_levelsSystem, _map, _gridObjectsFactory, _mainCamera.Camera);
        PathFindingSystem pathFinding = new(grid, _pathSplines);
        BuildingSystem building = new(grid, _gridObjectsFactory, _mainCamera);

        Container.Register(_levelsSystem);
        Container.Register(grid);
        Container.Register(building);
        Container.Register(pathFinding);
        Container.Register(_widgetCanvas);
        Container.Register(_mainCamera);
        Container.Register(_gridObjectsFactory);
        Container.Register(_unitFactory);

        InjectScene(Container);
    }

    private void Start()
    {
        _levelsSystem.StartLevel();
    }

    private void Update()
    {
        Container.Update(Time.deltaTime);
    }

    private void InjectScene(SimpleDI di)
    {
        var objects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        foreach (var @object in objects)
            di.InjectConstructors(@object);
    }
}
