using Characters.Common.Combat.Weapons;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "WeaponVacuumRadiusUpgradeSO", menuName = "Game/Upgrades/Weapon Upgrades/Single Upgrades/Vacuum/Vacuum Radius Upgrade")]
    public class WeaponVacuumRadiusUpgradeSO : SingleUniversalUpgradeSO<IUpgradableVacuumWeapon>
    {
        [Tooltip("The percentage to expand the vacuum pulling area by (e.g., 10 for 10%)")]
        [SerializeField, Min(0f)] private float _vacuumRadiusUpgradePercent = 0f;

        protected override string GetDefaultUpgradeName() => "Pull Reach";

        protected override string GetUpgradeValueInfo(char originalSign, char displaySign)
        {
            return $"{displaySign}{_vacuumRadiusUpgradePercent}%";
        }

        public override void ApplyUpgrade(IUpgradableVacuumWeapon target)
        {
            target.ApplyVacuumRadiusUpgrade(_vacuumRadiusUpgradePercent / 100f);
        }

        public override void ApplyDowngrade(IUpgradableVacuumWeapon target)
        {
            target.ApplyVacuumRadiusUpgrade(-(_vacuumRadiusUpgradePercent / 100f));
        }
    }
}
