using Characters.Common.Combat.Weapons;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "WeaponAttackSpeedUpgradeSO", menuName = "Game/Upgrades/Weapon Upgrades/Single Upgrades/Attack Speed Upgrade")]
    public class WeaponAttackSpeedUpgradeSO : SingleUniversalUpgradeSO<IUpgradableWeapon>
    {
        [Tooltip("The percentage to modify the attack speed by (e.g., 10 for 10%)")]
        [SerializeField, Min(0)] private float _speedUpgradePercent = 0f;

        protected override string GetDefaultUpgradeName() => "Attack Speed";

        protected override string GetUpgradeValueInfo(char originalSign, char displaySign)
        {
            return $"{displaySign}{_speedUpgradePercent}%";
        }

        public override void ApplyUpgrade(IUpgradableWeapon target)
        {
            target.ApplyAttackSpeedUpgrade(_speedUpgradePercent / 100f);
        }

        public override void ApplyDowngrade(IUpgradableWeapon target)
        {
            target.ApplyAttackSpeedUpgrade(-(_speedUpgradePercent / 100f));
        }
    }
}
