using System;
using UnityEngine;

public class CheckpointModel : GridObjectModel
{
    public CheckpointModel(Transform transform, Faction faction, bool isDraft, int number)
        : base(transform, faction, isDraft)
    {
        SetNumber(number);
    }

    public int Number { get; private set; }

    public event Action<int> NumberChanged;

    public void SetNumber(int number)
    {
        Number = number;
        NumberChanged?.Invoke(number);
    }
}