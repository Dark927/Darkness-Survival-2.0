using System;
using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    /// <summary>
    /// Any entity that is spawned as a reaction (like an explosion on death) must implement this.
    /// </summary>
    public interface IReactiveAutonomousEntity
    {
        void ActivateReaction(
            Vector3 position,
            float radius,
            float visualLifeTime,
            LayerMask targetMask,
            Action<Collider2D> onHit,
            Action<AutonomousEntityBase> onDie);
    }
}
