using Characters.Common.Combat.Weapons;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "WeaponReloadUpgradeSO", menuName = "Game/Upgrades/Weapon Upgrades/Single Upgrades/Reload Speed Upgrade")]
    public class WeaponReloadUpgradeSO : SingleUniversalUpgradeSO<IUpgradableWeapon>
    {
        [Tooltip("Percentage to REDUCE reload time (e.g., 10 for -10% time)")]
        [SerializeField, Min(0)] private float _reloadReductionPercent = 10f;

        private float ActualUpgradePercent => -_reloadReductionPercent;

        protected override string GetInfo(char sign)
        {
            char displaySign = sign == '+' ? '-' : '+';
            return $" {StatNameUI} : {displaySign}{_reloadReductionPercent}%";
        }

        protected override string GetDefaultStatNameUI()
        {
            return "Reload Time";
        }

        public override void ApplyUpgrade(IUpgradableWeapon target)
        {
            target.ApplyReloadSpeedUpgrade(ActualUpgradePercent / 100f);
        }

        public override void ApplyDowngrade(IUpgradableWeapon target)
        {
            target.ApplyReloadSpeedUpgrade(-(ActualUpgradePercent / 100f));
        }
    }
}
