using System;
using UnityEngine;

public class CheckpointModel : GridObjectModel
{
    public CheckpointModel(Transform transform, Faction faction, bool isDraft) : base(transform, faction, isDraft)
    {
    }

    public event Action<int> NumberChanged;

    public void SetNumber(int number)
    {
        NumberChanged?.Invoke(number);
    }
}