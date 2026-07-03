using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "Economy", menuName = "Configs/Economy")]
public class EconomyConfig : ScriptableObject
{
    [SerializeField] private SerializedDictionary<Faction, float> _cooldowns;

    public IReadOnlyDictionary<Faction, float> StartingCooldowns => _cooldowns;
}