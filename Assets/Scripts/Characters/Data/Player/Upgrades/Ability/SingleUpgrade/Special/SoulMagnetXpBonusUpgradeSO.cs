using Characters.Player.Abilities;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "SoulMagnetXpBonusUpgradeSO", menuName = "Game/Upgrades/Ability Upgrades/Single Upgrades/Soul Magnet XP Bonus")]
    public class SoulMagnetXpBonusUpgradeSO : SingleUniversalUpgradeSO<IUpgradableAbility>
    {
        [Tooltip("The flat percentage of BONUS EXP granted when pulling a soul.")]
        [SerializeField] private float _xpBonusPercent = 20f;

        protected override string GetDefaultUpgradeName() => "Bonus Soul XP";

        protected override string GetUpgradeValueInfo(char originalSign, char displaySign)
        {
            return $"{displaySign}{_xpBonusPercent}% xp.";
        }

        public override void ApplyUpgrade(IUpgradableAbility target)
        {
            if (target is SoulMagnetAbility soulMagnetAbility)
            {
                soulMagnetAbility.ApplyXpBonusUpgrade(_xpBonusPercent / 100f);
            }
        }

        public override void ApplyDowngrade(IUpgradableAbility target)
        {
            if (target is SoulMagnetAbility soulMagnetAbility)
            {
                soulMagnetAbility.ApplyXpBonusUpgrade(-(_xpBonusPercent / 100f));
            }
        }
    }
}
