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

        protected override List<StatUIInfo> GetInfo(char originalSign)
        {
            // Get the base list from SingleUpgradeBaseSO
            List<StatUIInfo> statsList = base.GetInfo(originalSign);

            // Abort and use custom text if the designer checked the override box
            if (_uiSettings.UseFullOverride)
            {
                return statsList;
            }

            // Adjust the format template for the first line ("Open WeaponName" instead of "Open : WeaponName")
            var firstStat = statsList[0];
            firstStat.FormatTemplate = "{0} {1}";
            statsList[0] = firstStat;

            Color nameColor = _uiSettings.UpgradeNameColor;
            Color valueColor = _uiSettings.UpgradeValueColor;

            IAttackSettings stats = _weaponData.AttackData.Settings;

            if (stats != null)
            {
                // Standard template for the subsequent weapon stats
                string statTemplate = "{0} : {1}";

                // Damage
                if (stats.Damage.Max > 0f)
                {
                    string damageRange = $"{stats.Damage.Min:0.##}-{stats.Damage.Max:0.##} hp.";
                    statsList.Add(new StatUIInfo { StatName = "Damage", StatValue = damageRange, NameColor = nameColor, ValueColor = valueColor, FormatTemplate = statTemplate });
                }

                // Attack Radius
                if (stats is AoeAttackSettings aoeSettings && aoeSettings.AttackRadius > 0f)
                {
                    string radiusVal = $"{aoeSettings.AttackRadius:0.##}";
                    statsList.Add(new StatUIInfo { StatName = "Attack Radius", StatValue = radiusVal, NameColor = nameColor, ValueColor = valueColor, FormatTemplate = statTemplate });
                }

                // Attack Duration
                if (stats.FullDurationTimeSec > 0f)
                {
                    string durVal = $"{stats.FullDurationTimeSec:0.##} s.";
                    statsList.Add(new StatUIInfo { StatName = "Attack Duration", StatValue = durVal, NameColor = nameColor, ValueColor = valueColor, FormatTemplate = statTemplate });
                }

                // Reload time
                if (stats.ReloadTimeSec > 0f)
                {
                    string reloadVal = $"{stats.ReloadTimeSec:0.##} s.";
                    statsList.Add(new StatUIInfo { StatName = "Reload time", StatValue = reloadVal, NameColor = nameColor, ValueColor = valueColor, FormatTemplate = statTemplate });
                }

                // Impact chance
                if (stats.Impact.UseImpact && stats.Impact.ChancePercent > 0)
                {
                    string impactVal = $"{stats.Impact.ChancePercent}%";
                    statsList.Add(new StatUIInfo { StatName = "Impact chance", StatValue = impactVal, NameColor = nameColor, ValueColor = valueColor, FormatTemplate = statTemplate });
                }
            }

            return statsList;
        }

        public override void ApplyUpgrade(IUpgradableCharacterLogic target)
        {
            target.WeaponsHandler.GiveWeaponAsync(_weaponData);
        }
    }
}
