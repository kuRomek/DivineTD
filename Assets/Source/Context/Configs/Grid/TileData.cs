using System;
using UnityEngine;

[Serializable]
public struct TileData
{
    [SerializeReference] public MapGridObjectData Object;
}
