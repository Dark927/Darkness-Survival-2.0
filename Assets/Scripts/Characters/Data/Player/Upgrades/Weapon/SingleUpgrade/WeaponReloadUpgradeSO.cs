using Characters.Common.Combat.Weapons;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "WeaponReloadUpgradeSO", menuName = "Game/Upgrades/Weapon Upgrades/Single Upgrades/Reload Speed Upgrade")]
    public class WeaponReloadUpgradeSO : SingleUniversalUpgradeSO<IUpgradableWeapon>
    {
        [SerializeField, Min(0)] private float _reloadReductionPercent = 10f;

        protected override string GetDefaultUpgradeName() => "Reload Time";

        protected override string GetUpgradeValueInfo(char originalSign, char displaySign)
        {
            // the base class handles the ReverseSignLogic - displaySign will automatically be
            // '-' when it's an upgrade, and '+' when it's a downgrade
            return $"{displaySign}{_reloadReductionPercent}%";
        }

        public override void ApplyUpgrade(IUpgradableWeapon target) => target.ApplyReloadSpeedUpgrade(-_reloadReductionPercent / 100f);
        public override void ApplyDowngrade(IUpgradableWeapon target) => target.ApplyReloadSpeedUpgrade(_reloadReductionPercent / 100f);
    }
}
