using Characters.Common.Combat.Weapons;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "WeaponReloadUpgradeSO", menuName = "Game/Upgrades/Weapon Upgrades/Single Upgrades/Stun Chance Upgrade")]
    public class WeaponImpactChanceUpgradeSO : SingleUniversalUpgradeSO<IUpgradableWeapon>
    {
        [SerializeField, Min(0)] private float _impactExtraChancePercent = 0;

        protected override string GetDefaultUpgradeName() => "Impact chance";

        protected override string GetUpgradeValueInfo(char originalSign, char displaySign)
        {
            return $"{displaySign}{_impactExtraChancePercent}%";
        }

        public override void ApplyUpgrade(IUpgradableWeapon target) => target.ApplyImpactChanceUpgrade(_impactExtraChancePercent);
        public override void ApplyDowngrade(IUpgradableWeapon target) => target.ApplyImpactChanceUpgrade(-_impactExtraChancePercent);
    }
}
