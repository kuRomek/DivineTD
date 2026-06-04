using System;
using kuRomek.SimpleVG;
using UnityEngine;

[Serializable]
public class GridObjectModel : Model, IFactionRelated
{
    public GridObjectModel(Transform transform, bool heavenFaction, bool isDraft) : base(transform)
    {
        IsHeavenFaction = heavenFaction;
        IsDraft = isDraft;
    }

    public event Action<bool> Moved;
    public event Action<bool> ToggledDrafting;
    public event Action Destroyed;

    public bool IsDraft { get; private set; }
    public bool IsHeavenFaction { get; private set; }

    public void MoveAt(Vector3 worldPosition)
    {
        bool differentPosition = Transform.position != worldPosition;
        Transform.position = worldPosition;

        Moved?.Invoke(differentPosition);
    }

    public void ToggleDrafting(bool isDraft)
    {
        IsDraft = isDraft;
        ToggledDrafting?.Invoke(isDraft);
    }

    public void Destroy()
    {
        Destroyed?.Invoke();
    }
}
