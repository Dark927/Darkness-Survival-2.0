using Characters.Player;
using Settings.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Settings.Global
{
    public class GameManager : IService
    {
        #region Fields

        public event Action OnStageStarted;

        #endregion


        #region Methods

        #region Init

        public GameManager()
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
