using System.Collections.Generic;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "CharacterUpgradeData", menuName = "Game/Upgrades/Character Upgrades/Character Upgrade Data")]
    public class CharacterUpgradeSO : UpgradeSO
    {
        [SerializeField] private List<UpgradeLevelSO<IUpgradableCharacterLogic>> _upgradeLevels;

        public override IEnumerable<IUpgradeLevelSO> UpgradeLevels => _upgradeLevels;
        public IEnumerable<UpgradeLevelSO<IUpgradableCharacterLogic>> CharacterUpgradeLevels => _upgradeLevels;
    }
}

