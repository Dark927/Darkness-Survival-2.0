using Characters.Common.Movement;
using Characters.Interfaces;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "SpeedUpgradeData", menuName = "Game/Upgrades/Character Upgrades/Single Upgrades/Speed Upgrade Data")]
    public class CharacterSpeedUpgradeSO : SingleUniversalUpgradeSO<ICharacterLogic>
    {
        [SerializeField, Min(0)] private float _speedUpgradePercent = 11;

        public override void ApplyUpgrade(ICharacterLogic target)
        {
            Apply(target, (1 + (_speedUpgradePercent / 100f)));

        }

        public override void ApplyDowngrade(ICharacterLogic target)
        {
            Apply(target, (1 - (_speedUpgradePercent / 100f)));
        }

        private void Apply(ICharacterLogic target, float speedMult)
        {
            EntitySpeed characterSpeed = target.Body.Movement.Speed;
            SpeedSettings updatedSettings = characterSpeed.Settings;
            updatedSettings.MaxSpeedMultiplier *= speedMult;

            characterSpeed.SetSettings(updatedSettings);
        }
    }
}
