using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "NewEnemySpawnSettings", menuName = "Game/Settings/Enemy Spawn Settings")]
    public class EnemySpawnSettings : ScriptableObject
    {
        [Header("Main Settings")]

        [SerializeField] private Vector2 _spawnPositionRange = Vector2.zero;
        [SerializeField] private Vector2 _spawnPositionOffset = Vector2.zero;

        public Vector2 SpawnPositionRange => _spawnPositionRange;
        public Vector2 SpawnPositionOffset => _spawnPositionOffset;
    }
}