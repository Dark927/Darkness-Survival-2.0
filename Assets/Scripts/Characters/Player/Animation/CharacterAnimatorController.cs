using Characters.Common.Combat.Weapons;
using Characters.Common.Visual;
using Characters.Player.Weapons;
using UnityEngine;

namespace Characters.Player.Animation
{
    public class CharacterAnimatorController : AnimatorController
    {

        #region Properties

        public new CharacterAnimatorParameters Parameters { get => base.Parameters as CharacterAnimatorParameters; }
        public new CharacterAnimationEvents Events { get => base.Events as CharacterAnimationEvents; protected set => base.Events = value; }

        public float Speed
        {
            get => Animator.GetFloat(Parameters.SpeedFieldID);
            set => Animator.SetFloat(Parameters.SpeedFieldID, value);
        }

        public CharacterBasicAttack.LocalType AttackType
        {
            get => (CharacterBasicAttack.LocalType)Animator.GetInteger(Parameters.AttackTypeFieldID);
            set => Animator.SetInteger(Parameters.AttackTypeFieldID, (int)value);
        }

        #endregion


        #region Methods

        #region Init

        public CharacterAnimatorController(Animator characterAnimator, CharacterAnimatorParameters parameters) : base(characterAnimator, parameters)
        {
        }

        public CharacterAnimatorController(Animator characterAnimator, CharacterAnimatorParameters parameters, CharacterAnimationEvents events) : base(characterAnimator, parameters, events)
        {
        }

        #endregion


        public void TriggerAttack(CharacterBasicAttack.LocalType type)
        {
            AttackType = type;
            Animator.SetTrigger(Parameters.AttackTriggerID);
        }

        public void TriggerDeath()
        {
            Animator.SetTrigger(Parameters.DeathTriggerID);
        }

        public void UpdateAttackSpeed(CharacterBasicAttack.LocalType targetAttackType, float percent)
        {
            switch (targetAttackType)
            {
                case BasicAttack.LocalType.Fast:
                    UpdateAttackSpeedByID(Parameters.FastAttackSpeedMultiplierID, percent);
                    break;

                case BasicAttack.LocalType.Heavy:
                    UpdateAttackSpeedByID(Parameters.HeavyAttackSpeedMultiplierID, percent);
                    break;

                case BasicAttack.LocalType.Default:
                    UpdateAttackSpeedByID(Parameters.FastAttackSpeedMultiplierID, percent);
                    UpdateAttackSpeedByID(Parameters.HeavyAttackSpeedMultiplierID, percent);
                    break;
                default:
                    throw new System.NotImplementedException();
            }

        }

        private void UpdateAttackSpeedByID(int attackParameterID, float percent)
        {
            var initialSpeed = Animator.GetFloat(attackParameterID);
            Animator.SetFloat(attackParameterID, initialSpeed * percent);
        }

        public void SpeedUpdateListener(object sender, SpeedChangedArgs args)
        {
            Speed = args.CurrentSpeed;
        }

        public void SetEvents(CharacterAnimationEvents events)
        {
            Events = events;
        }

        #endregion
    }
}
