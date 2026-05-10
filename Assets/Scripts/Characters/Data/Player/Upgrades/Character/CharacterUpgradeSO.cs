using System.Collections.Generic;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "CharacterUpgradeData", menuName = "Game/Upgrades/Character Upgrades/Character Upgrade Data")]
    public class CharacterUpgradeSO : UpgradeDataSO
    {
        // By using the Marker Interface, the Dropdown will ONLY show:
        // - CharacterUpgradeLevelData
        // - WeaponUnlockLevelData
        // - AbilityUnlockLevelData
        // It will mathematically block other upgrades
        [SerializeReference, SubclassSelector]
        private List<ICharacterUpgradeLevelData> _upgradeLevels = new List<ICharacterUpgradeLevelData>();

        public override IEnumerable<IUpgradeLevelData> UpgradeLevels => _upgradeLevels;

        // Keep the validation guard to protect the Upgrades/Downgrades inside the levels themselves
        private void OnValidate()
        {
            if (_upgradeLevels == null) return;
            foreach (var level in _upgradeLevels)
            {
                level?.ValidateEditorConstraints();
            }
        }
    }
}
