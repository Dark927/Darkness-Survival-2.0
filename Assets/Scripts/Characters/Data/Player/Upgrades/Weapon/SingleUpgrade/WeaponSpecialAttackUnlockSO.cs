using UnityEngine;
using Characters.Common.Combat.Weapons;
namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "WeaponSpecialAttackUnlockSO", menuName = "Game/Upgrades/Weapon Upgrades/Single Upgrades/Special Attack Unlock")]
    public class WeaponSpecialAttackUnlockSO : SingleUniversalUpgradeSO<IUpgradableWeapon>
    {
        protected override string GetDefaultUpgradeName() => "Special Attack";

        protected override string GetUpgradeValueInfo(char originalSign, char displaySign)
        {
            // We ignore displaySign here because we don't want a + or - sign, just words.
            return originalSign == '+' ? "Unlock" : "Lock";
        }

        public override void ApplyUpgrade(IUpgradableWeapon target)
        {
            if (target is IWeaponWithSpecialAttack weaponWithSpecAttack)
            {
                weaponWithSpecAttack.EnableSpecialAttack();
            }
        }

        public override void ApplyDowngrade(IUpgradableWeapon target)
        {
            if (target is IWeaponWithSpecialAttack weaponWithSpecAttack)
            {
                weaponWithSpecAttack.DisableSpecialAttack();
            }
        }
    }
}
