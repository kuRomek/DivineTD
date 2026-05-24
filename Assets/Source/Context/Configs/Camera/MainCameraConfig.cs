using UnityEngine;

[CreateAssetMenu(fileName = "MainCamera", menuName = "Configs/MainCamera")]
public class MainCameraConfig : ScriptableObject
{
    [field: Range(0.5f, 5f), SerializeField] public float Sensitivity { get; private set; }
}
