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
    /// NON-GENERIC BASE: Unity uses this to hold mixed lists of upgrades in the Inspector.
    /// Handles all the UI text formatting.
    /// </summary>
    public abstract class AbstractUpgradeSO : ScriptableObject, ISingleUpgrade
    {
        [SerializeField]
        protected SingleUpgradeUIOverrides _uiSettings = SingleUpgradeUIOverrides.Default();

        public virtual List<StatUIInfo> GetUpgradeInfo() => GetInfo('+');
        public virtual List<StatUIInfo> GetDowngradeInfo() => GetInfo('-');

        protected virtual List<StatUIInfo> GetInfo(char originalSign)
        {
            var infoList = new List<StatUIInfo>();
            var statInfo = new StatUIInfo();

            if (_uiSettings.UseFullOverride)
            {
                statInfo.UseFullOverride = true;
                statInfo.FullOverrideString = _uiSettings.FullOverrideString;
                infoList.Add(statInfo);
                return infoList;
            }

            statInfo.StatName = string.IsNullOrEmpty(_uiSettings.UpgradeNameOverride)
                ? GetDefaultUpgradeName()
                : _uiSettings.UpgradeNameOverride;

            bool isUpgrade = (originalSign == '+');
            char displaySign = _uiSettings.ReverseSignLogic ? (isUpgrade ? '-' : '+') : originalSign;

            statInfo.NameColor = _uiSettings.UpgradeNameColor;
            statInfo.ValueColor = isUpgrade ? _uiSettings.UpgradeValueColor : _uiSettings.DowngradeValueColor;

            statInfo.StatValue = GetUpgradeValueInfo(originalSign, displaySign);
            statInfo.FormatTemplate = "{0} : {1}";

            infoList.Add(statInfo);
            return infoList;
        }

        protected abstract string GetUpgradeValueInfo(char originalSign, char displaySign);
        protected virtual string GetDefaultUpgradeName() => "DefaultStatName";
    }
}
