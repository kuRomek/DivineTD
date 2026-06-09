using UnityEngine;

public class MapEditorRoot : MonoBehaviour
{
    [SerializeField] private MainCamera _mainCamera;
    [SerializeField] private Transform _cursor;

    [Header("Map")]
    [SerializeField] private Map _map;

    [Header("UI")]
    [SerializeField] private WidgetCanvas _widgetCanvas;

    [Header("Factories")]
    [SerializeField] private GridObjectsFactory _gridObjectsFactory;
    [SerializeField] private UnitFactory _unitFactory;

    public static SimpleDI Container;

    private void Awake()
    {
        Container = new SimpleDI();

        GameState.LoadSave();

        GridSystem grid = new(null, _map, _gridObjectsFactory, _mainCamera.Camera);
        MapEditingSystem mapEditing = new(_mainCamera, grid, _gridObjectsFactory, _cursor);

        Container.Register(grid);
        Container.Register(mapEditing);
        Container.Register(_mainCamera);
        Container.Register(_widgetCanvas);
        Container.Register(_gridObjectsFactory);
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