using Characters.Common.Combat.Weapons;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "WeaponMoveSpeedUpgradeSO", menuName = "Game/Upgrades/Weapon Upgrades/Single Upgrades/Mobile/Movement Speed Upgrade")]
    public class WeaponMoveSpeedUpgradeSO : SingleUniversalUpgradeSO<IUpgradableMobileWeapon>
    {
        [Tooltip("The percentage to modify the mobile weapon's travel speed by (e.g., 10 for 10%)")]
        [SerializeField, Min(0f)] private float _moveSpeedUpgradePercent = 0f;

        protected override string GetDefaultUpgradeName() => "Travel Speed";

        protected override string GetUpgradeValueInfo(char originalSign, char displaySign)
        {
            return $"{displaySign}{_moveSpeedUpgradePercent}%";
        }

        public override void ApplyUpgrade(IUpgradableMobileWeapon target)
        {
            target.ApplyMovementSpeedUpgrade(_moveSpeedUpgradePercent / 100f);
        }

        public override void ApplyDowngrade(IUpgradableMobileWeapon target)
        {
            target.ApplyMovementSpeedUpgrade(-(_moveSpeedUpgradePercent / 100f));
        }
    }
}
