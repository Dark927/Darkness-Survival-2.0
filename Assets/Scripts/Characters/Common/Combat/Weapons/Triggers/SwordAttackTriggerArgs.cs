using Characters.Common.Combat.Weapons;
using UnityEngine;

namespace Characters.Common.Combat
{
    public class SwordAttackTriggerArgs : AttackTriggerArgs
    {
        public BasicAttack.LocalType AttackType { get; }

        public SwordAttackTriggerArgs(Collider2D targetCollider, BasicAttack.LocalType attackType) : base(targetCollider)
        {
            AttackType = attackType;
        }
    }
}
