using Cysharp.Threading.Tasks;
using Settings;
using Settings.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using World.Data;
using Zenject;

namespace World.Components.EnemyLogic
{
    public class EnemySpawner : MonoBehaviour, IDisposable
    {
        #region Fields 

        [SerializeField] private List<Data.EnemySpawnData> _enemySpawnData;
        private EnemyContainer _enemyContainer;

        private EnemySource _source;
        private GameTimer _timer;
        private List<UniTask> _actualSpawnTasks;
        private CancellationTokenSource _cancellationTokenSource;

        private Settings.EnemySpawnerSettingsData _spawnSettings;
        private ICharacterConfigurator<EnemyController> _configurator;

        private DiContainer _diContainer;

        #endregion


        #region Methods

        #region Init

        [Inject]
        public void Construct(DiContainer diContainer, Settings.EnemySpawnerSettingsData spawnSettings, GameTimer timer)
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
                _configurator = new DefaultEnemyConfigurator(_spawnSettings.SpawnPositionRange, _spawnSettings.SpawnPositionOffset);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                Dispose();
                gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _timer.OnTimeChanged += TrySpawnEnemy;
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
            _actualSpawnTasks.AddRange(markedToSpawn.Select(data => SpawnEnemyTask(data, _cancellationTokenSource.Token)));

            // Remove all used enemy spawn data
            _enemySpawnData.RemoveAll(data => markedToSpawn.Contains(data));
        }

        private async UniTask SpawnEnemyTask(Data.EnemySpawnData data, CancellationToken token)
        {
            Transform targetPlayer = ServiceLocator.Current.Get<PlayerManager>()?.GetCharacterTransform();

            if (targetPlayer == null)
            {
                return;
            }

            int count = data.Count;
            float spawnInterval = (data.SpawnDuration / (float)count);
            EnemyController enemy;

            while (count > 0)
            {
                enemy = _source.GetEnemy(data.EnemyData.ID);

                if (enemy != null)
                {
                    _configurator.Configure(enemy, targetPlayer);
                    enemy.gameObject.SetActive(true);
                }
                count--;
                await UniTask.WaitForSeconds(spawnInterval, cancellationToken: token);
            }
            return;
        }

        private void StopAllSpawnTasks()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
            _actualSpawnTasks.Clear();
        }

        private void FilterSpawnData()
        {
            _enemySpawnData = _enemySpawnData.Distinct().OrderBy(data => data.SpawnTime).ToList();
        }

        public void Dispose()
        {
            StopAllSpawnTasks();
            _enemyContainer = null;
            _source = null;
            _timer.OnTimeChanged -= TrySpawnEnemy;
        }

        private void OnDestroy()
        {
            Dispose();
        }

        #endregion
    }
}
