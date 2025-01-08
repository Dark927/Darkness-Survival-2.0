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
            base(poolSettings, data.Prefab)
        {
            _enemyData = data;
            InitPool(preloadCount);
        }

        [Inject]
        public EnemyPool(ObjectPoolSettings poolSettings, EnemyData data, GameObjectsContainer container, int preloadCount = ObjectPoolSettings.NotIdentifiedPreloadCount) :
            base(poolSettings, data.Prefab, container)
        {
            _enemyData = data;
            InitPool(preloadCount);
        }

        #endregion

        protected override GameObject PreloadFunc(GameObject prefab, GameObjectsContainer container = null)
        {
            GameObject createdObj = UnityEngine.Object.Instantiate(_enemyData.Prefab);
            createdObj.name = $"{_enemyData.Name} {_enemyData.Type}".Replace(" ", "_");

            if (container != null)
            {
                createdObj.transform.parent = container.transform;
            }

            return createdObj;
        }

        protected override void ReturnAction(GameObject obj)
        {
            base.ReturnAction(obj);
            obj.SetActive(false);
        }

        #endregion
    }
}
