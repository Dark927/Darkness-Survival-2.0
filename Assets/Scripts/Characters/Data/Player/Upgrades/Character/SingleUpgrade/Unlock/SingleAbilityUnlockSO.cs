using System.Collections.Generic;
using Characters.Common.Abilities;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "NewSingleAbilityUnlockData", menuName = "Game/Upgrades/Character Upgrades/Unlock/Single Ability Unlock Data")]
    public class SingleAbilityUnlockSO : SingleAbilityUnlockBaseSO
    {
        [SerializeField] private EntityAbilityData _abilityData;

        public override int AbilityID => _abilityData.ID;

        protected override string GetDefaultUpgradeName() => "Open";

        protected override string GetUpgradeValueInfo(char originalSign, char displaySign) => _abilityData.Name;

        protected override string GetInfo(char originalSign)
        {
            // Abort and use custom text if the designer checked the override box
            if (_uiSettings.UseFullOverride)
            {
                return _uiSettings.FullOverrideString;
            }

            // Let the base class generate the first line: "<color>Open</color> <color>AbilityName</color>"
            List<string> lines = new List<string>
            {
                base.GetInfo(originalSign)
            };

            // Calculate the hex colors so our extra lines perfectly match the UI settings
            bool isUpgrade = originalSign == '+';
            if (_uiSettings.ReverseSignLogic)
            {
                isUpgrade = !isUpgrade;
            }

            Color valueColor = isUpgrade ? _uiSettings.UpgradeValueColor : _uiSettings.DowngradeValueColor;
            string nameHex = ColorUtility.ToHtmlStringRGBA(_uiSettings.UpgradeNameColor);
            string valueHex = ColorUtility.ToHtmlStringRGBA(valueColor);

            // Append the ability stats using the helper method
            AbilityStats stats = _abilityData.AbilityStats;
            AbilityStatsUI statsUI = _abilityData.AbilityStatsUI;

            if (stats.StrengthValue > 0f)
            {
                lines.Add(FormatAbilityConcreteStatInfo(statsUI.StrengthUIName, nameHex, stats.StrengthValue, valueHex));
            }

            if (stats.StrengthPercent > 0f)
            {
                lines.Add(FormatAbilityConcreteStatInfo(statsUI.StrengthUIName, nameHex, stats.StrengthPercent, valueHex, "%"));
            }

            if (stats.Radius > 0f)
            {
                lines.Add(FormatAbilityConcreteStatInfo(statsUI.RadiusUIName, nameHex, stats.Radius, valueHex));
            }

            if (stats.Duration > 0f)
            {
                lines.Add(FormatAbilityConcreteStatInfo(statsUI.DurationUIName, nameHex, stats.Duration, valueHex, " s."));
            }

            // Join them all together with a line break (\n)
            return string.Join("\n", lines);
        }

        public override void ApplyUpgrade(IUpgradableCharacterLogic target)
        {
            target.AbilitiesHandler.GiveAbilityAsync(_abilityData);
        }
    }
}
