using UnityEngine;

namespace Gameplay.Data
{
    [CreateAssetMenu(fileName = "NewEnemyManagementData", menuName = "Game/World/Data/Enemy Management Data")]
    public class EnemyManagementData : ScriptableObject
    {

        #region Fields 

        [SerializeField] private bool _useDamageIndicators = true;


        #endregion


        #region Properties

        public bool UseDamageIndicators => _useDamageIndicators;

        #endregion

    }
}
