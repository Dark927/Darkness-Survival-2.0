using System;
using System.Collections.Generic;
using Characters.Player;
using Cinemachine;
using Gameplay.Components;
using Settings.CameraManagement;
using Settings.Global.Audio;
using Settings.SceneManagement;
using UI;
using UnityEngine;
using Utilities.Attributes;
using Zenject;

namespace Settings.Global
{
    public class GlobalServiceLoader : MonoBehaviour, IEventListener
    {
        #region Fields

        [CustomHeader("Game Audio - Settings", 2, 0)]
        [SerializeField] private MusicData _mainMenuTheme;
        [SerializeField] private MusicData _pauseMenuTheme;

        private Dictionary<Type, IService> _services;
        private DiContainer _diContainer;

        #endregion


        #region Methods

        #region Init

        [Inject]
        public void Construct(DiContainer container)
        {
            _diContainer = container;
        }

        private void Awake()
        {
            CreateServices();
            RegisterServices();
            InitServices();
        }


        private void CreateServices()
        {
            // Create 

            AudioSource musicSource = Camera.main.GetComponent<AudioSource>();
            GameAudioService audioService = new GameAudioService(musicSource);
            audioService.AddMusicClips(_mainMenuTheme);

            PlayerService playerService = new PlayerService();
            GameStateService gameStateService = _diContainer.Instantiate<GameStateService>();

            CinemachineVirtualCamera virtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>();
            CameraService cameraService = new CameraService(virtualCamera);

            _services = new()
            {
                [gameStateService.GetType()] = gameStateService,
                [playerService.GetType()] = playerService,
                [cameraService.GetType()] = cameraService,
                [audioService.GetType()] = audioService,
            };

            // Configure services and self

            playerService.PlayerEvent.Subscribe(gameStateService);

            RegisterOwnEvents(gameStateService);
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

        private void RegisterOwnEvents(GameStateService gameStateService)
        {
            gameStateService.GameEvent.Subscribe(this);
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
            var gameStateService = ServiceLocator.Current.Get<GameStateService>();
            gameStateService.GameEvent.Unsubscribe(this);
            GameSceneLoadHandler.Instance.SceneCleanEvent.Unsubscribe(this);
        }

        public void Listen(object sender, EventArgs args)
        {
            switch (sender)
            {
                case GameStateService:
                    HandleGameStateEvent(args as GameEventArgs);
                    break;

                case GameSceneLoadHandler:
                    HandleSceneCleanEvent();
                    break;
            }
        }

        private void HandleGameStateEvent(GameEventArgs args)
        {
            if (args.EventType != GameStateEventType.StageStarted)
            {
                return;
            }

            PlayerService playerService = ServiceLocator.Current.Get<PlayerService>();
            CameraService cameraService = ServiceLocator.Current.Get<CameraService>();

            if (playerService.TryGetPlayer(out PlayerCharacterController player))
            {
                cameraService.FollowPlayer(player);
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
