using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "NewCharacterSpeedUpgradeSO", menuName = "Game/Upgrades/Character Upgrades/Single Upgrades/Speed Upgrade Data")]
    public class CharacterSpeedUpgradeSO : SingleUniversalUpgradeSO<IUpgradableCharacterLogic>
    {
        [SerializeField, Min(0)] private float _speedUpgradePercent = 0;

        protected override string GetInfo(char sign)
        {
            return $"Max SPEED : {sign}{_speedUpgradePercent}%";
        }

        public override void ApplyUpgrade(IUpgradableCharacterLogic target)
        {
            target.ApplySpeedUpgrade(1 + (_speedUpgradePercent / 100f));
        }

        public override void ApplyDowngrade(IUpgradableCharacterLogic target)
        {
            target.ApplySpeedUpgrade(1 - (_speedUpgradePercent / 100f));
        }
    }
}
