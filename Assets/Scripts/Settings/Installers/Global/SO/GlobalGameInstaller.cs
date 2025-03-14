using Cinemachine;
using Gameplay.Components;
using Settings.CameraManagement;
using Settings.Global;
using Settings.Global.Audio;
using UI.CustomCursor;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GlobalGameInstaller", menuName = "Installers/Global Game Installer")]
public class GlobalGameInstaller : ScriptableObjectInstaller<GlobalGameInstaller>
{
    [Header("Game UI - Settings")]
    [SerializeField] private CustomCursorData _cursorData;

    public override void InstallBindings()
    {
        BindData();
    }

    public void BindData()
    {
        Container
            .Bind<CustomCursorData>()
            .FromInstance(_cursorData)
            .AsSingle();
    }

}
