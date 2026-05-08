using UnityEngine;
using Characters.Common.Combat.Weapons;
using Characters.Common;
namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "WeaponSpecialVisualUnlockSO", menuName = "Game/Upgrades/Weapon Upgrades/Single Upgrades/Special Visual Unlock")]
    public class WeaponSpecialVisualUnlockSO : SingleUniversalUpgradeSO<IUpgradableWeapon>
    {
        protected override string GetDefaultUpgradeName() => "Special Visual";

        protected override string GetUpgradeValueInfo(char originalSign, char displaySign)
        {
            // We ignore displaySign here because we don't want a + or - sign, just words.
            return originalSign == '+' ? "Unlock" : "Lock";
        }

        public override void ApplyUpgrade(IUpgradableWeapon target)
        {
            if (target is IElementWithExtraVisual weaponWithSpecVisual)
            {
                weaponWithSpecVisual.EnableSpecialVisual();
            }
        }

        public override void ApplyDowngrade(IUpgradableWeapon target)
        {
            if (target is IElementWithExtraVisual weaponWithSpecVisual)
            {
                weaponWithSpecVisual.DisableSpecialVisual();
            }
        }
    }
}
