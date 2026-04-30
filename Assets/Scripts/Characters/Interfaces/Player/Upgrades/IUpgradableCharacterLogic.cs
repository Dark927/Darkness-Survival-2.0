
using System;
using System.Collections.Generic;
using Characters.Common;
using Characters.Common.Levels;

namespace Characters.Player.Upgrades
{
    public interface IUpgradableCharacterLogic : IUpgradable, ICharacterLogic
    {
        public event Action<IUpgradableCharacterLogic, EntityLevelArgs> OnReadyForUpgrade;
        public event Action<IUpgradableCharacterLogic, List<UpgradeProvider>> OnCustomUpgradesReceived;
        public event Action<IUpgradableCharacterLogic, UpgradeAppearTime> OnSpecificTimeUpgradesRequested;
        public event Action<IUpgradableCharacterLogic> OnIntroUpgradesReceived;

        public CharacterUpgradesCoordinator UpgradesCoordinator { get; }

        public void ReceiveCustomUpgrades(List<UpgradeProvider> upgrades);
        public void ReceiveIntroUpgrades(List<UpgradeProvider> upgrades);
        public void NotifyUpgradeApplied();

        public void ApplySpeedUpgrade(float percent);
        public void ApplyDamageUpgrade(float percent);
        public void ApplyMaxHealthUpgrade(float percent);
        public void ApplyHealthRegenerationUpgrade(float hpPerSec, bool downgrade = false);
        public void ApplyLightIntensityUpgrade(float multiplier);
        public void ApplyLightRadiusUpgrade(float multiplier);
    }
}
