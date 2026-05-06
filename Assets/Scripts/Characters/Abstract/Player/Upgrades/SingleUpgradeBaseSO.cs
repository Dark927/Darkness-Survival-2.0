using System.Collections.Generic;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    public struct StatUIInfo
    {
        public string StatName;
        public string StatValue;
        public Color NameColor;
        public Color ValueColor;

        public bool UseFullOverride;
        public string FullOverrideString; // Here can be any HTML-tag

        // {0} - upgrade stat name (with color applied)
        // {1} - upgrade name value (with color applied)
        public string FormatTemplate;
    }

    /// <summary>
    /// The base class for all upgrades where should be some stats, etc.
    /// </summary>
    public abstract class SingleUpgradeBaseSO<TUpgradeTarget> : ScriptableObject, ISingleUpgrade where TUpgradeTarget : IUpgradable
    {
        [SerializeField]
        protected SingleUpgradeUIOverrides _uiSettings = SingleUpgradeUIOverrides.Default();

        public virtual List<StatUIInfo> GetUpgradeInfo()
        {
            return GetInfo('+');
        }

        protected virtual List<StatUIInfo> GetInfo(char originalSign)
        {
            var infoList = new List<StatUIInfo>();
            var statInfo = new StatUIInfo();

            // Check for a complete custom override
            if (_uiSettings.UseFullOverride)
            {
                statInfo.UseFullOverride = true;
                statInfo.FullOverrideString = _uiSettings.FullOverrideString;
                infoList.Add(statInfo);
                return infoList;
            }

            // Determine the name
            statInfo.StatName = string.IsNullOrEmpty(_uiSettings.UpgradeNameOverride)
                ? GetDefaultUpgradeName()
                : _uiSettings.UpgradeNameOverride;

            // Determine sign and colors
            bool isUpgrade = (originalSign == '+');
            char displaySign = originalSign;

            if (_uiSettings.ReverseSignLogic)
            {
                displaySign = isUpgrade ? '-' : '+';
            }

            statInfo.NameColor = _uiSettings.UpgradeNameColor;
            statInfo.ValueColor = isUpgrade ? _uiSettings.UpgradeValueColor : _uiSettings.DowngradeValueColor;

            statInfo.StatValue = GetUpgradeValueInfo(originalSign, displaySign);

            // Set default format template
            statInfo.FormatTemplate = "{0} : {1}";

            infoList.Add(statInfo);
            return infoList;
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


