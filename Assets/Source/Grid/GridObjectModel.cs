using System;
using kuRomek.SimpleVG;
using UnityEngine;

public class GridObjectModel : Model, IFactionRelated
{
    public GridObjectModel(Transform transform, Faction faction, bool isDraft) : base(transform)
    {
        Faction = faction;
        IsDraft = isDraft;
    }

    public event Action<bool> Moved;
    public event Action<bool> ToggledDrafting;
    public event Action Destroyed;

    public bool IsDraft { get; private set; }
    public Faction Faction { get; private set; }
    public bool AlwaysFollowCursor { get; private set; }

    public void MoveAt(Vector3 worldPosition)
    {
        bool differentPosition = Transform.position != worldPosition;
        Transform.position = worldPosition;

        Moved?.Invoke(differentPosition);
    }

    public void SetCursorFollowing(bool alwaysFollowCursor)
    {
        AlwaysFollowCursor = alwaysFollowCursor;
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
