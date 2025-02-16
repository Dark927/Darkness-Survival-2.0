
using UnityEngine;
using World.Tile;

namespace Settings
{
    [CreateAssetMenu(fileName = "NewGlobalGameConfig", menuName = "Game/Settings/Global Game Config")]
    public class GlobalGameConfig : ScriptableObject
    {
        #region Fields 

        [Header("Gameplay Settings")]
        [SerializeField] private ObjectPoolData _poolSettings;

        [Space, Header("Enemy Settings")]
        [SerializeField] private EnemySpawnerSettingsData _enemySpawnSettings;
        [SerializeField] private EnemyGlobalData _enemySettings;


        [Space, Header("World Settings")]

        [SerializeField] private WorldGenerationData _worldGenerationSettings;

        #endregion


        #region Properties

        public ObjectPoolData PoolsSettings => _poolSettings;
        public EnemySpawnerSettingsData EnemySpawnSettings => _enemySpawnSettings;
        public EnemyGlobalData EnemySettings => _enemySettings;

        #endregion
    }
}