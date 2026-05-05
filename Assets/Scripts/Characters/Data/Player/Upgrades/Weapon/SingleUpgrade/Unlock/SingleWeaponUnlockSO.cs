using System.Collections.Generic;
using Characters.Common.Combat;
using Characters.Common.Combat.Weapons.Data;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "NewSingleWeaponUnlockData", menuName = "Game/Upgrades/Weapon Upgrades/Unlock/Single Weapon Unlock Data")]
    public class SingleWeaponUnlockSO : SingleAbilityUnlockBaseSO
    {
        [SerializeField] private EntityWeaponData _weaponData;
        public override int AbilityID => _weaponData.ID;

        protected override string GetDefaultUpgradeName() => "Open";

        protected override string GetUpgradeValueInfo(char originalSign, char displaySign)
        {
            return _weaponData.WeaponName;
        }

        protected override string GetInfo(char originalSign)
        {
            // Abort and use custom text if the designer checked the override box
            if (_uiSettings.UseFullOverride)
            {
                return _uiSettings.FullOverrideString;
            }

            // Let the base class generate the first line: "<color>Open</color> <color>WeaponName</color>"
            List<string> lines = new List<string>
            {
                base.GetInfo(originalSign)
            };

            Color valueColor = _uiSettings.UpgradeValueColor;
            string nameHex = ColorUtility.ToHtmlStringRGBA(_uiSettings.UpgradeNameColor);
            string valueHex = ColorUtility.ToHtmlStringRGBA(valueColor);

            IAttackSettings stats = _weaponData.AttackData.Settings;

            if (stats != null)
            {
                // Damage
                if (stats.Damage.Max > 0f)
                {
                    string damageRange = $"{stats.Damage.Min:0.##}-{stats.Damage.Max:0.##}";
                    lines.Add(FormatAbilityConcreteStatInfo("Damage", nameHex, damageRange, valueHex, " hp."));
                }

                // Attack Duration
                if (stats.FullDurationTimeSec > 0f)
                {
                    lines.Add(FormatAbilityConcreteStatInfo("Attack Duration", nameHex, stats.FullDurationTimeSec.ToString("0.##"), valueHex, " s."));
                }

                // Reload time
                if (stats.ReloadTimeSec > 0f)
                {
                    lines.Add(FormatAbilityConcreteStatInfo("Reload time", nameHex, stats.ReloadTimeSec.ToString("0.##"), valueHex, " s."));
                }

                // Impact chance
                // We only display this if UseImpact is true AND the chance is greater than 0
                if (stats.Impact.UseImpact && stats.Impact.ChancePercent > 0)
                {
                    lines.Add(FormatAbilityConcreteStatInfo("Impact chance", nameHex, stats.Impact.ChancePercent.ToString(), valueHex, "%"));
                }
            }

            return string.Join("\n", lines);
        }

        public override void ApplyUpgrade(IUpgradableCharacterLogic target)
        {
            target.WeaponsHandler.GiveWeaponAsync(_weaponData);
        }
    }
}
