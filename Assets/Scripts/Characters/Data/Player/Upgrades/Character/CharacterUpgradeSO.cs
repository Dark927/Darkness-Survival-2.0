using System.Collections.Generic;
using Characters.Common.Combat.Weapons;
using Characters.Interfaces;
using Characters.Player.Upgrades;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "CharacterUpgradeData", menuName = "Game/Upgrades/Character Upgrades/Character Upgrade Data")]
    public class CharacterUpgradeSO : UpgradeSO
    {
        [SerializeField] private List<UpgradeLevelSO<ICharacterLogic>> _upgradeLevels;

        public override IEnumerable<IUpgradeLevelSO> UpgradeLevels => _upgradeLevels;
        public IEnumerable<UpgradeLevelSO<ICharacterLogic>> CharacterUpgradeLevels => _upgradeLevels;
    }
}

