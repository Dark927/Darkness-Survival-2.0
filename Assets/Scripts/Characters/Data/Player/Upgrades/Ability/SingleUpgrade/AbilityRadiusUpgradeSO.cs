using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "AbilityRadiusUpgradeSO", menuName = "Game/Upgrades/Ability Upgrades/Single Upgrades/Ability Radius Upgrade")]
    public class AbilityRadiusUpgradeSO : SingleUniversalUpgradeSO<IUpgradableAbility>
    {
        public enum RadiusMode
        {
            ExpandRadius,
            ReduceRadius // Useful if an ability gets stronger by concentrating its AoE into a smaller zone
        }

        [Tooltip("The percentage to modify the core effect by (e.g., 10 for 10%)")]
        [SerializeField, Min(0)] private float _radiusUpgradePercent = 10f;

        [Tooltip("Should this upgrade expand the radius (+), or reduce it (-)?")]
        [SerializeField] private RadiusMode _mode = RadiusMode.ExpandRadius;

        private float ActualUpgradePercent => _mode == RadiusMode.ReduceRadius ? -_radiusUpgradePercent : _radiusUpgradePercent;


        protected override string GetInfo(char sign)
        {
            char displaySign = sign;

            if (_mode == RadiusMode.ReduceRadius)
            {
                displaySign = sign == '+' ? '-' : '+';
            }

            return $"{StatNameUI} : {displaySign}{_radiusUpgradePercent}%";
        }

        protected override string GetDefaultStatNameUI()
        {
            return "Effect radius";
        }

        public override void ApplyUpgrade(IUpgradableAbility target)
        {
            target.ApplyRadiusUpgrade(ActualUpgradePercent / 100f);
        }

        public override void ApplyDowngrade(IUpgradableAbility target)
        {
            target.ApplyRadiusUpgrade(-(ActualUpgradePercent / 100f));
        }
    }
}
