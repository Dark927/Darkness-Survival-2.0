
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "WeaponUnlockData", menuName = "Game/Upgrades/Weapon Upgrades/Unlock/Weapon Unlock Data")]
    public class WeaponUnlockSO : UpgradeSO
    {
        [SerializeField] private List<WeaponUnlockLevelSO> _upgradeLevels;

        public override IEnumerable<IUpgradeLevelSO> UpgradeLevels => _upgradeLevels;
        public IEnumerable<WeaponUnlockLevelSO> WeaponUnlockLevels => _upgradeLevels;
    }
}


