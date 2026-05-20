using kuRomek.SimpleVG;
using UnityEngine;

public static class ContextLoader
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Load()
    {
        Utils.InstallerLoader.Load<InputController>(nameof(InputController));
    }
}