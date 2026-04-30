
using System.Collections.Generic;
using Characters.Player.Upgrades;
using UnityEngine;

namespace Gameplay.Components.Items
{
    [System.Serializable]
    public class ItemUpgradeArtifactParameters : ItemParametersBase
    {
        // add here some prefab or other data asset to give a weapon for player
        [SerializeField] private List<UpgradeConfigurationSO> _customUpgrades;
        [SerializeField] private bool _areIntroUpgrades = false;

        public List<UpgradeConfigurationSO> CustomUpgrades => _customUpgrades;
        public bool AreIntroUpgrades => _areIntroUpgrades;
    }
}
