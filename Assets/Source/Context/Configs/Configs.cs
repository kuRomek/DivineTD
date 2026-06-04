using UnityEngine;

public static class Configs
{
    public static MainCameraConfig MainCamera { get; private set; }
    public static LevelsConfig Levels { get; private set; }
    public static BuildingsConfig Buildings { get; private set; }
    public static UnitsConfig Units { get; private set; }
    public static GridConfig Grid { get; private set; }

    public static void Load()
    {
        MainCamera = Resources.Load<MainCameraConfig>(nameof(MainCamera));
        Levels = Resources.Load<LevelsConfig>(nameof(Levels));
        Buildings = Resources.Load<BuildingsConfig>(nameof(Buildings));
        Units = Resources.Load<UnitsConfig>(nameof(Units));
        Grid = Resources.Load<GridConfig>(nameof(Grid));
    }
}