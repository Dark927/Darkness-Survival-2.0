using System.Collections.Generic;
using UnityEngine;
using Utilities.Attributes;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "NewUpgradesSetData", menuName = "Game/Upgrades/Upgrades Set Data")]
    public class UpgradesSetData : ScriptableObject
    {
        [CustomHeader("Main Settings", count = 1, depth = 0)]
        [SerializeField] private List<UpgradeConfigurationSO> _upgradesConfigurations;

        public List<UpgradeConfigurationSO> UpgradesConfigurations => _upgradesConfigurations;
    }
}
