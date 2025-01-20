using Cysharp.Threading.Tasks;
using Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using World.Data;
using Zenject;

namespace World.Components.EnemyLogic
{
    public class EnemySpawner : MonoBehaviour, IDisposable
    {
        #region Fields 

        [SerializeField] private List<EnemySpawnData> _enemySpawnData;
        private EnemyContainer _enemyContainer;

        private EnemySource _source;
        private GameTimer _timer;
        private List<UniTask> _actualSpawnTasks;

        private EnemySpawnSettings _spawnSettings;
        private ICharacterConfigurator _configurator;

        private DiContainer _diContainer;

        #endregion


        #region Methods

        #region Init

        [Inject]
        public void Construct(DiContainer diContainer, EnemySpawnSettings spawnSettings, GameTimer timer)
        {
            _diContainer = diContainer;
            _spawnSettings = spawnSettings;
            _timer = timer;
        }

        private void Awake()
        {
            try
            {
                CheckTimer();
                TryInitContainer();
                InitEnemySource();

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
            if (_enemyContainer == null)
            {
                GameObject obj = new GameObject(EnemyContainer.ContainerDefaultName, typeof(GameObjectsContainer));
                obj.transform.parent = transform;
                _enemyContainer = new EnemyContainer(obj.GetComponent<GameObjectsContainer>());
            }
        }

        private void CheckTimer()
        {
            if (_timer == null)
            {
                throw new NullReferenceException($"{_timer} is null! Can not spawn enemies using {nameof(EnemySpawnData)}!");
            }
        }

        private void InitEnemySource()
        {
            FilterSpawnData();
            _source = _diContainer.Instantiate<EnemySource>(new object[] { _enemySpawnData.Select(spawnData => spawnData.EnemyData).ToList(), _enemyContainer });
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
            _enemyContainer = null;
            _source = null;
            _timer.OnTimeChanged -= TrySpawnEnemy;
        }

        #endregion
    }
}
