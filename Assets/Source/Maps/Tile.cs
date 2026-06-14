using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private MeshRenderer _mesh;

    public GridObjectModel Object { get; private set; }

    public bool Walkable => Object == null || Object is CheckpointModel || Object is CastleModel;

    public void SetObject(GridObjectModel @object)
    {
        RemoveObject()?.Destroy();
        Object = @object;

        if (@object != null)
        {
            @object.Transform.SetParent(transform, false);
            @object.Transform.localPosition = default;
        }
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