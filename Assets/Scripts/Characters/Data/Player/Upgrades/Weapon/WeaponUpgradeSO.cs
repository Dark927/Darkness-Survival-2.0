using Characters.Common.Combat.Weapons;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "WeaponUpgradeData", menuName = "Game/Upgrades/Weapon Upgrades/Weapon Upgrade Data")]
    public class WeaponUpgradeSO : UpgradeSO
    {
        [SerializeField] private List<UpgradeLevelSO<IWeapon>> _upgradeLevels;

        public override IEnumerable<IUpgradeLevelSO> UpgradeLevels => _upgradeLevels;
        public IEnumerable<UpgradeLevelSO<IWeapon>> WeaponUpgradeLevels => _upgradeLevels;
    }
}
