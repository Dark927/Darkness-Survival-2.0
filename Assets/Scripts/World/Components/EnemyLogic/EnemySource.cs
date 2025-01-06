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
        private GameObjectsContainer _container;

        #endregion


        #region Methods

        #region Init

        public EnemySource(List<EnemyData> enemyDataList, GameObjectsContainer container = null)
        {
            InitContainer(container);
            CreatePools(enemyDataList);
        }

        private void CreatePools(List<EnemyData> enemyDataList)
        {
            _poolsDict = new Dictionary<int, EnemyPool>();

            EnemyPool pool;
            GameObjectsContainer container;

            foreach (EnemyData enemyData in enemyDataList)
            {
                container = GetContainer(enemyData);

                pool = new EnemyPool(enemyData, container);
                _poolsDict.Add(enemyData.ID, pool);
            }
        }

        private void InitContainer(GameObjectsContainer container)
        {
            if (container != null)
            {
                _container = container;
                return;
            }

            GameObject obj = new GameObject("Default_Enemies_Container", typeof(GameObjectsContainer));
            _container = obj.GetComponent<GameObjectsContainer>();
        }

        #endregion


        // ToDo : Move this logic to the separate script
        public GameObjectsContainer GetContainer(EnemyData data)
        {
            string outerContainerName = ($"{data.Name} container").Replace(" ", "_");
            string innerContainerName = ($"{data.Type}").Replace(" ", "_");

            return _container.GetChild(outerContainerName, true).GetChild(innerContainerName, true);
        }

        public GameObject GetEnemy(int enemyId)
        {
            if (_poolsDict.ContainsKey(enemyId))
            {
                return _poolsDict[enemyId].RequestObject();
            }
            return null;
        }

        #endregion
    }
}

