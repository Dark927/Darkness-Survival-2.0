using Characters.Common.Combat.Weapons;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "WeaponAttackCountUpgradeSO", menuName = "Game/Upgrades/Weapon Upgrades/Single Upgrades/Attack Count Upgrade")]
    public class WeaponAttackCountUpgradeSO : SingleUniversalUpgradeSO<IUpgradableBurstWeapon>
    {
        [Tooltip("How many attacks will be added (e.g., 2 for +2 projectiles)")]
        [SerializeField, Min(0)] private int _countUpgradeNumber = 0;

        protected override string GetDefaultUpgradeName() => "Attack Count";

        protected override string GetUpgradeValueInfo(char originalSign, char displaySign)
        {
            return $"{displaySign}{_countUpgradeNumber}";
        }

        public override void ApplyUpgrade(IUpgradableBurstWeapon target)
        {
            target.ApplyAttackCountUpgrade(_countUpgradeNumber);
        }

        public override void ApplyDowngrade(IUpgradableBurstWeapon target)
        {
            target.ApplyAttackCountUpgrade(-_countUpgradeNumber);
        }
    }
}
