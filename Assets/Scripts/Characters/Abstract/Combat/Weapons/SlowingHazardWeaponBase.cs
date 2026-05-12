using Characters.Common.CustomPhysics2D;
using Characters.Common.Statuses;
using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    public abstract class SlowingHazardWeaponBase<TSettings, TEntity> : HazardWeaponBase<TSettings, TEntity>, IUpgradableSlowingWeapon
        where TSettings : SlowingHazardAttackSettings
        where TEntity : AutonomousEntityBase
    {
        private float _slowStrengthOffset = 0f;

        public virtual void ApplySlowStrengthUpgrade(float additionalSlowPenalty)
        {
            _slowStrengthOffset += additionalSlowPenalty;
            UpgradedAttackSettings.SlowMultiplier = Mathf.Clamp(InitialAttackSettings.SlowMultiplier - _slowStrengthOffset, 0.1f, 1f);
        }

        protected override void FireWeapon()
        {
            int spawnCount = Mathf.Max(1, UpgradedAttackSettings.SpawnCount);
            Transform ownerTransform = Owner.Body.TargetingTransform;

            for (int i = 0; i < spawnCount; i++)
            {
                Vector2 offsetDir = Random.insideUnitCircle.normalized;
                float randomDistance = Random.Range(UpgradedAttackSettings.MinSpawnRadius, UpgradedAttackSettings.MaxSpawnRadius);
                Vector3 spawnPosition = ownerTransform.position + (Vector3)(offsetDir * randomDistance);

                TEntity hazard = EntityPool.RequestObject();
                if (hazard != null)
                {
                    // Delegate the specific activation to the child class
                    SetupAndActivateHazard(hazard, spawnPosition, offsetDir);
                    ActiveHazards.Add(hazard);
                }
            }
        }

        // Child classes MUST implement this to activate their specific entity type
        protected abstract void SetupAndActivateHazard(TEntity entity, Vector3 spawnPosition, Vector2 spawnDirection);


        // Shared helper to apply Damage and Slow instantly
        protected void ApplyStandardDamageAndSlow(Collider2D targetCollider)
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
