using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Characters.Common.Combat.Weapons
{
    /// <summary>
    /// Base class for all static world-coordinate hazards (Puddles, Traps, Zones).
    /// Bypasses the burst-pulse loop to spawn persistent entities.
    /// </summary>
    public abstract class StaticHazardWeaponBase<TSettings, TEntity> : AutonomousBurstWeaponBase<TSettings, TEntity>, IUpgradableHazardWeapon
        where TSettings : HazardAttackSettings
        where TEntity : StaticHazardEntity
    {
        protected readonly List<TEntity> ActiveHazards = new List<TEntity>();

        // Multiplier tracking for spawn boundaries
        private float _minSpawnRadiusMultiplier = 1f;
        private float _maxSpawnRadiusMultiplier = 1f;

        protected override async UniTask PerformAttackPhase(CancellationToken token)
        {
            FireWeapon();

            // Wait for the entire attack phase to finish while the hazards sit in the world
            await UniTask.WaitForSeconds(UpgradedAttackSettings.FullDurationTimeSec, cancellationToken: token)
                         .SuppressCancellationThrow();

            ActiveHazards.Clear();
        }

        #region Runtime Hazard Upgrade Handlers

        public virtual void ApplyMinSpawnRadiusUpgrade(float multiplier)
        {
            _minSpawnRadiusMultiplier += multiplier;
            UpgradedAttackSettings.MinSpawnRadius = InitialAttackSettings.MinSpawnRadius * _minSpawnRadiusMultiplier;

            // Note: No Invalidate and Refresh here
            // Existing hazards stay where they are; only new ones use the new spawn radius.
        }

        public virtual void ApplyMaxSpawnRadiusUpgrade(float multiplier)
        {
            _maxSpawnRadiusMultiplier += multiplier;
            UpgradedAttackSettings.MaxSpawnRadius = InitialAttackSettings.MaxSpawnRadius * _maxSpawnRadiusMultiplier;
        }

        #endregion


        // Concrete implementations will override FireWeapon() 
        // to calculate specific spawn offsets and instantiate the entities.

        protected virtual void HandleHazardDeath(AutonomousEntityBase entity)
        {
            if (entity is TEntity hazard)
            {
                ActiveHazards.Remove(hazard);
                EntityPool.ReturnItem(hazard);
            }
        }
    }
}
