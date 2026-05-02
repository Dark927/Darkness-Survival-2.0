
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

        protected override string GetInfo(char sign)
        {
            List<string> lines = new List<string>
            {
                $"Open <color=orange>{_abilityData.Name}</color>"
            };

            AbilityStats stats = _abilityData.AbilityStats;

            // Format floats using ":0.##" so "5.0" prints as "5", but "5.25" prints as "5.25"
            if (stats.StrengthValue > 0f)
            {
                lines.Add($"{_abilityData.AbilityStatsUI.StrengthUIName} : {stats.StrengthValue:0.##}");
            }

            if (stats.StrengthPercent > 0f)
            {
                lines.Add($"{_abilityData.AbilityStatsUI.StrengthUIName} : {stats.StrengthPercent:0.##}%");
            }

            if (stats.Radius > 0f)
            {
                lines.Add($"{_abilityData.AbilityStatsUI.RadiusUIName} : {stats.Radius:0.##}");
            }

            if (stats.Duration > 0f)
            {
                lines.Add($"{_abilityData.AbilityStatsUI.DurationUIName} : {stats.Duration:0.##} s.");
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
