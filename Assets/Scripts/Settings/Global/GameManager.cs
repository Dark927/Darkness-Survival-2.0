using System;
using UnityEngine;

namespace Settings.Global
{
    public class GameManager : IService
    {
        #region Fields

        public event Action OnStageStarted;
        public Material BlinkMaterial;

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
