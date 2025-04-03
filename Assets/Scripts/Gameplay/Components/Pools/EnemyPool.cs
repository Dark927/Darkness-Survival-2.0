using Characters.Enemy.Data;
using Gameplay.Data;
using Settings;
using UnityEngine;
using Zenject;

namespace Gameplay.Components.Enemy
{
    public sealed class EnemyPool : ComponentsPoolBase<EnemyController>
    {
        #region Fields

        private readonly EnemyData _enemyData;

        #endregion


        #region Methods

        #region Init

        [Inject]
        public EnemyPool(EnemyPoolData enemyPoolData, EnemySpawnData data, Transform container) :
            base(enemyPoolData.Settings, data.EnemyPrefab, container)
        {
            _enemyData = data.EnemyData;
        }

        #endregion

        protected override EnemyController PreloadFunc(Transform container = null)
        {
            EnemyController createdEnemy = base.PreloadFunc(container);

            if (createdEnemy != null)
            {
                createdEnemy.Initialize(_enemyData);
            }

            return createdEnemy;
        }

        protected override string GenerateDefaultItemName(EnemyController enemy)
        {
            return $"{_enemyData.Name} {_enemyData.Type}".Replace(" ", "_");
        }

        protected override void ReturnAction(EnemyController enemy)
        {
            base.ReturnAction(enemy);
            enemy.gameObject.SetActive(false);
        }

        #endregion
    }
}
