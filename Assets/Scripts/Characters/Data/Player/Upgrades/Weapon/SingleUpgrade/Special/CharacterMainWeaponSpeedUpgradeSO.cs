
using Characters.Common.Combat.Weapons;
using Characters.Common.Movement;
using Characters.Interfaces;
using Materials;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    /// <summary>
    /// Use this single upgrade SO to upgrade the weapon's attack speed which depends on attack animation events.
    /// </summary>
    [CreateAssetMenu(fileName = "NewCharacterMainWeaponSpeedUpgradeSO", menuName = "Game/Upgrades/Weapon Upgrades/Single Upgrades/Special/Main Weapon Speed Upgrade Data")]
    public class CharacterMainWeaponSpeedUpgradeSO : SingleUniversalUpgradeSO<IUpgradableWeapon>
    {
        [SerializeField, Min(0)] private float _speedUpgradePercent = 0;

        protected override string GetInfo(char sign)
        {
            return $"Max SPEED : {sign}{_speedUpgradePercent}%";
        }

        public override void ApplyUpgrade(IUpgradableWeapon target)
        {
            //Apply(target, (1 + (_speedUpgradePercent / 100f)));
        }

        public override void ApplyDowngrade(IUpgradableWeapon target)
        {
            //Apply(target, (1 - (_speedUpgradePercent / 100f)));
        }
    }
}
