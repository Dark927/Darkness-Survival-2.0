using System.Collections.Generic;
using Characters.Enemy;
using Gameplay.GlobalSettings;
using Zenject;

namespace Gameplay.Components.Enemy
{
    public class EnemySource
    {
        #region Fields

        private Dictionary<int, EnemyPool> _poolsDict;
        private EnemyContainer _enemyContainer;

        private readonly DiContainer _diContainer;

        #endregion


        #region Methods

        #region Init

        [Inject]
        public EnemySource(DiContainer diContainer, List<EnemySpawnData> enemyDataList, EnemyContainer container = null)
        {
            _enemyContainer = container;
            _diContainer = diContainer;
            CreatePools(enemyDataList);
        }

        private void CreatePools(List<EnemySpawnData> enemyDataList)
        {
            _poolsDict = new Dictionary<int, EnemyPool>();

            EnemyPool pool;
            GameObjectsContainer container;

            foreach (var enemySpawnData in enemyDataList)
            {
                container = _enemyContainer.GetChildContainer(enemySpawnData);

                pool = _diContainer.Instantiate<EnemyPool>(new object[] { enemySpawnData, container.transform });
                pool.Initialize();
                _poolsDict.Add(enemySpawnData.EnemyData.CommonInfo.TypeID, pool);
            }
        }

        #endregion

        public EnemyController GetEnemy(int enemyId)
        {
            if (_poolsDict.ContainsKey(enemyId))
            {
                return _poolsDict[enemyId].RequestObject();
            }
            return null;
        }

        public void ReturnEnemy(EnemyController enemyController)
        {
            int enemyId = enemyController.Data.CommonInfo.TypeID;
            _poolsDict[enemyId].ReturnItem(enemyController);
        }

        #endregion
    }
}

