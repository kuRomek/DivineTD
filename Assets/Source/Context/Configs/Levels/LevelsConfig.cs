using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Levels", menuName = "Configs/Levels")]
public class LevelsConfig : ScriptableObject
{
    [SerializeField] private List<LevelData> _heavenSectionData;
    [SerializeField] private List<LevelData> _hellSectionData;

    [field: SerializeField] public LevelButton LevelButtonPrefab { get; private set; }

    public IReadOnlyList<LevelData> HeavenSection => _heavenSectionData;
    public IReadOnlyList<LevelData> HellSection => _hellSectionData;

    public MapData GetMapData(Faction section, int levelNumber)
    {
        var dataList = section == Faction.Heaven ? _heavenSectionData : _hellSectionData;

        if (levelNumber == dataList.Count + 1)
        {
            SaveMapData(new(true), section, levelNumber);
            return dataList[dataList.Count - 1].Map;
        }

        return dataList[ConvertToLevelIndex(levelNumber, dataList)].Map;
    }

    public int GetCastleData(Faction levelsSection, Faction faction, int levelNumber)
    {
        var section = levelsSection == Faction.Heaven ? HeavenSection : HellSection;

        return section[ConvertToLevelIndex(levelNumber, section)].GetCastleHealth(faction);
    }

    public (int Income, int Amount) GetFunds(Faction levelsSection, Faction faction, int levelNumber)
    {
        var section = levelsSection == Faction.Heaven ? HeavenSection : HellSection;

        return section[ConvertToLevelIndex(levelNumber, section)].GetFunds(faction);
    }

#if UNITY_EDITOR
    public void SaveMapData(MapData data, Faction faction, int levelNumber)
    {
        Undo.RecordObject(this, $"{faction} map on level {levelNumber} saved.");

        var dataList = faction == Faction.Heaven ? _heavenSectionData : _hellSectionData;

        if (levelNumber == dataList.Count + 1)
        {
            dataList.Add(new LevelData(data));
        }
        else
        {
            var levelData = dataList[ConvertToLevelIndex(levelNumber, dataList)];
            levelData.Map = data;
            dataList[ConvertToLevelIndex(levelNumber, dataList)] = levelData;
        }

        EditorUtility.SetDirty(this);
        Debug.Log("Map saved successfully");
    }
#endif

    private int ConvertToLevelIndex(int levelNumber, IReadOnlyList<LevelData> dataList)
    {
        return Mathf.Max(levelNumber - 1, 0) % dataList.Count;
    }
}
