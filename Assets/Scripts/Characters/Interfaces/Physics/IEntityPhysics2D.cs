

using Characters.Interfaces;
using Settings.Global;
using System;
using UnityEngine;

namespace Characters.Common.Physics2D
{
    public interface IEntityPhysics2D : IImpactable, IInitializable
    {
        public IEntityDynamicLogic EntityLogic { get; }
        public Rigidbody2D Rigidbody2D { get; }
        public Collider2D Collider { get; }
        public void TryPerformPhysicsActions(EntityPhysicsActions physicsActions);


        public void SetStatic();
        public void SetDynamic();
    }
}
