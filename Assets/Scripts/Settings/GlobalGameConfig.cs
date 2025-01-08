
using UnityEngine;
using World.Tile;

namespace Settings
{
    [CreateAssetMenu(fileName = "NewGlobalGameConfig", menuName = "Game/Settings/Global Game Config")]
    public class GlobalGameConfig : ScriptableObject
    {
        #region Fields 

        [Header("Gameplay Settings")]
        [SerializeField] private ObjectPoolSettings _poolSettings;
        [SerializeField] private EnemySpawnSettings _enemySpawnSettings;

        [Space, Header("World Settings")]

        [SerializeField] private GenerationSettings _worldGenerationSettings;

        #endregion


        #region Properties

        public ObjectPoolSettings PoolsSettings => _poolSettings;
        public EnemySpawnSettings EnemySpawnSettings => _enemySpawnSettings;
        public GenerationSettings WorldGenerationSettings => _worldGenerationSettings;

        #endregion
    }
}