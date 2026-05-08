using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "NewEnemySpawnerSettingsData", menuName = "Game/Settings/Enemy Spawner Data")]
    public class EnemySpawnerSettingsData : ScriptableObject
    {
        [Header("Dynamic Geometry Settings")]
        [Tooltip("Extra distance past the camera corner before enemies can spawn.")]
        [SerializeField] private float _safeZonePadding = 2f;

        [Tooltip("The thickness of the ring where enemies actually spawn.")]
        [SerializeField] private float _spawnRingThickness = 5f;

        [Tooltip("How far past the spawn ring an enemy must be to be teleported/recycled.")]
        [SerializeField] private float _despawnPadding = 10f;

        [Header("Vector Spawning Logic")]
        [Tooltip("Chance (0.0 to 1.0) to spawn enemies in the frontal cone when the player is moving.")]
        [SerializeField, Range(0f, 1f)] private float _frontalSpawnChance = 0.7f;

        [Tooltip("The angle (in degrees) of the frontal spawn cone.")]
        [SerializeField, Range(30f, 180f)] private float _frontalConeAngle = 90f;

        public float SafeZonePadding => _safeZonePadding;
        public float SpawnRingThickness => _spawnRingThickness;
        public float DespawnPadding => _despawnPadding;

        public float FrontalSpawnChance => _frontalSpawnChance;
        public float FrontalConeAngle => _frontalConeAngle;
    }
}
