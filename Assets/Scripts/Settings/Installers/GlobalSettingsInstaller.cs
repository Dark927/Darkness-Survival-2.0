using Cinemachine;
using Settings.CameraManagement;
using Settings.Global;
using Zenject;

public class GlobalSettingsInstaller : MonoInstaller
{
    #region Fields

    private GameManager _gameManager;
    private CameraController _cameraController;
    private PlayerManager _playerManager;

    #endregion


    #region Methods

    public override void InstallBindings()
    {
        InitComponents();
        Bind();
    }

    private void InitComponents()
    {
        _gameManager = new GameManager();
        _playerManager = new PlayerManager();

        CinemachineVirtualCamera virtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>();
        _cameraController = new CameraController(virtualCamera);
    }

    private void Bind()
    {
        Container
            .Bind<GameManager>()
            .FromInstance(_gameManager)
            .AsSingle();          
        
        Container
            .Bind<PlayerManager>()
            .FromInstance(_playerManager)
            .AsSingle();   
        
        Container
            .Bind<CameraController>()
            .FromInstance(_cameraController)
            .AsSingle();
    }

    #endregion
}
