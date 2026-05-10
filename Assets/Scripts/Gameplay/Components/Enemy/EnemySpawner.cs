using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Characters.Common;
using Characters.Common.Movement;
using Characters.Enemy;
using Cysharp.Threading.Tasks;
using Gameplay.GlobalSettings;
using Settings;
using Settings.Global;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Zenject;

namespace Gameplay.Components.Enemy
{
    public class EnemySpawner : MonoBehaviour, IDisposable, Settings.Global.IInitializable
    {
        #region Fields 

        [SerializeField] private List<EnemySpawnData> _enemySpawnDataList;
        [SerializeField] private Transform _enemyDropItemsContainer;
        [SerializeField] private float _recycleOutOfBoundsIntervalSec = 0.5f;
        private EnemyContainer _enemyContainer;

        private EnemySource _source;
        private GameTimer _timer;
        private List<UniTask> _actualSpawnTasks;
        private CancellationTokenSource _cancellationTokenSource;

        private EnemySpawnerSettingsData _spawnSettings;
        private EnemyConfigurator _configurator;

        private DiContainer _diContainer;
        private PlayerService _playerService;

        private readonly HashSet<IEnemyLogic> _activeEnemies = new HashSet<IEnemyLogic>();

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
                _configurator = new EnemyConfigurator(_spawnSettings, _enemyDropItemsContainer);
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

            _activeEnemies.Clear();
            RecycleOutOfBoundsEnemiesTask(_cancellationTokenSource.Token).Forget();
        }

        public void ReturnEnemy(EnemyController enemy)
        {
            _activeEnemies.Remove(enemy.Logic);

            _configurator.DeconfigureCompletely(enemy);
            _source.ReturnEnemy(enemy);
        }

        public void StopAllSpawnTasks()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
            _actualSpawnTasks?.Clear();
            _activeEnemies.Clear();
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
            ICharacterLogic targetPlayer = _playerService.GetCharacter();

            if (targetPlayer == null)
            {
                return;
            }

            int count = spawnData.Count;
            float spawnInterval = (spawnData.SpawnDuration / (float)count);
            EnemyController enemy;

            while (count > 0)
            {
                enemy = _source.GetEnemy(spawnData.EnemyData.CommonInfo.TypeID);

                if (enemy != null)
                {
                    enemy.SetTargetSpawner(this);
                    _configurator.ConfigureCompletely(enemy, targetPlayer);
                    enemy.gameObject.SetActive(true);

                    _activeEnemies.Add(enemy.Logic);

                    count--;
                }


                bool isCanceled = await UniTask.WaitForSeconds(spawnInterval, cancellationToken: token).SuppressCancellationThrow();

                // Safely exit the spawn loop if the spawner is destroyed or the wave ends
                if (isCanceled)
                {
                    return;
                }
            }
            return;
        }

        // The Engine of the Wrap-around System
        private async UniTask RecycleOutOfBoundsEnemiesTask(CancellationToken token)
        {
            // this check extremely cheap for the CPU.
            while (!token.IsCancellationRequested)
            {
                await UniTask.WaitForSeconds(_recycleOutOfBoundsIntervalSec, cancellationToken: token);

                ICharacterLogic playerLogic = _playerService.GetCharacter();

                if (Camera.main == null || playerLogic == null)
                {
                    continue;
                }

                Vector2 cameraPos = Camera.main.transform.position;

                // Calculate Despawn Distance
                float safeRadius = Utilities.World.PositionGenerator.CalculateSafeRadius(Camera.main, _spawnSettings.SafeZonePadding);
                float spawnRadius = safeRadius + _spawnSettings.SpawnRingThickness;
                float despawnRadius = spawnRadius + _spawnSettings.DespawnPadding;

                // Use sqrMagnitude to completely avoid slow Square Root math
                float despawnSqr = despawnRadius * despawnRadius;

                // Find the player's movement direction for the teleport calculations
                Vector2 playerDirection = Vector2.zero;
                var playerMovement = playerLogic.Body.Movement;

                if (playerMovement != null && playerMovement.IsMoving)
                {
                    playerDirection = playerMovement.Direction;
                }

                // Create a temporary list to avoid modifying the HashSet while looping
                List<IEnemyLogic> enemiesToRecycle = new List<IEnemyLogic>();

                foreach (var enemyLogic in _activeEnemies)
                {
                    Vector2 enemyPos = enemyLogic.Body.OriginalTransform.position;

                    if ((enemyPos - cameraPos).sqrMagnitude > despawnSqr)
                    {
                        enemiesToRecycle.Add(enemyLogic);
                    }
                }

                foreach (var enemy in enemiesToRecycle)
                {
                    RecycleEnemy(enemy, playerLogic);
                }
            }
        }

        private void RecycleEnemy(IEnemyLogic enemyLogic, ICharacterLogic targetPlayer)
        {
            _configurator.DeconfigureFastReuseSettings(enemyLogic);
            _configurator.ConfigureFastReuseSettings(enemyLogic, targetPlayer);
        }

        private void FilterSpawnData()
        {
            _enemySpawnDataList = _enemySpawnDataList.Distinct().OrderBy(data => data.SpawnStartTime).ToList();
        }

        #endregion


        #region Debug & Visualization

        private void OnDrawGizmos()
        {
            if (_spawnSettings == null || Camera.main == null) return;

            Vector3 center = Camera.main.transform.position;
            center.z = 0; // Flatten for 2D

            float safeRadius = Utilities.World.PositionGenerator.CalculateSafeRadius(Camera.main, _spawnSettings.SafeZonePadding);
            float spawnRadius = safeRadius + _spawnSettings.SpawnRingThickness;
            float despawnRadius = spawnRadius + _spawnSettings.DespawnPadding;

            // Safe Zone (Red)
            Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
            Gizmos.DrawWireSphere(center, safeRadius);

            // Spawn Zone (Yellow)
            Gizmos.color = new Color(1f, 0.92f, 0.016f, 0.5f);
            Gizmos.DrawWireSphere(center, spawnRadius);

            // Despawn / Dead Zone (Gray)
            Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.3f);
            Gizmos.DrawWireSphere(center, despawnRadius);

            // Frontal Cone Preview (Cyan)
            Gizmos.color = new Color(0f, 1f, 1f, 0.8f);

            var character = _playerService?.GetCharacter();

            if (character != null)
            {
                DrawGizmoSpawnCone(center, character.Body.Movement.Direction, safeRadius, spawnRadius, _spawnSettings.FrontalConeAngle);
            }
            else
            {
                DrawGizmoSpawnCone(center, Vector3.right, safeRadius, spawnRadius, _spawnSettings.FrontalConeAngle);
            }
        }

        /// <summary>
        /// Draws a 2D geometric cone slice between an inner and outer radius.
        /// </summary>
        private void DrawGizmoSpawnCone(Vector3 center, Vector3 direction, float innerRadius, float outerRadius, float angleDegrees)
        {
            float halfAngle = angleDegrees / 2f;

            // Calculate the left and right boundary directions
            Quaternion leftRotation = Quaternion.Euler(0, 0, halfAngle);
            Quaternion rightRotation = Quaternion.Euler(0, 0, -halfAngle);

            Vector3 leftDirection = leftRotation * direction;
            Vector3 rightDirection = rightRotation * direction;

            // Draw the two straight side walls of the cone
            Gizmos.DrawLine(center + leftDirection * innerRadius, center + leftDirection * outerRadius);
            Gizmos.DrawLine(center + rightDirection * innerRadius, center + rightDirection * outerRadius);

            // Draw the curved inner and outer arcs
            int segments = 20; // curve smoothness
            float angleStep = angleDegrees / segments;

            Vector3 prevInner = center + leftDirection * innerRadius;
            Vector3 prevOuter = center + leftDirection * outerRadius;

            for (int i = 1; i <= segments; i++)
            {
                // Rotate step-by-step from left to right
                Quaternion stepRotation = Quaternion.Euler(0, 0, halfAngle - (angleStep * i));
                Vector3 currentDirection = stepRotation * direction;

                Vector3 currentInner = center + currentDirection * innerRadius;
                Vector3 currentOuter = center + currentDirection * outerRadius;

                // Draw the short line segments
                Gizmos.DrawLine(prevInner, currentInner);
                Gizmos.DrawLine(prevOuter, currentOuter);

                prevInner = currentInner;
                prevOuter = currentOuter;
            }
        }

        #endregion
    }
}
