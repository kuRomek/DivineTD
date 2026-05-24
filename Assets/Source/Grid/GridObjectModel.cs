using kuRomek.SimpleVG;
using UnityEngine;

public class GridObjectModel : Model
{
    public bool IsDraft { get; private set; }
    public bool IsHeavenFaction { get; private set; }

    public GridObjectModel(Transform transform, bool isHeavenFaction, bool isDraft) : base(transform)
    {
        IsHeavenFaction = isHeavenFaction;
        IsDraft = isDraft;
    }

    public void ToggleDrafting(bool isDraft)
        => IsDraft = isDraft;
}
