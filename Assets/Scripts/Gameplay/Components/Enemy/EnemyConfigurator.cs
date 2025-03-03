using Characters.Enemy;
using Characters.Interfaces;
using Settings.Global;
using UnityEngine;
using Utilities.World;

namespace Gameplay.Components.Enemy
{
    public class DefaultEnemyConfigurator : ICharacterConfigurator<EnemyController>
    {
        private EnemyManagementService _managementService;
        private Vector2 _spawnPositionOffset = Vector2.zero;
        private Vector2 _spawnPositionRange = Vector2.zero;

        public DefaultEnemyConfigurator() { }

        public DefaultEnemyConfigurator(Vector2 spawnPositionRange, Vector2 spawnPositionOffset)
        {
            SetSpawnPositionRange(spawnPositionRange);
            SetSpawnPositionOffset(spawnPositionOffset);
        }

        public void SetSpawnPositionOffset(Vector2 spawnPositionOffset)
        {
            _spawnPositionOffset = spawnPositionOffset;
        }

        public void SetSpawnPositionRange(Vector2 spawnPositionRange)
        {
            _spawnPositionRange = spawnPositionRange;
        }

        public void Configure(EnemyController enemy, Transform target = null)
        {
            _managementService ??= ServiceLocator.Current.Get<EnemyManagementService>();

            enemy.transform.position = PositionGenerator.GetRandomPositionOutsideCamera(Camera.main, _spawnPositionRange, _spawnPositionOffset);

            if (target == null)
            {
                return;
            }

            IEnemyLogic enemyLogic = enemy.GetComponentInChildren<IEnemyLogic>();

            if (enemyLogic != null)
            {
                DefaultEnemyBody enemyBody = (enemyLogic.Body as DefaultEnemyBody);
                enemyBody.SetTarget(target);
                enemyBody.OnBodyDamagedWithArgs += _managementService.EnemyDamagedListener;
            }
            enemy.ConfigureEventLinks();
        }

        public void Deconfigure(EnemyController enemy)
        {
            enemy.ResetCharacter();
            enemy.EntityLogic.Body.OnBodyDamagedWithArgs -= _managementService.EnemyDamagedListener;
        }
    }
}
