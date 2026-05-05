
using UnityEngine;

namespace Characters.Player.Upgrades
{
    public abstract class SingleAbilityUnlockBaseSO : SingleUpgradeBaseSO<IUpgradableCharacterLogic>
    {
        [SerializeField] private UpgradeConfigurationSO _abilityUpgradeConfiguration;

        protected override string FormatStatUpgradeLine(string upgradeName, string upgradeNameHex, string upgradeValue, string upgradeValueHex)
        {
            return $"<color=#{upgradeNameHex}>{upgradeName}</color> <color=#{upgradeValueHex}>{upgradeValue}</color>";
        }

        protected virtual string FormatAbilityConcreteStatInfo(string statName, string statNameHex, float statValue, string statValueHex, string statValueUnits = "")
        {
            return $"<color=#{statNameHex}>{statName}</color> : <color=#{statValueHex}>{statValue:0.##}{statValueUnits}</color>";
        }

        protected virtual string FormatAbilityConcreteStatInfo(string statName, string statNameHex, string statValue, string statValueHex, string statValueUnits = "")
        {
            return $"<color=#{statNameHex}>{statName}</color> : <color=#{statValueHex}>{statValue}{statValueUnits}</color>";
        }

        protected override string GetDefaultUpgradeName()
        {
            return "Open";
        }

        public virtual int AbilityID => -1;
        public UpgradeConfigurationSO AbilityUpgradeConfiguration => _abilityUpgradeConfiguration;
    }
}
