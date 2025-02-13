using Characters.Enemy;
using Characters.Interfaces;
using UnityEngine;
using Utilities.World;
using Cysharp.Threading.Tasks;

namespace World.Components.EnemyLogic
{
    public class DefaultEnemyConfigurator : ICharacterConfigurator<EnemyController>
    {
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
            enemy.transform.position = PositionGenerator.GetRandomPositionOutsideCamera(Camera.main, _spawnPositionRange, _spawnPositionOffset);

            if (target == null)
            {
                return;
            }

            IEnemyLogic enemyLogic = enemy.GetComponentInChildren<IEnemyLogic>();

            if (enemyLogic != null)
            {
                (enemyLogic.Body as DefaultEnemyBody).SetTarget(target);
            }

            enemy.ResetCharacter();
            enemy.ConfigureEventLinks();
        }
    }
}
