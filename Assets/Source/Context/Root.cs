using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private MainCamera _mainCamera;
    [SerializeField] private Grid _heavenGrid;
    [SerializeField] private Grid _hellGrid;

    [Header("UI")]
    [SerializeField] private WidgetCanvas _widgetCanvas;

    [Header("Factories")]
    [SerializeField] private TowerFactory _towerFactory;

    private void Awake()
    {
        SimpleDI di = new SimpleDI();

        GridSystem gridSystem = new(_heavenGrid, _hellGrid, _mainCamera.Camera);
        BuildingSystem buildingSystem = new(gridSystem, _towerFactory, _mainCamera);

        di.Register(gridSystem);
        di.Register(buildingSystem);
        di.Register(_widgetCanvas);

        InjectScene(di);
    }

    private void InjectScene(SimpleDI di)
    {
        var objects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        foreach (var @object in objects)
            di.InjectConstructors(@object);
    }
}
