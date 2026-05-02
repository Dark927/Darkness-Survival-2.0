using Characters.Player.Abilities;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "SoulMagnetXpBonusUpgradeSO", menuName = "Game/Upgrades/Ability Upgrades/Single Upgrades/Soul Magnet XP Bonus")]
    public class SoulMagnetXpBonusUpgradeSO : SingleUniversalUpgradeSO<IUpgradableAbility>
    {
        [Tooltip("The flat percentage of BONUS EXP granted when pulling a soul.")]
        [SerializeField] private float _xpBonusPercent = 20f;

        protected override string GetInfo(char sign)
        {
            return $"{StatNameUI} : {sign}{_xpBonusPercent}% xp.";
        }

        protected override string GetDefaultStatNameUI()
        {
            return "Bonus Soul XP";
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
