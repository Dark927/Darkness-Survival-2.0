using System;
using System.Collections.Generic;
using Characters.Player.Controls;
using Cinemachine;
using Gameplay.Components;
using Settings.CameraManagement;
using Settings.Global.Audio;
using Settings.SceneManagement;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Audio;
using Utilities.Attributes;
using Utilities.ErrorHandling;
using Zenject;

namespace Settings.Global
{
    public class GlobalServiceLoader : MonoBehaviour, IEventListener
    {
        #region Fields

        [CustomHeader("Game Audio - Settings", 2, 0)]
        [SerializeField] private MusicData _mainMenuTheme;
        [SerializeField] private MusicData _pauseMenuTheme;

        [CustomHeader("Cameras", 2, 0)]
        [SerializeField] private CinemachineVirtualCamera _playerCamera;
        [SerializeField] private CinemachineVirtualCamera _introCamera;

        private Dictionary<Type, IService> _services;
        private DiContainer _diContainer;
        private IAudioProvider _audioProvider;
        private AudioMixer _mainAudioMixer;
        private ISettingsStorage _settingsStorage;

        #endregion


        #region Methods

        #region Init

        [Inject]
        public void Construct(DiContainer container, IAudioProvider audioProvider, AudioMixer mainMixer, ISettingsStorage settingsStorage)
        {
            _diContainer = container;
            _audioProvider = audioProvider;
            _mainAudioMixer = mainMixer;
            _settingsStorage = settingsStorage;
        }

        private void Awake()
        {
            CreateServices();
            RegisterServices();
            InitServices();
        }

        private void CreateServices()
        {
            //////////////
            // Create 
            //////////////

            GameAudioService audioService = new GameAudioService(_audioProvider, _mainAudioMixer, _settingsStorage);
            audioService.MusicPlayer.AddMusicClips(_mainMenuTheme);
            audioService.MusicPlayer.AddMusicClips(_pauseMenuTheme);

            // Player
            PlayerService playerService = new PlayerService();

            // Controls

            InputService inputService = new InputService(_settingsStorage);

            // Game State
            GameStateService gameStateService = _diContainer.Instantiate<GameStateService>();

            // Camera
            if (_playerCamera == null || _introCamera == null)
            {
                ErrorLogger.LogError("[GlobalServiceLoader] Player or Intro Camera is missing! Please assign them in the Inspector.");
            }

            CameraService cameraService = new CameraService(_playerCamera, _introCamera);

            // Graphics

            GraphicsService graphicsService = new GraphicsService(_settingsStorage);

            // All Services 
            _services = new()
            {
                [gameStateService.GetType()] = gameStateService,
                [playerService.GetType()] = playerService,
                [cameraService.GetType()] = cameraService,
                [audioService.GetType()] = audioService,
                [graphicsService.GetType()] = graphicsService,
                [inputService.GetType()] = inputService,
            };

            //////////////
            // Configure services and self
            //////////////

            playerService.PlayerEvent.Subscribe(gameStateService);

            RegisterOwnEvents();
        }

        private void RegisterServices()
        {
            ServiceLocator.Initialize();

            foreach (var serviceInfo in _services)
            {
                ServiceLocator.Current.Register(serviceInfo.Key, serviceInfo.Value);
            }
        }

        private void InitServices()
        {
            foreach (var service in _services.Values)
            {
                if (service is IInitializable initializableService)
                {
                    initializableService.Initialize();
                }
            }
        }

        private void RegisterOwnEvents()
        {
            GameSceneLoadHandler.Instance.SceneCleanEvent.Subscribe(this);
        }

        #endregion


        private void OnDestroy()
        {
            UnregisterOwnEvents();

            foreach (var service in _services.Values)
            {
                ServiceLocator.Current?.Unregister(service);

                if (service is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }

            _services.Clear();
            _services = null;
        }

        private void UnregisterOwnEvents()
        {
            GameSceneLoadHandler.Instance.SceneCleanEvent.Unsubscribe(this);
        }

        public void Listen(object sender, EventArgs args)
        {
            switch (sender)
            {
                case GameSceneLoadHandler:
                    HandleSceneCleanEvent();
                    break;
            }
        }

        private void HandleSceneCleanEvent()
        {
            foreach (var service in _services.Values)
            {
                if (service is ICleanable cleanableService)
                {
                    cleanableService.Clean();
                }
            }
        }

        #endregion
    }
}
