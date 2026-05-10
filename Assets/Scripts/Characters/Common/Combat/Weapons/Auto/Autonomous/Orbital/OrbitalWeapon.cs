using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    /// <summary>
    /// Handles weapons that maintain an active orbit around a target transform.
    /// Supports real-time state invalidation to instantly reflect runtime stat upgrades.
    /// </summary>
    public class OrbitalWeapon : AutonomousBurstWeaponBase<OrbitalAttackSettings, OrbitalEntity>, IUpgradableOrbitalWeapon
    {
        private float _orbitSpeedMultiplier = 1f;
        private float _currentPhaseEndTime;

        private readonly List<OrbitalEntity> _activeEntities = new List<OrbitalEntity>();

        protected override async UniTask PerformAttackPhase(CancellationToken token)
        {
            // Cache the duration so the UniTask wait and the EndTime math are perfectly synchronized,
            // even if the duration stat is upgraded mid-phase.
            float currentPhaseDuration = UpgradedAttackSettings.FullDurationTimeSec;
            _currentPhaseEndTime = Time.time + currentPhaseDuration;

            FireWeapon();

            await UniTask.WaitForSeconds(currentPhaseDuration, cancellationToken: token)
                         .SuppressCancellationThrow();

            _activeEntities.Clear();
        }

        #region Runtime Upgrade Handlers

        public override void ApplyAttackCountUpgrade(int additionalCount)
        {
            base.ApplyAttackCountUpgrade(additionalCount);
            InvalidateAndRefreshEntities();
        }

        public void ApplyOrbitalSpeedUpgrade(float multiplier)
        {
            _orbitSpeedMultiplier += multiplier;
            UpgradedAttackSettings.OrbitSpeed = InitialAttackSettings.OrbitSpeed * _orbitSpeedMultiplier;
            InvalidateAndRefreshEntities();
        }

        public override void ApplyAttackRadiusUpgrade(float multiplier)
        {
            base.ApplyAttackRadiusUpgrade(multiplier);
            InvalidateAndRefreshEntities();
        }

        #endregion

        #region Entity Spawning & Pooling

        protected override void FireWeapon()
        {
            _activeEntities.Clear();

            // Calculate exact time remaining for the current attack phase to prevent visual overlap bugs
            float remainingLifetime = Mathf.Max(0f, _currentPhaseEndTime - Time.time);

            // Prevent spawning if the phase is mathematically over
            if (remainingLifetime <= 0f) return;

            int entityCount = UpgradedAttackSettings.SpawnCount;
            float angleStep = 360f / entityCount;
            float finalOrbitRadius = UpgradedAttackSettings.AttackRadius;
            float finalOrbitSpeed = UpgradedAttackSettings.OrbitSpeed;

            Transform orbitCenter = Owner.Body.TargetingTransform;

            for (int i = 0; i < entityCount; i++)
            {
                float startAngle = angleStep * i;
                SpawnEntity(orbitCenter, startAngle, finalOrbitRadius, finalOrbitSpeed, remainingLifetime);
            }
        }

        private void SpawnEntity(Transform center, float startAngle, float radius, float speed, float lifetime)
        {
            OrbitalEntity entity = EntityPool.RequestObject();
            if (entity == null)
            {
                return;
            }

            entity.gameObject.SetActive(true);

            entity.ActivateOrbit(
                orbitCenter: center,
                startAngleDegrees: startAngle,
                orbitRadius: radius,
                orbitSpeed: speed,
                lifeTime: lifetime,
                damageTickRate: UpgradedAttackSettings.TriggerActivityTimeSec,
                targetMask: _enemyLayerMask,
                onHit: HandleEntityHit,
                onDie: HandleEntityDeath
            );

            _activeEntities.Add(entity);
        }

        /// <summary>
        /// Flushes current active entities and forces a respawn to guarantee 
        /// symmetrical angle distribution after a runtime stat modification.
        /// </summary>
        private void InvalidateAndRefreshEntities()
        {
            if (_activeEntities.Count == 0) return;

            for (int i = _activeEntities.Count - 1; i >= 0; i--)
            {
                var entity = _activeEntities[i];
                entity.gameObject.SetActive(false);
                EntityPool.ReturnItem(entity);
            }

            _activeEntities.Clear();
            FireWeapon();
        }

        #endregion

        #region Callbacks

        private void HandleEntityHit(Collider2D targetCollider, OrbitalEntity entity)
        {
            HitTargetListener(this, new AttackTriggerArgs(targetCollider));
        }

        private void HandleEntityDeath(AutonomousEntityBase entity)
        {
            if (entity is OrbitalEntity orbitalEntity)
            {
                _activeEntities.Remove(orbitalEntity);
                EntityPool.ReturnItem(orbitalEntity);
            }
        }

        #endregion
    }
}
