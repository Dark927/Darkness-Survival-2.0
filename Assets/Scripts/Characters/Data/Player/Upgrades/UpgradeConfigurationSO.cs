
using UnityEngine;
using Utilities.Attributes;

namespace Characters.Player.Upgrades
{
    public enum UpgradeAppearTime
    {
        Any,
        Day,
        Night,
    }

    public enum UpgradeType
    {
        Character,
        Ability,
        AbilityUnlock,
    }

    [CreateAssetMenu(fileName = "NewUpgradeConfigurationData", menuName = "Game/Upgrades/Main/Upgrade Configuration Data")]
    public class UpgradeConfigurationSO : ScriptableObject
    {
        [SerializeField] private UpgradeSO _upgrade;

        [Space, CustomHeader("Main Info", 9, 0)]

        [SerializeField] private UpgradeAppearTime _appearTime;
        [SerializeField] private string _name;
        [SerializeField] private UpgradeType _type;
        [SerializeField, TextArea] private string _description;

        // Icon is unique for each upgrade, so we do not add it to the Visual Data (which can be reused)
        [SerializeField] private Sprite _icon;


        [Space, CustomHeader("Visual Settings", count: 2, depth: 1, CustomHeaderAttribute.HeaderColor.cyan)]
        [SerializeField] private UpgradeVisualDataUI _visualDataUI;


        public string Name => _name;
        public UpgradeAppearTime AppearTime => _appearTime;
        public UpgradeType Type => _type;
        public string Description => _description;
        public Sprite Icon => _icon;

        public UpgradeSO Upgrade => _upgrade;
        public UpgradeVisualDataUI VisualData => _visualDataUI;
    }
}
