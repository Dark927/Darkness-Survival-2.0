using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "AbilityStrengthUpgradeSO", menuName = "Game/Upgrades/Ability Upgrades/Single Upgrades/Ability Strength Upgrade")]
    public class AbilityStrengthUpgradeSO : SingleUniversalUpgradeSO<IUpgradableAbility>
    {
        [Tooltip("The percentage to increase the core effect by (e.g., 10 for +10%)")]
        [SerializeField, Min(0)] private float _strengthUpgradePercent = 10f;

        protected override string GetInfo(char sign)
        {
            return $"Effect strength : {sign}{_strengthUpgradePercent}%";
        }

        public override void ApplyUpgrade(IUpgradableAbility target)
        {
            // Pass the RAW value. 1.25 stays 1.25.
            target.ApplyStrengthUpgrade(_strengthUpgradePercent);
        }

        public override void ApplyDowngrade(IUpgradableAbility target)
        {
            // Invert the RAW value to subtract it.
            target.ApplyStrengthUpgrade(-_strengthUpgradePercent);
        }
    }
}
