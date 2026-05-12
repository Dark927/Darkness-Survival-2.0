using System;
using System.Collections.Generic;
using Characters.Common.CustomPhysics2D;
using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    public class TornadoHazardEntity : AutonomousEntityBase, IResizableAutonomousEntity
    {
        [Header("Entity Stats")]
        private float _attackRadius;
        private float _pullRadius;
        private float _pullStrength;
        private float _tickRate;
        private float _moveSpeed;
        private LayerMask _targetMask;

        [Header("Locomotion & Steering")]
        private Vector3 _currentDirection;
        private float _timeAlive;
        private float _randomNoiseOffset;
        private Camera _mainCamera;

        // Steering Weights
        private readonly float _wanderTurnSpeed = 90f;
        private readonly float _boundaryTurnSpeed = 5f;
        private readonly float _repelSpeed = 8f;

        [Header("State Tracking")]
        private float _tickTimer;
        private Action<Collider2D, TornadoHazardEntity> _onDamageTickAction;
        public event Action<float> OnRadiusUpdated;

        // --- PERFORMANCE CACHES ---

        // PERF: Hive-mind list for Boids separation. Prevents needing physics casts to find other tornadoes.
        private static readonly List<TornadoHazardEntity> ActiveTornadoes = new();

        // PERF: Shared, pre-allocated memory buffer for OverlapCircle calls to eliminate GC allocation per frame.
        private const int MAX_HITS_PER_TICK = 64;
        private static readonly Collider2D[] HitBuffer = new Collider2D[MAX_HITS_PER_TICK];

        public void ActivateTornado(
            Vector3 position,
            Vector3 direction,
            float moveSpeed,
            float attackRadius,
            float pullRadius,
            float pullStrength,
            float lifeTime,
            float tickRate,
            LayerMask targetMask,
            Action<Collider2D, TornadoHazardEntity> onDamageTick,
            Action<AutonomousEntityBase> onDie)
        {
            transform.position = position;
            _currentDirection = direction.normalized;
            _moveSpeed = moveSpeed;
            _pullRadius = pullRadius;
            _pullStrength = pullStrength;
            _tickRate = tickRate;
            _targetMask = targetMask;
            _onDamageTickAction = onDamageTick;

            SetRadius(attackRadius);

            _tickTimer = 0f;
            _timeAlive = 0f;

            // Seed the Perlin noise so multiple instances don't sync their wandering patterns
            _randomNoiseOffset = UnityEngine.Random.Range(0f, 1000f);

            // Cache MainCamera. NOTE: If the game supports split-screen or dynamic cameras later, 
            // this needs to be passed in via an injection framework (ServiceLocator) instead of Camera.main.
            _mainCamera = Camera.main;

            if (!ActiveTornadoes.Contains(this))
            {
                ActiveTornadoes.Add(this);
            }

            base.Activate(lifeTime, onDie);
        }

        protected override void Die()
        {
            base.Die();

            // Failsafe: Ensure we remove from the static list when returned to the Object Pool
            ActiveTornadoes.Remove(this);
        }

        protected override void Update()
        {
            base.Update();
            _timeAlive += Time.deltaTime;

            UpdateWanderingMovement();
            PerformVacuum();

            _tickTimer -= Time.deltaTime;
            if (_tickTimer <= 0f)
            {
                _tickTimer = _tickRate;
                PerformDamageTick();
            }
        }

        private void UpdateWanderingMovement()
        {
            if (_mainCamera == null) return;

            Vector3 viewportPos = _mainCamera.WorldToViewportPoint(transform.position);

            // 10% screen margin
            float margin = 0.1f;
            bool isNearEdge = viewportPos.x < margin || viewportPos.x > 1f - margin ||
                              viewportPos.y < margin || viewportPos.y > 1f - margin;

            Vector3 separationVector = CalculateSeparation();

            // Priority Steering Behaviors
            if (separationVector.sqrMagnitude > 0.01f)
            {
                // PRIORITY 1: Boids Separation (Prevent overlapping instances)
                _currentDirection = Vector3.Lerp(_currentDirection, separationVector.normalized, Time.deltaTime * _repelSpeed).normalized;
            }
            else if (isNearEdge)
            {
                // PRIORITY 2: Viewport Bounds (Keep hazard on screen)
                Vector3 screenCenterWorld = _mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, viewportPos.z));
                Vector3 directionToCenter = (screenCenterWorld - transform.position).normalized;
                _currentDirection = Vector3.Lerp(_currentDirection, directionToCenter, Time.deltaTime * _boundaryTurnSpeed).normalized;
            }
            else
            {
                // PRIORITY 3: Natural Wandering (Perlin wind sheer)
                float noise = Mathf.PerlinNoise(_timeAlive * 0.5f, _randomNoiseOffset) * 2f - 1f; // * 2f - 1f  -> remap range from [0, 1] to [-1, 1]
                float turnAngle = noise * _wanderTurnSpeed * Time.deltaTime;
                _currentDirection = Quaternion.Euler(0, 0, turnAngle) * _currentDirection;
            }

            transform.position += _currentDirection * (_moveSpeed * Time.deltaTime);
        }

        private Vector3 CalculateSeparation()
        {
            Vector3 separation = Vector3.zero;
            float repelDistance = _pullRadius * 1.5f;

            // Uses the static list rather than an expensive PhysicsCast
            foreach (var other in ActiveTornadoes)
            {
                if (other == this)
                {
                    continue;
                }

                Vector3 difference = transform.position - other.transform.position;
                float distance = difference.magnitude;

                if ((distance > 0) && (distance < repelDistance))
                {
                    // Linear falloff based on proximity
                    float pushStrength = 1f - (distance / repelDistance);
                    separation += (difference.normalized * pushStrength);
                }
            }

            return separation;
        }

        private void PerformVacuum()
        {
            int hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, _pullRadius, HitBuffer, _targetMask);
            Vector2 tornadoCenter = transform.position;

            for (int i = 0; i < hitCount; i++)
            {
                if (HitBuffer[i].TryGetComponent<EntityColliderLink>(out var link) && (link.Logic.Body != null))
                {
                    Vector2 pullDirection = (tornadoCenter - (Vector2)HitBuffer[i].bounds.center).normalized;

                    // Normalize the vacuum force by deltaTime since this runs every frame
                    float frameForce = _pullStrength * Time.deltaTime;
                    link.Logic.Body.Movement.ApplyExternalPush(pullDirection, frameForce);
                }
            }
        }

        private void PerformDamageTick()
        {
            int hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, _attackRadius, HitBuffer, _targetMask);

            for (int i = 0; i < hitCount; i++)
            {
                _onDamageTickAction?.Invoke(HitBuffer[i], this);
            }
        }

        public void SetRadius(float radius)
        {
            _attackRadius = radius;
            OnRadiusUpdated?.Invoke(_attackRadius);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.8f, 0.2f, 0.2f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, _attackRadius);

            Gizmos.color = new Color(0.2f, 0.8f, 0.8f, 0.3f);
            Gizmos.DrawWireSphere(transform.position, _pullRadius);
        }
#endif
    }
}
