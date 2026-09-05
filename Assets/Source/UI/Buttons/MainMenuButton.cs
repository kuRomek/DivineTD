using UnityEngine.SceneManagement;

public class MainMenuButton : MenuButton
{
    protected override void OnButtonClicked()
    {
        SceneManager.LoadScene((int)Scenes.Menu);
    }
}