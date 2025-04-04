using System;
using Characters.Player;
using Characters.Player.Controls;
using Settings.Global;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Components
{
    [RequireComponent(typeof(PlayerInput))]
    public class GameplayInputManager : MonoBehaviour, IEventListener
    {
        #region Fields 

        private GameStateService _gameStateService;
        private PlayerInput _globalInput;
        private GamePlayerInputHandler _playerInputHandler;
        private PlayerService _playerService;

        #endregion


        #region Properties

        #endregion


        #region Methods

        #region Init

        private void Awake()
        {
            _globalInput = GetComponent<PlayerInput>();
            _globalInput.DeactivateInput();
        }

        private void Start()
        {
            _gameStateService = ServiceLocator.Current.Get<GameStateService>();
            _gameStateService.GameEvent.Subscribe(this);

            _playerService = ServiceLocator.Current.Get<PlayerService>();

            if (_playerService.TryGetPlayer(out PlayerCharacterController player))
            {
                _playerInputHandler = new GamePlayerInputHandler(player);
                _playerInputHandler.TryBlockCharacterInput();
            }
        }

        private void OnDestroy()
        {
            _gameStateService.GameEvent.Unsubscribe(this);
        }

        #endregion

        public void PauseKeyListener(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _gameStateService.SwitchPauseState();
            }
        }

        public void Listen(object sender, EventArgs args)
        {
            switch (sender)
            {
                case GameStateService:
                    HandleGameEvent(args as GameEventArgs);
                    break;
            }
        }

        private void HandleGameEvent(GameEventArgs args)
        {
            switch (args.EventType)
            {
                case GameStateEventType.StagePaused:
                    _playerInputHandler.TryBlockCharacterInput();
                    break;

                case GameStateEventType.StageStarted:
                case GameStateEventType.StageUnpaused:
                    _globalInput.ActivateInput();
                    _playerInputHandler.TryUnblockCharacterInput();
                    break;

                case GameStateEventType.StageStartFinishing:
                    _globalInput.DeactivateInput();
                    break;
            }
        }

        #endregion
    }
}
