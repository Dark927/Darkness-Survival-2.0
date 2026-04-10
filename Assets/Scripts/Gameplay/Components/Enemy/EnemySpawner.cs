using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Characters.Enemy;
using Cysharp.Threading.Tasks;
using Gameplay.GlobalSettings;
using Settings;
using Settings.Global;
using UnityEngine;
using Zenject;

namespace Gameplay.Components.Enemy
{
    public class EnemySpawner : MonoBehaviour, IDisposable, Settings.Global.IInitializable
    {
        #region Fields 

        [SerializeField] private List<EnemySpawnData> _enemySpawnDataList;
        private EnemyContainer _enemyContainer;

        private EnemySource _source;
        private GameTimer _timer;
        private List<UniTask> _actualSpawnTasks;
        private CancellationTokenSource _cancellationTokenSource;

        private EnemySpawnerSettingsData _spawnSettings;
        private ICharacterConfigurator<EnemyController> _configurator;

        private DiContainer _diContainer;
        private PlayerService _playerService;

        #endregion


        #region Methods

        #region Init

        [Inject]
        public void Construct(DiContainer diContainer, EnemySpawnerSettingsData spawnSettings, GameTimer timer)
        {
            _diContainer = diContainer;
            _spawnSettings = spawnSettings;
            _timer = timer;
        }

        public void Initialize()
        {
            try
            {
                CheckTimer();
                TryInitContainer();
                InitEnemySource();

                _actualSpawnTasks = new List<UniTask>();
                _configurator = new EnemyConfigurator(_spawnSettings.SpawnPositionRange, _spawnSettings.SpawnPositionOffset);
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
            _source = _diContainer.Instantiate<EnemySource>(new object[] { _enemySpawnDataList, _enemyContainer });
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

        public void StartEnemySpawn()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _timer.OnTimeChanged += TrySpawnEnemy;
            _playerService = ServiceLocator.Current.Get<PlayerService>();
        }

        public void ReturnEnemy(EnemyController enemy)
        {
            _configurator.Deconfigure(enemy);
            _source.ReturnEnemy(enemy);
        }

        public void StopAllSpawnTasks()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
            _actualSpawnTasks?.Clear();
        }

        private void TrySpawnEnemy(StageTime time)
        {
            // Filter enemy spawn data
            var markedToSpawn = _enemySpawnDataList
                .Where(data => data.SpawnStartTime <= time);

            // Add tasks for the filtered data
            _actualSpawnTasks.AddRange(markedToSpawn.Select(data => SpawnEnemyTask(data, _cancellationTokenSource.Token)));

            // Remove all used enemy spawn data
            _enemySpawnDataList.RemoveAll(data => markedToSpawn.Contains(data));
        }

        private async UniTask SpawnEnemyTask(EnemySpawnData spawnData, CancellationToken token)
        {
            Transform targetPlayer = _playerService.GetCharacterTransform();

            if (targetPlayer == null)
            {
                return;
            }

            int count = spawnData.Count;
            float spawnInterval = (spawnData.SpawnDuration / (float)count);
            EnemyController enemy;

            while (count > 0 && !token.IsCancellationRequested)
            {
                enemy = _source.GetEnemy(spawnData.EnemyData.CommonInfo.TypeID);

                if (enemy != null)
                {
                    enemy.SetTargetSpawner(this);
                    _configurator.Configure(enemy, targetPlayer);
                    enemy.gameObject.SetActive(true);
                }
                count--;
                await UniTask.WaitForSeconds(spawnInterval, cancellationToken: token);
            }
            return;
        }

        private void FilterSpawnData()
        {
            _enemySpawnDataList = _enemySpawnDataList.Distinct().OrderBy(data => data.SpawnStartTime).ToList();
        }

        #endregion
    }
}
