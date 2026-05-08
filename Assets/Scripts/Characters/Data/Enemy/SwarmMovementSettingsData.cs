using UnityEngine;

namespace Characters.Enemy.Settings
{
    [CreateAssetMenu(fileName = "NewSwarmMovementData", menuName = "Game/Settings/Swarm Movement Data")]
    public class SwarmMovementSettingsData : ScriptableObject
    {
        [Header("Soft Separation Radar")]
        [Tooltip("The layer mask used to detect other enemies.")]
        [SerializeField] private LayerMask _enemyLayerMask;

        [Tooltip("How close an enemy needs to be before they start pushing away.")]
        [SerializeField, Min(0f)] private float _separationRadius = 1.0f;

        [Tooltip("How strongly they repel each other vs pursuing the player (0 = no repel, 1 = maximum repel).")]
        [SerializeField, Range(0f, 2f)] private float _separationWeight = 0.8f;

        public LayerMask EnemyLayerMask => _enemyLayerMask;
        public float SeparationRadius => _separationRadius;
        public float SeparationWeight => _separationWeight;
    }
}
