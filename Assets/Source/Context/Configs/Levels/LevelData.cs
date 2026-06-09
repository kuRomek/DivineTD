using System;
using UnityEngine;

[Serializable]
public struct LevelData
{
    //[ReadOnly, AllowNesting]
    [SerializeField] public MapData Map;
    public int HeavenCastleHealth;
    public int HellCastleHealth;
}