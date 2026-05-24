using AYellowpaper.SerializedCollections;
using UnityEngine;

public class TowerFactory : MonoBehaviour
{
    [Header("Essentials")]
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Grid _heavenGrid;
    [SerializeField] private Grid _hellGrid;

    [Header("Heaven Tower Prefabs")]
    [SerializeField] private SerializedDictionary<TowerType, TowerView> _heavenTowers;

    [Header("Hell Tower Prefabs")]
    [SerializeField] private SerializedDictionary<TowerType, TowerView> _hellTowers;

    private static TowerFactory _instance;

    private GridSystem _gridSystem;

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError($"Multiple instances of {nameof(TowerFactory)} detected. Leaving the last instantiated one.");
            Destroy(_instance.gameObject);
        }

        _instance = this;
    }

    public void Construct(GridSystem gridSystem)
    {
        _gridSystem = gridSystem;
    }

    public TowerModel CreateTower(bool isHeavenFaction, TowerType type, bool isDraft)
    {
        TowerView tower = Instantiate(isHeavenFaction ? _heavenTowers[type] : _hellTowers[type]);
        TowerModel towerModel = new(tower.transform, isDraft, isHeavenFaction);
        tower.AttachPresenter(new Tower(tower, towerModel, _gridSystem));

        return towerModel;
    }
}
