using UnityEngine;
using Characters.Common.Combat.Weapons;
namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "WeaponSpecialAttackUnlockSO", menuName = "Game/Upgrades/Weapon Upgrades/Single Upgrades/Special Attack Unlock")]
    public class WeaponSpecialAttackUnlockSO : SingleUniversalUpgradeSO<IUpgradableWeapon>
    {
        protected override string GetInfo(char sign)
        {
            if (sign == '+')
            {
                return $"{StatNameUI} : Unlock";
            }
            else
            {
                return $"{StatNameUI} : Lock";
            }
        }

        protected override string GetDefaultStatNameUI()
        {
            return "Special Attack";
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
