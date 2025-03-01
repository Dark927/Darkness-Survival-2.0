using Cinemachine;
using Settings.CameraManagement;
using Settings.Global;
using Zenject;

public class GlobalSettingsInstaller : MonoInstaller
{
    #region Methods

    public override void InstallBindings()
    {
        // Init 

        GameStateService gameStateService = new GameStateService();
        PlayerService playerService = new PlayerService();

        CinemachineVirtualCamera virtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>();
        CameraController cameraController = new CameraController(virtualCamera);


        // Bind

        Container
            .Bind<GameStateService>()
            .FromInstance(gameStateService)
            .AsSingle();

        Container
            .Bind<PlayerService>()
            .FromInstance(playerService)
            .AsSingle();



        Container
            .Bind<CameraController>()
            .FromInstance(cameraController)
            .AsSingle();
    }

    #endregion
}