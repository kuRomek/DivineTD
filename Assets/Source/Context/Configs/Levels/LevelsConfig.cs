using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Levels", menuName = "Configs/Levels")]
public class LevelsConfig : ScriptableObject
{
    [SerializeField] private List<LevelData> _dataHeaven;
    [SerializeField] private List<LevelData> _dataHell;

    public IReadOnlyList<LevelData> DataHeaven => _dataHeaven;
    public IReadOnlyList<LevelData> DataHell => _dataHell;

    public Map GetMapPrefab(bool heaven, int levelNumber)
    {
        var dataList = heaven ? _dataHeaven : _dataHell;

        levelNumber = (levelNumber - 1) % dataList.Count;

        return dataList[levelNumber].Map;
    }
}
