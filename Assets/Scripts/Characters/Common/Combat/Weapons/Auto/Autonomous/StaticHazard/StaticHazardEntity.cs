using System;
using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    /// <summary>
    /// A physics-less, universal static hazard that uses NonAlloc ticking 
    /// </summary>
    public class StaticHazardEntity : AutonomousEntityBase
    {
        private float _radius;
        private float _tickRate;
        private LayerMask _targetMask;
        private Action<Collider2D, StaticHazardEntity> _onTickHitAction;

        public event Action<float> OnHazardRadiusSet;

        private float _tickTimer;

        private const int STATIC_BUFFER_SIZE = 64;
        // Optimization: A shared buffer for every static hazard in the game.
        // Prevents memory allocation spikes during mass AoE overlaps.
        private static readonly Collider2D[] HitBuffer = new Collider2D[STATIC_BUFFER_SIZE];

        public void ActivateHazard(
            Vector3 position,
            float radius,
            float lifeTime,
            float tickRate,
            LayerMask targetMask,
            Action<Collider2D, StaticHazardEntity> onTickHit,
            Action<AutonomousEntityBase> onDie)
        {
            transform.position = position;
            _radius = radius;
            _tickRate = tickRate;
            _targetMask = targetMask;
            _onTickHitAction = onTickHit;

            // Force an immediate tick the exact frame it spawns
            _tickTimer = 0f;
            OnHazardRadiusSet?.Invoke(_radius);

            base.Activate(lifeTime, onDie);
        }

        protected override void Update()
        {
            base.Update(); // Handles the LifeTime expiration and Die() callback

            _tickTimer -= Time.deltaTime;

            if (_tickTimer <= 0f)
            {
                _tickTimer = _tickRate;
                PerformTick();
            }
        }

        private void PerformTick()
        {
            int hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, _radius, HitBuffer, _targetMask);

            for (int i = 0; i < hitCount; i++)
            {
                _onTickHitAction?.Invoke(HitBuffer[i], this);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.5f, 0f, 0.5f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}
