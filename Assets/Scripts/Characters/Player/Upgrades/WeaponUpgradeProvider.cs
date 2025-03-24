

namespace Characters.Player.Upgrades
{
    /// <summary>
    /// This class extends the base UpgradeProvider and contains some extra data for weapon upgrade applying.
    /// </summary>
    public class WeaponUpgradeProvider : UpgradeProvider
    {
        #region Fields 

        private int _targetWeaponID;

        #endregion


        #region Properties 

        public int TargetWeaponID => _targetWeaponID;

        #endregion


        #region Methods 

        #region Init

        public WeaponUpgradeProvider(UpgradeConfigurationSO upgradeConfigurationSO, int targetWeaponID) : base(upgradeConfigurationSO)
        {
            _targetWeaponID = targetWeaponID;
        }

        #endregion

        #endregion
    }
}
