using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "NewCharacterHealthRegenUpgradeSO", menuName = "Game/Upgrades/Character Upgrades/Single Upgrades/Health Regen Upgrade Data")]
    public class CharacterHealthRegenUpgradeSO : SingleUniversalUpgradeSO<IUpgradableCharacterLogic>
    {
        [SerializeField, Range(0, 25f)] private float _hpPercentPerSec = 0;

        protected override string GetInfo(char sign)
        {
            return $"HP regen : {sign}{_hpPercentPerSec}% hp/sec";
        }

        public override void ApplyUpgrade(IUpgradableCharacterLogic target)
        {
            target.ApplyHealthRegenerationUpgrade(_hpPercentPerSec / 100f, false);
        }

        public override void ApplyDowngrade(IUpgradableCharacterLogic target)
        {
            target.ApplyHealthRegenerationUpgrade(_hpPercentPerSec / 100f, true);
        }

        private void OnValidate()
        {
            if (_hpPercentPerSec <= 0)
            {
                _hpPercentPerSec = 0f;
            }
        }
    }
}
