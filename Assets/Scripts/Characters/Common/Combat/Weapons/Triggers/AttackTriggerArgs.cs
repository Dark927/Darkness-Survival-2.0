using System;
using UnityEngine;

namespace Characters.Common.Combat
{
    public class AttackTriggerArgs : EventArgs
    {
        public Collider2D TargetCollider { get; }

        public AttackTriggerArgs(Collider2D targetCollider)
        {
            TargetCollider = targetCollider;
        }
    }
}
