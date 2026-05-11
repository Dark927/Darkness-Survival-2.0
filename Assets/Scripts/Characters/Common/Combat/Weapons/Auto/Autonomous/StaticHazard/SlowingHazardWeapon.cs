using System.Threading;
using Characters.Common.Combat.Weapons.Data;
using Characters.Common.CustomPhysics2D;
using Characters.Common.Statuses;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    public class SlowingHazardWeapon : StaticHazardWeaponBase<SlowingHazardAttackSettings, StaticHazardEntity>, IUpgradableSlowingWeapon
    {
        private float _slowStrengthOffset = 0f;

        public virtual void ApplySlowStrengthUpgrade(float additionalSlowPenalty)
        {
            _slowStrengthOffset += additionalSlowPenalty;
            UpgradedAttackSettings.SlowMultiplier = Mathf.Clamp(InitialAttackSettings.SlowMultiplier - _slowStrengthOffset, 0.1f, 1f);
        }

        protected override void FireWeapon()
        {
            int spawnCount = UpgradedAttackSettings.SpawnCount;
            if (spawnCount < 1) spawnCount = 1;

            float radius = UpgradedAttackSettings.AttackRadius;
            float lifetime = UpgradedAttackSettings.FullDurationTimeSec;
            float tickRate = UpgradedAttackSettings.TriggerActivityTimeSec;

            // Pull the boundaries from your new settings class
            float minRadius = UpgradedAttackSettings.MinSpawnRadius;
            float maxRadius = UpgradedAttackSettings.MaxSpawnRadius;

            Transform ownerTransform = Owner.Body.TargetingTransform;

            for (int i = 0; i < spawnCount; i++)
            {
                // random direction
                Vector2 offsetDir = Random.insideUnitCircle.normalized;

                // get a random distance between the Min and Max boundaries
                float randomDistance = Random.Range(minRadius, maxRadius);

                // calculate final spawn position
                Vector3 spawnPosition = ownerTransform.position + (Vector3)(offsetDir * randomDistance);

                SpawnHazard(spawnPosition, radius, lifetime, tickRate);
            }
        }

        private void SpawnHazard(Vector3 position, float radius, float lifetime, float tickRate)
        {
            StaticHazardEntity hazard = EntityPool.RequestObject();
            if (hazard == null) return;

            hazard.ActivateHazard(
                position: position,
                radius: radius,
                lifeTime: lifetime,
                tickRate: tickRate,
                targetMask: _enemyLayerMask,
                onTickHit: HandleHazardTickHit,
                onDie: HandleHazardDeath
            );

            ActiveHazards.Add(hazard);
        }

        private void HandleHazardTickHit(Collider2D targetCollider, StaticHazardEntity hazard)
        {
            HitTargetListener(this, new AttackTriggerArgs(targetCollider));

            if (targetCollider.TryGetComponent<EntityColliderLink>(out var link) && link.Logic.Status != null)
            {
                float statusDuration = UpgradedAttackSettings.TriggerActivityTimeSec + 0.1f;
                link.Logic.Status.Apply(new SlowStatusEffect(statusDuration, UpgradedAttackSettings.SlowMultiplier));
            }
        }
    }
}
