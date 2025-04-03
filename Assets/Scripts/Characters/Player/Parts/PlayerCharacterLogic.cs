using System;
using Characters.Common;
using Characters.Common.Levels;
using Characters.Common.Movement;
using Characters.Health;
using Characters.Interfaces;
using Characters.Player.Data;
using Characters.Player.Levels;
using Characters.Player.Upgrades;
using Characters.Player.Weapons;
using Characters.Stats;
using UnityEngine;
using World.Data;

namespace Characters.Player
{
    [RequireComponent(typeof(IEntityPhysicsBody))]
    public class PlayerCharacterLogic : AttackableEntityLogic, IUpgradableCharacterLogic
    {
        #region Events 

        public event Action<IUpgradableCharacterLogic, EntityLevelArgs> OnReadyForUpgrade;
        public event Action<IUpgradableCharacterLogic, UpgradeAppearTime> OnSpecificTimeUpgradesRequested;

        #endregion


        #region Fields

        private ICharacterLevel _level;
        private CharacterUpgradesCoordinator _upgradesCoordinator;

        #endregion


        #region Properties

        public CharacterBasicAttack BasicAttack => base.BasicAttacks as CharacterBasicAttack;
        public ICharacterLevel Level => _level;
        public CharacterUpgradesCoordinator UpgradesCoordinator => _upgradesCoordinator;

        #endregion


        #region Methods

        #region Init

        public override void Initialize(IEntityData data)
        {
            base.Initialize(data);
            PlayerCharacterData playerCharacterData = data as PlayerCharacterData;
            _level = new PlayerCharacterLevel(playerCharacterData.CharacterLevelData);
            _upgradesCoordinator = new CharacterUpgradesCoordinator(this);
        }

        public override void ConfigureEventLinks()
        {
            base.ConfigureEventLinks();
            _level.OnLevelUp += LevelUpListener;
        }

        public override void RemoveEventLinks()
        {
            base.RemoveEventLinks();
            _level.OnLevelUp -= LevelUpListener;
        }

        #endregion


        public void ReactToDayStateChange(DayTimeType dayTime)
        {
            if (dayTime == DayTimeType.Day)
            {
                OnSpecificTimeUpgradesRequested?.Invoke(this, UpgradeAppearTime.Day);
            }

            if (dayTime == DayTimeType.Night)
            {
                OnSpecificTimeUpgradesRequested?.Invoke(this, UpgradeAppearTime.Night);
            }
        }

        public void ApplySpeedUpgrade(float percent)
        {
            EntitySpeed characterSpeed = Body.Movement.Speed;
            SpeedSettings updatedSettings = characterSpeed.Settings;
            updatedSettings.MaxSpeedMultiplier *= percent;

            characterSpeed.SetSettings(updatedSettings);
        }

        // ToDo : apply damage upgrade
        public void ApplyDamageUpgrade(float percent)
        {
            throw new NotImplementedException();
        }

        public void ApplyMaxHealthUpgrade(float percent)
        {
            IHealth characterHealth = Body.Health;
            float targetMaxHp = characterHealth.MaxHp * percent;
            float hpDiff = targetMaxHp - characterHealth.MaxHp;

            characterHealth.SetMaxHpLimit(targetMaxHp);

            if (hpDiff > 0f)
            {
                characterHealth.Heal(hpDiff);
            }
        }

        public void ApplyHealthRegenerationUpgrade(float hpPerSec, bool downgrade = false)
        {
            IHealth characterHealth = Body.Health;

            if (downgrade)
            {
                characterHealth.ReducePermanentHpRegeneration(hpPerSec);
            }
            else
            {
                characterHealth.RegenerateHpAlways(hpPerSec, 1f, true);
            }
        }

        public void ApplyLightIntensityUpgrade(float multiplier)
        {
            if (Body.TryGetEntityLight(out var light))
            {
                float targetIntensityLimit = light.LightIntensityLimit * multiplier;
                light.SetLightIntensityLimit(targetIntensityLimit);
            }
        }

        public void ApplyLightRadiusUpgrade(float multiplier)
        {
            if (Body.TryGetEntityLight(out var light))
            {
                light.UpdateLightRadius(multiplier);
            }
        }

        private void LevelUpListener(object sender, EntityLevelArgs args)
        {
            OnReadyForUpgrade?.Invoke(this, args);
        }
        #endregion
    }

}
