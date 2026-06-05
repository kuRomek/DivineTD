using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Levels", menuName = "Configs/Levels")]
public class LevelsConfig : ScriptableObject
{
    [SerializeField] private List<LevelData> _heavenSectionData;
    [SerializeField] private List<LevelData> _hellSectionData;

    public IReadOnlyList<LevelData> HeavenSection => _heavenSectionData;
    public IReadOnlyList<LevelData> HellSection => _hellSectionData;

    public MapData GetMapData(Faction faction, int levelNumber)
    {
        var dataList = faction == Faction.Heaven ? _heavenSectionData : _hellSectionData;

        return dataList[GetLevelIndex(levelNumber, dataList)].Map;
    }

    public void SaveMapData(MapData data, int levelNumber, bool heaven)
    {
        var dataList = heaven ? _heavenSectionData : _hellSectionData;

        var levelData = dataList[GetLevelIndex(levelNumber, dataList)];
        levelData.Map = data;
        dataList[GetLevelIndex(levelNumber, dataList)] = levelData;
    }

    private int GetLevelIndex(int levelNumber, ICollection dataList)
    {
        return (levelNumber - 1) % dataList.Count;
    }
}
