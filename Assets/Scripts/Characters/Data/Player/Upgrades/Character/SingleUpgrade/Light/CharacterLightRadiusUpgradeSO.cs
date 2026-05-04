using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "NewCharacterLightRadiusUpgradeSO", menuName = "Game/Upgrades/Character Upgrades/Single Upgrades/Light Radius Upgrade Data")]
    public class CharacterLightRadiusUpgradeSO : SingleUniversalUpgradeSO<IUpgradableCharacterLogic>
    {
        [SerializeField, Range(0f, 100f)] private float _radiusUpgradePercent = 0f;

        protected override string GetDefaultUpgradeName() => "Sight distance";

        protected override string GetUpgradeValueInfo(char originalSign, char displaySign)
        {
            return $"{displaySign}{_radiusUpgradePercent}%";
        }

        public override void ApplyUpgrade(IUpgradableCharacterLogic target)
        {
            target.ApplyLightRadiusUpgrade(1 + (_radiusUpgradePercent / 100f));
        }

        public override void ApplyDowngrade(IUpgradableCharacterLogic target)
        {
            target.ApplyLightRadiusUpgrade(1 - (_radiusUpgradePercent / 100f));
        }
    }
}
