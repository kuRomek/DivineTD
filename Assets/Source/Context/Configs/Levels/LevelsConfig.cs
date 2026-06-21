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

    public MapData GetMapData(Faction faction, int levelNumber)
    {
        var dataList = faction == Faction.Heaven ? _heavenSectionData : _hellSectionData;

        if (levelNumber == dataList.Count + 1)
        {
            SaveMapData(new(true), faction, levelNumber);
            return dataList[dataList.Count - 1].Map;
        }

        return dataList[GetLevelIndex(levelNumber, dataList)].Map;
    }

#if UNITY_EDITOR
    public void SaveMapData(MapData data, Faction faction, int levelNumber)
    {
        Undo.RecordObject(this, $"{faction} map on level {levelNumber} saved.");

        var dataList = faction == Faction.Heaven ? _heavenSectionData : _hellSectionData;

        if (levelNumber == dataList.Count + 1)
        {
            dataList.Add(new LevelData() { HeavenCastleHealth = 100, HellCastleHealth = 100, Map = data });
        }
        else
        {
            var levelData = dataList[GetLevelIndex(levelNumber, dataList)];
            levelData.Map = data;
            dataList[GetLevelIndex(levelNumber, dataList)] = levelData;
        }

        EditorUtility.SetDirty(this);
        Debug.Log("Map saved successfully");
    }
#endif

    public IReadOnlyDictionary<Faction, int> GetCastleData(Faction faction, int levelNumber)
    {
        Dictionary<Faction, int> data = new();

        var section = faction == Faction.Heaven ? HeavenSection : HellSection;

        data.Add(Faction.Heaven, section[GetLevelIndex(levelNumber, section)].HeavenCastleHealth);
        data.Add(Faction.Hell, section[GetLevelIndex(levelNumber, section)].HellCastleHealth);

        return data;
    }

    private int GetLevelIndex(int levelNumber, IReadOnlyList<LevelData> dataList)
    {
        return Mathf.Max(levelNumber - 1, 0) % dataList.Count;
    }
}
