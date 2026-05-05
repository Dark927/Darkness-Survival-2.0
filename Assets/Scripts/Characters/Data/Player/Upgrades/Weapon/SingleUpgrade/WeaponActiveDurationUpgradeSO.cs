using Characters.Common.Combat.Weapons;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "WeaponDurationUpgradeSO", menuName = "Game/Upgrades/Weapon Upgrades/Single Upgrades/Active Duration Upgrade")]
    public class WeaponActiveDurationUpgradeSO : SingleUniversalUpgradeSO<IUpgradableWeapon>
    {
        [Tooltip("The percentage to increase the weapon's active duration (e.g., 10 for +10%)")]
        [SerializeField, Min(0)] private float _durationUpgradePercent = 0f;

        protected override string GetDefaultUpgradeName() => "Active Time";

        protected override string GetUpgradeValueInfo(char originalSign, char displaySign)
        {
            return $"{displaySign}{_durationUpgradePercent}%";
        }

        public override void ApplyUpgrade(IUpgradableWeapon target)
        {
            target.ApplyActiveDurationUpgrade(_durationUpgradePercent / 100f);
        }

        public override void ApplyDowngrade(IUpgradableWeapon target)
        {
            target.ApplyActiveDurationUpgrade(-(_durationUpgradePercent / 100f));
        }
    }
}
