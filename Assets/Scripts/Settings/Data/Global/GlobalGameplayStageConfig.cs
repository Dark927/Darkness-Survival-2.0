
using Unity.VisualScripting;
using UnityEngine;
using Gameplay.Tile;

namespace Settings
{
    [CreateAssetMenu(fileName = "NewGlobalGameConfig", menuName = "Game/Settings/Global Game Config")]
    public class GlobalGameplayStageConfig : ScriptableObject
    {
        #region Fields 

        [Header("Gameplay Settings")]

        [Space, Header("Enemy Settings")]
        [SerializeField] private EnemyPoolData _enemyPoolData;
        [SerializeField] private EnemySpawnerSettingsData _enemySpawnerData;
        [SerializeField] private EnemyGlobalData _enemyCommonData;

        [Space, Header("Items Settings")]
        [SerializeField] private ItemPoolData _itemPoolData;

        [Space, Header("World Settings")]

        [SerializeField] private WorldGenerationData _worldGenerationData;

        #endregion


        #region Properties

        public EnemyPoolData EnemyPoolsData => _enemyPoolData;

        public EnemySpawnerSettingsData EnemySpawnerSettingsData => _enemySpawnerData;
        public EnemyGlobalData EnemyCommonData => _enemyCommonData;
        public ItemPoolData ItemPoolsData => _itemPoolData;

        #endregion
    }
}