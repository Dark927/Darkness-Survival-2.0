using System.Collections.Generic;
using Characters.Common.Combat.Weapons;
using Characters.Common.Visual;
using Characters.Player.Weapons;
using UnityEngine;
using Utilities.ErrorHandling;

namespace Characters.Player.Animation
{
    public class CharacterAnimatorController : AnimatorController
    {
        private Dictionary<int, float> _baseAttackSpeeds = new();

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
            CacheBaseSpeeds();
        }

        public CharacterAnimatorController(Animator characterAnimator, CharacterAnimatorParameters parameters, CharacterAnimationEvents events) : base(characterAnimator, parameters, events)
        {
            CacheBaseSpeeds();
        }

        private void CacheBaseSpeeds()
        {
            _baseAttackSpeeds[Parameters.FastAttackSpeedMultiplierID] = Animator.GetFloat(Parameters.FastAttackSpeedMultiplierID);
            _baseAttackSpeeds[Parameters.HeavyAttackSpeedMultiplierID] = Animator.GetFloat(Parameters.HeavyAttackSpeedMultiplierID);
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

        public void UpdateAttackSpeed(CharacterBasicAttack.LocalType targetAttackType, float speedMultiplier)
        {
            switch (targetAttackType)
            {
                case BasicAttack.LocalType.Fast:
                    UpdateAttackSpeedByID(Parameters.FastAttackSpeedMultiplierID, speedMultiplier);
                    break;

                case BasicAttack.LocalType.Heavy:
                    UpdateAttackSpeedByID(Parameters.HeavyAttackSpeedMultiplierID, speedMultiplier);
                    break;

                case BasicAttack.LocalType.Default:
                    UpdateAttackSpeedByID(Parameters.FastAttackSpeedMultiplierID, speedMultiplier);
                    UpdateAttackSpeedByID(Parameters.HeavyAttackSpeedMultiplierID, speedMultiplier);
                    break;
                default:
                    throw new System.NotImplementedException();
            }

        }

        private void UpdateAttackSpeedByID(int attackParameterID, float totalMultiplier)
        {
            if (_baseAttackSpeeds.TryGetValue(attackParameterID, out float baseSpeed))
            {
                //ErrorLogger.Log("Base speed : " + baseSpeed + '\n' + "Total Multiplier : " + totalMultiplier + '\n' + "Result : " + baseSpeed * totalMultiplier);
                Animator.SetFloat(attackParameterID, baseSpeed * totalMultiplier);
            }
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
