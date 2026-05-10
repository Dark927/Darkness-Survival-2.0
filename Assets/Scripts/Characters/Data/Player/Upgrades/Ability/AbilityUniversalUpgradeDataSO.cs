using System.Collections.Generic;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "AbilityUpgradeData", menuName = "Game/Upgrades/Ability Upgrades/Ability Upgrade Data")]
    public class AbilityUniversalUpgradeDataSO : UpgradeDataSO
    {
        // Unlocks the dropdown, but strictly limits it to Ability levels
        [SerializeReference, SubclassSelector]
        private List<IAbilityUpgradeLevelData> _upgradeLevels = new List<IAbilityUpgradeLevelData>();

        // Covariant cast to the base interface
        public override IEnumerable<IUpgradeLevelData> UpgradeLevels => _upgradeLevels;

        // Editor Validation Guard
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
