
using Characters.Common.Levels;
using System;
using Characters.Interfaces;

namespace Characters.Player.Upgrades
{
    public interface IUpgradableCharacterLogic : IUpgradable, ICharacterLogic
    {
        public event Action<IUpgradableCharacterLogic, EntityLevelArgs> OnReadyForUpgrade;

        public CharacterUpgradesCoordinator UpgradesCoordinator { get; }

        public void ApplySpeedUpgrade(float percent);
        public void ApplyDamageUpgrade(float percent);
        public void ApplyMaxHealthUpgrade(float percent);
        public void ApplyHealthRegenerationUpgrade(float hpPerSec, bool downgrade = false);
    }
}
