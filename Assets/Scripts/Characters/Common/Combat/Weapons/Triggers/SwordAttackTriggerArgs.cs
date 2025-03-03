using Characters.Common.Combat.Weapons;
using UnityEngine;

namespace Characters.Common.Combat
{
    public class SwordAttackTriggerArgs : AttackTriggerArgs
    {
        public CharacterSword.AttackType AttackType { get; }

        public SwordAttackTriggerArgs(Collider2D targetCollider, CharacterSword.AttackType attackType) : base(targetCollider)
        {
            AttackType = attackType;
        }
    }
}
