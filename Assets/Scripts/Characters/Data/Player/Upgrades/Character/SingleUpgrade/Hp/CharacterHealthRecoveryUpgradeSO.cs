using Characters.Health;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "NewCharacterHealthExtraBonusSO", menuName = "Game/Upgrades/Character Upgrades/Single Upgrades/Health Recovery Upgrade Data")]
    public class CharacterHealthRecoveryUpgradeSO : SingleUniversalUpgradeSO<IUpgradableCharacterLogic>
    {
        [SerializeField, Range(0, 95f)] private float _hpPercent = 0;

        [Header("<color=yellow>Note : this parameter overrides previous ones</color>")]
        [SerializeField] private bool _healFullHp = false;

        protected override string GetDefaultUpgradeName() => "HP fast recovery";
        protected override string GetUpgradeValueInfo(char originalSign, char displaySign)
        {
            return $"{displaySign}{_hpPercent}%";
        }

        public override void ApplyUpgrade(IUpgradableCharacterLogic target)
        {
            var targetHealth = target.Body.Health;

            if (_healFullHp)
            {
                HealFullTargetHp(targetHealth);
                return;
            }

            targetHealth.Heal(targetHealth.MaxHp * (_hpPercent / 100f));
        }

        public override void ApplyDowngrade(IUpgradableCharacterLogic target)
        {
            var targetHealth = target.Body.Health;
            targetHealth.ReduceCurrentHp(targetHealth.CurrentHp * (_hpPercent / 100f));
        }

        private void HealFullTargetHp(IHealth targetHealth)
        {
            targetHealth.Heal(targetHealth.MaxHp);
        }
    }
}
