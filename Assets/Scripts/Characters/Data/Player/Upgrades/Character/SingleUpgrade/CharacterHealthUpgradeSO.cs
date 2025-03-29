using Characters.Health;
using Characters.Interfaces;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "NewCharacterHealthUpgradeSO", menuName = "Game/Upgrades/Character Upgrades/Single Upgrades/Health Upgrade Data")]
    public class CharacterHealthUpgradeSO : SingleUniversalUpgradeSO<IUpgradableCharacterLogic>
    {
        [SerializeField, Min(0)] private float _hpUpgradePercent = 0;

        protected override string GetInfo(char sign)
        {
            return $"Max HP : {sign}{_hpUpgradePercent}%";
        }

        public override void ApplyDowngrade(IUpgradableCharacterLogic target)
        {
            target.ApplyHealthUpgrade(1 - _hpUpgradePercent / 100f);
        }

        public override void ApplyUpgrade(IUpgradableCharacterLogic target)
        {
            target.ApplyHealthUpgrade(1 + _hpUpgradePercent / 100f);
        }
    }
}
