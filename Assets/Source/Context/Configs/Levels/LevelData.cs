using System;
using NaughtyAttributes;
using UnityEngine;

[Serializable]
public struct LevelData
{
    [/* ReadOnly, AllowNesting,  */SerializeField] public MapData Map;
    public int HeavenCastleHealth;
    public int HellCastleHealth;
}