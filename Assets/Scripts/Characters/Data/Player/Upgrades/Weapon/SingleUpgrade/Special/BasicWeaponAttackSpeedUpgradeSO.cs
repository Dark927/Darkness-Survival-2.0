
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

        protected override string GetDefaultUpgradeName()
        {
            return _attackType switch
            {
                BasicAttack.LocalType.Default => "All attacks speed",
                BasicAttack.LocalType.Fast => "Light attack speed",
                BasicAttack.LocalType.Heavy => "Heavy attack speed",
                _ => throw new System.NotImplementedException()
            };
        }

        protected override string GetUpgradeValueInfo(char originalSign, char displaySign)
        {
            return $"{displaySign}{_speedUpgradePercent}%";
        }

        public override void ApplyUpgrade(IUpgradableWeapon target)
        {
            if (target is BasicCharacterWeapon basicWeapon)
            {
                basicWeapon.ApplyConcreteAttackSpeedUpgrade(_attackType, _speedUpgradePercent / 100f);
            }
        }

        public override void ApplyDowngrade(IUpgradableWeapon target)
        {
            if (target is BasicCharacterWeapon basicWeapon)
            {
                basicWeapon.ApplyConcreteAttackSpeedUpgrade(_attackType, -(_speedUpgradePercent / 100f));
            }
        }
    }
}
