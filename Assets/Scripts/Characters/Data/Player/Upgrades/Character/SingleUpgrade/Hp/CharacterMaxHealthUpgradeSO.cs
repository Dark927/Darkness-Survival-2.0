using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "NewCharacterHealthUpgradeSO", menuName = "Game/Upgrades/Character Upgrades/Single Upgrades/Max Health Upgrade Data")]
    public class CharacterMaxHealthUpgradeSO : SingleUniversalUpgradeSO<IUpgradableCharacterLogic>
    {
        [SerializeField, Min(0)] private float _hpUpgradePercent = 0;

        protected override string GetDefaultUpgradeName() => "Max HP";

        protected override string GetUpgradeValueInfo(char originalSign, char displaySign)
        {
            return $"{displaySign}{_hpUpgradePercent}";
        }

        public override void ApplyDowngrade(IUpgradableCharacterLogic target)
        {
            target.ApplyMaxHealthUpgrade(1 - _hpUpgradePercent / 100f);
        }

        public override void ApplyUpgrade(IUpgradableCharacterLogic target)
        {
            target.ApplyMaxHealthUpgrade(1 + _hpUpgradePercent / 100f);
        }
    }
}
