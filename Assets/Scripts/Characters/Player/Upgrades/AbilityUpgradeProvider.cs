

namespace Characters.Player.Upgrades
{
    public enum AbilityType
    {
        Weapon,
        Passive,
    }


    /// <summary>
    /// This class extends the base UpgradeProvider and contains some extra data for weapon upgrade applying.
    /// </summary>
    public class AbilityUpgradeProvider : UpgradeProvider
    {
        #region Fields 

        private int _targetAbilityID;
        private AbilityType _abilityType;

        #endregion


        #region Properties 

        public int TargetAbilityID => _targetAbilityID;
        public AbilityType AbilityType => _abilityType;

        #endregion


        #region Methods 

        #region Init

        public AbilityUpgradeProvider(UpgradeConfigurationSO upgradeConfigurationSO, AbilityType abilityType, int targetID) : base(upgradeConfigurationSO)
        {
            _targetAbilityID = targetID;
            _abilityType = abilityType;
        }

        #endregion

        #endregion
    }
}
