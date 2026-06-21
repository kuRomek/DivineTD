using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.U2D;

public class EditorRoot : MonoBehaviour
{
    [SerializeField] private MainCamera _mainCamera;
    [SerializeField] private Transform _cursor;

    [Header("Map")]
    [SerializeField] private Map _map;

    [Header("UI")]
    [SerializeField] private WidgetCanvas _widgetCanvas;

    [Header("Path Splines")]
    [SerializeField] private SerializedDictionary<Faction, SpriteShapeController> _pathSplines;

    [Header("Factories")]
    [SerializeField] private GridObjectsFactory _gridObjectsFactory;

    public static SimpleDI Container;

    private void Awake()
    {
        Container = new SimpleDI();

        GameState.LoadSave();

        GridSystem grid = new(null, _map, _gridObjectsFactory, _mainCamera.Camera);
        PathFindingSystem pathFinding = new(grid, _pathSplines);
        MapEditingSystem mapEditing = new(_mainCamera, grid, _gridObjectsFactory, _cursor);

        Container.Register(grid);
        Container.Register(pathFinding);
        Container.Register(mapEditing);
        Container.Register(_mainCamera);
        Container.Register(_widgetCanvas);
        Container.Register(_gridObjectsFactory);

        InjectScene(Container);
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