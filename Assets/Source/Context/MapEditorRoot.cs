using UnityEngine;

public class MapEditorRoot : MonoBehaviour
{
    [SerializeField] private MainCamera _mainCamera;
    [SerializeField] private Transform _cursor;

    [Header("Grids")]
    [SerializeField] private Grid _heavenGrid;
    [SerializeField] private Grid _hellGrid;
    [SerializeField] private Map _map;

    [Header("Camera Spots")]
    [SerializeField] private Transform _heavenCameraInitialSpot;
    [SerializeField] private Transform _hellCameraInitialSpot;

    [Header("UI")]
    [SerializeField] private WidgetCanvas _widgetCanvas;

    [Header("Factories")]
    [SerializeField] private TowerFactory _towerFactory;
    [SerializeField] private UnitFactory _unitFactory;

    public static SimpleDI Container;

    private void Awake()
    {
        Container = new SimpleDI();

        GameState.LoadSave();

        _mainCamera.transform.position = _heavenCameraInitialSpot.position;

        GridSystem grid = new(null, _heavenGrid, _hellGrid, _map, _towerFactory, _mainCamera.Camera);
        BuildingSystem building = new(grid, _towerFactory, _mainCamera);
        MapEditingSystem mapEditing = new(_mainCamera, grid, _towerFactory, _cursor);

        Container.Register(grid);
        Container.Register(building);
        Container.Register(mapEditing);
        Container.Register(_mainCamera);
        Container.Register(_widgetCanvas);
        Container.Register(_towerFactory);
        Container.Register(_unitFactory);

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