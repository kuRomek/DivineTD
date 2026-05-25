using System;
using kuRomek.SimpleVG;
using UnityEngine;

public class GridObjectModel : Model
{
    public GridObjectModel(Transform transform, bool isHeavenFaction, bool isDraft) : base(transform)
    {
        IsHeavenFaction = isHeavenFaction;
        IsDraft = isDraft;
    }

    public bool IsDraft { get; private set; }
    public bool IsHeavenFaction { get; private set; }

    public event Action<bool> Moved;
    public event Action<bool> ToggledDrafting;

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
}
