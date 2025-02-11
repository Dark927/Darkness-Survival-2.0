﻿using Characters.Common;
using Characters.Interfaces;
using Characters.Player.Weapons;
using UnityEngine;

namespace Characters.Player
{
    [RequireComponent(typeof(CharacterBodyBase))]
    public class PlayerCharacterLogic : AttackableCharacterBase, ICharacterLogic
    {
        #region Fields

        #endregion


        #region Properties

        public new CharacterBasicAttack BasicAttacks => base.BasicAttacks as CharacterBasicAttack;

        #endregion


        #region Methods


        #region Init

        #endregion


    }

    #endregion
}
