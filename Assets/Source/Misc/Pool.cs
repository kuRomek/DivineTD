using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : MonoBehaviour, IPooledObject
{
    private readonly Queue<T> _objects = new();
    private readonly T _prefab;

    public Pool(T prefab)
    {
        _prefab = prefab;
    }

    public T Get()
    {
        T @object = _objects.Count > 0 ? _objects.Dequeue() : Object.Instantiate(_prefab);
        @object.OnGet();

        return @object;
    }

    public void Release(T @object)
    {
        _objects.Enqueue(@object);
        @object.OnRelease();
    }
}
