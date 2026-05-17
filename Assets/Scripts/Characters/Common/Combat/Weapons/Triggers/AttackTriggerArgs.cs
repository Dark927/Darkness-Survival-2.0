using System;
using UnityEngine;

namespace Characters.Common.Combat
{
    public class AttackTriggerArgs : EventArgs
    {
        public Collider2D TargetCollider { get; }
        public Vector3? Epicenter { get; }

        public AttackTriggerArgs(Collider2D targetCollider, Vector3? epicenter = null)
        {
            TargetCollider = targetCollider;
            Epicenter = epicenter;
        }
    }
}
