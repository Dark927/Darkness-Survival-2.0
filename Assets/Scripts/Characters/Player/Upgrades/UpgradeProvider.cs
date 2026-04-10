

using System;
using System.Collections.Generic;
using System.Linq;
using UI.Characters.Upgrades;
using UnityEngine;

/// <summary>
/// This class is used to provide upgrade's next level data and increment levels.
/// </summary>

namespace Characters.Player.Upgrades
{
    // TODO : Inherit this class for Weapons and add field to store TargetWeaponID for upgrade.
    // We will set this value when a weapon is unlocked and reuse it to upgrade this weapon.
    // This will prevent the loading of EntityWeaponData to compare IDs.

    public class UpgradeProvider
    {
        #region Fields 

        private UpgradeConfigurationSO _upgradeConfigurationSO;
        private UpgradeSO _upgradeSO;
        private List<IUpgradeLevelSO> _upgradeLevelsList;
        private int _currentUpgradeLevel;
        private IUpgradeLevelSO _upgradeLevel;

        #endregion


        #region Properties 

        public int CommonLevelsCount => _upgradeSO.UpgradeLevels.Count();
        public int CurrentLevelToApply => _currentUpgradeLevel;
        public int RemainingLevels => (CommonLevelsCount + 1) - CurrentLevelToApply;
        public bool HasNextLevel => RemainingLevels > 0;
        public UpgradeVisualDataUI VisualData => _upgradeLevel.CustomUpgradeVisualDataUI == null
                                                                    ? _upgradeConfigurationSO.VisualData
                                                                    : _upgradeLevel.CustomUpgradeVisualDataUI;
        public UpgradeType Type => _upgradeConfigurationSO.Type;
        public UpgradeAppearTime AppearTime => _upgradeConfigurationSO.AppearTime;

        #endregion


        #region Methods 

        #region Init

        public UpgradeProvider(UpgradeConfigurationSO upgradeConfigurationSO)
        {
            if (upgradeConfigurationSO == null || upgradeConfigurationSO.Upgrade == null || upgradeConfigurationSO.Upgrade.UpgradeLevels == null)
            {
                throw new NullReferenceException($"# {nameof(upgradeConfigurationSO)} is incorrect! - {this}");
            }

            _upgradeConfigurationSO = upgradeConfigurationSO;
            _upgradeSO = upgradeConfigurationSO.Upgrade;
            _upgradeLevelsList = _upgradeSO.UpgradeLevels.ToList();

            Reset();
        }

        public void Reset()
        {
            _currentUpgradeLevel = 1;
        }

        #endregion

        /// <summary>
        /// Get next upgrade level and increment current level.
        /// </summary>
        /// <param name="upgradeLevel">out the upgrade level of specific requested type ([ == null] when the method returns false)</param>
        /// <returns>true - next upgrade level is not null, false - levels are over</returns>
        public bool TryGetNextUpgradeLevel<TTargetUpgradeLevel>(out TTargetUpgradeLevel upgradeLevel) where TTargetUpgradeLevel : class, IUpgradeLevelSO
        {
            upgradeLevel = null;

            if (HasNextLevel)
            {
                upgradeLevel = GetNextUpgradeLevel<TTargetUpgradeLevel>();
            }

            return upgradeLevel != null;
        }

        public TTargetUpgradeLevel GetNextUpgradeLevel<TTargetUpgradeLevel>() where TTargetUpgradeLevel : class, IUpgradeLevelSO
        {
            _upgradeLevel = _upgradeLevelsList[CurrentLevelToApply - 1];
            _currentUpgradeLevel++;
            return _upgradeLevel as TTargetUpgradeLevel;
        }

        public UpgradeInfoUI GetUpgradeMainInfo()
        {
            string upgradeLevel = $"LVL : {_currentUpgradeLevel - 1} -> <color=red>{_currentUpgradeLevel}</color>/{CommonLevelsCount}";
            string upgradeType = GetUpgradeTypeInfo(_upgradeConfigurationSO.Type);

            return new UpgradeInfoUI()
            {
                Title = _upgradeConfigurationSO.Name,
                Type = upgradeType,
                Level = upgradeLevel,
                Description = _upgradeConfigurationSO.Description,
                Icon = _upgradeConfigurationSO.Icon,
            };
        }

        public string GetCurrentUpgradeLevelInfo()
        {
            _upgradeLevel = PeekNextLevel();
            Debug.Assert(_upgradeLevel != null, "Upgrade level is null! - " + this.ToString());

            return _upgradeLevel.Description;
        }

        private IUpgradeLevelSO PeekNextLevel()
        {
            if (HasNextLevel)
            {
                return _upgradeLevelsList[CurrentLevelToApply - 1];
            }
            else
            {
                return null;
            }
        }

        private string GetUpgradeTypeInfo(UpgradeType type)
        {
            return type switch
            {
                UpgradeType.Character => "Character Upgrade",
                UpgradeType.Ability => "Ability Upgrade",
                UpgradeType.AbilityUnlock => "Ability Unlock",
                _ => throw new NotImplementedException(),
            };

        }

        #endregion
    }
}
