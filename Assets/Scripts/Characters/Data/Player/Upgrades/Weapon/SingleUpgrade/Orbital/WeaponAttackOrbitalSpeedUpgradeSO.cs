using Characters.Common.Combat.Weapons;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    /// <summary>
    /// Represents a percentage-based upgrade specifically for the orbital speed of kinematic weapons.
    /// Requires the target weapon to implement the IUpgradableOrbitalWeapon interface.
    /// </summary>
    [CreateAssetMenu(fileName = "WeaponAttackOrbitalSpeedUpgradeSO", menuName = "Game/Upgrades/Weapon Upgrades/Single Upgrades/Attack Orbital Speed Upgrade")]
    public class WeaponAttackOrbitalSpeedUpgradeSO : SingleUniversalUpgradeSO<IUpgradableOrbitalWeapon>
    {
        #region Fields

        [Tooltip("The percentage to modify the orbital rotation speed by (e.g., 20 for 20%).")]
        [SerializeField, Min(0f)] private float _orbitalSpeedUpgradePercent = 0f;

        #endregion


        #region Methods

        protected override string GetDefaultUpgradeName() => "Orbital Speed";

        protected override string GetUpgradeValueInfo(char originalSign, char displaySign)
        {
            return $"{displaySign}{_orbitalSpeedUpgradePercent}%";
        }

        public override void ApplyUpgrade(IUpgradableOrbitalWeapon target)
        {
            target.ApplyOrbitalSpeedUpgrade(_orbitalSpeedUpgradePercent / 100f);
        }

        public override void ApplyDowngrade(IUpgradableOrbitalWeapon target)
        {
            target.ApplyOrbitalSpeedUpgrade(-(_orbitalSpeedUpgradePercent / 100f));
        }

        #endregion
    }
}
