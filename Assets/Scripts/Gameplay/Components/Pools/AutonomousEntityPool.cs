using System;
using Gameplay.Components;
using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    /// <summary>
    /// A universal pool for any AutonomousEntity. 
    /// It intercepts the creation of new items to automatically apply current visual states.
    /// </summary>
    public class AutonomousEntityPool<T> : ComponentsPoolBase<T>, IElementWithExtraVisual where T : AutonomousEntityBase
    {
        private bool _isSpecialVisualActive = false;
        public bool IsSpecialVisualActive => _isSpecialVisualActive;

        public AutonomousEntityPool(ObjectPoolSettings poolSettings, GameObject poolItemPrefab, Transform container)
            : base(poolSettings, poolItemPrefab, container)
        {
        }

        // Intercept creation to apply state to dynamically spawned items
        protected override T PreloadFunc(Transform container = null)
        {
            // Let the base class instantiate the prefab
            T newEntity = base.PreloadFunc(container);

            // Apply the extra visual to this new item instantly if needed
            if (_isSpecialVisualActive && newEntity.TryGetComponent<IElementWithExtraVisual>(out var visual))
            {
                visual.EnableSpecialVisual();
            }

            return newEntity;
        }

        public void EnableSpecialVisual()
        {
            _isSpecialVisualActive = true;
            ApplyVisualStateToAll(visual => visual.EnableSpecialVisual());
        }

        public void DisableSpecialVisual()
        {
            _isSpecialVisualActive = false;
            ApplyVisualStateToAll(visual => visual.DisableSpecialVisual());
        }

        private void ApplyVisualStateToAll(Action<IElementWithExtraVisual> action)
        {
            ApplyActionToAllItems(entity =>
            {
                if (entity.TryGetComponent<IElementWithExtraVisual>(out var visual))
                {
                    action(visual);
                }
            });
        }
    }
}
