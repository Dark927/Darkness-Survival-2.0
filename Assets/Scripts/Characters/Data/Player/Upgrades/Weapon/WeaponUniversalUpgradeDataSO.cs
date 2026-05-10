using System.Collections.Generic;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "WeaponUpgradeData", menuName = "Game/Upgrades/Weapon Universal Upgrade Data")]
    public class WeaponUniversalUpgradeDataSO : UpgradeDataSO
    {
        [SerializeReference, SubclassSelector]
        private List<IWeaponUpgradeLevelData> _upgradeLevels = new List<IWeaponUpgradeLevelData>();

        public override IEnumerable<IUpgradeLevelData> UpgradeLevels => _upgradeLevels;

        // Add Unity's built-in Editor validation method
        private void OnValidate()
        {
            if (_upgradeLevels == null) return;

            // Pass the validation check down to every level in the list
            foreach (var level in _upgradeLevels)
            {
                level?.ValidateEditorConstraints();
            }
        }
    }
}
