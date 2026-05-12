using Characters.Common.Combat.Weapons;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "WeaponVacuumStrengthUpgradeSO", menuName = "Game/Upgrades/Weapon Upgrades/Single Upgrades/Vacuum/Vacuum Strength Upgrade")]
    public class WeaponVacuumStrengthUpgradeSO : SingleUniversalUpgradeSO<IUpgradableVacuumWeapon>
    {
        [Tooltip("The percentage to increase the vacuum pulling force by (e.g., 10 for 10%)")]
        [SerializeField, Min(0f)] private float _vacuumStrengthUpgradePercent = 0f;

        protected override string GetDefaultUpgradeName() => "Pull Strength";

        protected override string GetUpgradeValueInfo(char originalSign, char displaySign)
        {
            return $"{displaySign}{_vacuumStrengthUpgradePercent}%";
        }

        public override void ApplyUpgrade(IUpgradableVacuumWeapon target)
        {
            target.ApplyVacuumStrengthUpgrade(_vacuumStrengthUpgradePercent / 100f);
        }

        public override void ApplyDowngrade(IUpgradableVacuumWeapon target)
        {
            target.ApplyVacuumStrengthUpgrade(-(_vacuumStrengthUpgradePercent / 100f));
        }
    }
}
