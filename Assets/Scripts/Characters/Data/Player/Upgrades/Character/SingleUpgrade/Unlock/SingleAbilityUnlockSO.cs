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

        protected override List<StatUIInfo> GetInfo(char originalSign)
        {
            List<StatUIInfo> statsList = base.GetInfo(originalSign);

            if (_uiSettings.UseFullOverride)
            {
                return statsList;
            }

            // Adjust the format template for the first line ("Open AbilityName" without a colon)
            var firstStat = statsList[0];
            firstStat.FormatTemplate = "{0} {1}";
            statsList[0] = firstStat;

            Color nameColor = _uiSettings.UpgradeNameColor;
            Color valueColor = _uiSettings.UpgradeValueColor;

            AbilityStats stats = _abilityData.AbilityStats;
            AbilityStatsUI statsUI = _abilityData.AbilityStatsUI;

            string statTemplate = "{0} : {1}";

            if (stats.StrengthValue > 0f)
            {
                statsList.Add(new StatUIInfo
                {
                    StatName = statsUI.StrengthUIName,
                    StatValue = stats.StrengthValue.ToString("0.##"),
                    NameColor = nameColor,
                    ValueColor = valueColor,
                    FormatTemplate = statTemplate
                });
            }

            if (stats.StrengthPercent > 0f)
            {
                statsList.Add(new StatUIInfo
                {
                    StatName = statsUI.StrengthUIName,
                    StatValue = $"{stats.StrengthPercent:0.##}%",
                    NameColor = nameColor,
                    ValueColor = valueColor,
                    FormatTemplate = statTemplate
                });
            }

            if (stats.Radius > 0f)
            {
                statsList.Add(new StatUIInfo
                {
                    StatName = statsUI.RadiusUIName,
                    StatValue = stats.Radius.ToString("0.##"),
                    NameColor = nameColor,
                    ValueColor = valueColor,
                    FormatTemplate = statTemplate
                });
            }

            if (stats.Duration > 0f)
            {
                statsList.Add(new StatUIInfo
                {
                    StatName = statsUI.DurationUIName,
                    StatValue = $"{stats.Duration:0.##} s.",
                    NameColor = nameColor,
                    ValueColor = valueColor,
                    FormatTemplate = statTemplate
                });
            }

            return statsList;
        }

        public override void ApplyUpgrade(IUpgradableCharacterLogic target)
        {
            target.AbilitiesHandler.GiveAbilityAsync(_abilityData);
        }
    }
}
