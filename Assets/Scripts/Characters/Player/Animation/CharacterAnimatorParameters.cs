using Characters.Common.Visual;
using UnityEngine;

namespace Characters.Player.Animation
{
    public class CharacterAnimatorParameters : IAnimatorParameters
    {
        #region Fields

        private int _speedFieldID = Animator.StringToHash("Speed");
        private int _attackTypeID = Animator.StringToHash("AttackType");
        private int _attackTriggerID = Animator.StringToHash("Attack");
        private int _deathTriggerID = Animator.StringToHash("Die");

        private int _fastAttackSpeedMultiplierID = Animator.StringToHash("FastAttackSpeedMultiplier");
        private int _heavyAttackSpeedMultiplierID = Animator.StringToHash("HeavyAttackSpeedMultiplier");

        #endregion


        #region Properties

        public int SpeedFieldID => _speedFieldID;
        public int AttackTypeFieldID => _attackTypeID;
        public int AttackTriggerID => _attackTriggerID;
        public int DeathTriggerID => _deathTriggerID;
        public int FastAttackSpeedMultiplierID => _fastAttackSpeedMultiplierID;
        public int HeavyAttackSpeedMultiplierID => _heavyAttackSpeedMultiplierID;

        #endregion
    }
}
