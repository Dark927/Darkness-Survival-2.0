using UnityEngine;
using Utilities.World;

namespace World.Components.EnemyLogic
{
    public class DefaultEnemyConfigurator : ICharacterConfigurator
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

        public void Configure(GameObject enemyObj)
        {
            enemyObj.transform.position = PositionGenerator.GetRandomPositionOutsideCamera(Camera.main, _spawnPositionRange, _spawnPositionOffset);
        }
    }
}
