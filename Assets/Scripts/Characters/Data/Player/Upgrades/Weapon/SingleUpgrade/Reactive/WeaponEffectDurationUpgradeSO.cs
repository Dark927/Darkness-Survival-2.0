using Characters.Common.Combat.Weapons;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "WeaponEffectDurationUpgradeSO", menuName = "Game/Upgrades/Weapon Upgrades/Single Upgrades/Effect Duration Upgrade")]
    public class WeaponEffectDurationUpgradeSO : SingleUniversalUpgradeSO<IUpgradableReactiveWeapon>
    {
        [Tooltip("The percentage to increase the weapon's mark/effect duration (e.g., 20 for +20%)")]
        [SerializeField, Min(0)] private float _durationUpgradePercent = 10f;

        protected override string GetDefaultUpgradeName() => "Mark Duration";

        protected override string GetUpgradeValueInfo(char originalSign, char displaySign)
        {
            return $"{displaySign}{_durationUpgradePercent}%";
        }

        public override void ApplyUpgrade(IUpgradableReactiveWeapon target)
        {
            target.ApplyEffectDurationUpgrade(_durationUpgradePercent / 100f);
        }

        public override void ApplyDowngrade(IUpgradableReactiveWeapon target)
        {
            target.ApplyEffectDurationUpgrade(-(_durationUpgradePercent / 100f));
        }
    }
}
