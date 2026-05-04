
using Characters.Common.Combat.Weapons.Data;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "NewSingleWeaponUnlockData", menuName = "Game/Upgrades/Weapon Upgrades/Unlock/Single Weapon Unlock Data")]
    public class SingleWeaponUnlockSO : SingleAbilityUnlockBaseSO
    {
        [SerializeField] private EntityWeaponData _weaponData;
        public override int AbilityID => _weaponData.ID;

        protected override string GetUpgradeValueInfo(char originalSign, char displaySign)
        {
            return _weaponData.WeaponName;
        }

        public override void ApplyUpgrade(IUpgradableCharacterLogic target)
        {
            target.WeaponsHandler.GiveWeaponAsync(_weaponData);
        }
    }
}
