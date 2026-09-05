using UnityEngine.SceneManagement;

public class MainMenuButton : MenuButton
{
    private WindowsSystem _windowsSystem;

    private void Construct(WindowsSystem windowsSystem)
    {
        _windowsSystem = windowsSystem;
    }

    protected override void OnButtonClicked()
    {
        _windowsSystem.CloseCurrentWindow();
        SceneManager.LoadScene((int)Scenes.Menu);
    }
}