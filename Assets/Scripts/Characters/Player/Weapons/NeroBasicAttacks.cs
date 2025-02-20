using Characters.Common.Combat.Weapons;
using Characters.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Characters.Player.Weapons
{
    public class NeroBasicAttacks : CharacterBasicAttack
    {
        #region Fields 

        private CharacterSword _sword;

        #endregion


        #region Methods

        #region Init

        public NeroBasicAttacks(ICharacterLogic characterLogic, IEnumerable<WeaponBase> basicWeapons) : base(characterLogic.Body, basicWeapons)
        {
            foreach (var weapon in BasicWeapons)
            {
                if (weapon.TryGetComponent(out CharacterSword sword))
                {
                    _sword = sword;
                    break;
                }
            }
        }

        public override void ConfigureEventLinks()
        {
            base.ConfigureEventLinks();
            AnimatorController.Events.OnAttackHit += TriggerAttack;
        }

        public override void RemoveEventLinks()
        {
            base.RemoveEventLinks();
            AnimatorController.Events.OnAttackHit -= TriggerAttack;
        }

        #endregion

        private void TriggerAttack(LocalType type)
        {
            if (_sword == null)
            {
                return;
            }

            switch (type)
            {
                case LocalType.Fast:
                    TriggerFastAttack();
                    break;

                case LocalType.Heavy:
                    TriggerHeavyAttack();
                    break;

                default:
                    throw new InvalidOperationException();
            }
        }

        private void TriggerFastAttack()
        {
            _sword.TriggerAttack(CharacterSword.AttackType.Fast);
        }

        private void TriggerHeavyAttack()
        {
            _sword.TriggerAttack(CharacterSword.AttackType.Heavy);
        }

        protected override void AnyAttackStarted()
        {
            base.AnyAttackStarted();

            if (!EntityBody.IsDying)
            {
                EntityBody.Movement.Block();
                EntityBody.Physics.SetStatic();
            }
        }

        protected override void AttackFinished()
        {
            base.AttackFinished();

            if (!EntityBody.IsDying)
            {
                EntityBody.Movement.Unblock();
                EntityBody.Physics.SetDynamic();
            }
        }

        #endregion
    }
}
