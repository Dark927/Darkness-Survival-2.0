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
            get => Animator.GetFloat(Parameters.SpeedFieldName);
            set => Animator.SetFloat(Parameters.SpeedFieldName, value);
        }

        public CharacterBasicAttack.Type AttackType
        {
            get => (CharacterBasicAttack.Type)Animator.GetInteger(Parameters.AttackTypeFieldName);
            set => Animator.SetInteger(Parameters.AttackTypeFieldName, (int)value);
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


        public void TriggerAttack(CharacterBasicAttack.Type type)
        {
            AttackType = type;
            Animator.SetTrigger(Parameters.AttackTriggerName);
        }

        public void TriggerDeath()
        {
            Animator.SetTrigger(Parameters.DeathTriggerName);
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