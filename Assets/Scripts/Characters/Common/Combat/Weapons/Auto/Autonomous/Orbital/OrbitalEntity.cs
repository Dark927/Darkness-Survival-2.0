using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    public class OrbitalEntity : AutonomousEntityBase
    {
        [Header("Collision Settings")]
        [SerializeField] private float _hitRadius = 1.0f;
        [SerializeField] private Vector2 _hitOffset;

        [Header("Orbit Settings")]
        [SerializeField] private bool _faceOrbitalTangent = false;

        private static readonly Collider2D[] _hitBuffer = new Collider2D[10];

        private Transform _orbitCenter;
        private float _orbitRadius;
        private float _orbitSpeed;
        private float _currentAngleDegrees;
        private float _damageTickRate;
        private LayerMask _targetMask;

        private Action<Collider2D, OrbitalEntity> _onHitCallback;

        // Tracks which enemies were hit and when they can be hit again
        private readonly Dictionary<Collider2D, float> _hitCooldowns = new Dictionary<Collider2D, float>();

        public void ActivateOrbit(
            Transform orbitCenter,
            float startAngleDegrees,
            float orbitRadius,
            float orbitSpeed,
            float lifeTime,
            float damageTickRate,
            LayerMask targetMask,
            Action<Collider2D, OrbitalEntity> onHit,
            Action<AutonomousEntityBase> onDie)
        {
            base.Activate(lifeTime, onDie);

            _orbitCenter = orbitCenter;
            _currentAngleDegrees = startAngleDegrees;
            _orbitRadius = orbitRadius;
            _orbitSpeed = orbitSpeed;
            _damageTickRate = damageTickRate;
            _targetMask = targetMask;
            _onHitCallback = onHit;

            // Clear dictionary so old entities don't block new hits
            _hitCooldowns.Clear();

            // Snap to start position immediately
            UpdatePosition();
        }

        protected override void Update()
        {
            base.Update();

            if (!this.enabled || _orbitCenter == null) return;

            // Progress Angle
            _currentAngleDegrees += _orbitSpeed * Time.deltaTime;

            // Keep angle normalized to prevent float precision issues over long runs
            if (_currentAngleDegrees >= 360f) _currentAngleDegrees -= 360f;
            else if (_currentAngleDegrees < 0f) _currentAngleDegrees += 360f;

            // Move Kinematically
            UpdatePosition();

            // Scan for Enemies and Deal Damage
            ProcessHits();
        }

        private void UpdatePosition()
        {
            float radians = _currentAngleDegrees * Mathf.Deg2Rad;

            float x = _orbitCenter.position.x + (Mathf.Cos(radians) * _orbitRadius);
            float y = _orbitCenter.position.y + (Mathf.Sin(radians) * _orbitRadius);

            transform.position = new Vector3(x, y, transform.position.z);

            if (_faceOrbitalTangent)
            {
                transform.rotation = Quaternion.Euler(0, 0, _currentAngleDegrees + 90f);
            }
        }

        private void ProcessHits()
        {
            Vector2 hitPosition = transform.TransformPoint(_hitOffset);
            int hitCount = Physics2D.OverlapCircleNonAlloc(hitPosition, _hitRadius, _hitBuffer, _targetMask);

            for (int i = 0; i < hitCount; i++)
            {
                Collider2D target = _hitBuffer[i];

                // If target is not in dictionary, OR the current time has passed their cooldown
                if (!_hitCooldowns.TryGetValue(target, out float nextHitTime) || Time.time >= nextHitTime)
                {
                    _onHitCallback?.Invoke(target, this);

                    // Record the time they can be hit again
                    _hitCooldowns[target] = Time.time + _damageTickRate;
                }
            }
        }

        protected override void Die()
        {
            // clear dictionary before returning to pool to prevent memory leaks
            _hitCooldowns.Clear();
            base.Die();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Vector2 hitPosition = transform.TransformPoint(_hitOffset);
            Gizmos.DrawWireSphere(hitPosition, _hitRadius);
        }
    }
}
