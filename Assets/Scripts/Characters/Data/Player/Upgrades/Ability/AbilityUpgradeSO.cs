using System.Collections.Generic;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "AbilityUpgradeData", menuName = "Game/Upgrades/Ability Upgrades/Ability Upgrade Data")]
    public class AbilityUpgradeSO : UpgradeSO
    {
        [SerializeField] private List<UpgradeLevelSO<IUpgradableAbility>> _upgradeLevels;

        public override IEnumerable<IUpgradeLevelSO> UpgradeLevels => _upgradeLevels;
        public IEnumerable<UpgradeLevelSO<IUpgradableAbility>> AbilityUpgradeLevels => _upgradeLevels;
    }
}
