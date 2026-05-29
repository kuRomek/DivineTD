using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameState
{
    public static bool IsCurrentFactionHeaven { get; private set; }

    public static void SwitchFaction(bool toHeaven)
    {
        if (SceneManager.GetActiveScene().name == "Main")
        {
            Debug.Log("Cannot switch faction while in game.");
            return;
        }

        IsCurrentFactionHeaven = toHeaven;
    }
}
