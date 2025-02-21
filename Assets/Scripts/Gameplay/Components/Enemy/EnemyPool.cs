using Characters.Common;
using Characters.Enemy.Data;
using Characters.Interfaces;
using Settings;
using UnityEngine;
using Gameplay.Data;
using Zenject;

namespace Gameplay.Components.Enemy
{
    public sealed class EnemyPool : ObjectPoolBase<GameObject>
    {
        #region Fields

        private readonly EnemyData _enemyData;

        #endregion


        #region Methods

        #region Init

        public EnemyPool(EnemyPoolData poolSettings, EnemySpawnData data, int preloadCount = ObjectPoolData.NotIdentifiedPreloadCount) :
            base(poolSettings, data.EnemyPrefab)
        {
            _enemyData = data.EnemyData;
            InitPool(preloadCount);
        }

        [Inject]
        public EnemyPool(EnemyPoolData poolSettings, EnemySpawnData data, GameObjectsContainer container, int preloadCount = ObjectPoolData.NotIdentifiedPreloadCount) :
            base(poolSettings, data.EnemyPrefab, container)
        {
            _enemyData = data.EnemyData;
            InitPool(preloadCount); 
        }

        #endregion

        protected override GameObject PreloadFunc(GameObject enemyPrefab, GameObjectsContainer container = null)
        {
            GameObject createdObj = base.PreloadFunc(enemyPrefab, container);

            if (createdObj.TryGetComponent(out EnemyController createdEnemy))
            {
                createdEnemy.Initialize(_enemyData);
            }

            return createdObj;
        }

        protected override string GenerateDefaultItemName(GameObject enemyPrefab)
        {
            return $"{_enemyData.Name} {_enemyData.Type}".Replace(" ", "_");
        }

        protected override void ReturnAction(GameObject obj)
        {
            base.ReturnAction(obj);
            obj.SetActive(false);
        }

        #endregion
    }
}
