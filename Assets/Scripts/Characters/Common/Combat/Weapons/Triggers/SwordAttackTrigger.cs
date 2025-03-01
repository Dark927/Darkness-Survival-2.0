using Characters.Common.Combat.Weapons;
using UnityEngine;

namespace Characters.Common.Combat
{
    public class SwordAttackTrigger : AttackTrigger
    {
        [SerializeField] private CharacterSword.AttackType _attackType;

        public CharacterSword.AttackType TargetAttackType => _attackType;

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            InvokeTrigger(TriggerType.Enter, this, new SwordAttackTriggerArgs(collision, _attackType));
        }

        protected override void OnTriggerStay2D(Collider2D collision)
        {
            InvokeTrigger(TriggerType.Stay, this, new SwordAttackTriggerArgs(collision, _attackType));
        }

        protected override void OnTriggerExit2D(Collider2D collision)
        {
            InvokeTrigger(TriggerType.Exit, this, new SwordAttackTriggerArgs(collision, _attackType));
        }
    }
}
