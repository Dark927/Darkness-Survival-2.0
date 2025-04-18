using System.Collections.Generic;
using Gameplay.Data;
using UnityEngine;
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

                pool = _diContainer.Instantiate<EnemyPool>(new object[] { enemySpawnData, container });
                _poolsDict.Add(enemySpawnData.EnemyData.ID, pool);
            }
        }

        #endregion

        public EnemyController GetEnemy(int enemyId)
        {
            if (_poolsDict.ContainsKey(enemyId))
            {
                GameObject requestedObj = _poolsDict[enemyId].RequestObject();
                return requestedObj != null ? requestedObj.GetComponent<EnemyController>() : null;
            }
            return null;
        }

        public void ReturnEnemy(EnemyController enemyController)
        {
            int enemyId = enemyController.Data.ID;
            _poolsDict[enemyId].ReturnObject(enemyController.gameObject);
        }

        #endregion
    }
}

