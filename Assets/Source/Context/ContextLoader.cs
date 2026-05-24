using UnityEngine;

public static class ContextLoader
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Load()
    {
    }
}