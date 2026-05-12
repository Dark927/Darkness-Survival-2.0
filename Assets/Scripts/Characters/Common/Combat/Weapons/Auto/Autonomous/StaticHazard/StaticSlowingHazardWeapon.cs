using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    public class StaticSlowingHazardWeapon : SlowingHazardWeaponBase<SlowingHazardAttackSettings, StaticHazardEntity>
    {
        protected override void SetupAndActivateHazard(StaticHazardEntity puddle, Vector3 spawnPosition, Vector2 spawnDirection)
        {
            puddle.ActivateHazard(
                position: spawnPosition,
                radius: UpgradedAttackSettings.AttackRadius,
                lifeTime: UpgradedAttackSettings.FullDurationTimeSec,
                tickRate: UpgradedAttackSettings.TriggerActivityTimeSec,
                targetMask: _enemyLayerMask,
                onTickHit: (target, entity) => ApplyStandardDamageAndSlow(target),
                onDie: HandleHazardDeath
            );
        }
    }
}
