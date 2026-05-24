using System;
using UnityEngine;

public class TowerModel : GridObjectModel
{
    public TowerModel(Transform transform, bool isDraft, bool isHeavenFaction) : base(transform, isDraft, isHeavenFaction)
    {
    }

    public event Action Destroyed;

    public void Destroy()
    {
        Destroyed?.Invoke();
    }
}