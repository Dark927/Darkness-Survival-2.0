using Characters.Common;
using Characters.Enemy;
using Settings;
using Settings.Global;
using UnityEngine;
using Utilities.ErrorHandling;
using Utilities.World;

namespace Gameplay.Components.Enemy
{
    public class EnemyConfigurator : ICharacterConfigurator<EnemyController>
    {
        private EnemyManagementService _managementService;
        private GameStateService _gameStateService;

        private EnemySpawnerSettingsData _spawnSettings;
        private Transform _dropItemsContainer;

        public EnemyConfigurator() { }

        public EnemyConfigurator(EnemySpawnerSettingsData spawnSettings, Transform dropItemsContainer = null)
        {
            _spawnSettings = spawnSettings;
            SetDropItemsContainer(dropItemsContainer);
        }

        public void SetDropItemsContainer(Transform container)
        {
            _dropItemsContainer = container;
        }

        // CORE SETUP (Called only once when pulling from the pool)
        public void ConfigureCompletely(EnemyController enemy, ICharacterLogic target = null)
        {
            _managementService ??= ServiceLocator.Current.Get<EnemyManagementService>();
            _gameStateService ??= ServiceLocator.Current.Get<GameStateService>();

            IEnemyLogic enemyLogic = enemy.Logic;

            // Wire up all references and events
            if (target != null && enemyLogic != null)
            {
                enemy.ResetCharacter();
                ConfigureFastReuseSettings(enemyLogic, target);
                enemyLogic.SetDropItemContainer(_dropItemsContainer);

                enemy.Logic.Body.Physics.EnableCollisions();
                enemyLogic.Body.OnBodyDamagedWithArgs += _managementService.EnemyDamagedListener;
                enemy.OnEntityKilled += _managementService.EnemyKilledListener;
                enemy.ConfigureEventLinks();
                _gameStateService?.GameEvent.Subscribe(enemy);
            }
        }

        public void ConfigureFastReuseSettings(IEnemyLogic enemyLogic, ICharacterLogic target)
        {
            // Move the enemy to the correct spawn location
            PlaceOnSpawnRing(enemyLogic, target);

            enemyLogic.SetTarget(target.Body.OriginalTransform);
        }

        public void DeconfigureFastReuseSettings(IEnemyLogic enemyLogic)
        {
            enemyLogic.ResetState();
        }

        // REUSABLE MATH (Called during spawn AND recycling)
        public void PlaceOnSpawnRing(IEnemyLogic enemyLogic, ICharacterLogic target = null)
        {
            Vector2 targetDirection = Vector2.zero;
            float targetSpeed = 0f;

            // Find Target Direction
            if (target != null)
            {
                var targetMovement = target.Body.Movement;
                if (targetMovement != null && targetMovement.IsMoving)
                {
                    targetDirection = targetMovement.Direction;
                    targetSpeed = targetMovement.Speed.ActualSpeed;
                }
                else
                {
                    var targetRb = target.Body.Physics.Rigidbody2D;
                    targetDirection = targetRb.velocity.normalized;
                    targetSpeed = targetDirection.sqrMagnitude;
                }
            }

            // Generate Directional Spawn Position
            Vector2 spawnPos;

            if (targetSpeed > 0.1f)
            {
                // Dynamic target - spawn in front of it with chance
                spawnPos = PositionGenerator.GetDirectionalSpawnPosition(
                    Camera.main,
                    _spawnSettings.SafeZonePadding,
                    _spawnSettings.SpawnRingThickness,
                    targetDirection,
                    _spawnSettings.FrontalSpawnChance,
                    _spawnSettings.FrontalConeAngle
                );
            }
            else
            {
                // Static target - just spawn around it
                spawnPos = PositionGenerator.GetDirectionalSpawnPosition(
                    Camera.main,
                    _spawnSettings.SafeZonePadding,
                    _spawnSettings.SpawnRingThickness
                );
            }


            // Force Position
            Rigidbody2D enemyRb = enemyLogic.Body.Physics.Rigidbody2D;

            if (enemyRb != null)
            {
                enemyRb.position = spawnPos;
                enemyRb.velocity = Vector2.zero;
                enemyLogic.Body.OriginalTransform.position = spawnPos; // Fallback sync
            }
            else
            {
                enemyLogic.Body.OriginalTransform.position = spawnPos;
            }
        }

        public void DeconfigureCompletely(EnemyController enemy)
        {
            enemy.Logic.Body.Physics.DisableCollisions();
            enemy.RemoveEventLinks();
            enemy.EntityLogic.Body.OnBodyDamagedWithArgs -= _managementService.EnemyDamagedListener;
            enemy.OnEntityKilled -= _managementService.EnemyKilledListener;
            _gameStateService?.GameEvent.Unsubscribe(enemy);
        }
    }
}
