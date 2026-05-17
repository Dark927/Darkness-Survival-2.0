using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Common.Statuses
{
    public class EntityStatusLogic : IEntityStatusLogic
    {
        private readonly IEntityDynamicLogic _owner;

        private readonly Dictionary<Type, IStatusEffect> _activeStatuses = new Dictionary<Type, IStatusEffect>();

        // A cached list to safely remove items from the dictionary without triggering GC spikes
        private readonly List<Type> _typesToRemove = new List<Type>(4);

        public EntityStatusLogic(IEntityDynamicLogic owner)
        {
            _owner = owner;
        }

        public void Apply(IStatusEffect newEffect)
        {
            Type effectType = newEffect.GetType();

            if (_activeStatuses.TryGetValue(effectType, out IStatusEffect existingEffect))
            {
                existingEffect.Merge(newEffect);
            }
            else
            {
                _activeStatuses.Add(effectType, newEffect);
                newEffect.OnApply(_owner);
            }
        }

        public void UpdateTimers()
        {
            if (_activeStatuses.Count == 0)
            {
                return;
            }

            float deltaTime = Time.deltaTime;

            // Update everything and flag finished statuses
            foreach (var kvp in _activeStatuses)
            {
                kvp.Value.OnUpdate(deltaTime);

                if (kvp.Value.IsFinished)
                {
                    kvp.Value.OnRemove(_owner);
                    _typesToRemove.Add(kvp.Key);
                }
            }

            // Safely remove them from the dictionary after the iteration is done
            if (_typesToRemove.Count > 0)
            {
                for (int i = 0; i < _typesToRemove.Count; i++)
                {
                    _activeStatuses.Remove(_typesToRemove[i]);
                }
                _typesToRemove.Clear();
            }
        }

        public void ClearAll()
        {
            foreach (var effect in _activeStatuses.Values)
            {
                effect.OnRemove(_owner);
            }

            _activeStatuses.Clear();
            _typesToRemove.Clear();
        }

        public void Remove<T>() where T : IStatusEffect
        {
            Type effectType = typeof(T);

            if (_activeStatuses.TryGetValue(effectType, out IStatusEffect existingEffect))
            {
                existingEffect.OnRemove(_owner);

                _activeStatuses.Remove(effectType);
                _typesToRemove.Remove(effectType);
            }
        }
    }
}
