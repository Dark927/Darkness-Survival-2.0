using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    public class SlowingTornadoWeapon : SlowingHazardWeaponBase<VacuumHazardAttackSettings, TornadoHazardEntity>, IUpgradableStormWeapon
    {
        private float _vacuumStrengthMultiplier = 1f;
        private float _vacuumRadiusMultiplier = 1f;
        private float _moveSpeedMultiplier = 1f;

        public void ApplyVacuumStrengthUpgrade(float multiplier)
        {
            _vacuumStrengthMultiplier += multiplier;
            UpgradedAttackSettings.PullStrength = InitialAttackSettings.PullStrength * _vacuumStrengthMultiplier;
        }

        public void ApplyVacuumRadiusUpgrade(float multiplier)
        {
            _vacuumRadiusMultiplier += multiplier;
            UpgradedAttackSettings.PullRadius = InitialAttackSettings.PullRadius * _vacuumRadiusMultiplier;
        }

        public void ApplyMovementSpeedUpgrade(float multiplier)
        {
            _moveSpeedMultiplier += multiplier;
            UpgradedAttackSettings.MoveSpeed = InitialAttackSettings.MoveSpeed * _moveSpeedMultiplier;
        }

        protected override void SetupAndActivateHazard(TornadoHazardEntity tornado, Vector3 spawnPosition, Vector2 spawnDirection)
        {
            tornado.ActivateTornado(
                position: spawnPosition,
                direction: spawnDirection,
                moveSpeed: UpgradedAttackSettings.MoveSpeed,
                attackRadius: UpgradedAttackSettings.AttackRadius,
                pullRadius: UpgradedAttackSettings.PullRadius,
                pullStrength: UpgradedAttackSettings.PullStrength,
                lifeTime: UpgradedAttackSettings.FullDurationTimeSec,
                tickRate: UpgradedAttackSettings.TriggerActivityTimeSec,
                targetMask: _enemyLayerMask,
                onDamageTick: HandleTornadoDamageTick,
                onDie: HandleHazardDeath
            );
        }

        private void HandleTornadoDamageTick(Collider2D targetCollider, TornadoHazardEntity tornado)
        {
            ApplyStandardDamageAndSlow(targetCollider);
        }
    }
}
