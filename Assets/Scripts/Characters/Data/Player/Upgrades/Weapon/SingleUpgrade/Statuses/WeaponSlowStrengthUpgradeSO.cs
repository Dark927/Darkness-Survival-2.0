using Characters.Common.Combat.Weapons;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "WeaponSlowStrengthUpgradeSO", menuName = "Game/Upgrades/Weapon Upgrades/Single Upgrades/Statuses/Slow Strength Upgrade")]
    public class WeaponSlowStrengthUpgradeSO : SingleUniversalUpgradeSO<IUpgradableSlowingWeapon>
    {
        [Tooltip("The percentage to increase the slow effect by (e.g., 10 makes a 60% speed penalty a 50% speed penalty).")]
        [SerializeField, Min(0f)] private float _slowStrengthUpgradePercent = 10f;

        protected override string GetDefaultUpgradeName() => "Slow Strength";

        protected override string GetUpgradeValueInfo(char originalSign, char displaySign)
        {
            return $"{displaySign} {_slowStrengthUpgradePercent}%";
        }

        public override void ApplyUpgrade(IUpgradableSlowingWeapon target)
        {
            // The weapon's internal math handles clamping the multiplier
            target.ApplySlowStrengthUpgrade(_slowStrengthUpgradePercent / 100f);
        }

        public override void ApplyDowngrade(IUpgradableSlowingWeapon target)
        {
            target.ApplySlowStrengthUpgrade(-(_slowStrengthUpgradePercent / 100f));
        }
    }
}
