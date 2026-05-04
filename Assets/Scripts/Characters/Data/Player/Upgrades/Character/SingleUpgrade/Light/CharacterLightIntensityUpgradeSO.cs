using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "NewCharacterLightIntensityUpgradeSO", menuName = "Game/Upgrades/Character Upgrades/Single Upgrades/Light Intensity Upgrade Data")]
    public class CharacterLightIntensityUpgradeSO : SingleUniversalUpgradeSO<IUpgradableCharacterLogic>
    {
        [SerializeField, Range(0f, 100f)] private float _intensityUpgradePercent = 0f;

        protected override string GetDefaultUpgradeName() => "Luminous vision";

        protected override string GetUpgradeValueInfo(char originalSign, char displaySign)
        {
            return $"{displaySign}{_intensityUpgradePercent}%";
        }

        public override void ApplyUpgrade(IUpgradableCharacterLogic target)
        {
            target.ApplyLightIntensityUpgrade(1 + (_intensityUpgradePercent / 100f));
        }

        public override void ApplyDowngrade(IUpgradableCharacterLogic target)
        {
            target.ApplyLightIntensityUpgrade(1 - (_intensityUpgradePercent / 100f));
        }
    }
}
