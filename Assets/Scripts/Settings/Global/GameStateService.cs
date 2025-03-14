using System;
using Characters.Player;
using Gameplay.Components;
using Settings.SceneManagement;
using UI;
using Zenject;

namespace Settings.Global
{
    public class GameStateService : IService, IDisposable, IEventListener
    {
        #region Fields

        private GameStateEvent _gameEvent;
        private GamePauseHandler _pauseHandler;
        private GamePanelManagerUI _panelManagerUI;


        #endregion

        public GameStateEvent GameEvent => _gameEvent;
        public GamePauseHandler PauseHandler => _pauseHandler;

        #region Methods

        #region Init

        [Inject]
        public void Construct(GamePanelManagerUI panelManagerUI)
        {
            _panelManagerUI = panelManagerUI;
        }

        public GameStateService()
        {
            _gameEvent = new GameStateEvent();
            _pauseHandler = new GamePauseHandler();
            GameSceneLoadHandler.Instance.SceneCleanEvent.Subscribe(this);
            _pauseHandler.GamePauseEvent?.Subscribe(this);
        }

        public void Dispose()
        {
            GameSceneLoadHandler.Instance.SceneCleanEvent.Unsubscribe(this);
            _pauseHandler.GamePauseEvent?.Unsubscribe(this);
        }

        #endregion

        public void StartStage(GameSceneData stageSceneData = null)
        {
            GameSceneLoadHandler.Instance.RequestStageLoad(stageSceneData, () =>
            {
                _gameEvent?.ListenEvent(this, new GameEventArgs(GameStateEventType.StageStarted));
            }, true);
        }

        public void RestartStage()
        {
            GameSceneLoadHandler.Instance.ReloadCurrentStage(() =>
            {
                _gameEvent?.ListenEvent(this, new GameEventArgs(GameStateEventType.StageStarted));
            }, true);
        }

        public void ExitToMenu()
        {
            GameSceneLoadHandler.Instance.RequestMainMenuLoad(true);
        }

        public void SwitchPauseState()
        {
            if (PauseHandler.IsGamePaused)
            {
                PauseHandler.RequestGameUnpause();
            }
            else
            {
                PauseHandler.RequestGamePause();
            }
        }

        public void Listen(object sender, EventArgs args)
        {
            switch (sender)
            {
                case PlayerService:
                    HandlePlayerEvent(args as PlayerEventArgs);
                    break;

                case GameSceneLoadHandler:
                    HandleSceneSwitchEvent();
                    break;

                case GamePauseHandler:
                    HandleGamePauseEvent(args as PauseEventArgs);
                    break;

            }
        }

        private void HandlePlayerEvent(PlayerEventArgs args)
        {
            switch (args.EventType)
            {
                case PlayerEvent.Type.Dies:
                    GameEvent.ListenEvent(this, new GameEventArgs(GameStateEventType.StageStartFinishing));
                    break;

                case PlayerEvent.Type.Win:
                    GameEvent.ListenEvent(this, new GameEventArgs(GameStateEventType.StageCompletelyPassed));
                    break;

                case PlayerEvent.Type.Dead:
                    GameEvent.ListenEvent(this, new GameEventArgs(GameStateEventType.StageCompletelyOver));
                    break;
            }
        }

        private void HandleSceneSwitchEvent()
        {
            _pauseHandler.TryUnpauseGame();
            GameEvent.ListenEvent(this, new GameEventArgs(GameStateEventType.StageStartFinishing));
        }

        private void HandleGamePauseEvent(PauseEventArgs args)
        {
            switch (args.EventType)
            {
                case PauseEvent.Type.Paused:
                    _gameEvent?.ListenEvent(this, new GameEventArgs(GameStateEventType.StagePaused));
                    break;
                case PauseEvent.Type.Unpaused:
                    _gameEvent?.ListenEvent(this, new GameEventArgs(GameStateEventType.StageUnpaused));
                    break;
            }
        }

        #endregion
    }
}
