using System.Collections.Generic;
using Characters.Interfaces;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "CharacterUpgradeData", menuName = "Game/Upgrades/Character Upgrades/Character Upgrade Data")]
    public class CharacterUpgradeSO : UpgradeSO
    {
        [SerializeField] private List<UpgradeLevelSO<ICharacterLogic>> _upgradeLevels;

        public IEnumerable<UpgradeLevelSO<ICharacterLogic>> UpgradeLevels => _upgradeLevels;
    }
}
