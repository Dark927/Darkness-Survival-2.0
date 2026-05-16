using System;
using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    /// <summary>
    /// A universal explosion/reaction entity. Used for ANY reaction prefab (Blood, Frost, Fire).
    /// </summary>
    public class ReactiveExplosionEntity : AutonomousEntityBase, IReactiveAutonomousEntity, IResizableAutonomousEntity
    {
        private float _explosionRadius;
        private LayerMask _targetMask;
        private Action<Collider2D> _onHitCallback;
        private bool _hasDetonated;

        private static readonly Collider2D[] HitBuffer = new Collider2D[32];

        public event Action<float> OnRadiusUpdated;

        public void ActivateReaction(
            Vector3 position,
            float radius,
            float lifeTime,
            LayerMask targetMask,
            Action<Collider2D> onHit,
            Action<AutonomousEntityBase> onDie)
        {
            transform.position = position;
            SetRadius(radius);
            _targetMask = targetMask;
            _onHitCallback = onHit;
            _hasDetonated = false;

            OnRadiusUpdated?.Invoke(radius);

            base.Activate(lifeTime, onDie);
        }

        public void SetRadius(float radius)
        {
            _explosionRadius = radius;
        }

        protected override void Update()
        {
            base.Update();

            // Fire the AoE damage on the exact frame it spawns, then never again.
            if (!_hasDetonated)
            {
                PerformDetonation();
                _hasDetonated = true;
            }
        }

        private void PerformDetonation()
        {
            int hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, _explosionRadius, HitBuffer, _targetMask);

            for (int i = 0; i < hitCount; i++)
            {
                _onHitCallback?.Invoke(HitBuffer[i]);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, _explosionRadius);
        }
#endif
    }
}
