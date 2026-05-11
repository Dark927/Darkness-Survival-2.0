using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Common.CustomPhysics2D
{
    public class EntityPhysicsActions
    {
        [Flags]
        public enum PhysicsActionType
        {
            Knockback = 0,
            Stun = 1,
        }

        private readonly Dictionary<PhysicsActionType, IPhysicsAction> _actionsDict = new();

        // Cached buffer for zero-allocation sorting
        private readonly List<IPhysicsAction> _sortedBuffer = new List<IPhysicsAction>(4);

        public EntityPhysicsActions AddKnockback(float force, Vector2 direction)
        {
            return AddAction(PhysicsActionType.Knockback, force, direction);
        }

        public EntityPhysicsActions AddStun(int durationMs)
        {
            return AddAction(PhysicsActionType.Stun, durationMs);
        }

        public IEnumerable<IPhysicsAction> Get()
        {
            _sortedBuffer.Clear();
            _sortedBuffer.AddRange(_actionsDict.Values);

            // Sorts based on the Priority integer (0 executes before 100)
            _sortedBuffer.Sort();

            return _sortedBuffer;
        }

        private EntityPhysicsActions AddAction(PhysicsActionType type, params object[] parameters)
        {
            if (_actionsDict.TryGetValue(type, out IPhysicsAction physicsAction))
            {
                physicsAction.SetValues(parameters);
            }
            else
            {
                _actionsDict.Add(type, CreatePhysicsAction(type, parameters));
            }

            return this;
        }

        private IPhysicsAction CreatePhysicsAction(PhysicsActionType type, params object[] parameters)
        {
            return type switch
            {
                PhysicsActionType.Knockback => new KnockbackAction(parameters),
                PhysicsActionType.Stun => new StunAction(parameters),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
