using Settings.CameraManagement;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Settings.Global
{
    public class GlobalServiceLoader : MonoBehaviour
    {
        #region Fields

        private List<IDisposable> _disposables;
        private CameraController _cameraController;
        private GameStateService _gameManager;
        private PlayerService _playerManager;

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
            _gameManager = gameManager;
            _playerManager = playerManager;
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
            ServiceLocator.Current.Register(_gameManager);
            ServiceLocator.Current.Register(_playerManager);
            ServiceLocator.Current.Register(_cameraController);
        }

        private void InitServices()
        {
            _cameraController.Init();
            _playerManager.Init();
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
