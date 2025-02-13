
using Characters.Common.Combat.Weapons;
using Characters.Interfaces;
using Characters.Player.Animation;
using System.Collections.Generic;
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

        public CharacterBasicAttack(IEntityBody characterBody, List<WeaponBase> basicWeapons) : base(characterBody, basicWeapons)
        {

        }

        public override void Init()
        {
            base.Init();
            _animatorController = EntityBody.Visual.GetAnimatorController<CharacterAnimatorController>();
        }


        public override void ConfigureEventLinks()
        {
            OnAttackOfTypeStarted += _animatorController.TriggerAttack;
            AnimatorController.Events.OnAttackFinished += RaiseAttackFinished;
        }

        public override void RemoveEventLinks()
        {
            OnAttackOfTypeStarted -= _animatorController.TriggerAttack;
            AnimatorController.Events.OnAttackFinished -= RaiseAttackFinished;
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
