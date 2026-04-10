
using System;
using Characters.Common;
using Characters.Common.Levels;

namespace Characters.Player.Upgrades
{
    public interface IUpgradableCharacterLogic : IUpgradable, ICharacterLogic
    {
        public event Action<IUpgradableCharacterLogic, EntityLevelArgs> OnReadyForUpgrade;
        public event Action<IUpgradableCharacterLogic, UpgradeAppearTime> OnSpecificTimeUpgradesRequested;

        public CharacterUpgradesCoordinator UpgradesCoordinator { get; }

        public void ApplySpeedUpgrade(float percent);
        public void ApplyDamageUpgrade(float percent);
        public void ApplyMaxHealthUpgrade(float percent);
        public void ApplyHealthRegenerationUpgrade(float hpPerSec, bool downgrade = false);
        public void ApplyLightIntensityUpgrade(float multiplier);
        public void ApplyLightRadiusUpgrade(float multiplier);
    }
}
