using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using UnityEngine;
using World.Components;
using World.Data;

namespace World.Components.EnemyLogic
{
    public class EnemySpawner : MonoBehaviour, IDisposable
    {
        #region Fields 

        public const string ContainerDefaultName = "Enemies_Container";

        [SerializeField] private List<EnemySpawnData> _enemySpawnData;
        [SerializeField] private GameObjectsContainer _objectsContainer;
        private EnemySource _source;
        private GameTimer _timer;
        private List<UniTask> _actualSpawnTasks;

        // ToDo : Move these fields to the EnemySpawn Config file and use it through GlobalGameSettings
        [SerializeField] private Vector2 _spawnPositionRange = Vector2.zero;
        [SerializeField] private Vector2 _spawnPositionOffset = Vector2.zero;

        private ICharacterConfigurator _configurator;

        #endregion


        #region Methods

        #region Init

        private void Awake()
        {
            try
            {
                InitTimer();
                TryInitContainer();
                InitSource();
                _actualSpawnTasks = new List<UniTask>();
                _timer.OnTimeChanged += TrySpawnEnemy;
                _configurator = new DefaultEnemyConfigurator(_spawnPositionRange, _spawnPositionOffset);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                Dispose();
                gameObject.SetActive(false);
            }
        }

        private void TryInitContainer()
        {
            if (_objectsContainer == null)
            {
                GameObject obj = new GameObject(ContainerDefaultName, typeof(GameObjectsContainer));
                obj.transform.parent = transform;
                _objectsContainer = obj.GetComponent<GameObjectsContainer>();
            }
        }

        private void InitTimer()
        {
            _timer = FindAnyObjectByType<GameTimer>();

            if (_timer == null)
            {
                throw new NullReferenceException($"{_timer} is null! Can not spawn enemies using {nameof(EnemySpawnData)}!");
            }
        }

        private void InitSource()
        {
            FilterSpawnData();
            _source = new EnemySource(_enemySpawnData.Select(spawnData => spawnData.EnemyData).ToList(), _objectsContainer);
        }


        #endregion

        private void TrySpawnEnemy(StageTime time)
        {
            List<EnemySpawnData> markedToSpawn = new List<EnemySpawnData>();

            foreach (var data in _enemySpawnData)
            {
                if (data.SpawnTime <= _timer.CurrentTime)
                {
                    _actualSpawnTasks.Add(SpawnEnemyTask(data));
                    markedToSpawn.Add(data);
                    continue;
                }
                break;
            }

            // Remove all used enemy spawn data

            foreach (var data in markedToSpawn)
            {
                _enemySpawnData.Remove(data);
            }
        }

        private async UniTask SpawnEnemyTask(EnemySpawnData data)
        {
            GameObject enemyObj;
            int count = data.Count;
            float spawnInterval = (data.SpawnDuration / (float)count);

            while (count > 0)
            {
                enemyObj = _source.GetEnemy(data.EnemyData.ID);

                if (enemyObj != null)
                {
                    _configurator.Configure(enemyObj);
                    enemyObj.SetActive(true);
                }
                count--;
                await UniTask.WaitForSeconds(spawnInterval);
            }
        }

        private void FilterSpawnData()
        {
            _enemySpawnData = _enemySpawnData.Distinct().OrderBy(data => data.SpawnTime).ToList();
        }

        public void Dispose()
        {
            _objectsContainer = null;
            _source = null;
            _timer.OnTimeChanged -= TrySpawnEnemy;
        }

        #endregion
    }
}
