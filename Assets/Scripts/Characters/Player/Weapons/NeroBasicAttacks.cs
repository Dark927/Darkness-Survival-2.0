using System;
using System.Collections.Generic;
using Characters.Common.Combat.Weapons;
using Characters.Interfaces;

namespace Characters.Player.Weapons
{
    public class NeroBasicAttacks : CharacterBasicAttack
    {
        private CharacterSword _sword;

        public NeroBasicAttacks(ICharacterLogic characterLogic, List<CharacterWeaponBase> basicWeapons) : base(characterLogic.Body, basicWeapons)
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

        public override void Init()
        {
            base.Init();

            AnimatorController.Events.OnAttackHit += TriggerAttack;
        }

        private void TriggerAttack(Type type)
        {
            if (_sword == null)
            {
                return;
            }

            switch (type)
            {
                case Type.Fast:
                    TriggerFastAttack();
                    break;

                case Type.Heavy:
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

        public override void Dispose()
        {
            AnimatorController.Events.OnAttackHit -= TriggerAttack;

            base.Dispose();
        }
    }
}
