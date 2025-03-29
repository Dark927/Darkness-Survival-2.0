
using Characters.Common.Combat.Weapons;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "NewBasicWeaponAttackSpeedUpgradeSO", menuName = "Game/Upgrades/Weapon Upgrades/Single Upgrades/Basic Weapon Attack Speed Upgrade Data")]
    public class BasicWeaponAttackSpeedUpgradeSO : SingleUniversalUpgradeSO<IUpgradableWeapon>
    {
        [SerializeField, Min(0)] private float _speedUpgradePercent = 0;

        [Header("<color=yellow>Note : Default value modifies speed for all attack types</color>")]
        [SerializeField] private BasicAttack.LocalType _attackType;

        protected override string GetInfo(char sign)
        {
            return _attackType switch
            {
                BasicAttack.LocalType.Default => $"All attacks speed : {sign}{_speedUpgradePercent}%",
                BasicAttack.LocalType.Fast => $"Fast attack speed : {sign}{_speedUpgradePercent}%",
                BasicAttack.LocalType.Heavy => $"Heavy attack speed : {sign}{_speedUpgradePercent}%",
                _ => throw new System.NotImplementedException()
            };
        }

        public override void ApplyUpgrade(IUpgradableWeapon target)
        {
            if (target is BasicCharacterWeapon basicWeapon)
            {
                basicWeapon.ApplyConcreteAttackSpeedUpgrade(_attackType, 1 + (_speedUpgradePercent / 100f));
            }
        }

        public override void ApplyDowngrade(IUpgradableWeapon target)
        {
            if (target is BasicCharacterWeapon basicWeapon)
            {
                basicWeapon.ApplyConcreteAttackSpeedUpgrade(_attackType, 1 - (_speedUpgradePercent / 100f));
            }
        }
    }
}
