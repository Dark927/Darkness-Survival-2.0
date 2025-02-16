using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "NewEnemySpawnerSettingsData", menuName = "Game/Settings/Enemy Spawner Data")]
    public class EnemySpawnerSettingsData : ScriptableObject
    {
        [Header("Main Settings")]

        [SerializeField] private Vector2 _spawnPositionRange = Vector2.zero;
        [SerializeField] private Vector2 _spawnPositionOffset = Vector2.zero;

        public Vector2 SpawnPositionRange => _spawnPositionRange;
        public Vector2 SpawnPositionOffset => _spawnPositionOffset;
    }
}