using UnityEngine;

namespace Settings.Global
{
    public class GamePauseHandler
    {
        #region Fields 

        private float _savedTimeScale = 1f;
        private bool _isPaused = false;

        private PauseEvent _pauseEvent;

        #endregion


        #region Properties
        public bool IsGamePaused => _isPaused;
        public PauseEvent GamePauseEvent => _pauseEvent;

        #endregion


        #region Methods

        public GamePauseHandler()
        {
            _pauseEvent = new PauseEvent();
        }

        public void RequestGamePause()
        {
            if (_isPaused)
            {
                return;
            }

            GamePauseEvent?.RaiseEvent(this, new PauseEventArgs(PauseEvent.Type.PauseRequested));
        }

        public void RequestGameUnpause()
        {
            if (!_isPaused)
            {
                return;
            }

            GamePauseEvent?.RaiseEvent(this, new PauseEventArgs(PauseEvent.Type.UnpauseRequested));
        }

        public bool TryPauseGame()
        {
            if (_isPaused)
            {
                return false;
            }

            _savedTimeScale = Time.timeScale;
            Time.timeScale = 0;
            GamePauseEvent?.RaiseEvent(this, new PauseEventArgs(PauseEvent.Type.Paused));

            _isPaused = true;
            return true;
        }

        public bool TryUnpauseGame()
        {
            if (!_isPaused)
            {
                return false;
            }

            Time.timeScale = _savedTimeScale;
            GamePauseEvent?.RaiseEvent(this, new PauseEventArgs(PauseEvent.Type.Unpaused));

            _isPaused = false;
            return true;
        }

        #endregion
    }
}
