using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "NewCharacterSpeedUpgradeSO", menuName = "Game/Upgrades/Character Upgrades/Single Upgrades/Speed Upgrade Data")]
    public class CharacterSpeedUpgradeSO : SingleUniversalUpgradeSO<IUpgradableCharacterLogic>
    {
        [SerializeField, Min(0)] private float _speedUpgradePercent = 0;

        protected override string GetDefaultUpgradeName() => "Max SPEED";
        protected override string GetUpgradeValueInfo(char originalSign, char displaySign) => $"{displaySign}{_speedUpgradePercent}%";

        public override void ApplyUpgrade(IUpgradableCharacterLogic target) => target.ApplySpeedUpgrade(1 + (_speedUpgradePercent / 100f));
        public override void ApplyDowngrade(IUpgradableCharacterLogic target) => target.ApplySpeedUpgrade(1 - (_speedUpgradePercent / 100f));
    }
}
