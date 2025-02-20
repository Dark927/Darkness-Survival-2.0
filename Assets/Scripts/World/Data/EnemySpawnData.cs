using Characters.Enemy.Data;
using UnityEngine;
using World.Components;


namespace World.Data
{
    [CreateAssetMenu(fileName = "NewEnemySpawnData", menuName = "Game/World/Data/Enemy Spawn Data")]
    public class EnemySpawnData : ScriptableObject
    {
        [Header("Enemy Settings")]

        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private GameObject _enemyPrefab;

        [Header("Time Settings")]
        [Space]
        [SerializeField] private int _count;

        [Tooltip("The time when enemies should be spawned.")]
        [SerializeField] private StageTime _spawnStartTime;

        [Space]
        [Tooltip("The time during which the enemies have to spawn.")]
        [SerializeField] private StageTime _spawnDuration = new StageTime() { Minutes = 0, Seconds = 1 };

        public EnemyData EnemyData => _enemyData;
        public GameObject EnemyPrefab => _enemyPrefab;
        public int Count => _count;
        public StageTime SpawnStartTime => _spawnStartTime;
        public StageTime SpawnDuration => _spawnDuration;


        public static implicit operator EnemyData(EnemySpawnData data)
        {
            return data.EnemyData;
        }
    }
}
