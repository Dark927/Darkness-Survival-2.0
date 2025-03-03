using System;
using System.Collections.Generic;
using Settings.CameraManagement;
using UnityEngine;
using Zenject;

namespace Settings.Global
{
    public class GlobalServiceLoader : MonoBehaviour
    {
        #region Fields

        private List<IDisposable> _disposables;
        private GameStateService _gameService;
        private PlayerService _playerService;
        private CameraController _cameraController;

        #endregion


        #region Methods

        #region Init

        [Inject]
        public void Construct(
            GameStateService gameManager,
            PlayerService playerManager,
            CameraController cameraController
            )
        {
            _gameService = gameManager;
            _playerService = playerManager;
            _cameraController = cameraController;
        }

        private void Awake()
        {
            _disposables = new List<IDisposable>();

            RegisterServices();
            InitServices();
            AddToDisposables();
        }

        private void RegisterServices()
        {
            ServiceLocator.Initialize();
            ServiceLocator.Current.Register(_gameService);
            ServiceLocator.Current.Register(_playerService);
            ServiceLocator.Current.Register(_cameraController);
        }

        private void InitServices()
        {
            _cameraController.Init();
            _playerService.Init();
        }

        private void AddToDisposables()
        {
            _disposables.Add(_cameraController);
        }

        #endregion


        private void OnDestroy()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }

        #endregion
    }
}
