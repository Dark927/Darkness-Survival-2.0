using System;
using System.Collections.Generic;
using Gameplay.Components;
using Settings.CameraManagement;
using Settings.Global.Audio;
using UnityEngine;
using Zenject;

namespace Settings.Global
{
    public class GlobalServiceLoader : MonoBehaviour
    {
        #region Fields

        private Dictionary<Type, IService> _services;

        #endregion


        #region Methods

        #region Init

        [Inject]
        public void Construct(
            GameStateService gameStateService,
            PlayerService playerService,
            CameraService cameraService,
            GameAudioService audioService
        )
        {
            _services = new()
            {
                [gameStateService.GetType()] = gameStateService,
                [playerService.GetType()] = playerService,
                [cameraService.GetType()] = cameraService,
                [audioService.GetType()] = audioService,
            };
        }

        private void Awake()
        {
            RegisterServices();
            InitServices();
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

        #endregion


        private void OnDestroy()
        {
            foreach (var service in _services.Values)
            {
                if (service is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }

            _services.Clear();
            _services = null;
        }

        #endregion
    }
}
