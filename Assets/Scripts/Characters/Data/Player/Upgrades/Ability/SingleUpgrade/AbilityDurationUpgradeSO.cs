using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "AbilityDurationUpgradeSO", menuName = "Game/Upgrades/Ability Upgrades/Single Upgrades/Ability Duration Upgrade")]
    public class AbilityDurationUpgradeSO : SingleUniversalUpgradeSO<IUpgradableAbility>
    {
        public enum DurationMode
        {
            ExtendDuration,
            ReduceDuration
        }

        [Tooltip("The percentage to modify the core effect by (e.g., 10 for 10%)")]
        [SerializeField, Min(0)] private float _durationUpgradePercent = 10f;

        [Tooltip("Should this upgrade extend the duration (+), or reduce it (-)?")]
        [SerializeField] private DurationMode _mode = DurationMode.ExtendDuration;

        private float ActualUpgradePercent => _mode == DurationMode.ReduceDuration ? -_durationUpgradePercent : _durationUpgradePercent;


        protected override string GetInfo(char sign)
        {
            // If the caller passes '+' (Upgrade) but our mode is Reduce, we want to display '-'.
            // If the caller passes '-' (Downgrade) but our mode is Reduce, we want to display '+'.
            char displaySign = sign;

            if (_mode == DurationMode.ReduceDuration)
            {
                displaySign = sign == '+' ? '-' : '+';
            }

            return $"{StatNameUI} : {displaySign}{_durationUpgradePercent}%";
        }

        protected override string GetDefaultStatNameUI()
        {
            return "Effect duration";
        }

        public override void ApplyUpgrade(IUpgradableAbility target)
        {
            target.ApplyDurationUpgrade(ActualUpgradePercent / 100f);
        }

        public override void ApplyDowngrade(IUpgradableAbility target)
        {
            target.ApplyDurationUpgrade(-(ActualUpgradePercent / 100f));
        }
    }
}
