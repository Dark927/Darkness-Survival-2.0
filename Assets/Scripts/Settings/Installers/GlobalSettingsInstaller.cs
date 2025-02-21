using Cinemachine;
using Settings.CameraManagement;
using Settings.Global;
using Zenject;

public class GlobalSettingsInstaller : MonoInstaller
{
    #region Fields

    #endregion


    #region Methods

    public override void InstallBindings()
    {
        // Init 

        GameStateService gameManager = new GameStateService();
        PlayerService playerManager = new PlayerService();

        CinemachineVirtualCamera virtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>();
        CameraController cameraController = new CameraController(virtualCamera);


        // Bind

        Container
            .Bind<GameStateService>()
            .FromInstance(gameManager)
            .AsSingle();

        Container
            .Bind<PlayerService>()
            .FromInstance(playerManager)
            .AsSingle();

        Container
            .Bind<CameraController>()
            .FromInstance(cameraController)
            .AsSingle();
    }


    #endregion
}
