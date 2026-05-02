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
            return $"{StatNameUI} : {sign}{_strengthUpgradePercent}%";
        }

        protected override string GetDefaultStatNameUI()
        {
            return "Effect strength";
        }

        public override void ApplyUpgrade(IUpgradableAbility target)
        {
            // Pass the multiplier value. 1.25 converts to 0.125
            target.ApplyStrengthUpgrade(_strengthUpgradePercent / 100f);
        }

        public override void ApplyDowngrade(IUpgradableAbility target)
        {
            // Invert the multiplier value to subtract it.
            target.ApplyStrengthUpgrade(-(_strengthUpgradePercent / 100f));
        }
    }
}
