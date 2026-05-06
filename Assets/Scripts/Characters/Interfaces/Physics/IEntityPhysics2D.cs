using System;
using Settings.Global;
using UnityEngine;

namespace Characters.Common.Physics2D
{
    public interface IEntityPhysics2D : IImpactable, IInitializable
    {
        public Rigidbody2D Rigidbody2D { get; }
        public Collider2D Collider { get; }
        public void TryPerformPhysicsActions(EntityPhysicsActions physicsActions);
        public void TriggerStunActivationEvent(int durationMs);

        public event Action<int> OnStunRequested;

        public void SetStatic();
        public void SetDynamic();
    }
}
