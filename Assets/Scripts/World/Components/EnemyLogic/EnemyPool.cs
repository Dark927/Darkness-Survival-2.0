using Characters.Common;
using Characters.Enemy.Data;
using Characters.Interfaces;
using Settings;
using UnityEngine;
using World.Data;
using Zenject;

namespace World.Components.EnemyLogic
{
    public sealed class EnemyPool : ObjectPoolBase<GameObject>
    {
        #region Fields

        private readonly EnemyData _enemyData;

        #endregion


        #region Methods

        #region Init

        public EnemyPool(ObjectPoolData poolSettings, EnemySpawnData data, int preloadCount = ObjectPoolData.NotIdentifiedPreloadCount) :
            base(poolSettings, data.EnemyPrefab)
        {
            _enemyData = data.EnemyData;
            InitPool(preloadCount);
        }

        [Inject]
        public EnemyPool(ObjectPoolData poolSettings, EnemySpawnData data, GameObjectsContainer container, int preloadCount = ObjectPoolData.NotIdentifiedPreloadCount) :
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
                createdEnemy.SetTargetPool(this);
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

            EnemyController enemy = obj.GetComponent<EnemyController>();
            enemy.ResetCharacter();

            obj.SetActive(false);
        }

        #endregion
    }
}
