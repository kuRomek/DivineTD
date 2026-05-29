using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private MainCamera _mainCamera;

    [Header("Grids")]
    [SerializeField] private Grid _heavenGrid;
    [SerializeField] private Grid _hellGrid;

    [Header("Castles")]
    [SerializeField] private CastleView _heavenCastle;
    [SerializeField] private CastleView _hellCastle;

    [Header("UI")]
    [SerializeField] private WidgetCanvas _widgetCanvas;

    [Header("Factories")]
    [SerializeField] private TowerFactory _towerFactory;
    [SerializeField] private UnitFactory _unitFactory;

    public static SimpleDI Container;

    private void Awake()
    {
        Container = new SimpleDI();

        if (GameState.IsCurrentFactionHeaven == false)
        {
            _mainCamera.transform.position = new(0f, _mainCamera.transform.position.y, -_mainCamera.transform.position.z);
            _mainCamera.transform.Rotate(new(0f, 180f, 0f), Space.World);
        }

        GridSystem gridSystem = new(_heavenGrid, _hellGrid, _mainCamera.Camera);
        BuildingSystem buildingSystem = new(gridSystem, _towerFactory, _mainCamera);

        Container.Register(gridSystem);
        Container.Register(buildingSystem);
        Container.Register(_widgetCanvas);
        Container.Register(_mainCamera);
        Container.Register(_towerFactory);
        Container.Register(_unitFactory);

        InitializeCastle(_heavenCastle, true);
        InitializeCastle(_hellCastle, false);

        InjectScene(Container);
    }

    private void InjectScene(SimpleDI di)
    {
        var objects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        foreach (var @object in objects)
            di.InjectConstructors(@object);
    }

    private void InitializeCastle(CastleView castleView, bool isHeavenFaction)
    {
        HealthView healthView = castleView.GetComponentInChildren<HealthView>(true);
        TriggerDetector triggerDetector = castleView.GetComponentInChildren<TriggerDetector>();

        if (healthView != null || triggerDetector != null)
        {
            HealthModel castleHealthModel = new(healthView.transform, 100f, 100f);
            healthView.AttachPresenter(new Health(healthView, castleHealthModel));
            CastleModel castleModel = new(castleView.transform, castleHealthModel, isHeavenFaction);
            castleView.AttachPresenter(new Castle(castleView, castleModel));

            triggerDetector.AttachComponents(castleModel, castleModel);
        }
        else
        {
            Debug.Log($"Health view or trigger detector has not been assigned to {castleView.name}.");
        }
    }
}
