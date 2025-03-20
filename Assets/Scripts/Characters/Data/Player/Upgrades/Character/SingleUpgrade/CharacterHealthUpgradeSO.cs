using Characters.Health;
using Characters.Interfaces;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "NewHealthUpgradeData", menuName = "Game/Upgrades/Character Upgrades/Single Upgrades/Health Upgrade Data")]
    public class CharacterHealthUpgradeSO : SingleUniversalUpgradeSO<ICharacterLogic>
    {
        [SerializeField, Min(0)] private float _hpUpgradePercent = 11;

        public override void ApplyDowngrade(ICharacterLogic target)
        {
            Apply(target, (1 - _hpUpgradePercent / 100f));
        }

        public override void ApplyUpgrade(ICharacterLogic target)
        {
            Apply(target, (1 + _hpUpgradePercent / 100f));
        }

        private void Apply(ICharacterLogic target, float multPercent01)
        {
            IHealth health = target.Body.Health;
            health.SetMaxHpLimit(health.MaxHp * multPercent01);
        }
    }
}
