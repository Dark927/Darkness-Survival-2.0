using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "NewCharacterHealthUpgradeSO", menuName = "Game/Upgrades/Character Upgrades/Single Upgrades/Health Upgrade Data")]
    public class CharacterHealthUpgradeSO : SingleUniversalUpgradeSO<IUpgradableCharacterLogic>
    {
        [SerializeField, Min(0)] private float _hpUpgradePercent = 0;
        [SerializeField] private bool _healFullHp = false;

        protected override string GetInfo(char sign)
        {
            if (_healFullHp)
            {
                return $"Max HP : {sign}{_hpUpgradePercent}%" +
                        $"\n<color=green>fully recover HP once</color>";
            }
            return $"Max HP : {sign}{_hpUpgradePercent}%";
        }

        public override void ApplyDowngrade(IUpgradableCharacterLogic target)
        {
            target.ApplyMaxHealthUpgrade(1 - _hpUpgradePercent / 100f);
            TryHealFullTargetHp(target);
        }

        public override void ApplyUpgrade(IUpgradableCharacterLogic target)
        {
            target.ApplyMaxHealthUpgrade(1 + _hpUpgradePercent / 100f);
            TryHealFullTargetHp(target);
        }

        private void TryHealFullTargetHp(IUpgradableCharacterLogic target)
        {
            if (!_healFullHp)
            {
                return;
            }

            var targetHealth = target.Body.Health;
            targetHealth.Heal(targetHealth.MaxHp);
        }
    }
}
