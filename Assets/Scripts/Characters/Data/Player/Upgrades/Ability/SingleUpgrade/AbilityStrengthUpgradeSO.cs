using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "AbilityStrengthUpgradeSO", menuName = "Game/Upgrades/Ability Upgrades/Single Upgrades/Ability Strength Upgrade")]
    public class AbilityStrengthUpgradeSO : SingleUniversalUpgradeSO<IUpgradableAbility>
    {
        [Tooltip("The percentage to increase the core effect by (e.g., 10 for +10%)")]
        [SerializeField, Min(0)] private float _strengthUpgradePercent = 10f;

        protected override string GetDefaultUpgradeName() => "Effect strength";

        protected override string GetUpgradeValueInfo(char originalSign, char displaySign)
        {
            return $"{displaySign}{_strengthUpgradePercent}%";
        }

        public override void ApplyUpgrade(IUpgradableAbility target)
        {
            target.ApplyStrengthUpgrade(_strengthUpgradePercent / 100f);
        }

        public override void ApplyDowngrade(IUpgradableAbility target)
        {
            target.ApplyStrengthUpgrade(-(_strengthUpgradePercent / 100f));
        }
    }
}
