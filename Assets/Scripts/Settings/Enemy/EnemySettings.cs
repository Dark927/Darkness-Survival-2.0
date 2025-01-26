using Characters.Enemy.Data;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "NewEnemySettings", menuName = "Game/Settings/Enemy Settings")]
    public class EnemySettings : ScriptableObject
    {
        #region Fields 

        [Header("Body Settings")]

        [SerializeField] private EnemyBodyStats _bodyStats;

        #endregion


        #region Properties

        public EnemyBodyStats BodyStats => _bodyStats;

        #endregion
    }

}