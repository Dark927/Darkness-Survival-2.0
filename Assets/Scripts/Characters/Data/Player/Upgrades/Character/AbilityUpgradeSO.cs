using System.Collections.Generic;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "AbilityUpgradeData", menuName = "Game/Upgrades/Character Upgrades/Ability/Ability Upgrade Data")]
    public class AbilityUpgradeSO : UpgradeSO
    {
        [SerializeField] private List<UpgradeLevelSO<IUpgradableAbility>> _upgradeLevels;

        public override IEnumerable<IUpgradeLevelSO> UpgradeLevels => _upgradeLevels;
        public IEnumerable<UpgradeLevelSO<IUpgradableAbility>> WeaponUpgradeLevels => _upgradeLevels;
    }
}
