using UnityEngine;

namespace Characters.Player.Upgrades
{
    /// <summary>
    /// The base class for all upgrades where should be some stats, etc.
    /// </summary>
    public abstract class SingleUpgradeBaseSO<TUpgradeTarget> : ScriptableObject, ISingleUpgrade where TUpgradeTarget : IUpgradable
    {
        [SerializeField]
        protected SingleUpgradeUIOverrides _uiSettings = SingleUpgradeUIOverrides.Default();

        public virtual string GetUpgradeInfo()
        {
            return GetInfo('+');
        }

        protected virtual string GetInfo(char originalSign)
        {
            // Check for a complete custom override
            if (_uiSettings.UseFullOverride)
            {
                return _uiSettings.FullOverrideString;
            }

            // Determine the name
            string statName = string.IsNullOrEmpty(_uiSettings.UpgradeNameOverride)
                ? GetDefaultUpgradeName()
                : _uiSettings.UpgradeNameOverride;

            // Determine sign and colors based on Reverse Logic
            bool isUpgrade = (originalSign == '+');
            char displaySign = originalSign;

            if (_uiSettings.ReverseSignLogic)
            {
                displaySign = isUpgrade ? '-' : '+';
            }

            Color valueColor = isUpgrade ? _uiSettings.UpgradeValueColor : _uiSettings.DowngradeValueColor;

            // Convert colors to Hex for TextMeshPro Rich Text
            string nameHex = ColorUtility.ToHtmlStringRGBA(_uiSettings.UpgradeNameColor);
            string valueHex = ColorUtility.ToHtmlStringRGBA(valueColor);

            // Get the formatted value from the subclass
            string statValue = GetUpgradeValueInfo(originalSign, displaySign);

            return FormatStatUpgradeLine(statName, nameHex, statValue, valueHex);
        }

        protected virtual string FormatStatUpgradeLine(string upgradeName, string upgradeNameHex, string upgradeValue, string upgradeValueHex)
        {
            return $"<color=#{upgradeNameHex}>{upgradeName}</color> : <color=#{upgradeValueHex}>{upgradeValue}</color>";
        }

        // Subclasses MUST implement this to provide their specific value string (e.g., "+10%")
        protected abstract string GetUpgradeValueInfo(char originalSign, char displaySign);

        protected virtual string GetDefaultUpgradeName()
        {
            return "DefaultStatName";
        }

        public abstract void ApplyUpgrade(TUpgradeTarget target);
    }
}


