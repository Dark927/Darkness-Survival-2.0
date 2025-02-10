
using UnityEngine;

namespace Characters.Enemy.Data
{
    [System.Serializable]
    public class EnemyBodyStats
    {
        #region Fields 

        [Header("Look Side Switch")]

        [SerializeField] private int _sideSwitchDelayInMs = 0;
        [SerializeField] private int _accelerationTimeInMs = 500;

        #endregion


        #region Properties

        public int SideSwitchDelayInMs => _sideSwitchDelayInMs;
        public int AccelerationTimeInMs => _accelerationTimeInMs;

        #endregion
    }
}
