using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Components;
using Settings;
using Settings.Global;
using UnityEngine;
using Zenject;

namespace UI
{
    public sealed class GameplayUI : MonoBehaviour, IEventListener
    {
        private List<Canvas> _canvases;
        private GamePanelManagerUI _gamePanelManager;
        private GameStateService _gameStateService;

        [Inject]
        public void Construct(GamePanelManagerUI panelManager)
        {
            _gamePanelManager = panelManager;
        }

        private void Awake()
        {
            _canvases = GetComponentsInChildren<Canvas>().ToList();
        }

        private void Start()
        {
            _gameStateService = ServiceLocator.Current.Get<GameStateService>();
            _gameStateService?.GameEvent?.Subscribe(this);
            _gameStateService?.PauseHandler.GamePauseEvent.Subscribe(this);
        }

        private void OnDestroy()
        {
            _canvases.Clear();
            _gameStateService?.GameEvent?.Unsubscribe(this);
            _gameStateService.PauseHandler.GamePauseEvent.Unsubscribe(this);
        }

        public void Activate()
        {
            foreach (var canvas in _canvases)
            {
                canvas.gameObject.SetActive(true);
            }
        }

        public void Deactivate()
        {
            foreach (var canvas in _canvases)
            {
                canvas.gameObject.SetActive(false);
            }
        }

        public void Listen(object sender, EventArgs args)
        {
            switch (sender)
            {
                case GameStateService:
                    HandleGameEvent(args as GameEventArgs);
                    break;

                case GamePauseHandler:
                    HandleGamePauseEvent(args as PauseEventArgs);
                    break;
            }
        }

        private void HandleGamePauseEvent(PauseEventArgs args)
        {
            switch (args.EventType)
            {
                case PauseEvent.Type.PauseRequested:
                    TryPauseGame();
                    break;

                case PauseEvent.Type.UnpauseRequested:
                    TryUnpauseGame();
                    break;
            }
        }

        private void TryPauseGame()
        {
            if (_gamePanelManager.TryOpenPanel(GamePanelManagerUI.PanelType.Pause))
            {
                _gameStateService.PauseHandler.TryPauseGame();
            }
        }

        private void TryUnpauseGame()
        {
            _gamePanelManager.ClosePauseMenu(() =>
            {
                _gameStateService.PauseHandler.TryUnpauseGame();
            });
        }

        private void HandleGameEvent(GameEventArgs gameEventArgs)
        {
            switch (gameEventArgs.EventType)
            {
                case GameStateEventType.StageStartFinishing:
                    Deactivate();
                    break;

                case GameStateEventType.StageCompletelyOver:
                    _gamePanelManager.TryOpenPanel(GamePanelManagerUI.PanelType.GameOver);
                    break;

                case GameStateEventType.StageCompletelyPassed:
                    _gamePanelManager.TryOpenPanel(GamePanelManagerUI.PanelType.GameWin);
                    break;
            }
        }
    }
}
