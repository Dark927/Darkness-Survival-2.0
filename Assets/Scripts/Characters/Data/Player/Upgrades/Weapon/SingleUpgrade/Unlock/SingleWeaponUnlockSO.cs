
using Characters.Common.Combat.Weapons.Data;
using Characters.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "NewSingleWeaponUnlockData", menuName = "Game/Upgrades/Weapon Upgrades/Unlock/Single Weapon Unlock Data")]
    public class SingleWeaponUnlockSO : SingleUpgradeBaseSO<IUpgradableCharacterLogic>
    {
        [SerializeField] private EntityWeaponData _weaponData;
        [SerializeField] private UpgradeConfigurationSO _weaponUpgradeConfiguration;

        public int WeaponID => _weaponData.ID;
        public UpgradeConfigurationSO WeaponUpgradeConfiguration => _weaponUpgradeConfiguration;

        protected override string GetInfo(char sign)
        {
            return $"Open <color=blue>{_weaponData.WeaponName}</color> weapon";
        }

        public override void ApplyUpgrade(IUpgradableCharacterLogic target)
        {
            target.Weapons.GiveFeatureAsync(_weaponData).Forget();
        }
    }
}
