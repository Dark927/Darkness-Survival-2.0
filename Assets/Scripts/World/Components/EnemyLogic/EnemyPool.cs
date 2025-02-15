using Characters.Enemy.Data;
using Settings;
using UnityEngine;
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

        public EnemyPool(ObjectPoolSettings poolSettings, EnemyData data, int preloadCount = ObjectPoolSettings.NotIdentifiedPreloadCount) :
            base(poolSettings, data.EnemyPrefab)
        {
            _enemyData = data;
            InitPool(preloadCount);
        }

        [Inject]
        public EnemyPool(ObjectPoolSettings poolSettings, EnemyData data, GameObjectsContainer container, int preloadCount = ObjectPoolSettings.NotIdentifiedPreloadCount) :
            base(poolSettings, data.EnemyPrefab, container)
        {
            _enemyData = data;
            InitPool(preloadCount); 
        }

        #endregion

        protected override GameObject PreloadFunc(GameObject enemyPrefab, GameObjectsContainer container = null)
        {
            GameObject createdObj = base.PreloadFunc(enemyPrefab, container);

            if (createdObj.TryGetComponent(out EnemyController createdEnemy))
            {
                createdEnemy.SetTargetPool(this);
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
