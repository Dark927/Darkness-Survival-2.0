using System;
using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    public class ProjectileEntity : AutonomousEntityBase
    {
        [Header("Collision Settings")]
        [Tooltip("The radius of the collision check.")]
        [SerializeField] private float _hitRadius = 0.5f;

        [Tooltip("Local offset for the hit circle. X moves it forward/back, Y moves it up/down.")]
        [SerializeField] private Vector2 _hitOffset;

        private static Collider2D[] _hitBuffer = new Collider2D[5];

        private Vector2 _direction;
        private float _speed;
        private LayerMask _targetMask;

        private Action<Collider2D, ProjectileEntity> _onHitCallback;

        public void Fire(Vector2 startPos, Vector2 direction, float speed, float lifeTime,
            LayerMask targetMask, Action<Collider2D, ProjectileEntity> onHit, Action<AutonomousEntityBase> onDie)
        {
            base.Activate(lifeTime, onDie);

            transform.position = startPos;
            _direction = direction.normalized;
            _speed = speed;
            _targetMask = targetMask;
            _onHitCallback = onHit;

            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        protected override void Update()
        {
            base.Update();

            if (!this.enabled) return;

            float step = _speed * Time.deltaTime;
            transform.Translate(_direction * step, Space.World);

            Vector2 hitPosition = transform.TransformPoint(_hitOffset);
            int hitCount = UnityEngine.Physics2D.OverlapCircleNonAlloc(hitPosition, _hitRadius, _hitBuffer, _targetMask);

            if (hitCount > 0)
            {
                Collider2D closestTarget = _hitBuffer[0];
                float closestDistance = float.MaxValue;

                for (int i = 0; i < hitCount; i++)
                {
                    float sqrDistance = (hitPosition - (Vector2)_hitBuffer[i].transform.position).sqrMagnitude;

                    if (sqrDistance < closestDistance)
                    {
                        closestDistance = sqrDistance;
                        closestTarget = _hitBuffer[i];
                    }
                }

                _onHitCallback?.Invoke(closestTarget, this);
                Die();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector2 hitPosition = transform.TransformPoint(_hitOffset);
            Gizmos.DrawWireSphere(hitPosition, _hitRadius);
        }
    }
}
