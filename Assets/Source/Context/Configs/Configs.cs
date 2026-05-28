using UnityEngine;

public static class Configs
{
    public static MainCameraConfig MainCamera { get; private set; }
    public static UnitsConfig Units { get; private set; }
    public static TowersConfig Towers { get; private set; }

    public static void LoadLocalConfigs()
    {
        MainCamera = Resources.Load<MainCameraConfig>(nameof(MainCamera));
        Units = Resources.Load<UnitsConfig>(nameof(Units));
        Towers = Resources.Load<TowersConfig>(nameof(Towers));
    }
}