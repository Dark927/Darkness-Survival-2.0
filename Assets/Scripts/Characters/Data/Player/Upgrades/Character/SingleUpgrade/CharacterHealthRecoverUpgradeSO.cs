using Characters.Health;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "NewCharacterHealthExtraBonusSO", menuName = "Game/Upgrades/Character Upgrades/Bonuses/Health Extra Bonus Data")]
    public class CharacterHealthExtraBonusSO : SingleUniversalUpgradeSO<IUpgradableCharacterLogic>
    {
        [SerializeField, Range(0, 95f)] private float _hpPercent = 0;

        [Header("<color=yellow>Note : this parameter overrides previous ones</color>")]
        [SerializeField] private bool _healFullHp = false;

        protected override string GetInfo(char sign)
        {
            if (_healFullHp)
            {
                return $"<color=green>fully recover current HP</color>";
            }

            if (sign == '+')
            {
                return $"<color=green>Current HP : {sign}{_hpPercent}% of Max HP </color>";
            }

            if (sign == '-')
            {
                return $"<color=red>Current HP : {sign}{_hpPercent}%</color>";
            }

            return "<color=red>error<color=red";
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
