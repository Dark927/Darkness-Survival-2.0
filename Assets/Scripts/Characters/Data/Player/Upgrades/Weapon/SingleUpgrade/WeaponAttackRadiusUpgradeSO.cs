using Characters.Common.Combat.Weapons;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "WeaponAttackSpeedUpgradeSO", menuName = "Game/Upgrades/Weapon Upgrades/Single Upgrades/Attack Radius Upgrade")]
    public class WeaponAttackRadiusUpgradeSO : SingleUniversalUpgradeSO<IUpgradableWeapon>
    {
        [Tooltip("The percentage to modify the attack radius by (e.g., 10 for 10%)")]
        [SerializeField, Min(0)] private float _radiusUpgradePercent = 0f;

        protected override string GetDefaultUpgradeName() => "Attack Radius";

        protected override string GetUpgradeValueInfo(char originalSign, char displaySign)
        {
            return $"{displaySign}{_radiusUpgradePercent}%";
        }

        public override void ApplyUpgrade(IUpgradableWeapon target)
        {
            target.ApplyAttackRadiusUpgrade(_radiusUpgradePercent / 100f);
        }

        public override void ApplyDowngrade(IUpgradableWeapon target)
        {
            target.ApplyAttackRadiusUpgrade(-(_radiusUpgradePercent / 100f));
        }
    }
}
