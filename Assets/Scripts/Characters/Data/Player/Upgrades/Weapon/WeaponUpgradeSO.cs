using System.Collections.Generic;
using Characters.Common.Combat.Weapons;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "WeaponUpgradeData", menuName = "Game/Upgrades/Weapon Upgrades/Weapon Upgrade Data")]
    public class WeaponUpgradeSO : UpgradeSO
    {
        [SerializeField] private List<UpgradeLevelSO<IUpgradableWeapon>> _upgradeLevels;

        public override IEnumerable<IUpgradeLevelSO> UpgradeLevels => _upgradeLevels;
        public IEnumerable<UpgradeLevelSO<IUpgradableWeapon>> WeaponUpgradeLevels => _upgradeLevels;
    }
}
