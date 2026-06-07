using NaughtyAttributes;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private MeshRenderer _mesh;

    [field: ReadOnly, SerializeField] public GridObjectModel Object { get; private set; }

    public void Initialize(bool heaven)
    {
        //_mesh.material = Configs.Grid.GetTileMaterial(heaven);
    }

    public void SetObject(GridObjectModel @object)
    {
        RemoveObject()?.Destroy();
        Object = @object;
        @object?.Transform.SetParent(transform);
    }

    public GridObjectModel RemoveObject()
    {
        var @object = Object;
        Object = null;
        return @object;
    }

    public void Destroy()
    {
        Object?.Destroy();
        Destroy(gameObject);
    }
}