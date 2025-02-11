using Characters.Enemy.Data;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace World.Components.EnemyLogic
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

        public EnemySource(DiContainer diContainer, List<EnemyData> enemyDataList, EnemyContainer container = null)
        {
            _enemyContainer = container;
            _diContainer = diContainer;
            CreatePools(enemyDataList);
        }

        private void CreatePools(List<EnemyData> enemyDataList)
        {
            _poolsDict = new Dictionary<int, EnemyPool>();

            EnemyPool pool;
            GameObjectsContainer container;

            foreach (EnemyData enemyData in enemyDataList)
            {
                container = _enemyContainer.GetChildContainer(enemyData);

                pool = _diContainer.Instantiate<EnemyPool>(new object[] { enemyData, container });
                _poolsDict.Add(enemyData.ID, pool);
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

        #endregion
    }
}

