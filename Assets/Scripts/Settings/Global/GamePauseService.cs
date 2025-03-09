using System;
using Characters.Player;
using Gameplay.Components;
using UnityEngine;

namespace Settings.Global
{
    public class GamePauseService : IService
    {
        #region Fields 

        private float _savedTimeScale = 1f;
        private bool _isPaused = false;
        private PlayerService _playerService;

        #endregion


        #region Properties
        public bool IsGamePaused => _isPaused;

        #endregion


        #region Methods

        public void SetPlayerService(PlayerService playerService)
        {
            _playerService = playerService;
        }

        public void PauseGame()
        {
            if (_isPaused)
            {
                return;
            }

            _savedTimeScale = Time.timeScale;
            Time.timeScale = 0;
            TryBlockCharacterInput();

            _isPaused = true;
        }

        public void UnpauseGame()
        {
            if (!_isPaused)
            {
                return;
            }

            Time.timeScale = _savedTimeScale;
            TryUnblockCharacterInput();

            _isPaused = false;
        }

        private void TryBlockCharacterInput()
        {
            DoPlayersAction(player => player.Input.DeactivateInput());
        }

        private void TryUnblockCharacterInput()
        {
            DoPlayersAction(player => player.Input.ActivateInput());
        }

        private void DoPlayersAction(Action<PlayerCharacterController> action)
        {
            if (_playerService == null)
            {
                return;
            }

            foreach (var player in _playerService.Players)
            {
                action?.Invoke(player);
            }
        }

        #endregion
    }
}
