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


    [Header("Game Audio - Settings")]
    [SerializeField] private MusicData _mainMenuTheme;
    [SerializeField] private MusicData _pauseMenuTheme;

    public override void InstallBindings()
    {
        BindData();
        BindServices();
    }

    public void BindData()
    {
        Container
            .Bind<CustomCursorData>()
            .FromInstance(_cursorData)
            .AsSingle();
    }

    private void BindServices()
    {
        AudioSource musicSource = Camera.main.GetComponent<AudioSource>();
        GameAudioService audioService = new GameAudioService(musicSource);
        audioService.AddMusicClips(_mainMenuTheme);

        GameStateService gameStateService = new GameStateService();
        PlayerService playerService = new PlayerService();

        CinemachineVirtualCamera virtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>();
        CameraService cameraController = new CameraService(virtualCamera);


        // Bind

        Container
            .Bind<GameAudioService>()
            .FromInstance(audioService)
            .AsSingle();

        Container
            .Bind<GameStateService>()
            .FromInstance(gameStateService)
            .AsSingle();


        Container
            .Bind<PlayerService>()
            .FromInstance(playerService)
            .AsSingle();

        Container
            .Bind<CameraService>()
            .FromInstance(cameraController)
            .AsSingle();
    }
}
