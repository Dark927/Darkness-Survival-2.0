using Characters.Common.Visual;
using UnityEngine;
using Utilities.ErrorHandling;

namespace Characters.Common.Statuses
{
    public class MarkedStatusEffect : StatusEffectBase
    {
        private readonly GameObject _markPrefab;
        private IEntityCustomVisualPart _spawnedVisual;

        public MarkedStatusEffect(GameObject markPrefab, float duration)
        {
            _markPrefab = markPrefab;
            Duration = duration;
        }

        public override void OnApply(IEntityDynamicLogic target)
        {
            base.OnApply(target);

            if (_markPrefab == null)
            {
                return;
            }

            // TODO : IMPORTANT - use Pool.Request() here later
            GameObject instance = Object.Instantiate(_markPrefab);
            _spawnedVisual = instance.GetComponent<IEntityCustomVisualPart>();

            if (_spawnedVisual != null)
            {
                _spawnedVisual.Initialize(target.Body);
                target.Body.Visual.GiveCustomVisualPart(_spawnedVisual);
            }
            else
            {
                ErrorLogger.LogWarning($"Mark Prefab {_markPrefab.name} is missing IEntityCustomVisualPart!");
            }
        }

        public override void OnRemove(IEntityDynamicLogic target)
        {
            base.OnRemove(target);

            if (_spawnedVisual != null)
            {
                target.Body.Visual.RemoveCustomVisualPart(_spawnedVisual);

                _spawnedVisual.Dispose();
                _spawnedVisual = null;
            }
        }

        public override void Merge(IStatusEffect newEffect)
        {
            // just extend the duration, no need to spawn a second icon
            base.Merge(newEffect);
        }
    }
}
