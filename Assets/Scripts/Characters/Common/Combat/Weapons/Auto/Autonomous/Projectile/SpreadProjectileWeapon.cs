using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    public class SpreadProjectileWeapon : AutonomousBurstWeaponBase<BurstAoeAttackSettings, ProjectileEntity>, IElementWithExtraVisual
    {
        [Header("Targeting Settings")]
        [Tooltip("How much random spread (in degrees) to apply when aiming at an enemy.")]
        [SerializeField] private float _aimSpreadDegrees = 10f;
        private static Collider2D[] _targetBuffer = new Collider2D[20];

        protected override void FireWeapon()
        {
            int projectilesToFire = UpgradedAttackSettings.SpawnCount;
            float dynamicSpeed = UpgradedAttackSettings.AttackRadius / UpgradedAttackSettings.TriggerActivityTimeSec;
            float projectileLifeTime = UpgradedAttackSettings.AttackRadius / dynamicSpeed;

            // Radar Scan
            float searchRadius = UpgradedAttackSettings.AttackRadius * 2f;
            int hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, searchRadius, _targetBuffer, _enemyLayerMask);

            // Sort the targets so the closest enemies are at the start of the array
            if (hitCount > 1)
            {
                SortTargetsByDistance(hitCount);
            }

            for (int i = 0; i < projectilesToFire; i++)
            {
                ProjectileEntity projectile = EntityPool.RequestObject();
                if (projectile == null) continue;

                Vector2 direction;

                if (hitCount > 0)
                {
                    // Round-Robin Targeting (prioritizes CLOSEST enemies first)
                    Collider2D target = _targetBuffer[i % hitCount];
                    Vector2 baseDirection = ((Vector2)target.bounds.center - (Vector2)transform.position).normalized;

                    // Center-Out Fan Math
                    int targetShotIndex = i / hitCount;
                    int multiplier = (targetShotIndex + 1) / 2;
                    float sign = (targetShotIndex % 2 == 0) ? 1f : -1f;
                    float spread = multiplier * sign * _aimSpreadDegrees;

                    direction = Quaternion.Euler(0, 0, spread) * baseDirection;
                }
                else
                {
                    // Fallback: completely random 360 if no enemies are around
                    float randomAngle = Random.Range(0f, 360f);
                    direction = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));
                }

                projectile.Fire(
                    startPos: transform.position,
                    direction: direction,
                    speed: dynamicSpeed,
                    lifeTime: projectileLifeTime,
                    targetMask: _enemyLayerMask,
                    onHit: HandleProjectileHit,
                    onDie: (baseEntity) => EntityPool.ReturnItem((ProjectileEntity)baseEntity)
                );
            }
        }

        /// <summary>
        /// A highly optimized, zero-allocation Insertion Sort.
        /// Sorts the buffer so index [0] is the closest enemy.
        /// </summary>
        private void SortTargetsByDistance(int validHitCount)
        {
            Vector3 currentPos = transform.position;

            for (int i = 1; i < validHitCount; i++)
            {
                Collider2D key = _targetBuffer[i];

                // Calculate distance to the center of the collider
                float keyDist = (key.bounds.center - currentPos).sqrMagnitude;

                int j = i - 1;

                // Compare against the center of the other colliders
                while (j >= 0 && (_targetBuffer[j].bounds.center - currentPos).sqrMagnitude > keyDist)
                {
                    _targetBuffer[j + 1] = _targetBuffer[j];
                    j--;
                }
                _targetBuffer[j + 1] = key;
            }
        }

        private void HandleProjectileHit(Collider2D targetCollider, ProjectileEntity projectile)
        {
            HitTargetListener(this, new AttackTriggerArgs(targetCollider));
        }

        private void OnDrawGizmosSelected()
        {
            if (UpgradedAttackSettings != null)
            {
                Gizmos.color = new Color(0f, 1f, 1f, 0.2f);
                Gizmos.DrawWireSphere(transform.position, UpgradedAttackSettings.AttackRadius * 2f);
            }
        }
    }
}
