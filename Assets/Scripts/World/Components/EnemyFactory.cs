using Characters.Enemy.Data;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using World.Components;
using World.Data;

namespace World.Components
{
    public class EnemyFactory
    {
        #region Fields

        private Dictionary<int, EnemyPool> _poolsDict;
        private GameObjectsContainer _container;

        #endregion


        #region Methods

        #region Init

        public EnemyFactory(List<EnemyData> enemyDataList)
        {
            InitContainer();
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

                pool = new EnemyPool(enemyData, container.transform);
                _poolsDict.Add(enemyData.ID, pool);
            }
        }

        private void InitContainer()
        {
            if(_container == null)
            {
                GameObject obj = new GameObject("Default_Enemies_Container", typeof(GameObjectsContainer));
                _container = obj.GetComponent<GameObjectsContainer>();
            }
        }

        #endregion


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

