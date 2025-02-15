using Characters.Common;
using Characters.Interfaces;
using Characters.Player.Weapons;
using UnityEngine;

namespace Characters.Player
{
    [RequireComponent(typeof(EntityBodyBase))]
    public class PlayerCharacterLogic : AttackableEntityLogic, ICharacterLogic
    {
        #region Fields

        #endregion


        #region Properties

        public CharacterBasicAttack BasicAttack => base.BasicAttacks as CharacterBasicAttack;

        #endregion


        #region Methods

        #region Init

        #endregion


    }

    #endregion
}
