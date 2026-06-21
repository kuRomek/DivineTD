using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Economy", menuName = "Configs/Economy")]
public class EconomyConfig : ScriptableObject
{
    private readonly Dictionary<Faction, float> _cooldowns;

    public IReadOnlyDictionary<Faction, float> StartingCooldowns => _cooldowns;
}