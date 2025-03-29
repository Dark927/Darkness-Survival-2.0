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

namespace Characters.Player
{
    [RequireComponent(typeof(IEntityPhysicsBody))]
    public class PlayerCharacterLogic : AttackableEntityLogic, IUpgradableCharacterLogic
    {
        #region Events 

        public event Action<IUpgradableCharacterLogic, EntityLevelArgs> OnReadyForUpgrade;

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

        private void LevelUpListener(object sender, EntityLevelArgs args)
        {
            OnReadyForUpgrade?.Invoke(this, args);
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

        public void ApplyHealthUpgrade(float percent)
        {
            IHealth characterHealth = Body.Health;
            characterHealth.SetMaxHpLimit(characterHealth.MaxHp * percent);
        }

        #endregion


    }

}
