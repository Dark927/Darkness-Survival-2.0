using System;

namespace Settings.Global
{
    public class GameStateService : IService
    {
        #region Fields

        public event Action OnStageStarted;

        #endregion


        #region Methods

        #region Init

        public GameStateService()
        {

        }

        #endregion

        public void StartStage()
        {
            OnStageStarted?.Invoke();
        }

        #endregion
    }
}
