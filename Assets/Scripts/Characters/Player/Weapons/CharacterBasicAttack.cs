
using System.Collections.Generic;
using Characters.Common.Combat.Weapons;
using Characters.Player.Animation;
using UnityEngine.InputSystem;

namespace Characters.Player.Weapons
{
    public class CharacterBasicAttack : BasicAttack
    {
        #region Fields 

        private CharacterAnimatorController _animatorController;

        #endregion


        #region Properties

        protected CharacterAnimatorController AnimatorController => _animatorController;

        #endregion


        #region Methods 

        #region Init 

        public CharacterBasicAttack(CharacterBodyBase characterBody, List<CharacterWeaponBase> basicWeapons) : base(characterBody, basicWeapons)
        {

        }

        public override void Init()
        {
            base.Init();
            _animatorController = CharacterBody.Visual.GetAnimatorController<CharacterAnimatorController>();
            OnAttackOfTypeStarted += _animatorController.TriggerAttack;
            AnimatorController.Events.OnAttackFinished += FinishAttack;
        }

        public override void Dispose()
        {
            base.Dispose();
            OnAttackOfTypeStarted -= _animatorController.TriggerAttack;
            AnimatorController.Events.OnAttackFinished -= FinishAttack;
        }

        #endregion

        public void AttackPerformedListener(InputAction.CallbackContext context)
        {
            int contextValue = (int)context.ReadValue<float>();
            TryPerformAttack((Type)contextValue);
        }
    }

    #endregion
}