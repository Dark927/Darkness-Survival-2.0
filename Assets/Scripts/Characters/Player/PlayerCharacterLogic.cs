using Characters.Common;
using Characters.Common.Levels;
using Characters.Interfaces;
using Characters.Player.Data;
using Characters.Player.Levels;
using Characters.Player.Weapons;
using Characters.Stats;
using UnityEngine;

namespace Characters.Player
{
    [RequireComponent(typeof(IEntityPhysicsBody))]
    public class PlayerCharacterLogic : AttackableEntityLogic, ICharacterLogic
    {
        #region Fields

        private ICharacterLevel _level;

        #endregion


        #region Properties

        public CharacterBasicAttack BasicAttack => base.BasicAttacks as CharacterBasicAttack;
        public ICharacterLevel Level => _level;

        #endregion


        #region Methods

        #region Init

        public override void Initialize(IEntityData data)
        {
            base.Initialize(data);
            PlayerCharacterData playerCharacterData = data as PlayerCharacterData;
            _level = new PlayerCharacterLevel(playerCharacterData.CharacterLevelData);
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

        private void LevelUpListener(IEntityLevel entityLevel)
        {

        }

        #endregion


    }

}
