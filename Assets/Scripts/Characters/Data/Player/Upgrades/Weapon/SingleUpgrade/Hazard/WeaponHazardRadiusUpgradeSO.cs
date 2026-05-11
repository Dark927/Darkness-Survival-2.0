using Characters.Common.Combat.Weapons;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "WeaponHazardRadiusUpgradeSO", menuName = "Game/Upgrades/Weapon Upgrades/Single Upgrades/Hazard Weapons/Hazard Radius Upgrade")]
    public class WeaponHazardRadiusUpgradeSO : SingleUniversalUpgradeSO<IUpgradableHazardWeapon>
    {
        public enum UpgradeType
        {
            MinimumRadius,
            MaximumRadius,
            Both
        }

        [Header("Hazard Target")]
        [Tooltip("Which boundary should this upgrade affect?")]
        [SerializeField] private UpgradeType _radiusTarget = UpgradeType.Both;

        [Header("Value")]
        [Tooltip("The percentage to increase the radius by (e.g., 20 for 20%).")]
        [SerializeField, Min(0f)] private float _radiusUpgradePercent = 0f;

        protected override string GetDefaultUpgradeName() => $"{_radiusTarget} Radius";

        protected override string GetUpgradeValueInfo(char originalSign, char displaySign)
        {
            return $"{displaySign}{_radiusUpgradePercent}%";
        }

        public override void ApplyUpgrade(IUpgradableHazardWeapon target)
        {
            float multiplier = _radiusUpgradePercent / 100f;
            ModifyRadius(target, multiplier);
        }

        public override void ApplyDowngrade(IUpgradableHazardWeapon target)
        {
            float multiplier = -(_radiusUpgradePercent / 100f);
            ModifyRadius(target, multiplier);
        }

        private void ModifyRadius(IUpgradableHazardWeapon target, float multiplier)
        {
            switch (_radiusTarget)
            {
                case UpgradeType.MinimumRadius:
                    target.ApplyMinSpawnRadiusUpgrade(multiplier);
                    break;
                case UpgradeType.MaximumRadius:
                    target.ApplyMaxSpawnRadiusUpgrade(multiplier);
                    break;
                case UpgradeType.Both:
                    target.ApplyMinSpawnRadiusUpgrade(multiplier);
                    target.ApplyMaxSpawnRadiusUpgrade(multiplier);
                    break;
            }
        }
    }
}
