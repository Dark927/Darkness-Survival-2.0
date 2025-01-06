using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using World.Data;
using Settings;
using Zenject;

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

        [Inject]
        private EnemySpawnSettings _spawnSettings;
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
                _configurator = new DefaultEnemyConfigurator(_spawnSettings.SpawnPositionRange, _spawnSettings.SpawnPositionOffset);
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
            // Filter enemy spawn data
            var markedToSpawn = _enemySpawnData
                .Where(data => data.SpawnTime <= _timer.CurrentTime);

            // Add tasks for the filtered data
            _actualSpawnTasks.AddRange(markedToSpawn.Select(data => SpawnEnemyTask(data)));

            // Remove all used enemy spawn data
            _enemySpawnData.RemoveAll(data => markedToSpawn.Contains(data));
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
