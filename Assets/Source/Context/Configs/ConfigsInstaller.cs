using UnityEngine;
using kuRomek.SimpleVG;

public partial class ConfigsInstaller : Installer
{
    private static ConfigsInstaller _instance;

    protected override void Install()
    {
        if (_instance != null)
        {
            Debug.LogError($"Multiple instances of {nameof(ConfigsInstaller)} detected. Leaving the last instantiated one.");
            Destroy(_instance.gameObject);
        }

        _instance = this;

        _instance.gameObject.name = nameof(ConfigsInstaller);

        Configs.Load();
    }
}